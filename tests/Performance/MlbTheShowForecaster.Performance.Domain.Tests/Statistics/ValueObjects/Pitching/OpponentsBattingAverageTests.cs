using com.brettnamba.MlbTheShowForecaster.Performance.Domain.Statistics.ValueObjects.Pitching;

namespace com.brettnamba.MlbTheShowForecaster.Performance.Domain.Tests.Statistics.ValueObjects.Pitching;

public class OpponentsBattingAverageTests
{
    [Fact]
    public void Value_HitsBattersWalksSacrificesInterferences_ReturnsCalculatedValue()
    {
        // Arrange
        const int hits = 85;
        const int battersFaced = 531;
        const int baseOnBalls = 55;
        const int hitBatsmen = 11;
        const int sacrificeHits = 0;
        const int sacrificeFlies = 1;
        const int catcherInterferences = 1;
        var opponentsBattingAverage = OpponentsBattingAverage.Create(hits, battersFaced, baseOnBalls, hitBatsmen,
            sacrificeHits, sacrificeFlies, catcherInterferences);

        // Act
        var actual = opponentsBattingAverage.Value;

        // Assert
        Assert.Equal(0.184m, actual);
        Assert.Equal(85, opponentsBattingAverage.Hits.Value);
        Assert.Equal(531, opponentsBattingAverage.BattersFaced.Value);
        Assert.Equal(55, opponentsBattingAverage.BaseOnBalls.Value);
        Assert.Equal(11, opponentsBattingAverage.HitBatsmen.Value);
        Assert.Equal(0, opponentsBattingAverage.SacrificeHits.Value);
        Assert.Equal(1, opponentsBattingAverage.SacrificeFlies.Value);
        Assert.Equal(1, opponentsBattingAverage.CatcherInterferences.Value);
    }
}