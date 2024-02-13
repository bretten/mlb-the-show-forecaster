using com.brettnamba.MlbTheShowForecaster.Performance.Domain.Statistics.ValueObjects.Batting;

namespace com.brettnamba.MlbTheShowForecaster.Performance.Domain.Tests.Statistics.ValueObjects.Batting;

public class OnBasePercentageTests
{
    [Fact]
    public void Value_HitsWalksHitByPitchesAtBatsSacFlies_ReturnsCalculatedValue()
    {
        // Arrange
        const int hits = 151;
        const int baseOnBalls = 91;
        const int hitByPitches = 3;
        const int atBats = 497;
        const int sacrificeFlies = 3;
        var onBasePercentage = OnBasePercentage.Create(hits, baseOnBalls, hitByPitches, atBats, sacrificeFlies);

        // Act
        var actual = onBasePercentage.Value;

        // Assert
        Assert.Equal(0.412m, actual);
        Assert.Equal(151, onBasePercentage.Hits.Value);
        Assert.Equal(91, onBasePercentage.BaseOnBalls.Value);
        Assert.Equal(3, onBasePercentage.HitByPitches.Value);
        Assert.Equal(497, onBasePercentage.AtBats.Value);
        Assert.Equal(3, onBasePercentage.SacrificeFlies.Value);
    }
}