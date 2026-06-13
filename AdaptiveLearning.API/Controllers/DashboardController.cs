using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using AdaptiveLearning.API.Data;

namespace AdaptiveLearning.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class DashboardController : ControllerBase
{
    private readonly AppDbContext _db;
    public DashboardController(AppDbContext db) { _db = db; }

    int CurrentUserId() => int.Parse(
        User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? "0");

    // GET api/dashboard/enrollments
    [HttpGet("enrollments")]
    public async Task<ActionResult> GetEnrollments()
    {
        var userId = CurrentUserId();
        var enrollments = await _db.Enrollments
            .Where(e => e.UserId == userId)
            .Include(e => e.Course)
                .ThenInclude(c => c.Modules)
            .ToListAsync();

        var result = enrollments.Select(e => new
        {
            courseId        = e.CourseId,
            courseTitle     = e.Course?.Title ?? "",
            progress        = e.Progress,
            isCompleted     = e.IsCompleted,
            enrolledAt      = e.EnrolledAt,
            totalModules    = e.Course?.Modules?.Count ?? 0,
            completedModules= e.Progress * (e.Course?.Modules?.Count ?? 0) / 100,
        });

        return Ok(result);
    }

    // GET api/dashboard/testresults
    [HttpGet("testresults")]
    public async Task<ActionResult> GetTestResults()
    {
        var userId = CurrentUserId();
        var results = await _db.TestResults
            .Where(t => t.UserId == userId && t.Score >= 0)
            .Include(t => t.Module)
            .OrderBy(t => t.TakenAt)
            .Select(t => new
            {
                moduleId    = t.ModuleId,
                moduleTitle = t.Module != null ? t.Module.Title : "",
                score       = t.Score,
                takenAt     = t.TakenAt,
                timeSpentSec= t.TimeSpentSec,
            })
            .ToListAsync();

        return Ok(results);
    }

    // GET api/dashboard/profile
    [HttpGet("profile")]
    public async Task<ActionResult> GetProfile()
    {
        var userId = CurrentUserId();
        var profile = await _db.LearningProfiles
            .FirstOrDefaultAsync(p => p.UserId == userId);

        if (profile == null)
            return Ok(new { dominantStyle = "reading", avgScore = 0 });

        // Считаем ratio из реальных данных если не заполнены
        if (profile.RatioVideo == 0 && profile.RatioText == 0)
        {
            var tests = await _db.TestResults
                .Where(t => t.UserId == userId)
                .ToListAsync();

            if (tests.Any())
            {
                profile.AvgScore = tests.Average(t => t.Score);
                await _db.SaveChangesAsync();
            }
        }

        return Ok(new
        {
            dominantStyle   = profile.DominantStyle,
            avgScore        = profile.AvgScore,
            ratioVideo      = profile.RatioVideo,
            ratioText       = profile.RatioText,
            ratioQuiz       = profile.RatioQuiz,
            predictedResult = profile.PredictedResult,
            weakTopics      = profile.WeakTopics,
            updatedAt       = profile.UpdatedAt,
        });
    }
}
