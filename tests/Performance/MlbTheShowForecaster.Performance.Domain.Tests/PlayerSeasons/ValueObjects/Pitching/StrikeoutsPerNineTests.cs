using com.brettnamba.MlbTheShowForecaster.Performance.Domain.Statistics.ValueObjects.Pitching;

namespace com.brettnamba.MlbTheShowForecaster.Performance.Domain.Tests.PlayerSeasons.ValueObjects.Pitching;

public class StrikeoutsPerNineTests
{
    [Fact]
    public void Value_PitchingStats_ReturnsCalculatedValue()
    {
        // Arrange
        const uint strikeouts = 167;
        const decimal inningsPitched = 132;
        var strikeoutsPerNine = StrikeoutsPerNine.Create(strikeouts, inningsPitched);

        // Act
        var actual = strikeoutsPerNine.Value;

        // Assert
        Assert.Equal(11.39m, Math.Round(actual, 2, MidpointRounding.AwayFromZero));
    }
}