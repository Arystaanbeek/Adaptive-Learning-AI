namespace AdaptiveLearning.API.Models;

public class User
{
    public int Id { get; set; }
    public string Name { get; set; } = "";
    public string Email { get; set; } = "";
    public string PasswordHash { get; set; } = "";
    public string Role { get; set; } = "Student"; // Student | Admin
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    public List<Enrollment> Enrollments { get; set; } = new();
    public List<TestResult> TestResults { get; set; } = new();
    public LearningProfile? LearningProfile { get; set; }
}

public class RegisterRequest
{
    public string Name { get; set; } = "";
    public string Email { get; set; } = "";
    public string Password { get; set; } = "";
}

public class LoginRequest
{
    public string Email { get; set; } = "";
    public string Password { get; set; } = "";
}

public class AuthResponse
{
    public string Token { get; set; } = "";
    public string Name { get; set; } = "";
    public string Email { get; set; } = "";
    public string Role { get; set; } = "";
    public int UserId { get; set; }
}
