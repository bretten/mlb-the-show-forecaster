using com.brettnamba.MlbTheShowForecaster.Common.Domain.ValueObjects;
using com.brettnamba.MlbTheShowForecaster.Performance.Domain.PlayerSeasons.ValueObjects;

namespace com.brettnamba.MlbTheShowForecaster.Performance.Domain.Tests.PlayerSeasons.ValueObjects;

public class BattingAverageOnBallsInPlayTests
{
    [Fact]
    public void Create_HitsHomeRunsAtBatsStrikeOutSacFlies_Created()
    {
        // Arrange
        var hits = NaturalNumber.Create(151);
        var homeRuns = NaturalNumber.Create(44);
        var atBats = NaturalNumber.Create(497);
        var strikeOuts = NaturalNumber.Create(143);
        var sacFlies = NaturalNumber.Create(3);

        // Act
        var actual = BattingAverageOnBallsInPlay.Create(hits, homeRuns, atBats, strikeOuts, sacFlies);

        // Assert
        Assert.Equal(0.342m, actual.AsRounded(3));
        Assert.Equal(151U, actual.Hits.Value);
        Assert.Equal(44U, actual.HomeRuns.Value);
        Assert.Equal(497U, actual.AtBats.Value);
        Assert.Equal(143U, actual.StrikeOuts.Value);
        Assert.Equal(3U, actual.SacrificeFlies.Value);
    }
}