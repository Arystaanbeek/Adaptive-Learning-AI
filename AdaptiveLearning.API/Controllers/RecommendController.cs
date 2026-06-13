using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using AdaptiveLearning.API.Data;
using AdaptiveLearning.API.Models;

namespace AdaptiveLearning.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class RecommendController : ControllerBase
{
    private readonly AppDbContext _db;
    public RecommendController(AppDbContext db) { _db = db; }

    int CurrentUserId() => int.Parse(
        User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? "0");

    [HttpGet]
    public async Task<ActionResult> GetRecommendations()
    {
        var userId = CurrentUserId();
        var profile = await _db.LearningProfiles
            .FirstOrDefaultAsync(p => p.UserId == userId);

        // Все завершённые тесты студента
        var testResults = await _db.TestResults
            .Where(t => t.UserId == userId)
            .Include(t => t.Module)
            .ThenInclude(m => m.Course)
            .ToListAsync();

        // Все зачисления
        var enrolledIds = await _db.Enrollments
            .Where(e => e.UserId == userId)
            .Select(e => e.CourseId)
            .ToListAsync();

        var allCourses = await _db.Courses
            .Where(c => c.IsPublished && !enrolledIds.Contains(c.Id))
            .Include(c => c.Modules)
            .ToListAsync();

        if (!allCourses.Any())
            return Ok(new List<object>());

        var style = profile?.DominantStyle ?? "reading";
        var avg = profile?.AvgScore ?? 0;

        // ── Анализируем слабые и сильные стороны ──
        var abilities = AnalyzeAbilities(testResults, profile);

        var recommendations = allCourses
            .Select(c => new {
                course = c,
                score = CalcScore(c, abilities, style, avg),
                reason = BuildReason(c, abilities, style),
                tag = BuildTag(c, abilities, avg),
            })
            .OrderByDescending(x => x.score)
            .Take(4)
            .Select(x => new {
                id = x.course.Id,
                title = x.course.Title,
                description = x.course.Description,
                category = x.course.Category,
                level = x.course.Level,
                moduleCount = x.course.Modules.Count,
                reason = x.reason,
                matchScore = x.score,
                tag = x.tag,
            })
            .ToList();

        return Ok(recommendations);
    }

    // ── Анализ способностей по результатам тестов ──
    private StudentAbilities AnalyzeAbilities(
        List<TestResult> results, LearningProfile? profile)
    {
        var abilities = new StudentAbilities();

        if (!results.Any())
        {
            // Новый студент — рекомендуем Beginner по стилю
            abilities.IsNewStudent = true;
            abilities.WeakCategories = new List<string>();
            abilities.StrongCategories = new List<string>();
            return abilities;
        }

        // Группируем результаты по категории курса
        var byCategory = results
            .Where(t => t.Module?.Course?.Category != null)
            .GroupBy(t => t.Module!.Course!.Category!)
            .ToDictionary(
                g => g.Key,
                g => g.Average(t => t.Score)
            );

        abilities.WeakCategories = byCategory
            .Where(x => x.Value < 60)
            .OrderBy(x => x.Value)
            .Select(x => x.Key)
            .ToList();

        abilities.StrongCategories = byCategory
            .Where(x => x.Value >= 75)
            .OrderByDescending(x => x.Value)
            .Select(x => x.Key)
            .ToList();

        abilities.AvgScore = results.Average(t => t.Score);
        abilities.TotalTests = results.Count;

        // Анализ типа контента — что даётся лучше
        var videoScores = results
            .Where(t => t.Module?.ContentType == "video")
            .Select(t => t.Score).ToList();
        var textScores = results
            .Where(t => t.Module?.ContentType == "text")
            .Select(t => t.Score).ToList();
        var quizScores = results
            .Where(t => t.Module?.ContentType == "quiz")
            .Select(t => t.Score).ToList();

        abilities.VideoAvg = videoScores.Any() ? videoScores.Average() : 0;
        abilities.TextAvg = textScores.Any() ? textScores.Average() : 0;
        abilities.QuizAvg = quizScores.Any() ? quizScores.Average() : 0;

        // Слабый тип контента — тот где балл ниже 60
        abilities.WeakContentType =
            abilities.QuizAvg < 60 && quizScores.Any() ? "quiz" :
            abilities.VideoAvg < 60 && videoScores.Any() ? "video" :
            abilities.TextAvg < 60 && textScores.Any() ? "text" : null;

        return abilities;
    }

