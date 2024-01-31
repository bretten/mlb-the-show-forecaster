using com.brettnamba.MlbTheShowForecaster.Performance.Domain.Statistics.ValueObjects.Batting;

namespace com.brettnamba.MlbTheShowForecaster.Performance.Domain.Tests.PlayerSeasons.ValueObjects.Batting;

public class SluggingTests
{
    [Fact]
    public void Value_TotalBasesAtBats_ReturnsCalculatedValue()
    {
        // Arrange
        const uint singles = 73;
        const uint doubles = 26;
        const uint triples = 8;
        const uint homeRuns = 44;
        var totalBases = TotalBases.Create(singles, doubles, triples, homeRuns);

        const uint atBats = 497;

        var slugging = Slugging.Create(totalBases, atBats);

        // Act
        var actual = slugging.Value;

        // Assert
        Assert.Equal(0.654m, actual);
        Assert.Equal(325U, slugging.TotalBases.Value);
        Assert.Equal(497U, slugging.AtBats.Value);
    }
}