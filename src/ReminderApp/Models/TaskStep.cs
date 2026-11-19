namespace ReminderApp.Models;

public class TaskStep
{
    public int Id { get; set; }
    public int TaskItemId { get; set; }
    public string Description { get; set; } = "";
    public bool IsDone { get; set; }
}
