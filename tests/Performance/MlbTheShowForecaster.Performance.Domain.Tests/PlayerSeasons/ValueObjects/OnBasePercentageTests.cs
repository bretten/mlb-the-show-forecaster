using com.brettnamba.MlbTheShowForecaster.Common.Domain.ValueObjects;
using com.brettnamba.MlbTheShowForecaster.Performance.Domain.PlayerSeasons.ValueObjects;

namespace com.brettnamba.MlbTheShowForecaster.Performance.Domain.Tests.PlayerSeasons.ValueObjects;

public class OnBasePercentageTests
{
    [Fact]
    public void Create_HitsWalksHitByPitchesAtBatsSacFlies_Created()
    {
        // Arrange
        var hits = NaturalNumber.Create(151);
        var baseOnBalls = NaturalNumber.Create(91);
        var hitByPitches = NaturalNumber.Create(3);
        var atBats = NaturalNumber.Create(497);
        var sacFlies = NaturalNumber.Create(3);

        // Act
        var actual = OnBasePercentage.Create(hits, baseOnBalls, hitByPitches, atBats, sacFlies);

        // Assert
        Assert.Equal(0.412m, actual.AsRounded(3));
        Assert.Equal(151U, actual.Hits.Value);
        Assert.Equal(91U, actual.BaseOnBalls.Value);
        Assert.Equal(3U, actual.HitByPitches.Value);
        Assert.Equal(497U, actual.AtBats.Value);
        Assert.Equal(3U, actual.SacrificeFlies.Value);
    }
}