using com.brettnamba.MlbTheShowForecaster.Performance.Domain.Statistics.ValueObjects.Pitching;

namespace com.brettnamba.MlbTheShowForecaster.Performance.Domain.Tests.Statistics.ValueObjects.Pitching;

public class StrikeoutToWalkRatioTests
{
    [Fact]
    public void Value_StrikeoutsWalks_ReturnsCalculatedValue()
    {
        // Arrange
        const int strikeouts = 167;
        const int baseOnBalls = 55;
        var strikeoutToWalkRatio = StrikeoutToWalkRatio.Create(strikeouts, baseOnBalls);

        // Act
        var actual = strikeoutToWalkRatio.Value;

        // Assert
        Assert.Equal(3.036m, actual);
        Assert.Equal(167, strikeoutToWalkRatio.Strikeouts.Value);
        Assert.Equal(55, strikeoutToWalkRatio.BaseOnBalls.Value);
    }
}