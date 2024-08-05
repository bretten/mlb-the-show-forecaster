using com.brettnamba.MlbTheShowForecaster.GameCards.Domain.Forecasts.ValueObjects;

namespace com.brettnamba.MlbTheShowForecaster.GameCards.Domain.Tests.Forecasts.ValueObjects;

public class DemandTests
{
    [Fact]
    public void OperatorSum_TwoDemands_ReturnsSum()
    {
        // Arrange
        var d1 = Demand.Create(1);
        var d2 = Demand.Create(2);

        // Act
        var actual = d1 + d2;

        // Assert
        Assert.Equal(3, actual.Value);
    }

    [Fact]
    public void OperatorDifference_TwoDemands_ReturnsDifference()
    {
        // Arrange
        var d1 = Demand.Create(3);
        var d2 = Demand.Create(2);

        // Act
        var actual = d1 - d2;

        // Assert
        Assert.Equal(1, actual.Value);
    }

    [Fact]
    public void OperatorGreaterThan_HigherDemand_ReturnsTrue()
    {
        // Arrange
        var d1 = Demand.High();
        var d2 = Demand.Low();
        var d3 = Demand.Stable();
        var d4 = Demand.Loss();
        var d5 = Demand.BigLoss();

        // Act
        var actual = d1 > d2 &&
                     d2 > d3 &&
                     d3 > d4 &&
                     d4 > d5;

        // Assert
        Assert.True(actual);
    }

    [Fact]
    public void OperatorLessThan_LowerDemand_ReturnsTrue()
    {
        // Arrange
        var d1 = Demand.BigLoss();
        var d2 = Demand.Loss();
        var d3 = Demand.Stable();
        var d4 = Demand.Low();
        var d5 = Demand.High();

        // Act
        var actual = d1 < d2 &&
                     d2 < d3 &&
                     d3 < d4 &&
                     d4 < d5;

        // Assert
        Assert.True(actual);
    }
}