using com.brettnamba.MlbTheShowForecaster.GameCards.Domain.Forecasts.ValueObjects;
using com.brettnamba.MlbTheShowForecaster.GameCards.Domain.Tests.Forecasts.TestClasses;

namespace com.brettnamba.MlbTheShowForecaster.GameCards.Domain.Tests.Forecasts.ValueObjects;

public class OverallRatingChangeForecastImpactTests
{
    [Fact]
    public void RarityImproved_NewerRarityBetter_ReturnsTrue()
    {
        // Arrange
        var impact = Faker.FakeOverallRatingChangeForecastImpact(oldOverallRating: 50, newOverallRating: 70);

        // Act
        var actual = impact.RarityImproved;

        // Assert
        Assert.True(actual);
    }

    [Fact]
    public void RarityDeclined_NewerRarityWorse_ReturnsTrue()
    {
        // Arrange
        var impact = Faker.FakeOverallRatingChangeForecastImpact(oldOverallRating: 70, newOverallRating: 60);

        // Act
        var actual = impact.RarityDeclined;

        // Assert
        Assert.True(actual);
    }

    [Fact]
    public void Demand_RarityDidNotChange_ReturnsZero()
    {
        // Arrange
        var impact = Faker.FakeOverallRatingChangeForecastImpact(oldOverallRating: 50, newOverallRating: 60);

        // Act
        var actual = impact.Demand;

        // Assert
        Assert.Equal(Demand.Stable(), actual);
    }
}