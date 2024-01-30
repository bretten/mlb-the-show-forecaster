using com.brettnamba.MlbTheShowForecaster.Performance.Domain.Statistics.ValueObjects.Pitching;

namespace com.brettnamba.MlbTheShowForecaster.Performance.Domain.Tests.PlayerSeasons.ValueObjects.Pitching;

public class PitchesPerInningTests
{
    [Fact]
    public void Value_PitchingStats_ReturnsCalculatedValue()
    {
        // Arrange
        const uint numberOfPitches = 1993;
        const decimal inningsPitched = 117 + (decimal)2 / 3;
        var pitchesPerInning = PitchesPerInning.Create(numberOfPitches, inningsPitched);

        // Act
        var actual = pitchesPerInning.Value;

        // Assert
        Assert.Equal(16.94m, Math.Round(actual, 2, MidpointRounding.AwayFromZero));
    }
}