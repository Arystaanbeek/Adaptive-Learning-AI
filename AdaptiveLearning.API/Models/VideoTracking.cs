public class VideoTracking
{
    public int Id { get; set; }
    public int UserId { get; set; }
    public int ModuleId { get; set; }
    public int WatchedPercent { get; set; } // 0-100
    public bool Completed { get; set; }
    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
}