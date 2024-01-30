using com.brettnamba.MlbTheShowForecaster.Common.Domain.ValueObjects;
using com.brettnamba.MlbTheShowForecaster.Performance.Domain.Statistics.ValueObjects.Batting;

namespace com.brettnamba.MlbTheShowForecaster.Performance.Domain.Tests.PlayerSeasons.ValueObjects.Batting;

public class TotalBasesTests
{
    [Fact]
    public void Create_SinglesDoublesTriplesHomeRuns_Created()
    {
        // Arrange
        var singles = NaturalNumber.Create(73);
        var doubles = NaturalNumber.Create(26);
        var triples = NaturalNumber.Create(8);
        var homeRuns = NaturalNumber.Create(44);

        // Act
        var actual = TotalBases.Create(singles, doubles, triples, homeRuns);

        // Assert
        Assert.Equal(325, actual.AsRounded(0));
        Assert.Equal(73U, actual.Singles.Value);
        Assert.Equal(26U, actual.Doubles.Value);
        Assert.Equal(8U, actual.Triples.Value);
        Assert.Equal(44U, actual.HomeRuns.Value);
    }

    [Fact]
    public void Create_HitsDoublesTriplesHomeRuns_Created()
    {
        // Arrange
        var hits = NaturalNumber.Create(151);
        var doubles = NaturalNumber.Create(26);
        var triples = NaturalNumber.Create(8);
        var homeRuns = NaturalNumber.Create(44);

        // Act
        var actual = TotalBases.CreateWithHits(hits, doubles, triples, homeRuns);

        // Assert
        Assert.Equal(325, actual.AsRounded(0));
        Assert.Equal(73U, actual.Singles.Value);
        Assert.Equal(26U, actual.Doubles.Value);
        Assert.Equal(8U, actual.Triples.Value);
        Assert.Equal(44U, actual.HomeRuns.Value);
    }
}