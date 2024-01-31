using com.brettnamba.MlbTheShowForecaster.Performance.Domain.Statistics.ValueObjects.Batting;

namespace com.brettnamba.MlbTheShowForecaster.Performance.Domain.Tests.PlayerSeasons.ValueObjects.Batting;

public class TotalBasesTests
{
    [Fact]
    public void Value_SinglesDoublesTriplesHomeRuns_ReturnsCalculatedValue()
    {
        // Arrange
        const uint singles = 73;
        const uint doubles = 26;
        const uint triples = 8;
        const uint homeRuns = 44;
        var totalBases = TotalBases.Create(singles, doubles, triples, homeRuns);

        // Act
        var actual = totalBases.Value;

        // Assert
        Assert.Equal(325, actual);
        Assert.Equal(73U, totalBases.Singles.Value);
        Assert.Equal(26U, totalBases.Doubles.Value);
        Assert.Equal(8U, totalBases.Triples.Value);
        Assert.Equal(44U, totalBases.HomeRuns.Value);
    }

    [Fact]
    public void Value_HitsDoublesTriplesHomeRuns_ReturnsCalculatedValue()
    {
        // Arrange
        const uint hits = 151;
        const uint doubles = 26;
        const uint triples = 8;
        const uint homeRuns = 44;
        var totalBases = TotalBases.CreateWithHits(hits, doubles, triples, homeRuns);

        // Act
        var actual = totalBases.Value;

        // Assert
        Assert.Equal(325, actual);
        Assert.Equal(73U, totalBases.Singles.Value);
        Assert.Equal(26U, totalBases.Doubles.Value);
        Assert.Equal(8U, totalBases.Triples.Value);
        Assert.Equal(44U, totalBases.HomeRuns.Value);
    }
}