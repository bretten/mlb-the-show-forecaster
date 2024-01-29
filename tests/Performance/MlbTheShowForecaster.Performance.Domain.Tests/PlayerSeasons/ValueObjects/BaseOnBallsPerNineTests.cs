using com.brettnamba.MlbTheShowForecaster.Performance.Domain.PlayerSeasons.ValueObjects;

namespace com.brettnamba.MlbTheShowForecaster.Performance.Domain.Tests.PlayerSeasons.ValueObjects;

public class BaseOnBallsPerNineTests
{
    [Fact]
    public void Value_PitchingStats_ReturnsCalculatedValue()
    {
        // Arrange
        const uint strikeouts = 55;
        const decimal inningsPitched = 132;
        var baseOnBallsPerNine = BaseOnBallsPerNine.Create(strikeouts, inningsPitched);

        // Act
        var actual = baseOnBallsPerNine.Value;

        // Assert
        Assert.Equal(3.75m, Math.Round(actual, 2, MidpointRounding.AwayFromZero));
    }
}