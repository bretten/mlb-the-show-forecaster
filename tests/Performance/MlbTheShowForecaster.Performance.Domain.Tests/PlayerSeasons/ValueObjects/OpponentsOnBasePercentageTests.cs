using com.brettnamba.MlbTheShowForecaster.Performance.Domain.PlayerSeasons.ValueObjects;

namespace com.brettnamba.MlbTheShowForecaster.Performance.Domain.Tests.PlayerSeasons.ValueObjects;

public class OpponentsOnBasePercentageTests
{
    [Fact]
    public void Value_PitchingStats_ReturnsCalculatedValue()
    {
        // Arrange
        const uint hits = 85;
        const uint baseOnBalls = 55;
        const uint hitBatsmen = 11;
        const uint atBats = 463;
        const uint sacrificeFlies = 1;
        var opponentsOnBasePercentage =
            OpponentsOnBasePercentage.Create(hits, baseOnBalls, hitBatsmen, atBats, sacrificeFlies);

        // Act
        var actual = opponentsOnBasePercentage.Value;

        // Assert
        Assert.Equal(0.285m, Math.Round(actual, 3, MidpointRounding.AwayFromZero));
    }
}