using com.brettnamba.MlbTheShowForecaster.Performance.Domain.Statistics.ValueObjects.Pitching;

namespace com.brettnamba.MlbTheShowForecaster.Performance.Domain.Tests.PlayerSeasons.ValueObjects.Pitching;

public class OpponentsBattingAverageTests
{
    [Fact]
    public void Value_ObaStats_ReturnsCalculatedValue()
    {
        // Arrange
        const uint hits = 85;
        const uint battersFaced = 531;
        const uint baseOnBalls = 55;
        const uint hitBatsmen = 11;
        const uint sacrificeHits = 0;
        const uint sacrificeFlies = 1;
        const uint catchersInterferences = 1;

        // Act
        var actual = OpponentsBattingAverage.Create(hits, battersFaced, baseOnBalls, hitBatsmen,
            sacrificeHits, sacrificeFlies, catchersInterferences);

        // Assert
        Assert.Equal(0.184m, actual.AsRounded(3));
        Assert.Equal(hits, actual.Hits.Value);
        Assert.Equal(battersFaced, actual.BattersFaced.Value);
        Assert.Equal(baseOnBalls, actual.BaseOnBalls.Value);
        Assert.Equal(hitBatsmen, actual.HitBatsmen.Value);
        Assert.Equal(sacrificeHits, actual.SacrificeHits.Value);
        Assert.Equal(sacrificeFlies, actual.SacrificeFlies.Value);
        Assert.Equal(catchersInterferences, actual.CatchersInterferences.Value);
    }
}