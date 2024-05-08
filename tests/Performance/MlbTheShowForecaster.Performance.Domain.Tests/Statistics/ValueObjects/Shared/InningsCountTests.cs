using com.brettnamba.MlbTheShowForecaster.Performance.Domain.Statistics.ValueObjects.Shared;
using com.brettnamba.MlbTheShowForecaster.Performance.Domain.Statistics.ValueObjects.Shared.Exceptions;
using com.brettnamba.MlbTheShowForecaster.Performance.Domain.Statistics.ValueObjects.Shared.Extensions;

namespace com.brettnamba.MlbTheShowForecaster.Performance.Domain.Tests.Statistics.ValueObjects.Shared;

public class InningsCountTests
{
    [Fact]
    public void Constructor_InvalidPartialInningsPitched_ThrowsException()
    {
        // Arrange
        const decimal inningsPitched = 0.3m;
        var action = () => InningsCount.Create(inningsPitched);

        // Act
        var actual = Record.Exception(action);

        // Assert
        Assert.NotNull(actual);
        Assert.IsType<InvalidInningsCountDecimalException>(actual);
    }

    [Fact]
    public void Value_WholeNumberInningsPitched_ReturnsWholeNumber()
    {
        // Arrange
        const int inningsPitched = 2;
        var ip = InningsCount.Create(inningsPitched);

        // Act
        var actual = ip.Value;

        // Assert
        Assert.Equal(2, actual);
    }

    [Fact]
    public void Value_ZeroInningsPitched_ReturnsZero()
    {
        // Arrange
        const decimal inningsPitched = 0.0m;
        var ip = InningsCount.Create(inningsPitched);

        // Act
        var actual = ip.Value;

        // Assert
        Assert.Equal(0, actual);
    }

    [Theory]
    [InlineData(0.1, 0.333)]
    [InlineData(0.2, 0.667)]
    [InlineData(1.1, 1.333)]
    [InlineData(1.2, 1.667)]
    public void Value_ShorthandPartialInningsPitched_ReturnsThirds(decimal inningsPitched, decimal expectedValue)
    {
        // Arrange
        var ip = InningsCount.Create(inningsPitched);

        // Act
        var actual = ip.Value;

        // Assert
        Assert.Equal(expectedValue, actual);
    }

    [Theory]
    [InlineData(0.333, 0.333)]
    [InlineData(0.667, 0.667)]
    [InlineData(1.333, 1.333)]
    [InlineData(1.667, 1.667)]
    public void Value_DecimalPartialInningsPitched_ReturnsThirds(decimal inningsPitched, decimal expectedValue)
    {
        // Arrange
        var ip = InningsCount.Create(inningsPitched);

        // Act
        var actual = ip.Value;

        // Assert
        Assert.Equal(expectedValue, actual);
    }

    /// <summary>
    /// NOTE: Can't use inline data for these fractional decimal cases
    /// </summary>
    [Fact]
    public void Value_OneThirdInningsPitched_ReturnsOneThird()
    {
        // Arrange
        const decimal inningsPitched = (decimal)1 / 3;
        var ip = InningsCount.Create(inningsPitched);

        // Act
        var actual = ip.Value;

        // Assert
        Assert.Equal(0.333m, actual);
    }

    [Fact]
    public void Value_TwoThirdsInningsPitched_ReturnsTwoThirds()
    {
        // Arrange
        const decimal inningsPitched = (decimal)2 / 3;
        var ip = InningsCount.Create(inningsPitched);

        // Act
        var actual = ip.Value;

        // Assert
        Assert.Equal(0.667m, actual);
    }

    [Fact]
    public void Value_OneAndOneThirdInningsPitched_ReturnsOneAndOneThird()
    {
        // Arrange
        const decimal inningsPitched = 1 + (decimal)1 / 3;
        var ip = InningsCount.Create(inningsPitched);

        // Act
        var actual = ip.Value;

        // Assert
        Assert.Equal(1.333m, actual);
    }

    [Fact]
    public void Value_OneAndTwoThirdsInningsPitched_ReturnsOneAndTwoThirds()
    {
        // Arrange
        const decimal inningsPitched = 1 + (decimal)2 / 3;
        var ip = InningsCount.Create(inningsPitched);

        // Act
        var actual = ip.Value;

        // Assert
        Assert.Equal(1.667m, actual);
    }

    [Theory]
    [InlineData(0.1, 0, 1, 0.333, 0.1)]
    [InlineData(0.2, 0, 2, 0.667, 0.2)]
    [InlineData(1.1, 1, 1, 0.333, 0.1)]
    [InlineData(1.2, 1, 2, 0.667, 0.2)]
    public void PartialInnings_ShorthandPartialInningsPitched_ReturnsAllComponentsOfPartialInnings(
        decimal inningsPitched, int expectedFullInnings, int expectedAdditionalOuts,
        decimal expectedPartialInningsDecimal, decimal expectedShorthandPartialInnings)
    {
        // Arrange
        var ip = InningsCount.Create(inningsPitched);

        // Act
        var actual = ip;

        // Assert
        Assert.Equal(expectedFullInnings, actual.FullInnings.Value);
        Assert.Equal(expectedAdditionalOuts, actual.AdditionalOuts.Value);
        Assert.Equal(expectedPartialInningsDecimal, actual.PartialInningsAsDecimal);
        Assert.Equal(expectedShorthandPartialInnings, actual.PartialInningsAsShorthand);
    }

