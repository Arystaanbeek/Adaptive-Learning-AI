using AdaptiveLearning.API.Models;

public class CourseReview
{
    public int Id { get; set; }
    public int UserId { get; set; }
    public int CourseId { get; set; }
    public int Rating { get; set; } // 1-5
    public string Comment { get; set; } = "";
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public User? User { get; set; }
}