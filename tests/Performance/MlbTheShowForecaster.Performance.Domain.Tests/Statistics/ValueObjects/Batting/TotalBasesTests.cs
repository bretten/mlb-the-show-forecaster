using com.brettnamba.MlbTheShowForecaster.Performance.Domain.Statistics.ValueObjects.Batting;

namespace com.brettnamba.MlbTheShowForecaster.Performance.Domain.Tests.Statistics.ValueObjects.Batting;

public class TotalBasesTests
{
    [Fact]
    public void Value_SinglesDoublesTriplesHomeRuns_ReturnsCalculatedValue()
    {
        // Arrange
        const int singles = 73;
        const int doubles = 26;
        const int triples = 8;
        const int homeRuns = 44;
        var totalBases = TotalBases.Create(singles, doubles, triples, homeRuns);

        // Act
        var actual = totalBases.Value;

        // Assert
        Assert.Equal(325, actual);
        Assert.Equal(73, totalBases.Singles.Value);
        Assert.Equal(26, totalBases.Doubles.Value);
        Assert.Equal(8, totalBases.Triples.Value);
        Assert.Equal(44, totalBases.HomeRuns.Value);
    }

    [Fact]
    public void Value_HitsDoublesTriplesHomeRuns_ReturnsCalculatedValue()
    {
        // Arrange
        const int hits = 151;
        const int doubles = 26;
        const int triples = 8;
        const int homeRuns = 44;
        var totalBases = TotalBases.CreateWithHits(hits, doubles, triples, homeRuns);

        // Act
        var actual = totalBases.Value;

        // Assert
        Assert.Equal(325, actual);
        Assert.Equal(73, totalBases.Singles.Value);
        Assert.Equal(26, totalBases.Doubles.Value);
        Assert.Equal(8, totalBases.Triples.Value);
        Assert.Equal(44, totalBases.HomeRuns.Value);
    }
}