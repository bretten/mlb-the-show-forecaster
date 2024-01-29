using com.brettnamba.MlbTheShowForecaster.Performance.Domain.PlayerSeasons.ValueObjects;

namespace com.brettnamba.MlbTheShowForecaster.Performance.Domain.Tests.PlayerSeasons.ValueObjects;

public class HitsPerNineTests
{
    [Fact]
    public void Value_PitchingStats_ReturnsCalculatedValue()
    {
        // Arrange
        const uint strikeouts = 85;
        const decimal inningsPitched = 132;
        var hitsPerNine = HitsPerNine.Create(strikeouts, inningsPitched);

        // Act
        var actual = hitsPerNine.Value;

        // Assert
        Assert.Equal(5.80m, Math.Round(actual, 2, MidpointRounding.AwayFromZero));
    }
}