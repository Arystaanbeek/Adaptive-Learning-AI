using Microsoft.EntityFrameworkCore;
using AdaptiveLearning.API.Models;

namespace AdaptiveLearning.API.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    public DbSet<User> Users { get; set; }
    public DbSet<Course> Courses { get; set; }
    public DbSet<Module> Modules { get; set; }
    public DbSet<QuizQuestion> QuizQuestions { get; set; }
    public DbSet<Enrollment> Enrollments { get; set; }
    public DbSet<TestResult> TestResults { get; set; }
    public DbSet<LearningProfile> LearningProfiles { get; set; }
    public DbSet<CourseReview> CourseReviews { get; set; }
    public DbSet<VideoTracking> VideoTrackings { get; set; }

    protected override void OnModelCreating(ModelBuilder mb)
    {
        mb.Entity<User>().HasIndex(u => u.Email).IsUnique();

        mb.Entity<LearningProfile>()
            .HasOne(lp => lp.User)
            .WithOne(u => u.LearningProfile)
            .HasForeignKey<LearningProfile>(lp => lp.UserId);

        mb.Entity<Module>()
            .HasOne(m => m.Course)
            .WithMany(c => c.Modules)
            .HasForeignKey(m => m.CourseId);

        mb.Entity<QuizQuestion>()
            .HasOne<Module>()
            .WithMany(m => m.Questions)
            .HasForeignKey(q => q.ModuleId);

        mb.Entity<Enrollment>()
            .HasOne(e => e.User).WithMany(u => u.Enrollments).HasForeignKey(e => e.UserId);
        mb.Entity<Enrollment>()
            .HasOne(e => e.Course).WithMany(c => c.Enrollments).HasForeignKey(e => e.CourseId);

        mb.Entity<TestResult>()
            .HasOne(t => t.User).WithMany(u => u.TestResults).HasForeignKey(t => t.UserId);
        mb.Entity<TestResult>()
            .HasOne(t => t.Module).WithMany(m => m.TestResults).HasForeignKey(t => t.ModuleId);
    }
}
