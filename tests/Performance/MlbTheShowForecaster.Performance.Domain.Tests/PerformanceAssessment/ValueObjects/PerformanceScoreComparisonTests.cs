using com.brettnamba.MlbTheShowForecaster.Performance.Domain.PerformanceAssessment.ValueObjects;
using com.brettnamba.MlbTheShowForecaster.Performance.Domain.Tests.PerformanceAssessment.TestClasses;

namespace com.brettnamba.MlbTheShowForecaster.Performance.Domain.Tests.PerformanceAssessment.ValueObjects;

public class PerformanceScoreComparisonTests
{
    [Fact]
    public void IsSignificantIncrease_BigDifferenceBetweenScores_ReturnsTrue()
    {
        // Arrange
        var oldScore = Faker.FakePerformanceScore(0.1m);
        var newScore = Faker.FakePerformanceScore(0.5m);
        const decimal percentageChangeThreshold = 0.25m;
        var comparison =
            PerformanceScoreComparison.Create(oldScore: oldScore, newScore: newScore, percentageChangeThreshold);

        // Act
        var actual = comparison.IsSignificantIncrease;

        // Assert
        Assert.True(actual);
    }

    [Fact]
    public void IsSignificantDecrease_BigDifferenceBetweenScores_ReturnsTrue()
    {
        // Arrange
        var oldScore = Faker.FakePerformanceScore(0.5m);
        var newScore = Faker.FakePerformanceScore(0.1m);
        const decimal percentageChangeThreshold = 0.25m;
        var comparison =
            PerformanceScoreComparison.Create(oldScore: oldScore, newScore: newScore, percentageChangeThreshold);

        // Act
        var actual = comparison.IsSignificantDecrease;

        // Assert
        Assert.True(actual);
    }
}