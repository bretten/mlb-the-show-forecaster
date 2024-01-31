using com.brettnamba.MlbTheShowForecaster.Common.Domain.Tests.ValueObjects.TestClasses;

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
}