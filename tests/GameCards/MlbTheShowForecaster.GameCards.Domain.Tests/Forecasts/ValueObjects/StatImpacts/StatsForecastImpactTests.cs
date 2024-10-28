using com.brettnamba.MlbTheShowForecaster.GameCards.Domain.Forecasts.ValueObjects;
using com.brettnamba.MlbTheShowForecaster.GameCards.Domain.Tests.Forecasts.TestClasses;

namespace com.brettnamba.MlbTheShowForecaster.GameCards.Domain.Tests.Forecasts.ValueObjects.StatImpacts;

public class StatsForecastImpactTests
{
    [Fact]
    public void IsImprovement_NewScoreHigher_ReturnsTrue()
    {
        // Arrange
        var impact = Faker.FakeBattingStatsForecastImpact(oldScore: 0.1m, newScore: 0.2m);

        // Act
        var actual = impact.IsImprovement;

        // Assert
        Assert.True(actual);
    }

    [Fact]
    public void IsImprovement_NewScoreLower_ReturnsFalse()
    {
        // Arrange
        var impact = Faker.FakeBattingStatsForecastImpact(oldScore: 0.2m, newScore: 0.1m);

        // Act
        var actual = impact.IsImprovement;

        // Assert
        Assert.False(actual);
    }

    [Fact]
    public void Demand_ScoreChangeNotAboveThreshold_ReturnsZero()
    {
        // Arrange
        var impact = Faker.FakeBattingStatsForecastImpact(oldScore: 0.1m, newScore: 0.11m);

        // Act
        var actual = impact.Demand;

        // Assert
        Assert.Equal(Demand.Stable(), actual);
    }

    [Fact]
    public void PercentageChange_ZeroInitialScore_DoesNotDivideByZero()
    {
        // Arrange
        var impact = Faker.FakeBattingStatsForecastImpact(oldScore: 0.0m, newScore: 0.11m);

        // Act
        var actual = impact.PercentageChange;

        // Assert
        Assert.Equal(11.0m, actual.PercentageChangeValue);
        Assert.True(actual.TreatZeroReferenceValueAsOne);
    }
}