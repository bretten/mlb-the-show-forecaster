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

    [Theory]
    [InlineData(PitchingStat.Losses)]
    [InlineData(PitchingStat.BlownSaves)]
    [InlineData(PitchingStat.Hits)]
    [InlineData(PitchingStat.Doubles)]
    [InlineData(PitchingStat.Triples)]
    [InlineData(PitchingStat.HomeRuns)]
    [InlineData(PitchingStat.Runs)]
    [InlineData(PitchingStat.EarnedRuns)]
    [InlineData(PitchingStat.BaseOnBalls)]
    [InlineData(PitchingStat.IntentionalWalks)]
    [InlineData(PitchingStat.HitBatsmen)]
    [InlineData(PitchingStat.WildPitches)]
    [InlineData(PitchingStat.NumberOfPitches)]
    [InlineData(PitchingStat.Balks)]
    [InlineData(PitchingStat.StolenBases)]
    [InlineData(PitchingStat.InheritedRunnersScored)]
    [InlineData(PitchingStat.CatcherInterferences)]
    [InlineData(PitchingStat.SacrificeBunts)]
    [InlineData(PitchingStat.SacrificeFlies)]
    [InlineData(PitchingStat.EarnedRunAverage)]
    [InlineData(PitchingStat.OpponentsBattingAverage)]
    [InlineData(PitchingStat.OpponentsOnBasePercentage)]
    [InlineData(PitchingStat.TotalBases)]
    [InlineData(PitchingStat.Slugging)]
    [InlineData(PitchingStat.OpponentsOnBasePlusSlugging)]
    [InlineData(PitchingStat.PitchesPerInning)]
    [InlineData(PitchingStat.WalksPlusHitsPerInningPitched)]
    [InlineData(PitchingStat.HitsPer9)]
    [InlineData(PitchingStat.StrikeoutsPer9)]
    [InlineData(PitchingStat.BaseOnBallsPer9)]
    [InlineData(PitchingStat.RunsScoredPer9)]
    [InlineData(PitchingStat.HomeRunsPer9)]
    [InlineData(PitchingStat.StolenBasePercentage)]
    public void IsLowerStatBetter_PitchingStatsThatAreBetterWhenLower_ReturnsTrue(PitchingStat stat)
    {
        // Arrange

        // Act
        var actual = stat.IsLowerStatBetter();

        // Assert
        Assert.True(actual);
    }

    [Theory]
    [InlineData(PitchingStat.Wins)]
    [InlineData(PitchingStat.GamesStarted)]
    [InlineData(PitchingStat.GamesFinished)]
    [InlineData(PitchingStat.CompleteGames)]
    [InlineData(PitchingStat.Shutouts)]
    [InlineData(PitchingStat.Holds)]
    [InlineData(PitchingStat.Saves)]
    [InlineData(PitchingStat.SaveOpportunities)]
    [InlineData(PitchingStat.InningsPitched)]
    [InlineData(PitchingStat.Strikeouts)]
    [InlineData(PitchingStat.Outs)]
    [InlineData(PitchingStat.GroundOuts)]
    [InlineData(PitchingStat.AirOuts)]
    [InlineData(PitchingStat.GroundIntoDoublePlays)]
    [InlineData(PitchingStat.Strikes)]
    [InlineData(PitchingStat.BattersFaced)]
    [InlineData(PitchingStat.AtBats)]
    [InlineData(PitchingStat.CaughtStealing)]
    [InlineData(PitchingStat.Pickoffs)]
    [InlineData(PitchingStat.InheritedRunners)]
    [InlineData(PitchingStat.QualityStart)]
    [InlineData(PitchingStat.StrikePercentage)]
    [InlineData(PitchingStat.StrikeoutToWalkRatio)]
    public void IsLowerStatBetter_PitchingStatsThatAreBetterWhenHigher_ReturnsFalse(PitchingStat stat)
    {
        // Arrange

        // Act
        var actual = stat.IsLowerStatBetter();

        // Assert
        Assert.False(actual);
    }

    [Theory]
    [InlineData(FieldingStat.Errors)]
    [InlineData(FieldingStat.ThrowingErrors)]
    [InlineData(FieldingStat.StolenBases)]
    [InlineData(FieldingStat.PassedBalls)]
    [InlineData(FieldingStat.CatcherInterferences)]
    [InlineData(FieldingStat.WildPitches)]
    [InlineData(FieldingStat.StolenBasePercentage)]
    public void IsLowerStatBetter_FieldingStatsThatAreBetterWhenLower_ReturnsTrue(FieldingStat stat)
    {
        // Arrange

        // Act
        var actual = stat.IsLowerStatBetter();

        // Assert
        Assert.True(actual);
    }

    [Theory]
    [InlineData(FieldingStat.GamesStarted)]
    [InlineData(FieldingStat.InningsPlayed)]
    [InlineData(FieldingStat.Assists)]
    [InlineData(FieldingStat.Putouts)]
    [InlineData(FieldingStat.DoublePlays)]
    [InlineData(FieldingStat.TriplePlays)]
    [InlineData(FieldingStat.FieldingPercentage)]
    [InlineData(FieldingStat.TotalChances)]
    [InlineData(FieldingStat.RangeFactorPer9)]
    [InlineData(FieldingStat.CaughtStealing)]
    [InlineData(FieldingStat.Pickoffs)]
    public void IsLowerStatBetter_FieldingStatsThatAreBetterWhenHigher_ReturnsFalse(FieldingStat stat)
    {
        // Arrange

        // Act
        var actual = stat.IsLowerStatBetter();

        // Assert
        Assert.False(actual);
    }
}