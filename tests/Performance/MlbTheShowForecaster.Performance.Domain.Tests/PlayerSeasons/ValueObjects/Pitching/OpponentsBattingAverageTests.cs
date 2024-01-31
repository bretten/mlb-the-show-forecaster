using com.brettnamba.MlbTheShowForecaster.Performance.Domain.Statistics.ValueObjects.Pitching;

namespace com.brettnamba.MlbTheShowForecaster.Performance.Domain.Tests.PlayerSeasons.ValueObjects.Pitching;

public class OpponentsBattingAverageTests
{
    [Fact]
    public void Value_HitsBattersWalksSacrificesInterferences_ReturnsCalculatedValue()
    {
        // Arrange
        const uint hits = 85;
        const uint battersFaced = 531;
        const uint baseOnBalls = 55;
        const uint hitBatsmen = 11;
        const uint sacrificeHits = 0;
        const uint sacrificeFlies = 1;
        const uint catchersInterferences = 1;
        var opponentsBattingAverage = OpponentsBattingAverage.Create(hits, battersFaced, baseOnBalls, hitBatsmen,
            sacrificeHits, sacrificeFlies, catchersInterferences);

        // Act
        var actual = opponentsBattingAverage.Value;

        // Assert
        Assert.Equal(0.184m, actual);
        Assert.Equal(hits, opponentsBattingAverage.Hits.Value);
        Assert.Equal(battersFaced, opponentsBattingAverage.BattersFaced.Value);
        Assert.Equal(baseOnBalls, opponentsBattingAverage.BaseOnBalls.Value);
        Assert.Equal(hitBatsmen, opponentsBattingAverage.HitBatsmen.Value);
        Assert.Equal(sacrificeHits, opponentsBattingAverage.SacrificeHits.Value);
        Assert.Equal(sacrificeFlies, opponentsBattingAverage.SacrificeFlies.Value);
        Assert.Equal(catchersInterferences, opponentsBattingAverage.CatchersInterferences.Value);
    }
}