using com.brettnamba.MlbTheShowForecaster.Performance.Domain.PlayerSeasons.ValueObjects;

namespace com.brettnamba.MlbTheShowForecaster.Performance.Domain.Tests.PlayerSeasons.ValueObjects;

public class HomeRunsPerNineTests
{
    [Fact]
    public void Value_PitchingStats_ReturnsCalculatedValue()
    {
        // Arrange
        const uint strikeouts = 18;
        const decimal inningsPitched = 132;
        var homeRunsPerNine = HomeRunsPerNine.Create(strikeouts, inningsPitched);

        // Act
        var actual = homeRunsPerNine.Value;

        // Assert
        Assert.Equal(1.23m, Math.Round(actual, 2, MidpointRounding.AwayFromZero));
    }
}