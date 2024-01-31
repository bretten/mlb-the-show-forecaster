using com.brettnamba.MlbTheShowForecaster.Performance.Domain.Statistics.ValueObjects.Batting;

namespace com.brettnamba.MlbTheShowForecaster.Performance.Domain.Tests.PlayerSeasons.ValueObjects.Batting;

public class OnBasePercentageTests
{
    [Fact]
    public void Value_HitsWalksHitByPitchesAtBatsSacFlies_ReturnsCalculatedValue()
    {
        // Arrange
        const uint hits = 151;
        const uint baseOnBalls = 91;
        const uint hitByPitches = 3;
        const uint atBats = 497;
        const uint sacrificeFlies = 3;
        var onBasePercentage = OnBasePercentage.Create(hits, baseOnBalls, hitByPitches, atBats, sacrificeFlies);

        // Act
        var actual = onBasePercentage.Value;

        // Assert
        Assert.Equal(0.412m, actual);
        Assert.Equal(151U, onBasePercentage.Hits.Value);
        Assert.Equal(91U, onBasePercentage.BaseOnBalls.Value);
        Assert.Equal(3U, onBasePercentage.HitByPitches.Value);
        Assert.Equal(497U, onBasePercentage.AtBats.Value);
        Assert.Equal(3U, onBasePercentage.SacrificeFlies.Value);
    }
}