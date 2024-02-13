using com.brettnamba.MlbTheShowForecaster.Performance.Domain.Statistics.ValueObjects.Batting;

namespace com.brettnamba.MlbTheShowForecaster.Performance.Domain.Tests.Statistics.ValueObjects.Batting;

public class SluggingTests
{
    [Fact]
    public void Value_TotalBasesAtBats_ReturnsCalculatedValue()
    {
        // Arrange
        const int singles = 73;
        const int doubles = 26;
        const int triples = 8;
        const int homeRuns = 44;
        var totalBases = TotalBases.Create(singles, doubles, triples, homeRuns);

        const int atBats = 497;

        var slugging = Slugging.Create(totalBases, atBats);

        // Act
        var actual = slugging.Value;

        // Assert
        Assert.Equal(0.654m, actual);
        Assert.Equal(325, slugging.TotalBases.Value);
        Assert.Equal(497, slugging.AtBats.Value);
    }
}