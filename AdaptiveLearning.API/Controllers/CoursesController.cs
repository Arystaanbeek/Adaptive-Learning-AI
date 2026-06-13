using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using AdaptiveLearning.API.Data;
using AdaptiveLearning.API.Models;
using AdaptiveLearning.API.Services;
using System.Security.Claims;

namespace AdaptiveLearning.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class CoursesController : ControllerBase
{
    private readonly AppDbContext _db;
    private readonly AiService   _ai;

    public CoursesController(AppDbContext db, AiService ai)
    {
        _db = db;
        _ai = ai;
    }

    int CurrentUserId() => int.Parse(
        User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? "0");

    // GET api/courses
    [HttpGet]
    public async Task<ActionResult> GetAll()
    {
        var userId     = CurrentUserId();
        var enrolledIds= userId > 0
            ? await _db.Enrollments.Where(e => e.UserId == userId)
                .Select(e => e.CourseId).ToListAsync()
            : new List<int>();

        var courses = await _db.Courses
            .Where(c => c.IsPublished)
            .Include(c => c.Modules)
            .ToListAsync();

        var result = courses.Select(c => new CourseDto
        {
            Id          = c.Id,
            Title       = c.Title,
            Description = c.Description,
            Category    = c.Category,
            Level       = c.Level,
            ThumbnailUrl= c.ThumbnailUrl,
            ModuleCount = c.Modules.Count,
            IsEnrolled  = enrolledIds.Contains(c.Id),
            Progress    = _db.Enrollments
                .Where(e => e.UserId == userId && e.CourseId == c.Id)
                .Select(e => e.Progress)
                .FirstOrDefault(),
        });

        return Ok(result);
    }

    // GET api/courses/{id}
    [HttpGet("{id}")]
    public async Task<ActionResult> GetById(int id)
    {
        var course = await _db.Courses
            .Include(c => c.Modules.OrderBy(m => m.OrderIndex))
            .FirstOrDefaultAsync(c => c.Id == id);
        if (course == null) return NotFound();
        return Ok(course);
    }

    // GET api/courses/{id}/modules
    [HttpGet("{id}/modules")]
    [Authorize]
    public async Task<ActionResult> GetModules(int id)
    {
        var modules = await _db.Modules
            .Where(m => m.CourseId == id)
            .OrderBy(m => m.OrderIndex)
            .ToListAsync();
        return Ok(modules);
    }

    // GET api/courses/module/{id}/questions
    [HttpGet("module/{moduleId}/questions")]
    [Authorize]
    public async Task<ActionResult> GetQuestions(int moduleId)
    {
        var questions = await _db.QuizQuestions
            .Where(q => q.ModuleId == moduleId)
            .OrderBy(q => q.OrderIndex)
            .ToListAsync();
        return Ok(questions);
    }

    // POST api/courses/enroll/{courseId}
    [HttpPost("enroll/{courseId}")]
    [Authorize]
    public async Task<ActionResult> Enroll(int courseId)
    {
        var userId = CurrentUserId();
        if (await _db.Enrollments.AnyAsync(e => e.UserId == userId && e.CourseId == courseId))
            return BadRequest(new { error = "Уже записан" });
        _db.Enrollments.Add(new Enrollment { UserId = userId, CourseId = courseId });
        await _db.SaveChangesAsync();
        return Ok(new { message = "Записан на курс" });
    }

