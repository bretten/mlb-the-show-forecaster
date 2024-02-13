using com.brettnamba.MlbTheShowForecaster.Performance.Domain.Statistics.ValueObjects.BaseRunning;

namespace com.brettnamba.MlbTheShowForecaster.Performance.Domain.Tests.Statistics.ValueObjects.BaseRunning;

public class StolenBasePercentageTests
{
    [Fact]
    public void Value_StolenBasesCaughtStealing_ReturnsCalculatedValue()
    {
        // Arrange
        const int stolenBases = 20;
        const int caughtStealing = 6;
        var stolenBasePercentage = StolenBasePercentage.Create(stolenBases, caughtStealing);

        // Act
        var actual = stolenBasePercentage.Value;

        // Assert
        Assert.Equal(0.769m, actual);
        Assert.Equal(20, stolenBasePercentage.StolenBases.Value);
        Assert.Equal(6, stolenBasePercentage.CaughtStealing.Value);
    }
}