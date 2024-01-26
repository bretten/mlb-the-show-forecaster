using com.brettnamba.MlbTheShowForecaster.Common.Domain.Tests.ValueObjects.TestClasses;
using com.brettnamba.MlbTheShowForecaster.Common.Domain.ValueObjects.Exceptions;

namespace com.brettnamba.MlbTheShowForecaster.Common.Domain.Tests.ValueObjects;

public class CalculatedStatTests
{
    [Fact]
    public void Constructor_NegativeValue_ThrowsException()
    {
        // Arrange
        const decimal value = -1.2m;
        var action = () => TestCalculatedStat.Create(value);

        // Act
        var actual = Record.Exception(action);

        // Assert
        Assert.NotNull(actual);
        Assert.IsType<CalculatedStatBelowZeroException>(actual);
    }

    [Fact]
    public void Value_CalculationComponents_ReturnsCalculatedValue()
    {
        // Arrange
        const int var1 = 4;
        const int var2 = 5;
        const int var3 = 1;
        var stat = TestCalculatedStat.Create(var1, var2, var3); // Calculation is (var1 / var2) + var3

        // Act
        var actual = stat.Value;

        // Assert
        Assert.Equal(1.8m, actual);
    }

    [Fact]
    public void AsRounded_NoDecimalCount_ReturnsValueRoundedToDefaultDecimalCount()
    {
        // Arrange
        const int var1 = 7;
        const int var2 = 6;
        const int var3 = 2;
        var stat = TestCalculatedStat.Create(var1, var2, var3); // Calculation is (var1 / var2) + var3

        // Act
        var actual = stat.AsRounded();

        // Assert
        Assert.Equal(3.167m, actual);
    }

    [Fact]
    public void AsRounded_WithDecimalCount_ReturnsValueRoundedToSpecifiedDecimalCount()
    {
        // Arrange
        const int var1 = 7;
        const int var2 = 6;
        const int var3 = 2;
        var stat = TestCalculatedStat.Create(var1, var2, var3); // Calculation is (var1 / var2) + var3

        // Act
        var actual = stat.AsRounded(2);

        // Assert
        Assert.Equal(3.17m, actual);
    }
}