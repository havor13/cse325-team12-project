namespace ReminderApp.Services;

public record Level(int Number, string Title, string Badge, int MinPoints);

/// <summary>Level titles, badges and thresholds. PLACEHOLDERS: the team still has to decide these.</summary>
public static class LevelService
{
    public static readonly IReadOnlyList<Level> Levels = new List<Level>
    {
        new(1, "Beginner", "🌱", 0),
        new(2, "Planner",  "📝", 50),
        new(3, "Achiever", "⭐", 150),
        new(4, "Pro",      "🏅", 300),
        new(5, "Master",   "🏆", 600),
        new(6, "Legend",   "👑", 1000),
    };

    public static Level GetLevel(int points) => Levels.Last(l => points >= l.MinPoints);

    public static Level? GetNext(int points) => Levels.FirstOrDefault(l => l.MinPoints > points);

    /// <summary>Percent progress (0-100) from the current level toward the next. 100 at max level.</summary>
    public static double ProgressToNext(int points)
    {
        var current = GetLevel(points);
        var next = GetNext(points);
        if (next is null) return 100;
        return 100.0 * (points - current.MinPoints) / (next.MinPoints - current.MinPoints);
    }
}
