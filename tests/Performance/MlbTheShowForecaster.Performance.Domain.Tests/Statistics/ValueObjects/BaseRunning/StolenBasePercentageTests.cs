using com.brettnamba.MlbTheShowForecaster.Performance.Domain.Statistics.ValueObjects.BaseRunning;

namespace com.brettnamba.MlbTheShowForecaster.Performance.Domain.Tests.Statistics.ValueObjects.BaseRunning;

public class StolenBasePercentageTests
{
    [Fact]
    public void Value_StolenBasesCaughtStealing_ReturnsCalculatedValue()
    {
        // Arrange
        const uint stolenBases = 20;
        const uint caughtStealing = 6;
        var stolenBasePercentage = StolenBasePercentage.Create(stolenBases, caughtStealing);

        // Act
        var actual = stolenBasePercentage.Value;

        // Assert
        Assert.Equal(0.769m, actual);
        Assert.Equal(20U, stolenBasePercentage.StolenBases.Value);
        Assert.Equal(6U, stolenBasePercentage.CaughtStealing.Value);
    }
}