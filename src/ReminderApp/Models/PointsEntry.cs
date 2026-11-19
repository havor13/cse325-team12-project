namespace ReminderApp.Models;

/// <summary>One row in the points history (earned or lost).</summary>
public class PointsEntry
{
    public int Id { get; set; }
    public int UserId { get; set; }
    public int? TaskItemId { get; set; }
    public int Amount { get; set; }
    public string Reason { get; set; } = "";
    public DateTime CreatedUtc { get; set; } = DateTime.UtcNow;
}
