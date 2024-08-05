using com.brettnamba.MlbTheShowForecaster.Common.Domain.Enums;
using com.brettnamba.MlbTheShowForecaster.GameCards.Domain.Forecasts.ValueObjects;
using com.brettnamba.MlbTheShowForecaster.GameCards.Domain.Tests.Forecasts.TestClasses;

namespace com.brettnamba.MlbTheShowForecaster.GameCards.Domain.Tests.Forecasts.ValueObjects;

public class PositionChangeForecastImpactTests
{
    [Theory]
    [InlineData(Position.FirstBase, Position.LeftField)]
    [InlineData(Position.FirstBase, Position.Catcher)]
    [InlineData(Position.FirstBase, Position.SecondBase)]
    public void Demand_DesiredNewPosition_ReturnsHighDemand(Position oldPosition, Position newPosition)
    {
        // Arrange
        var impact = Faker.FakePositionChangeForecastImpact(oldPosition: oldPosition, newPosition: newPosition);

        // Act
        var actual = impact.Demand;

        // Assert
        Assert.Equal(Demand.High(), actual);
    }

    [Theory]
    [InlineData(Position.LeftField, Position.FirstBase)]
    [InlineData(Position.LeftField, Position.Shortstop)]
    [InlineData(Position.LeftField, Position.ThirdBase)]
    [InlineData(Position.LeftField, Position.CenterField)]
    [InlineData(Position.LeftField, Position.RightField)]
    public void Demand_UndesiredNewPosition_ReturnsNoDemand(Position oldPosition, Position newPosition)
    {
        // Arrange
        var impact = Faker.FakePositionChangeForecastImpact(oldPosition: oldPosition, newPosition: newPosition);

        // Act
        var actual = impact.Demand;

        // Assert
        Assert.Equal(Demand.Stable(), actual);
    }
}