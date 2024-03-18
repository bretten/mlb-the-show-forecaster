using com.brettnamba.MlbTheShowForecaster.Common.Domain.ValueObjects;

namespace com.brettnamba.MlbTheShowForecaster.Common.Domain.Tests.ValueObjects;

public class PercentageChangeTests
{
    [Fact]
    public void PercentageChangeValue_LowReferenceValueHighNewValue_ReturnsPositivePercentageChange()
    {
        // Arrange
        const decimal referenceValue = 1.024m;
        const decimal newValue = 1.078m;
        var percentageChange = PercentageChange.Create(referenceValue, newValue);

        // Act
        var actual = percentageChange.PercentageChangeValue;

        // Assert
        Assert.Equal(5.27m, actual);
    }

    [Fact]
    public void PercentageChangeValue_HighReferenceValueLowNewValue_ReturnsNegativePercentageChange()
    {
        // Arrange
        const decimal referenceValue = 1.078m;
        const decimal newValue = 1.024m;
        var percentageChange = PercentageChange.Create(referenceValue, newValue);

        // Act
        var actual = percentageChange.PercentageChangeValue;

        // Assert
        Assert.Equal(-5.01m, actual);
    }

    [Fact]
    public void HasIncreasedBy_ThresholdMagnitudeLessThanActualMagnitude_ReturnsTrue()
    {
        // Arrange
        const decimal referenceValue = 1.024m;
        const decimal newValue = 1.078m;
        var percentageChange = PercentageChange.Create(referenceValue, newValue);

        // Act
        var actual = percentageChange.HasIncreasedBy(5.25m);

        // Assert
        Assert.True(actual);
    }

    [Fact]
    public void HasIncreasedBy_ThresholdMagnitudeEqualToActualMagnitude_ReturnsTrue()
    {
        // Arrange
        const decimal referenceValue = 1.024m;
        const decimal newValue = 1.078m;
        var percentageChange = PercentageChange.Create(referenceValue, newValue);

        // Act
        var actual = percentageChange.HasIncreasedBy(5.27m);

        // Assert
        Assert.True(actual);
    }

    [Fact]
    public void HasIncreasedBy_ThresholdMagnitudeGreaterThanActualMagnitude_ReturnsFalse()
    {
        // Arrange
        const decimal referenceValue = 1.024m;
        const decimal newValue = 1.078m;
        var percentageChange = PercentageChange.Create(referenceValue, newValue);

        // Act
        var actual = percentageChange.HasIncreasedBy(5.28m);

        // Assert
        Assert.False(actual);
    }

    [Fact]
    public void HasDecreasedBy_ThresholdMagnitudeLessThanActualMagnitude_ReturnsTrue()
    {
        // Arrange
        const decimal referenceValue = 1.078m;
        const decimal newValue = 1.024m;
        var percentageChange = PercentageChange.Create(referenceValue, newValue);

        // Act
        var actual = percentageChange.HasDecreasedBy(5.00m);

        // Assert
        Assert.True(actual);
    }

    [Fact]
    public void HasDecreasedBy_ThresholdMagnitudeEqualToActualMagnitude_ReturnsTrue()
    {
        // Arrange
        const decimal referenceValue = 1.078m;
        const decimal newValue = 1.024m;
        var percentageChange = PercentageChange.Create(referenceValue, newValue);

        // Act
        var actual = percentageChange.HasDecreasedBy(5.01m);

        // Assert
        Assert.True(actual);
    }

    [Fact]
    public void HasDecreasedBy_ThresholdMagnitudeGreaterThanActualMagnitude_ReturnsFalse()
    {
        // Arrange
        const decimal referenceValue = 1.078m;
        const decimal newValue = 1.024m;
        var percentageChange = PercentageChange.Create(referenceValue, newValue);

        // Act
        var actual = percentageChange.HasDecreasedBy(5.02m);

        // Assert
        Assert.False(actual);
    }

    [Fact]
    public void Create_TwoNaturalNumbers_ReturnsPercentageChange()
    {
        // Arrange
        var referenceValue = NaturalNumber.Create(4);
        var newValue = NaturalNumber.Create(12);

        // Act
        var actual = PercentageChange.Create(referenceValue: referenceValue, newValue: newValue);

        // Assert
        Assert.Equal(200m, actual.PercentageChangeValue);
    }
}