    // POST api/courses/module/{id}/complete — завершить модуль + автоматический AI анализ
    [HttpPost("module/{moduleId}/complete")]
    [Authorize]
    public async Task<ActionResult> CompleteModule(int moduleId,
        [FromBody] CompleteModuleRequest req)
    {
        var userId = CurrentUserId();
        var module = await _db.Modules.FindAsync(moduleId);
        if (module == null) return NotFound();

        // Сохраняем результат теста
        if (req.Score >= 0)
        {
            _db.TestResults.Add(new TestResult
            {
                UserId       = userId,
                ModuleId     = moduleId,
                Score        = req.Score,
                TimeSpentSec = req.TimeSpentSec,
            });
        }

        // Обновляем прогресс курса
        var enrollment = await _db.Enrollments
            .FirstOrDefaultAsync(e => e.UserId == userId && e.CourseId == module.CourseId);
        if (enrollment != null)
        {
            var total = await _db.Modules.CountAsync(m => m.CourseId == module.CourseId);
            var done  = await _db.TestResults
                .Where(t => t.UserId == userId)
                .Select(t => t.ModuleId).Distinct().CountAsync();
            enrollment.Progress        = total > 0 ? Math.Min(done * 100 / total, 100) : 0;
            enrollment.CurrentModuleId = moduleId;
        }

        await _db.SaveChangesAsync();

        // ── АВТОМАТИЧЕСКИЙ AI АНАЛИЗ ─────────────────────────────────────────
        var aiResult = await RunAutoAiAnalysis(userId);

        // Адаптивный следующий модуль
        var nextModule = await GetNextAdaptiveModule(
            userId, module.CourseId, moduleId, req.Score,
            aiResult?.LearningStyle ?? "");

        return Ok(new
        {
            message        = "Модуль завершён",
            nextModuleId   = nextModule?.Id,
            nextModuleTitle= nextModule?.Title,
            nextModuleType = nextModule?.ContentType,
            aiPrediction   = aiResult?.Prediction,
            learningStyle  = aiResult?.LearningStyle,
            recommendation = aiResult?.Recommendation,
        });
    }

    // GET api/courses/{id}/ai-profile
    [HttpGet("{id}/ai-profile")]
    [Authorize]
    public async Task<ActionResult> GetAiProfile(int id)
    {
        var userId = CurrentUserId();

        var tests = await _db.TestResults
            .Where(t => t.UserId == userId)
            .Include(t => t.Module)
            .OrderBy(t => t.Id)
            .ToListAsync();

        var enrollment = await _db.Enrollments
            .FirstOrDefaultAsync(e => e.UserId == userId && e.CourseId == id);

        if (!tests.Any())
            return Ok(new
            {
                prediction = "—",
                probabilities = new Dictionary<string, double>(),
                learningStyle = "—",
                recommendation = "Пройди первый модуль для анализа",
                riskLevel = "none",
                progress = enrollment?.Progress ?? 0
            });

        // Собираем признаки
        var avgScore = tests.Average(t => t.Score);
        var maxScore = tests.Max(t => t.Score);
        var minScore = tests.Min(t => t.Score);
        var stdScore = tests.Count > 1
            ? Math.Sqrt(tests.Average(t => Math.Pow(t.Score - avgScore, 2))) : 0;
        var scoreTrend = tests.Count > 1
            ? tests.Last().Score - tests.First().Score : 0;
        var totalTime = tests.Sum(t => t.TimeSpentSec) + 1;
        var videoTime = tests.Where(t => t.Module?.ContentType == "video").Sum(t => t.TimeSpentSec);
        var textTime = tests.Where(t => t.Module?.ContentType == "text").Sum(t => t.TimeSpentSec);
        var quizTime = tests.Where(t => t.Module?.ContentType == "quiz").Sum(t => t.TimeSpentSec);
        var ratioVideo = videoTime / (double)totalTime;
        var ratioText = textTime / (double)totalTime;
        var ratioQuiz = quizTime / (double)totalTime;
        double styleEnc = ratioVideo > ratioText && ratioVideo > ratioQuiz ? 4
                          : ratioQuiz > ratioText ? 0 : 1;

        var input = new AdaptInput
        {
            avg_score = avgScore,
            max_score = maxScore,
            min_score = minScore,
            std_score = stdScore,
            submission_count = tests.Count,
            submission_rate = 1.0,
            score_trend = scoreTrend,
            total_clicks = tests.Sum(t => t.TimeSpentSec / 10.0),
            total_sessions = tests.Count,
            ratio_video = ratioVideo,
            ratio_oucontent = ratioText,
            ratio_quiz = ratioQuiz,
            dominant_style_enc = styleEnc,
            unique_resources = tests.Select(t => t.ModuleId).Distinct().Count(),
            avg_clicks_per_res = tests.Count > 0 ? tests.Sum(t => t.TimeSpentSec / 10.0) / tests.Count : 0,
            revisit_rate = 1.0,
            study_regularity = Math.Min(tests.Count * 10, 100),
            studied_credits = 60,
            age_band_enc = 2,
            q1_clicks = tests.Sum(t => t.TimeSpentSec / 40.0),
            q2_clicks = tests.Sum(t => t.TimeSpentSec / 40.0),
            q3_clicks = tests.Sum(t => t.TimeSpentSec / 40.0),
            q4_clicks = tests.Sum(t => t.TimeSpentSec / 40.0),
        };

        var result = await _ai.PredictAdaptationAsync(input);
        if (result == null)
            return Ok(new
            {
                prediction = "—",
                probabilities = new { },
                learningStyle = "—",
                recommendation = "AI сервис недоступен",
                riskLevel = "none",
                progress = enrollment?.Progress ?? 0
            });

        // Определяем уровень риска
        var failProb = result.Probabilities.GetValueOrDefault("Fail", 0)
                     + result.Probabilities.GetValueOrDefault("Withdrawn", 0);
        var riskLevel = failProb > 0.6 ? "high" : failProb > 0.35 ? "medium" : "low";

        return Ok(new
        {
            prediction = result.Prediction,
            probabilities = result.Probabilities,
            learningStyle = result.LearningStyle,
            recommendation = result.Recommendation,
            riskLevel,
            avgScore = Math.Round(avgScore, 1),
            scoreTrend,
            modulesCompleted = tests.Select(t => t.ModuleId).Distinct().Count(),
            progress = enrollment?.Progress ?? 0
        });
    }

