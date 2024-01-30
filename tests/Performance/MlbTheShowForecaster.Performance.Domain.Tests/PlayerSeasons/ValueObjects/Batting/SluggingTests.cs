using com.brettnamba.MlbTheShowForecaster.Common.Domain.ValueObjects;
using com.brettnamba.MlbTheShowForecaster.Performance.Domain.Statistics.ValueObjects.Batting;

namespace com.brettnamba.MlbTheShowForecaster.Performance.Domain.Tests.PlayerSeasons.ValueObjects.Batting;

public class SluggingTests
{
    [Fact]
    public void Create_TotalBasesAtBats_Created()
    {
        // Arrange
        var singles = NaturalNumber.Create(73);
        var doubles = NaturalNumber.Create(26);
        var triples = NaturalNumber.Create(8);
        var homeRuns = NaturalNumber.Create(44);
        var totalBases = TotalBases.Create(singles, doubles, triples, homeRuns);

        var atBats = NaturalNumber.Create(497);

        // Act
        var actual = Slugging.Create(totalBases, atBats);

        // Assert
        Assert.Equal(0.654m, actual.AsRounded(3));
        Assert.Equal(totalBases, actual.TotalBases);
        Assert.Equal(497U, actual.AtBats.Value);
    }
}