    // ── Скоринг курса ──
    private int CalcScore(Course c, StudentAbilities ab, string style, double avg)
    {
        int score = 0;

        // 1. ПРИОРИТЕТ: курсы которые прокачивают слабые категории
        if (ab.WeakCategories.Any())
        {
            var catLower = c.Category?.ToLower() ?? "";
            var titleLower = c.Title?.ToLower() ?? "";
            foreach (var weak in ab.WeakCategories)
            {
                if (catLower.Contains(weak.ToLower()) ||
                    titleLower.Contains(weak.ToLower()))
                {
                    score += 50; // Главный буст — прокачка слабых мест
                    break;
                }
            }
        }

        // 2. Уровень по успеваемости
        var preferredLevel = avg switch
        {
            >= 80 => "Advanced",
            >= 60 => "Intermediate",
            _ => "Beginner"
        };
        if (c.Level == preferredLevel) score += 30;
        else if (c.Level == "Beginner") score += 10;

        // 3. Совпадение стиля с типом контента в курсе
        var styleKeywords = style switch
        {
            "visual" => new[] { "видео", "дизайн", "графика", "video", "visual" },
            "reading" => new[] { "текст", "теория", "анализ", "text", "reading" },
            "interactive" => new[] { "практика", "программирование", "coding", "math" },
            "social" => new[] { "бизнес", "менеджмент", "business", "english" },
            "research" => new[] { "наука", "данные", "data", "science", "аналитика" },
            _ => Array.Empty<string>()
        };
        var titleLow = c.Title?.ToLower() ?? "";
        var catLow = c.Category?.ToLower() ?? "";
        if (styleKeywords.Any(k => titleLow.Contains(k) || catLow.Contains(k)))
            score += 25;

        // 4. Новый студент — предпочитаем Beginner
        if (ab.IsNewStudent && c.Level == "Beginner") score += 20;

        // 5. Детерминированная вариативность
        score += c.Id % 10;

        return score;
    }

    // ── Причина рекомендации — конкретная и полезная ──
    private string BuildReason(Course c, StudentAbilities ab, string style)
    {
        var catLower = c.Category?.ToLower() ?? "";
        var titleLower = c.Title?.ToLower() ?? "";

        // Если курс закрывает слабую категорию
        foreach (var weak in ab.WeakCategories)
        {
            if (catLower.Contains(weak.ToLower()) ||
                titleLower.Contains(weak.ToLower()))
            {
                return $"Прокачает твои навыки в области «{weak}» — здесь есть точки роста";
            }
        }

        // Если у студента слабый тип контента
        if (ab.WeakContentType != null)
        {
            var contentDesc = ab.WeakContentType switch
            {
                "quiz" => "тесты и квизы — это усилит теоретическую базу",
                "video" => "видеоматериалы — поможет воспринимать этот формат",
                "text" => "текстовые материалы — расширит навык работы с теорией",
                _ => "разнообразный формат"
            };
            return $"Содержит {contentDesc}";
        }

        // Если новый студент
        if (ab.IsNewStudent)
        {
            return style switch
            {
                "visual" => "Идеально для старта — много видеоматериалов под твой стиль",
                "reading" => "Идеально для старта — структурированные тексты под твой стиль",
                "interactive" => "Идеально для старта — практические задания с первого шага",
                _ => "Хорошая отправная точка для твоего обучения"
            };
        }

        // Сильные стороны — предлагаем развивать дальше
        if (ab.StrongCategories.Any())
            return $"Развивает твои сильные стороны на новом уровне";

        return style switch
        {
            "visual" => "Много видеоматериалов — твой формат",
            "reading" => "Глубокие текстовые материалы — твой формат",
            "interactive" => "Практические задания для закрепления навыков",
            _ => "Подобран под твой профиль обучения"
        };
    }

    // ── Тег карточки ──
    private string BuildTag(Course c, StudentAbilities ab, double avg)
    {
        var catLower = c.Category?.ToLower() ?? "";
        var titleLower = c.Title?.ToLower() ?? "";

        if (ab.WeakCategories.Any(w =>
            catLower.Contains(w.ToLower()) || titleLower.Contains(w.ToLower())))
            return "🎯 Точка роста";

        if (avg >= 80) return "🔥 Для продвинутых";
        if (ab.IsNewStudent) return "✨ Для старта";

        return c.Level switch
        {
            "Advanced" => "🚀 Продвинутый",
            "Intermediate" => "📈 Средний",
            _ => "⭐ Рекомендовано"
        };
    }
}

// ── Вспомогательная модель ──
public class StudentAbilities
{
    public bool IsNewStudent { get; set; }
    public List<string> WeakCategories { get; set; } = new();
    public List<string> StrongCategories { get; set; } = new();
    public double AvgScore { get; set; }
    public int TotalTests { get; set; }
    public double VideoAvg { get; set; }
    public double TextAvg { get; set; }
    public double QuizAvg { get; set; }
    public string? WeakContentType { get; set; }
}