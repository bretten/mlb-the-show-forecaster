using com.brettnamba.MlbTheShowForecaster.Performance.Domain.Statistics.ValueObjects.Batting;

namespace com.brettnamba.MlbTheShowForecaster.Performance.Domain.Tests.Statistics.ValueObjects.Batting;

public class BattingAverageTests
{
    [Fact]
    public void Value_HitsAndAtBats_ReturnsCalculatedValue()
    {
        // Arrange
        const uint hits = 151;
        const uint atBats = 497;
        var battingAverage = BattingAverage.Create(hits, atBats);

        // Act
        var actual = battingAverage.Value;

        // Assert
        Assert.Equal(0.304m, actual);
        Assert.Equal(151U, battingAverage.Hits.Value);
        Assert.Equal(497U, battingAverage.AtBats.Value);
    }
}