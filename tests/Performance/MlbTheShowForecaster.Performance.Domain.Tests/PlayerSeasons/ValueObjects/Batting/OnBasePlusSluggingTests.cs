using com.brettnamba.MlbTheShowForecaster.Common.Domain.ValueObjects;
using com.brettnamba.MlbTheShowForecaster.Performance.Domain.Statistics.ValueObjects.Batting;

namespace com.brettnamba.MlbTheShowForecaster.Performance.Domain.Tests.PlayerSeasons.ValueObjects.Batting;

public class OnBasePlusSluggingTests
{
    [Fact]
    public void Create_OnBaseSlugging_Created()
    {
        // Arrange
        var hits = NaturalNumber.Create(151);
        var baseOnBalls = NaturalNumber.Create(91);
        var hitByPitches = NaturalNumber.Create(3);
        var atBats = NaturalNumber.Create(497);
        var sacFlies = NaturalNumber.Create(3);
        var obp = OnBasePercentage.Create(hits, baseOnBalls, hitByPitches, atBats, sacFlies);

        var singles = NaturalNumber.Create(73);
        var doubles = NaturalNumber.Create(26);
        var triples = NaturalNumber.Create(8);
        var homeRuns = NaturalNumber.Create(44);
        var totalBases = TotalBases.Create(singles, doubles, triples, homeRuns);
        var slg = Slugging.Create(totalBases, atBats);

        // Act
        var actual = OnBasePlusSlugging.Create(obp, slg);

        // Assert
        Assert.Equal(1.066m, actual.AsRounded(3));
        Assert.Equal(0.412m, actual.OnBasePercentage.AsRounded(3));
        Assert.Equal(0.654m, actual.Slugging.AsRounded(3));
    }
}