using com.brettnamba.MlbTheShowForecaster.Performance.Domain.PlayerSeasons.ValueObjects;

namespace com.brettnamba.MlbTheShowForecaster.Performance.Domain.Tests.PlayerSeasons.ValueObjects;

public class StrikePercentageTests
{
    [Fact]
    public void Value_PitchingStats_ReturnsValue()
    {
        // Arrange
        const uint strikes = 1337;
        const uint numberOfPitches = 2094;
        var strikePercentage = StrikePercentage.Create(strikes, numberOfPitches);

        // Act
        var actual = strikePercentage.Value;

        // Assert
        Assert.Equal(0.638m, Math.Round(actual, 3, MidpointRounding.AwayFromZero));
    }
}