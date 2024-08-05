using com.brettnamba.MlbTheShowForecaster.GameCards.Domain.Forecasts.ValueObjects;
using com.brettnamba.MlbTheShowForecaster.GameCards.Domain.Tests.Forecasts.TestClasses;

namespace com.brettnamba.MlbTheShowForecaster.GameCards.Domain.Tests.Forecasts.ValueObjects;

public class BoostForecastImpactTests
{
    [Fact]
    public void DemandOn_NoBoostDaysLeft_ReturnsZero()
    {
        // Arrange
        var endDate = new DateOnly(2024, 8, 3);
        var impact = Faker.FakeBoostForecastImpact(endDate: endDate);

        // Act
        var actual = impact.DemandOn(endDate.AddDays(1));

        // Assert
        Assert.Equal(Demand.Stable(), actual);
    }

    [Fact]
    public void DemandOn_OneBoostDayLeft_ReturnsPartialDemand()
    {
        // Arrange
        var endDate = new DateOnly(2024, 8, 3);
        var impact = Faker.FakeBoostForecastImpact(endDate: endDate);

        // Act
        var actual = impact.DemandOn(endDate.AddDays(-1));

        // Assert
        Assert.Equal(Demand.Low(), actual);
    }

    [Fact]
    public void DemandOn_ManyBoostDaysLeft_ReturnsFullDemand()
    {
        // Arrange
        var endDate = new DateOnly(2024, 8, 3);
        var impact = Faker.FakeBoostForecastImpact(endDate: endDate);

        // Act
        var actual = impact.DemandOn(endDate.AddDays(-4));

        // Assert
        Assert.Equal(Demand.High(), actual);
    }
}