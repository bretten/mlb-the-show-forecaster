using com.brettnamba.MlbTheShowForecaster.Performance.Domain.Statistics.ValueObjects.Batting;

namespace com.brettnamba.MlbTheShowForecaster.Performance.Domain.Tests.Statistics.ValueObjects.Batting;

public class OnBasePlusSluggingTests
{
    [Fact]
    public void Value_OnBaseSlugging_ReturnsCalculatedValue()
    {
        // Arrange
        const int hits = 151;
        const int baseOnBalls = 91;
        const int hitByPitches = 3;
        const int atBats = 497;
        const int sacrificeFlies = 3;
        var onBasePercentage = OnBasePercentage.Create(hits, baseOnBalls, hitByPitches, atBats, sacrificeFlies);

        const int singles = 73;
        const int doubles = 26;
        const int triples = 8;
        const int homeRuns = 44;
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