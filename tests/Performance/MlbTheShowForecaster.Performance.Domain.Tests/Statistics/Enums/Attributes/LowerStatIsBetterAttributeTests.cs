using com.brettnamba.MlbTheShowForecaster.Performance.Domain.Statistics.Enums;
using com.brettnamba.MlbTheShowForecaster.Performance.Domain.Statistics.Enums.Attributes;

namespace com.brettnamba.MlbTheShowForecaster.Performance.Domain.Tests.Statistics.Enums.Attributes;

public class LowerStatIsBetterAttributeTests
{
    [Theory]
    [InlineData(BattingStat.Strikeouts)]
    [InlineData(BattingStat.CaughtStealing)]
    [InlineData(BattingStat.LeftOnBase)]
    [InlineData(BattingStat.GroundOuts)]
    [InlineData(BattingStat.GroundIntoDoublePlays)]
    [InlineData(BattingStat.GroundIntoTriplePlays)]
    [InlineData(BattingStat.AirOuts)]
    public void IsLowerStatBetter_BattingStatsThatAreBetterWhenLower_ReturnsTrue(BattingStat stat)
    {
        // Arrange

        // Act
        var actual = stat.IsLowerStatBetter();

        // Assert
        Assert.True(actual);
    }

    [Theory]
    [InlineData(BattingStat.PlateAppearances)]
    [InlineData(BattingStat.AtBats)]
    [InlineData(BattingStat.Runs)]
    [InlineData(BattingStat.Hits)]
    [InlineData(BattingStat.Doubles)]
    [InlineData(BattingStat.Triples)]
    [InlineData(BattingStat.HomeRuns)]
    [InlineData(BattingStat.RunsBattedIn)]
    [InlineData(BattingStat.BaseOnBalls)]
    [InlineData(BattingStat.IntentionalWalks)]
    [InlineData(BattingStat.StolenBases)]
    [InlineData(BattingStat.HitByPitches)]
    [InlineData(BattingStat.SacrificeBunts)]
    [InlineData(BattingStat.SacrificeFlies)]
    [InlineData(BattingStat.NumberOfPitchesSeen)]
    [InlineData(BattingStat.CatcherInterferences)]
    [InlineData(BattingStat.BattingAverage)]
    [InlineData(BattingStat.OnBasePercentage)]
    [InlineData(BattingStat.BattingAverageOnBallsInPlay)]
    [InlineData(BattingStat.TotalBases)]
    [InlineData(BattingStat.Slugging)]
    [InlineData(BattingStat.OnBasePlusSlugging)]
    [InlineData(BattingStat.StolenBasePercentage)]
    public void IsLowerStatBetter_BattingStatsThatAreBetterWhenHigher_ReturnsFalse(BattingStat stat)
    {
        // Arrange

        // Act
        var actual = stat.IsLowerStatBetter();

        // Assert
        Assert.False(actual);
    }
}