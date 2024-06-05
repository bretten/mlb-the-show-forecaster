using com.brettnamba.MlbTheShowForecaster.Performance.Domain.PerformanceAssessment.ValueObjects;
using com.brettnamba.MlbTheShowForecaster.Performance.Domain.PerformanceAssessment.ValueObjects.Exceptions;

namespace com.brettnamba.MlbTheShowForecaster.Performance.Domain.Tests.PerformanceAssessment.ValueObjects;

public class PerformanceScoreTests
{
    [Theory]
    [InlineData(-0.01)]
    [InlineData(1.01)]
    public void Constructor_InvalidScore_ThrowsException(double scoreAsDouble)
    {
        // Arrange
        var score = (decimal)scoreAsDouble;
        var action = () => PerformanceScore.Create(score);

        // Act
        var actual = Record.Exception(action);

        // Assert
        Assert.NotNull(actual);
        Assert.IsType<InvalidPerformanceScoreException>(actual);
    }

    [Fact]
    public void Value_ValidScore_ReturnsScore()
    {
        // Arrange
        const decimal score = 0.7m;

        // Act
        var actual = PerformanceScore.Create(score);

        // Assert
        Assert.Equal(0.7m, actual.Value);
    }
}