using com.brettnamba.MlbTheShowForecaster.Common.Domain.Tests.ValueObjects.TestClasses;
using com.brettnamba.MlbTheShowForecaster.Common.Domain.ValueObjects.Exceptions;

namespace com.brettnamba.MlbTheShowForecaster.Common.Domain.Tests.ValueObjects;

public class CalculatedStatTests
{
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
    public void Value_ZeroDenominator_ReturnsZero()
    {
        // Arrange
        const int var1 = 4;
        const int var2 = 0;
        const int var3 = 0;
        var stat = TestCalculatedStat.Create(var1, var2, var3); // Calculation is (var1 / var2) + var3

        // Act
        var actual = stat.Value;

        // Assert
        Assert.Equal(0, actual);
    }

    [Fact]
    public void FractionalDigitCount_Two_ReturnsValueRoundedToSpecifiedFractionalDigitCount()
    {
        // Arrange
        const int var1 = 7;
        const int var2 = 6;
        const int var3 = 2;
        var stat = TestCalculatedStat.Create(var1, var2, var3); // Calculation is (var1 / var2) + var3

        // Act
        var actual = stat.Value;

        // Assert
        Assert.Equal(3.17m, actual);
    }

    [Fact]
    public void ToInt_DecimalTooSmall_ThrowsException()
    {
        // Arrange
        const int var1 = -int.MaxValue;
        const int var2 = 1;
        const int var3 = -int.MaxValue;
        var stat = TestCalculatedStat.Create(var1, var2, var3); // Calculation is (var1 / var2) + var3
        Func<object> action = () => stat.ToInt();

        // Act
        var actual = Record.Exception(action);

        // Assert
        Assert.NotNull(actual);
        Assert.IsType<CalculatedStatCannotBeConvertedToIntException>(actual);
    }

    [Fact]
    public void ToInt_DecimalTooLarge_ThrowsException()
    {
        // Arrange
        const int var1 = int.MaxValue;
        const int var2 = 1;
        const int var3 = int.MaxValue;
        var stat = TestCalculatedStat.Create(var1, var2, var3); // Calculation is (var1 / var2) + var3
        Func<object> action = () => stat.ToInt();

        // Act
        var actual = Record.Exception(action);

        // Assert
        Assert.NotNull(actual);
        Assert.IsType<CalculatedStatCannotBeConvertedToIntException>(actual);
    }

    [Fact]
    public void ToInt_DecimalThatCanBeCastToInt_ReturnsValueAsInt()
    {
        // Arrange
        const int var1 = 10;
        const int var2 = 1;
        const int var3 = 20;
        var stat = TestCalculatedStat.Create(var1, var2, var3); // Calculation is (var1 / var2) + var3

        // Act
        var actual = stat.ToInt();

        // Assert
        Assert.Equal(30, actual);
    }

    [Fact]
    public void ToNaturalNumber_DecimalThatCanBeConvertedToNaturalNumber_ReturnsValueAsNaturalNumber()
    {
        // Arrange
        const int var1 = 10;
        const int var2 = 1;
        const int var3 = 20;
        var stat = TestCalculatedStat.Create(var1, var2, var3); // Calculation is (var1 / var2) + var3

        // Act
        var actual = stat.ToNaturalNumber();

        // Assert
        Assert.Equal(30, actual.Value);
    }
}