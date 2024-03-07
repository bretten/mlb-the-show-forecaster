using com.brettnamba.MlbTheShowForecaster.Performance.Domain.Statistics.ValueObjects.Shared;
using com.brettnamba.MlbTheShowForecaster.Performance.Domain.Statistics.ValueObjects.Shared.Exceptions;

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

    [Fact]
    public void Value_ShorthandPointOneInningsPitched_ReturnsOneThird()
    {
        // Arrange
        const decimal inningsPitched = 0.1m;
        var ip = InningsCount.Create(inningsPitched);

        // Act
        var actual = ip.Value;

        // Assert
        Assert.Equal(0.333m, actual);
    }

    [Fact]
    public void Value_ShorthandPointTwoInningsPitched_ReturnsTwoThirds()
    {
        // Arrange
        const decimal inningsPitched = 0.2m;
        var ip = InningsCount.Create(inningsPitched);

        // Act
        var actual = ip.Value;

        // Assert
        Assert.Equal(0.667m, actual);
    }

    [Fact]
    public void Value_ShorthandOnePointOneInningsPitched_ReturnsOneAndOneThird()
    {
        // Arrange
        const decimal inningsPitched = 1.1m;
        var ip = InningsCount.Create(inningsPitched);

        // Act
        var actual = ip.Value;

        // Assert
        Assert.Equal(1.333m, actual);
    }

    [Fact]
    public void Value_ShorthandOnePointTwoInningsPitched_ReturnsOneAndTwoThirds()
    {
        // Arrange
        const decimal inningsPitched = 1.2m;
        var ip = InningsCount.Create(inningsPitched);

        // Act
        var actual = ip.Value;

        // Assert
        Assert.Equal(1.667m, actual);
    }

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
}