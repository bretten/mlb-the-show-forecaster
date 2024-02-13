using com.brettnamba.MlbTheShowForecaster.Performance.Domain.Statistics.ValueObjects.Pitching;

namespace com.brettnamba.MlbTheShowForecaster.Performance.Domain.Tests.Statistics.ValueObjects.Pitching;

public class WalksPlusHitsPerInningPitchedTests
{
    [Fact]
    public void Value_WalksHitsInnings_ReturnsCalculatedValue()
    {
        // Arrange
        const int baseOnBalls = 55;
        const int hits = 85;
        const decimal inningsPitched = 132;
        var walksPlusHitsPerInningPitched = WalksPlusHitsPerInningPitched.Create(baseOnBalls, hits, inningsPitched);

        // Act
        var actual = walksPlusHitsPerInningPitched.Value;

        // Assert
        Assert.Equal(1.061m, actual);
        Assert.Equal(55, walksPlusHitsPerInningPitched.BaseOnBalls.Value);
        Assert.Equal(85, walksPlusHitsPerInningPitched.Hits.Value);
        Assert.Equal(132, walksPlusHitsPerInningPitched.InningsPitched.Value);
    }
}