    // Автоматический AI анализ на основе реальной активности студента
    async Task<AdaptResult?> RunAutoAiAnalysis(int userId)
    {
        try
        {
            var tests = await _db.TestResults
                .Where(t => t.UserId == userId && t.Score >= 0)
                .Include(t => t.Module)
                .ToListAsync();

            if (!tests.Any()) return null;

            var enrollments = await _db.Enrollments
                .Where(e => e.UserId == userId)
                .ToListAsync();

            // Считаем признаки из реальных данных
            var avgScore       = tests.Average(t => t.Score);
            var maxScore       = tests.Max(t => t.Score);
            var minScore       = tests.Min(t => t.Score);
            var stdScore       = tests.Count > 1
                ? Math.Sqrt(tests.Average(t => Math.Pow(t.Score - avgScore, 2)))
                : 0;
            var submissionRate = tests.Count > 0 ? 1.0 : 0;
            var totalSessions  = tests.Count;
            var totalClicks    = tests.Sum(t => t.TimeSpentSec / 10.0);

            // Определяем доли типов контента
            var videoTime = tests.Where(t => t.Module?.ContentType == "video").Sum(t => t.TimeSpentSec);
            var textTime  = tests.Where(t => t.Module?.ContentType == "text").Sum(t => t.TimeSpentSec);
            var quizTime  = tests.Where(t => t.Module?.ContentType == "quiz").Sum(t => t.TimeSpentSec);
            var totalTime = videoTime + textTime + quizTime + 1;

            var ratioVideo    = videoTime / (double)totalTime;
            var ratioOucontent= textTime  / (double)totalTime;
            var ratioQuiz     = quizTime  / (double)totalTime;

            // Определяем стиль кодировкой
            double styleEnc = 1; // reading по умолчанию
            if (ratioVideo > ratioOucontent && ratioVideo > ratioQuiz) styleEnc = 4; // visual
            else if (ratioQuiz > ratioOucontent)                        styleEnc = 0; // interactive
            else                                                         styleEnc = 1; // reading

            var input = new AdaptInput
            {
                avg_score           = avgScore,
                max_score           = maxScore,
                min_score           = minScore,
                std_score           = stdScore,
                submission_count    = tests.Count,
                submission_rate     = submissionRate,
                score_trend         = tests.Count > 1
                    ? tests.Last().Score - tests.First().Score
                    : 0,
                total_clicks        = totalClicks,
                total_sessions      = totalSessions,
                ratio_video         = ratioVideo,
                ratio_oucontent     = ratioOucontent,
                ratio_quiz          = ratioQuiz,
                dominant_style_enc  = styleEnc,
                revisit_rate        = 1.0,
                study_regularity    = 50,
                unique_resources    = tests.Select(t => t.ModuleId).Distinct().Count(),
                avg_clicks_per_res  = totalClicks / Math.Max(tests.Count, 1),
                q1_clicks           = totalClicks * 0.25,
                q2_clicks           = totalClicks * 0.25,
                q3_clicks           = totalClicks * 0.25,
                q4_clicks           = totalClicks * 0.25,
                unregistered        = 0,
                num_of_prev_attempts= 0,
                studied_credits     = 60,
                age_band_enc        = 2,
            };

            var result = await _ai.PredictAdaptationAsync(input);

            // Обновляем профиль студента
            if (result != null)
            {
                var profile = await _db.LearningProfiles
                    .FirstOrDefaultAsync(p => p.UserId == userId);
                if (profile == null)
                {
                    profile = new LearningProfile { UserId = userId };
                    _db.LearningProfiles.Add(profile);
                }
                profile.DominantStyle   = result.LearningStyle;
                profile.AvgScore        = avgScore;
                profile.RatioVideo      = ratioVideo;
                profile.RatioText       = ratioOucontent;
                profile.RatioQuiz       = ratioQuiz;
                profile.PredictedResult = result.Prediction;
                profile.UpdatedAt       = DateTime.UtcNow;
                await _db.SaveChangesAsync();
            }

            return result;
        }
        catch { return null; }
    }

