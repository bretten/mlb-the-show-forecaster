using com.brettnamba.MlbTheShowForecaster.Common.Domain.ValueObjects;
using com.brettnamba.MlbTheShowForecaster.Performance.Domain.Events.Improvement;
using com.brettnamba.MlbTheShowForecaster.Performance.Domain.Tests.Events.TestClasses;

namespace com.brettnamba.MlbTheShowForecaster.Performance.Domain.Tests.Events;

public class StatImprovementEventTests
{
    [Fact]
    public void PercentageImprovement_CurrentAndPreviousStat_ReturnsValue()
    {
        // Arrange
        var currentStat =
            StatSnapshot.Create(NaturalNumber.Create(50), new DateTime(2024, 4, 1), new DateTime(2024, 4, 10));
        var previousStat =
            StatSnapshot.Create(NaturalNumber.Create(22), new DateTime(2024, 3, 1), new DateTime(2024, 3, 31));
        var improvementEvent = new TestStatImprovementEvent(currentStat, previousStat)
            with
            {
                CurrentStat = currentStat, PreviousStat = previousStat // Code coverage for init of a record
            };

        // Act
        var actual = improvementEvent.PercentageImprovement;

        // Assert
        Assert.Equal(127.27m, actual);
    }
}