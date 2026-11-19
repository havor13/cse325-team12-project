using Microsoft.EntityFrameworkCore;
using ReminderApp.Models;

namespace ReminderApp.Data;

public class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options)
{
    public DbSet<AppUser> Users => Set<AppUser>();
    public DbSet<TaskItem> Tasks => Set<TaskItem>();
    public DbSet<TaskStep> Steps => Set<TaskStep>();
    public DbSet<PointsEntry> PointsHistory => Set<PointsEntry>();

    protected override void OnModelCreating(ModelBuilder b)
    {
        b.Entity<AppUser>().HasIndex(u => u.ExternalId).IsUnique();

        b.Entity<TaskItem>()
            .HasMany(t => t.Steps)
            .WithOne()
            .HasForeignKey(s => s.TaskItemId)
            .OnDelete(DeleteBehavior.Cascade);

        b.Entity<TaskItem>().HasIndex(t => new { t.UserId, t.DueOn });
    }
}