    // Адаптивный выбор следующего модуля с учётом стиля
    async Task<Module?> GetNextAdaptiveModule(
        int userId, int courseId, int currentModuleId,
        int score, string learningStyle)
    {
        var allModules = await _db.Modules
            .Where(m => m.CourseId == courseId)
            .OrderBy(m => m.OrderIndex)
            .ToListAsync();

        var completedIds = await _db.TestResults
            .Where(t => t.UserId == userId)
            .Select(t => t.ModuleId).Distinct()
            .ToListAsync();

        var currentOrder = allModules
            .FirstOrDefault(m => m.Id == currentModuleId)?.OrderIndex ?? 0;

        var remaining = allModules
            .Where(m => m.OrderIndex > currentOrder && !completedIds.Contains(m.Id))
            .ToList();

        if (!remaining.Any()) return null;

        // Если балл низкий — приоритет на тип контента под стиль студента
        if (score < 60 && !string.IsNullOrEmpty(learningStyle))
        {
            var preferred = learningStyle switch
            {
                "visual"      => "video",
                "reading"     => "text",
                "interactive" => "quiz",
                _             => ""
            };
            var styled = remaining.FirstOrDefault(m => m.ContentType == preferred);
            if (styled != null) return styled;
        }

        return remaining.First();
    }

    // ── ADMIN endpoints ──────────────────────────────────────────────────────

    [HttpPost]
    [Authorize(Roles = "Admin")]
    public async Task<ActionResult> Create([FromBody] Course course)
    {
        course.CreatedBy = CurrentUserId();
        course.CreatedAt = DateTime.UtcNow;
        _db.Courses.Add(course);
        await _db.SaveChangesAsync();
        return Ok(course);
    }

    [HttpPut("{id}/publish")]
    [Authorize(Roles = "Admin")]
    public async Task<ActionResult> Publish(int id)
    {
        var c = await _db.Courses.FindAsync(id);
        if (c == null) return NotFound();
        c.IsPublished = !c.IsPublished;
        await _db.SaveChangesAsync();
        return Ok(new { published = c.IsPublished });
    }

    [HttpPost("{courseId}/modules")]
    [Authorize(Roles = "Admin")]
    public async Task<ActionResult> AddModule(int courseId, [FromBody] Module module)
    {
        module.CourseId = courseId;
        _db.Modules.Add(module);
        await _db.SaveChangesAsync();
        return Ok(module);
    }

    [HttpPost("module/{moduleId}/questions")]
    [Authorize(Roles = "Admin")]
    public async Task<ActionResult> AddQuestion(int moduleId, [FromBody] QuizQuestion q)
    {
        q.ModuleId = moduleId;
        _db.QuizQuestions.Add(q);
        await _db.SaveChangesAsync();
        return Ok(q);
    }

