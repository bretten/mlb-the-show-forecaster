using com.brettnamba.MlbTheShowForecaster.Performance.Domain.Events.Improvement;
using com.brettnamba.MlbTheShowForecaster.Performance.Domain.Events.Improvement.Pitching;
using com.brettnamba.MlbTheShowForecaster.Performance.Domain.Statistics.ValueObjects.Pitching;

namespace com.brettnamba.MlbTheShowForecaster.Performance.Domain.Tests.Events.Pitching;

public class EraImprovementEventTests
{
    [Fact]
    public void PercentageImprovement_CurrentLowEraAndPreviousHighEra_ReturnsValue()
    {
        // Arrange
        var currentEra = EarnedRunAverage.Create(18, 54);
        var previousEra = EarnedRunAverage.Create(15, 27);
        var currentEraSnapshot = StatSnapshot.Create(currentEra, new DateTime(2024, 3, 1), new DateTime(2024, 3, 31));
        var previousEraSnapshot = StatSnapshot.Create(previousEra, new DateTime(2024, 4, 1), new DateTime(2024, 4, 10));
        var improvementEvent = new EraImprovementEvent(currentEraSnapshot, previousEraSnapshot);

        // Act
        var actual = improvementEvent.PercentageImprovement;

        // Assert
        Assert.Equal(40.00m, actual);
    }
}