namespace ReminderApp.Services;

/// <summary>
/// "Hitting the Mark" point rules. PLACEHOLDER VALUES: the team still has to decide the real numbers.
/// Pure functions, so they are easy to unit test.
/// </summary>
public static class PointsService
{
    public const int BasePoints = 10;
    public const int PointsPerStep = 5;
    public const int PointsPerWindowDay = 1;   // longer planning window = more effort
    public const int EarlyBonusPerDay = 2;     // only if the due date was never changed
    public const int PenaltyBase = 10;
    public const int PenaltyPerStep = 2;

    /// <param name="stepCount">Number of steps (task complexity).</param>
    /// <param name="windowDays">Days between creating the task and its due date.</param>
    /// <param name="daysEarly">Days between completion and the due date.</param>
    /// <param name="bonusEligible">False if the due date was edited.</param>
    public static int CalculateReward(int stepCount, int windowDays, int daysEarly, bool bonusEligible)
    {
        var points = BasePoints + stepCount * PointsPerStep + Math.Max(0, windowDays) * PointsPerWindowDay;
        if (bonusEligible) points += Math.Max(0, daysEarly) * EarlyBonusPerDay;
        return points;
    }

    /// <summary>Points lost for a task not completed by its due date (returned as a positive number).</summary>
    public static int CalculatePenalty(int stepCount) => PenaltyBase + stepCount * PenaltyPerStep;
}
