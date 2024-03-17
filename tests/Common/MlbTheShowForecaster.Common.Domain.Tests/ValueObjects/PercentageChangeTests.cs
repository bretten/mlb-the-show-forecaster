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