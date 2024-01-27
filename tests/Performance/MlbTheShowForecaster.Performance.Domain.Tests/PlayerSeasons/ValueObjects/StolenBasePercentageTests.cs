using com.brettnamba.MlbTheShowForecaster.Common.Domain.ValueObjects;
using com.brettnamba.MlbTheShowForecaster.Performance.Domain.PlayerSeasons.ValueObjects;

namespace com.brettnamba.MlbTheShowForecaster.Performance.Domain.Tests.PlayerSeasons.ValueObjects;

public class StolenBasePercentageTests
{
    [Fact]
    public void Create_StolenBasesCaughtStealing_Created()
    {
        // Arrange
        var stolenBases = NaturalNumber.Create(20);
        var caughtStealing = NaturalNumber.Create(6);

        // Act
        var actual = StolenBasePercentage.Create(stolenBases, caughtStealing);

        // Assert
        Assert.Equal(0.769m, actual.AsRounded(3));
        Assert.Equal(20U, actual.StolenBases.Value);
        Assert.Equal(6U, actual.CaughtStealing.Value);
    }
}