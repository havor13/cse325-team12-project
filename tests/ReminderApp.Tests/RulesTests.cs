using ReminderApp.Models;
using ReminderApp.Services;
using Xunit;

namespace ReminderApp.Tests;

public class RulesTests
{
    [Fact]
    public void Reward_IncludesEarlyBonus_WhenEligible()
    {
        var withBonus = PointsService.CalculateReward(stepCount: 3, windowDays: 7, daysEarly: 2, bonusEligible: true);
        var withoutBonus = PointsService.CalculateReward(stepCount: 3, windowDays: 7, daysEarly: 2, bonusEligible: false);
        Assert.Equal(withoutBonus + 2 * PointsService.EarlyBonusPerDay, withBonus);
    }

    [Fact]
    public void Reward_GrowsWithMoreSteps()
    {
        Assert.True(PointsService.CalculateReward(5, 7, 0, true) > PointsService.CalculateReward(2, 7, 0, true));
    }

    [Theory]
    [InlineData(0, 1)]
    [InlineData(49, 1)]
    [InlineData(50, 2)]
    [InlineData(1000, 6)]
    public void Level_MatchesThresholds(int points, int expectedLevel)
    {
        Assert.Equal(expectedLevel, LevelService.GetLevel(points).Number);
    }

    [Fact]
    public void StepPercent_SplitsEvenly()
    {
        var task = new TaskItem { Steps = { new TaskStep(), new TaskStep(), new TaskStep { IsDone = true } } };
        Assert.Equal(33.33, task.StepPercent, 2);
        Assert.Equal(33.33, task.ProgressPercent, 2);
    }
}
