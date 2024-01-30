using com.brettnamba.MlbTheShowForecaster.Performance.Domain.PlayerSeasons.ValueObjects;
using com.brettnamba.MlbTheShowForecaster.Performance.Domain.PlayerSeasons.ValueObjects.Exceptions;

namespace com.brettnamba.MlbTheShowForecaster.Performance.Domain.Tests.PlayerSeasons.ValueObjects;

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
    public void Value_WholeNumberInningsPitched_ReturnsValue()
    {
        // Arrange
        const int inningsPitched = 2;
        var ip = InningsCount.Create(inningsPitched);

        // Act
        var actual = ip.Value;

        // Assert
        Assert.Equal(inningsPitched, actual);
    }

    [Fact]
    public void Value_ZeroInningsPitched_ReturnsValue()
    {
        // Arrange
        const decimal inningsPitched = 0.0m;
        var ip = InningsCount.Create(inningsPitched);

        // Act
        var actual = ip.Value;

        // Assert
        Assert.Equal(inningsPitched, actual);
    }

    [Fact]
    public void Value_ShorthandPointOneInningsPitched_ReturnsValue()
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
    public void Value_ShorthandPointTwoInningsPitched_ReturnsValue()
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
    public void Value_ShorthandOnePointOneInningsPitched_ReturnsValue()
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
    public void Value_ShorthandOnePointTwoInningsPitched_ReturnsValue()
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
    public void Value_OneThirdInningsPitched_ReturnsValue()
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
    public void Value_TwoThirdsInningsPitched_ReturnsValue()
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
    public void Value_OneAndOneThirdInningsPitched_ReturnsValue()
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
    public void Value_OneAndTwoThirdsInningsPitched_ReturnsValue()
    {
        // Arrange
        const decimal inningsPitched = 1 + (decimal)2 / 3;
        var ip = InningsCount.Create(inningsPitched);

        // Act
        var actual = ip.Value;

        // Assert
        Assert.Equal(1.667m, actual);
    }
}