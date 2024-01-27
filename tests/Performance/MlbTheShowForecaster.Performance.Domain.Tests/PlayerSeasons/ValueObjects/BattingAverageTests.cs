using com.brettnamba.MlbTheShowForecaster.Common.Domain.ValueObjects;
using com.brettnamba.MlbTheShowForecaster.Performance.Domain.PlayerSeasons.ValueObjects;

namespace com.brettnamba.MlbTheShowForecaster.Performance.Domain.Tests.PlayerSeasons.ValueObjects;

public class BattingAverageTests
{
    [Fact]
    public void Create_HitsAndAtBats_Created()
    {
        // Arrange
        var hits = NaturalNumber.Create(151);
        var atBats = NaturalNumber.Create(497);

        // Act
        var actual = BattingAverage.Create(hits, atBats);

        // Assert
        Assert.Equal(0.304m, actual.AsRounded(3));
        Assert.Equal(151U, actual.Hits.Value);
        Assert.Equal(497U, actual.AtBats.Value);
    }
}