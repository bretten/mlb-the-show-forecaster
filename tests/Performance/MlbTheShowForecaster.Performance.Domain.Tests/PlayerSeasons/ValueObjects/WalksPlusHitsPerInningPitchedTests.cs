using com.brettnamba.MlbTheShowForecaster.Performance.Domain.PlayerSeasons.ValueObjects;

namespace com.brettnamba.MlbTheShowForecaster.Performance.Domain.Tests.PlayerSeasons.ValueObjects;

public class WalksPlusHitsPerInningPitchedTests
{
    [Fact]
    public void Value_WhipStats_ReturnsCalculatedValue()
    {
        // Arrange
        const uint hits = 85;
        const uint baseOnBalls = 55;
        const uint inningsPitched = 132;
        var walksPlusHitsPerInningPitched = WalksPlusHitsPerInningPitched.Create(hits, baseOnBalls, inningsPitched);

        // Act
        var actual = walksPlusHitsPerInningPitched.Value;

        // Assert
        Assert.Equal(1.06m, Math.Round(actual, 2, MidpointRounding.AwayFromZero));
    }
}