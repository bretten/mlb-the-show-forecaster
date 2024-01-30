using com.brettnamba.MlbTheShowForecaster.Performance.Domain.Statistics.ValueObjects.Pitching;

namespace com.brettnamba.MlbTheShowForecaster.Performance.Domain.Tests.PlayerSeasons.ValueObjects.Pitching;

public class StrikeoutToWalkRatioTests
{
    [Fact]
    public void Value_PitchingStats_ReturnsCalculatedValue()
    {
        // Arrange
        const uint strikeouts = 167;
        const uint baseOnBalls = 55;
        var strikeoutToWalkRatio = StrikeoutToWalkRatio.Create(strikeouts, baseOnBalls);

        // Act
        var actual = strikeoutToWalkRatio.Value;

        // Assert
        Assert.Equal(3.04m, Math.Round(actual, 2, MidpointRounding.AwayFromZero));
    }
}