    [Theory]
    [InlineData(0.333, 0, 1, 0.333, 0.1)]
    [InlineData(0.667, 0, 2, 0.667, 0.2)]
    [InlineData(1.333, 1, 1, 0.333, 0.1)]
    [InlineData(1.667, 1, 2, 0.667, 0.2)]
    public void PartialInnings_DecimalPartialInningsPitched_ReturnsAllComponentsOfPartialInnings(decimal inningsPitched,
        int expectedFullInnings, int expectedAdditionalOuts, decimal expectedPartialInningsDecimal,
        decimal expectedShorthandPartialInnings)
    {
        // Arrange
        var ip = InningsCount.Create(inningsPitched);

        // Act
        var actual = ip;

        // Assert
        Assert.Equal(expectedFullInnings, actual.FullInnings.Value);
        Assert.Equal(expectedAdditionalOuts, actual.AdditionalOuts.Value);
        Assert.Equal(expectedPartialInningsDecimal, actual.PartialInningsAsDecimal);
        Assert.Equal(expectedShorthandPartialInnings, actual.PartialInningsAsShorthand);
    }

    [Fact]
    public void Constructor_InvalidStringInningsPitched_ThrowsException()
    {
        // Arrange
        const string inningsPitched = "";
        var action = () => InningsCount.Create(inningsPitched);

        // Act
        var actual = Record.Exception(action);

        // Assert
        Assert.NotNull(actual);
        Assert.IsType<InvalidInningsCountDecimalException>(actual);
    }

    [Fact]
    public void Value_StringZeroInningsPitched_ReturnsZero()
    {
        // Arrange
        const string inningsPitched = "0.0";
        var ip = InningsCount.Create(inningsPitched);

        // Act
        var actual = ip.Value;

        // Assert
        Assert.Equal(0, actual);
    }

    [Fact]
    public void Value_StringPointOneInningsPitched_ReturnsOneThird()
    {
        // Arrange
        const string inningsPitched = "0.1";
        var ip = InningsCount.Create(inningsPitched);

        // Act
        var actual = ip.Value;

        // Assert
        Assert.Equal(0.333m, actual);
    }

    [Fact]
    public void Value_StringPointTwoInningsPitched_ReturnsTwoThirds()
    {
        // Arrange
        const string inningsPitched = "0.2";
        var ip = InningsCount.Create(inningsPitched);

        // Act
        var actual = ip.Value;

        // Assert
        Assert.Equal(0.667m, actual);
    }

    [Fact]
    public void Value_StringOnePointZeroInningsPitched_ReturnsOneAndOneThird()
    {
        // Arrange
        const string inningsPitched = "1.0";
        var ip = InningsCount.Create(inningsPitched);

        // Act
        var actual = ip.Value;

        // Assert
        Assert.Equal(1m, actual);
    }

    [Fact]
    public void Value_StringOnePointOneInningsPitched_ReturnsOneAndOneThird()
    {
        // Arrange
        const string inningsPitched = "1.1";
        var ip = InningsCount.Create(inningsPitched);

        // Act
        var actual = ip.Value;

        // Assert
        Assert.Equal(1.333m, actual);
    }

    [Fact]
    public void Value_StringOnePointTwoInningsPitched_ReturnsOneAndTwoThirds()
    {
        // Arrange
        const string inningsPitched = "1.2";
        var ip = InningsCount.Create(inningsPitched);

        // Act
        var actual = ip.Value;

        // Assert
        Assert.Equal(1.667m, actual);
    }

    [Theory]
    // Only full innings
    [InlineData(0, 0, 0, 0)]
    [InlineData(1, 2, 3, 6)]
    // Decimal format
    [InlineData(1.333, 2.333, 3.333, 7)]
    [InlineData(1.667, 2.667, 3.667, 8)]
    [InlineData(1.0, 2.333, 3.667, 7)]
    [InlineData(1.333, 2.0, 3.0, 6.333)]
    [InlineData(1.667, 2.0, 3.0, 6.667)]
    // Shorthand format
    [InlineData(1.1, 2.1, 3.1, 7)]
    [InlineData(1.2, 2.2, 3.2, 8)]
    [InlineData(1.0, 2.1, 3.2, 7)]
    [InlineData(1.1, 2.0, 3.0, 6.333)]
    [InlineData(1.2, 2.0, 3.0, 6.667)]
    public void SumInnings_InningsPitchedCollection_SumsInningsCountCollection(decimal ip1, decimal ip2, decimal ip3,
        decimal totalInningsPitched)
    {
        // Arrange
        var inningsPitched1 = InningsCount.Create(ip1);
        var inningsPitched2 = InningsCount.Create(ip2);
        var inningsPitched3 = InningsCount.Create(ip3);

        var collection = new List<InningsCount>() { inningsPitched1, inningsPitched2, inningsPitched3 };

        // Actual
        var actual = collection.SumInnings();

        // Assert
        Assert.Equal(totalInningsPitched, actual.Value);
    }
}