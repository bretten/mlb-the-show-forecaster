using com.brettnamba.MlbTheShowForecaster.Performance.Domain.Statistics.ValueObjects.Batting;

namespace com.brettnamba.MlbTheShowForecaster.Performance.Domain.Tests.Statistics.ValueObjects.Batting;

public class BattingAverageOnBallsInPlayTests
{
    [Fact]
    public void Value_HitsHomeRunsAtBatsStrikeOutSacFlies_ReturnsCalculatedValue()
    {
        // Arrange
        const uint hits = 151;
        const uint homeRuns = 44;
        const uint atBats = 497;
        const uint strikeOuts = 143;
        const uint sacrificeFlies = 3;
        var battingAverageOnBallsInPlay =
            BattingAverageOnBallsInPlay.Create(hits, homeRuns, atBats, strikeOuts, sacrificeFlies);

        // Act
        var actual = battingAverageOnBallsInPlay.Value;

        // Assert
        Assert.Equal(0.342m, actual);
        Assert.Equal(151U, battingAverageOnBallsInPlay.Hits.Value);
        Assert.Equal(44U, battingAverageOnBallsInPlay.HomeRuns.Value);
        Assert.Equal(497U, battingAverageOnBallsInPlay.AtBats.Value);
        Assert.Equal(143U, battingAverageOnBallsInPlay.StrikeOuts.Value);
        Assert.Equal(3U, battingAverageOnBallsInPlay.SacrificeFlies.Value);
    }
}