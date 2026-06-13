namespace AdaptiveLearning.API.Models;

public class Course
{
    public int Id { get; set; }
    public string Title { get; set; } = "";
    public string Description { get; set; } = "";
    public string Category { get; set; } = "";
    public string Level { get; set; } = "Beginner"; // Beginner | Intermediate | Advanced
    public string ThumbnailUrl { get; set; } = "";
    public int CreatedBy { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public bool IsPublished { get; set; } = false;

    public List<Module> Modules { get; set; } = new();
    public List<Enrollment> Enrollments { get; set; } = new();
}

public class Module
{
    public int Id { get; set; }
    public int CourseId { get; set; }
    public string Title { get; set; } = "";
    public string Description { get; set; } = "";
    public string ContentType { get; set; } = "text"; // video | text | audio | quiz
    public string ContentUrl { get; set; } = "";      // ссылка на видео/файл
    public string ContentText { get; set; } = "";     // текстовый контент
    public int OrderIndex { get; set; } = 0;
    public int DurationMinutes { get; set; } = 10;
    public bool IsQuiz { get; set; } = false;

    public Course? Course { get; set; }
    public List<QuizQuestion> Questions { get; set; } = new();
    public List<TestResult> TestResults { get; set; } = new();
}

public class QuizQuestion
{
    public int Id { get; set; }
    public int ModuleId { get; set; }
    public string Question { get; set; } = "";
    public string OptionA { get; set; } = "";
    public string OptionB { get; set; } = "";
    public string OptionC { get; set; } = "";
    public string OptionD { get; set; } = "";
    public string CorrectAnswer { get; set; } = "A"; // A B C D
    public int OrderIndex { get; set; } = 0;
}
