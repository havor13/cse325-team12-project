namespace ReminderApp.Models;

public enum TaskState { Pending, Completed, Missed }

public class TaskItem
{
    public int Id { get; set; }
    public int UserId { get; set; }
    public string Title { get; set; } = "";
    public DateOnly CreatedOn { get; set; }
    public DateOnly DueOn { get; set; }
    public DateOnly? CompletedOn { get; set; }
    public TaskState Status { get; set; } = TaskState.Pending;

    /// <summary>Set to false when the due date is changed (feature 4).</summary>
    public bool BonusEligible { get; set; } = true;

    public List<TaskStep> Steps { get; set; } = new();

    /// <summary>Each step is worth an equal share: 2 steps = 50% each, 3 steps = 33.33% each.</summary>
    public double StepPercent => Steps.Count == 0 ? 0 : 100.0 / Steps.Count;
    public double ProgressPercent => StepPercent * Steps.Count(s => s.IsDone);
}
