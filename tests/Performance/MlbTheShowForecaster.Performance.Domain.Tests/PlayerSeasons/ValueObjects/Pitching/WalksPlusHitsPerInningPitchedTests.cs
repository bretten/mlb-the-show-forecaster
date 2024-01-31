using com.brettnamba.MlbTheShowForecaster.Performance.Domain.Statistics.ValueObjects.Pitching;

namespace com.brettnamba.MlbTheShowForecaster.Performance.Domain.Tests.PlayerSeasons.ValueObjects.Pitching;

public class WalksPlusHitsPerInningPitchedTests
{
    [Fact]
    public void Value_WalksHitsInnings_ReturnsCalculatedValue()
    {
        // Arrange
        const uint baseOnBalls = 55;
        const uint hits = 85;
        const decimal inningsPitched = 132;
        var walksPlusHitsPerInningPitched = WalksPlusHitsPerInningPitched.Create(baseOnBalls, hits, inningsPitched);

        // Act
        var actual = walksPlusHitsPerInningPitched.Value;

        // Assert
        Assert.Equal(1.061m, actual);
        Assert.Equal(55U, walksPlusHitsPerInningPitched.BaseOnBalls.Value);
        Assert.Equal(85U, walksPlusHitsPerInningPitched.Hits.Value);
        Assert.Equal(132U, walksPlusHitsPerInningPitched.InningsPitched.Value);
    }
}