using com.brettnamba.MlbTheShowForecaster.Performance.Domain.Statistics.ValueObjects.Batting;

namespace com.brettnamba.MlbTheShowForecaster.Performance.Domain.Tests.Statistics.ValueObjects.Batting;

public class OnBasePlusSluggingTests
{
    [Fact]
    public void Value_OnBaseSlugging_ReturnsCalculatedValue()
    {
        // Arrange
        const uint hits = 151;
        const uint baseOnBalls = 91;
        const uint hitByPitches = 3;
        const uint atBats = 497;
        const uint sacrificeFlies = 3;
        var onBasePercentage = OnBasePercentage.Create(hits, baseOnBalls, hitByPitches, atBats, sacrificeFlies);

        const uint singles = 73;
        const uint doubles = 26;
        const uint triples = 8;
        const uint homeRuns = 44;
        var totalBases = TotalBases.Create(singles, doubles, triples, homeRuns);
        var slugging = Slugging.Create(totalBases, atBats);

        var onBasePlusSlugging = OnBasePlusSlugging.Create(onBasePercentage, slugging);

        // Act
        var actual = onBasePlusSlugging.Value;

        // Assert
        Assert.Equal(1.066m, actual);
        Assert.Equal(0.412m, onBasePlusSlugging.OnBasePercentage.Value);
        Assert.Equal(0.654m, onBasePlusSlugging.Slugging.Value);
    }
}