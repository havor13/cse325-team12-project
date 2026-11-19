namespace ReminderApp.Models;

/// <summary>App-side record for a signed-in user. No personal data is stored, only the OAuth subject id.</summary>
public class AppUser
{
    public int Id { get; set; }
    public string ExternalId { get; set; } = "";
    public int Points { get; set; }
}