    [HttpGet("admin/all")]
    [Authorize(Roles = "Admin")]
    public async Task<ActionResult> AdminGetAll()
    {
        var courses = await _db.Courses
            .Include(c => c.Modules)
            .Select(c => new {
                c.Id, c.Title, c.Category, c.Level,
                c.IsPublished, c.CreatedAt,
                ModuleCount = c.Modules.Count,
                Enrollments = _db.Enrollments.Count(e => e.CourseId == c.Id),
            }).ToListAsync();
        return Ok(courses);
    }

    [HttpGet("admin/students")]
    [Authorize(Roles = "Admin")]
    public async Task<ActionResult> AdminGetStudents()
    {
        var students = await _db.Users
            .Where(u => u.Role == "Student")
            .Include(u => u.LearningProfile)
            .Select(u => new {
                u.Id, u.Name, u.Email, u.CreatedAt,
                Style      = u.LearningProfile != null ? u.LearningProfile.DominantStyle : "",
                AvgScore   = u.LearningProfile != null ? u.LearningProfile.AvgScore : 0,
                Prediction = u.LearningProfile != null ? u.LearningProfile.PredictedResult : "",
                Courses    = _db.Enrollments.Count(e => e.UserId == u.Id),
            }).ToListAsync();
        return Ok(students);
    }
    // GET api/courses/{id}/reviews
    [HttpGet("{id}/reviews")]
    public async Task<ActionResult> GetReviews(int id)
    {
        var reviews = await _db.CourseReviews
            .Where(r => r.CourseId == id)
            .Include(r => r.User)
            .OrderByDescending(r => r.CreatedAt)
            .Select(r => new {
                r.Id,
                r.Rating,
                r.Comment,
                r.CreatedAt,
                UserName = r.User != null ? r.User.Name : "Студент"
            })
            .ToListAsync();

        var avg = reviews.Any() ? reviews.Average(r => r.Rating) : 0;
        return Ok(new { reviews, avgRating = Math.Round(avg, 1), count = reviews.Count });
    }

    // POST api/courses/{id}/review
    [HttpPost("{id}/review")]
    [Authorize]
    public async Task<ActionResult> AddReview(int id, [FromBody] ReviewRequest req)
    {
        var userId = CurrentUserId();
        // Проверяем что записан на курс
        if (!await _db.Enrollments.AnyAsync(e => e.UserId == userId && e.CourseId == id))
            return BadRequest(new { error = "Нужно быть записанным на курс" });

        // Один отзыв на курс
        var existing = await _db.CourseReviews
            .FirstOrDefaultAsync(r => r.UserId == userId && r.CourseId == id);
        if (existing != null)
        {
            existing.Rating = req.Rating;
            existing.Comment = req.Comment;
            existing.CreatedAt = DateTime.UtcNow;
        }
        else
        {
            _db.CourseReviews.Add(new CourseReview
            {
                UserId = userId,
                CourseId = id,
                Rating = req.Rating,
                Comment = req.Comment
            });
        }
        await _db.SaveChangesAsync();
        return Ok(new { message = "Отзыв сохранён" });
    }

    // POST api/courses/module/{id}/video-progress
    [HttpPost("module/{moduleId}/video-progress")]
    [Authorize]
    public async Task<ActionResult> SaveVideoProgress(int moduleId,
        [FromBody] VideoProgressRequest req)
    {
        var userId = CurrentUserId();
        var tracking = await _db.VideoTrackings
            .FirstOrDefaultAsync(v => v.UserId == userId && v.ModuleId == moduleId);
        if (tracking == null)
        {
            tracking = new VideoTracking { UserId = userId, ModuleId = moduleId };
            _db.VideoTrackings.Add(tracking);
        }
        tracking.WatchedPercent = Math.Max(tracking.WatchedPercent, req.WatchedPercent);
        tracking.Completed = req.WatchedPercent >= 90;
        tracking.UpdatedAt = DateTime.UtcNow;
        await _db.SaveChangesAsync();
        return Ok(new { completed = tracking.Completed, percent = tracking.WatchedPercent });
    }
}

public class CompleteModuleRequest
{
    public int Score        { get; set; } = -1;
    public int TimeSpentSec { get; set; } = 0;
}

public class ReviewRequest
{
    public int Rating { get; set; }
    public string Comment { get; set; } = "";
}

public class VideoProgressRequest
{
    public int WatchedPercent { get; set; }
}