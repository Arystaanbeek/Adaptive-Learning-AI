namespace AdaptiveLearning.API.Models;

public class Enrollment
{
    public int Id { get; set; }
    public int UserId { get; set; }
    public int CourseId { get; set; }
    public DateTime EnrolledAt { get; set; } = DateTime.UtcNow;
    public int Progress { get; set; } = 0; // 0-100 %
    public int CurrentModuleId { get; set; } = 0;
    public bool IsCompleted { get; set; } = false;

    public User? User { get; set; }
    public Course? Course { get; set; }
}

public class TestResult
{
    public int Id { get; set; }
    public int UserId { get; set; }
    public int ModuleId { get; set; }
    public int Score { get; set; }        // 0-100
    public int TimeSpentSec { get; set; } // сколько секунд провёл
    public DateTime TakenAt { get; set; } = DateTime.UtcNow;

    public User? User { get; set; }
    public Module? Module { get; set; }
}

public class LearningProfile
{
    public int Id { get; set; }
    public int UserId { get; set; }
    public string DominantStyle { get; set; } = "reading"; // visual|reading|interactive|social|research
    public double AvgScore { get; set; } = 0;
    public double RatioVideo { get; set; } = 0;
    public double RatioText { get; set; } = 0;
    public double RatioQuiz { get; set; } = 0;
    public string WeakTopics { get; set; } = "[]"; // JSON массив тем
    public string PredictedResult { get; set; } = ""; // Pass|Fail|Distinction|Withdrawn
    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

    public User? User { get; set; }
}

// DTO для фронтенда
public class CourseDto
{
    public int Id { get; set; }
    public string Title { get; set; } = "";
    public string Description { get; set; } = "";
    public string Category { get; set; } = "";
    public string Level { get; set; } = "";
    public string ThumbnailUrl { get; set; } = "";
    public int ModuleCount { get; set; }
    public bool IsEnrolled { get; set; }
    public int Progress { get; set; }
}
