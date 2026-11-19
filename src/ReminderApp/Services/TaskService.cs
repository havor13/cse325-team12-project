using Microsoft.EntityFrameworkCore;
using ReminderApp.Data;
using ReminderApp.Models;

namespace ReminderApp.Services;

/// <summary>
/// Task CRUD plus the reward/penalty rules. Every query is filtered by userId so
/// users can only see and change their own data.
/// </summary>
public class TaskService(IDbContextFactory<AppDbContext> factory)
{
    public const int MaxDueDays = 14;

    private static DateOnly Today => DateOnly.FromDateTime(DateTime.UtcNow);

    public async Task<List<TaskItem>> GetAllAsync(int userId)
    {
        await using var db = await factory.CreateDbContextAsync();
        return await db.Tasks.Include(t => t.Steps)
            .Where(t => t.UserId == userId)
            .OrderBy(t => t.DueOn)
            .ToListAsync();
    }

    public async Task<TaskItem?> GetAsync(int userId, int taskId)
    {
        await using var db = await factory.CreateDbContextAsync();
        return await db.Tasks.Include(t => t.Steps)
            .FirstOrDefaultAsync(t => t.UserId == userId && t.Id == taskId);
    }

    public async Task<TaskItem> CreateAsync(int userId, string title, DateOnly dueOn, IEnumerable<string> steps)
    {
        var stepList = steps.Select(s => s.Trim()).Where(s => s.Length > 0).ToList();
        Validate(title, dueOn, stepList.Count);

        var task = new TaskItem
        {
            UserId = userId,
            Title = title.Trim(),
            CreatedOn = Today,
            DueOn = dueOn,
            Steps = stepList.Select(s => new TaskStep { Description = s }).ToList(),
        };

        await using var db = await factory.CreateDbContextAsync();
        db.Tasks.Add(task);
        await db.SaveChangesAsync();
        return task;
    }

    /// <summary>Edit title and/or due date. Changing the due date removes the early-completion bonus.</summary>
    public async Task UpdateAsync(int userId, int taskId, string title, DateOnly dueOn)
    {
        await using var db = await factory.CreateDbContextAsync();
        var task = await db.Tasks.Include(t => t.Steps)
            .FirstOrDefaultAsync(t => t.UserId == userId && t.Id == taskId);
        if (task is null || task.Status != TaskState.Pending) return;

        var dueChanged = dueOn != task.DueOn;
        Validate(title, dueChanged ? dueOn : Today, Math.Max(1, task.Steps.Count));

        task.Title = title.Trim();
        if (dueChanged)
        {
            task.DueOn = dueOn;
            task.BonusEligible = false;
        }
        await db.SaveChangesAsync();
    }

    public async Task AddStepAsync(int userId, int taskId, string description)
    {
        if (string.IsNullOrWhiteSpace(description)) return;
        await using var db = await factory.CreateDbContextAsync();
        var task = await db.Tasks.Include(t => t.Steps)
            .FirstOrDefaultAsync(t => t.UserId == userId && t.Id == taskId);
        if (task is null || task.Status != TaskState.Pending) return;

        task.Steps.Add(new TaskStep { Description = description.Trim() });
        await db.SaveChangesAsync();
    }

    public async Task RemoveStepAsync(int userId, int stepId)
    {
        await using var db = await factory.CreateDbContextAsync();
        var task = await db.Tasks.Include(t => t.Steps)
            .FirstOrDefaultAsync(t => t.UserId == userId && t.Steps.Any(s => s.Id == stepId));
        if (task is null || task.Status != TaskState.Pending || task.Steps.Count <= 1) return; // keep at least one step

        task.Steps.Remove(task.Steps.First(s => s.Id == stepId));
        await db.SaveChangesAsync();
    }

    public async Task DeleteAsync(int userId, int taskId)
    {
        await using var db = await factory.CreateDbContextAsync();
        var task = await db.Tasks.FirstOrDefaultAsync(t => t.UserId == userId && t.Id == taskId);
        if (task is null) return;
        db.Tasks.Remove(task);
        await db.SaveChangesAsync();
    }

    /// <summary>Tick or untick a step. Ticking the last step completes the task and awards points.</summary>
    public async Task ToggleStepAsync(int userId, int stepId)
    {
        await using var db = await factory.CreateDbContextAsync();
        var task = await db.Tasks.Include(t => t.Steps)
            .FirstOrDefaultAsync(t => t.UserId == userId && t.Steps.Any(s => s.Id == stepId));
        if (task is null || task.Status != TaskState.Pending || task.DueOn < Today) return; // overdue tasks get swept as missed

        var step = task.Steps.First(s => s.Id == stepId);
        step.IsDone = !step.IsDone;

        if (task.Steps.All(s => s.IsDone))
        {
            var today = Today;
            task.Status = TaskState.Completed;
            task.CompletedOn = today;

            var windowDays = task.DueOn.DayNumber - task.CreatedOn.DayNumber;
            var daysEarly = task.DueOn.DayNumber - today.DayNumber;
            var points = PointsService.CalculateReward(task.Steps.Count, windowDays, daysEarly, task.BonusEligible);
            await ApplyPointsAsync(db, task, points, "Completed on time");
        }
        await db.SaveChangesAsync();
    }

    /// <summary>Marks overdue pending tasks as missed and deducts points. Call when the dashboard loads.</summary>
    public async Task SweepMissedAsync(int userId)
    {
        await using var db = await factory.CreateDbContextAsync();
        var today = Today;
        var overdue = await db.Tasks.Include(t => t.Steps)
            .Where(t => t.UserId == userId && t.Status == TaskState.Pending && t.DueOn < today)
            .ToListAsync();

        foreach (var task in overdue)
        {
            task.Status = TaskState.Missed;
            await ApplyPointsAsync(db, task, -PointsService.CalculatePenalty(task.Steps.Count), "Task not completed");
        }
        if (overdue.Count > 0) await db.SaveChangesAsync();
    }

    private static async Task ApplyPointsAsync(AppDbContext db, TaskItem task, int amount, string reason)
    {
        var user = await db.Users.FirstAsync(u => u.Id == task.UserId);
        var newTotal = Math.Max(0, user.Points + amount); // points never go below zero
        var applied = newTotal - user.Points;
        user.Points = newTotal;
        db.PointsHistory.Add(new PointsEntry { UserId = user.Id, TaskItemId = task.Id, Amount = applied, Reason = reason });
    }

    private static void Validate(string title, DateOnly dueOn, int stepCount)
    {
        if (string.IsNullOrWhiteSpace(title) || title.Trim().Length > 200)
            throw new ArgumentException("Title is required (max 200 characters).");
        if (stepCount < 1)
            throw new ArgumentException("Add at least one step.");
        if (dueOn < Today || dueOn > Today.AddDays(MaxDueDays))
            throw new ArgumentException($"Due date must be within the next {MaxDueDays} days.");
    }
}
