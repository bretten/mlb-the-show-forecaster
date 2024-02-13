using com.brettnamba.MlbTheShowForecaster.Performance.Domain.Statistics.ValueObjects.Batting;

namespace com.brettnamba.MlbTheShowForecaster.Performance.Domain.Tests.Statistics.ValueObjects.Batting;

public class BattingAverageOnBallsInPlayTests
{
    [Fact]
    public void Value_HitsHomeRunsAtBatsStrikeOutSacFlies_ReturnsCalculatedValue()
    {
        // Arrange
        const int hits = 151;
        const int homeRuns = 44;
        const int atBats = 497;
        const int strikeouts = 143;
        const int sacrificeFlies = 3;
        var battingAverageOnBallsInPlay =
            BattingAverageOnBallsInPlay.Create(hits, homeRuns, atBats, strikeouts, sacrificeFlies);

        // Act
        var actual = battingAverageOnBallsInPlay.Value;

        // Assert
        Assert.Equal(0.342m, actual);
        Assert.Equal(151, battingAverageOnBallsInPlay.Hits.Value);
        Assert.Equal(44, battingAverageOnBallsInPlay.HomeRuns.Value);
        Assert.Equal(497, battingAverageOnBallsInPlay.AtBats.Value);
        Assert.Equal(143, battingAverageOnBallsInPlay.StrikeOuts.Value);
        Assert.Equal(3, battingAverageOnBallsInPlay.SacrificeFlies.Value);
    }
}