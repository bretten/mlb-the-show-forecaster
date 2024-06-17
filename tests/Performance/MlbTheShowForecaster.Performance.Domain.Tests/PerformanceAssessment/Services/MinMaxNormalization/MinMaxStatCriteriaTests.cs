using com.brettnamba.MlbTheShowForecaster.Performance.Domain.PerformanceAssessment.Services.MinMaxNormalization;
using com.brettnamba.MlbTheShowForecaster.Performance.Domain.PerformanceAssessment.Services.MinMaxNormalization.Exceptions;
using com.brettnamba.MlbTheShowForecaster.Performance.Domain.Statistics.ValueObjects.Batting;
using com.brettnamba.MlbTheShowForecaster.Performance.Domain.Statistics.ValueObjects.Pitching;

namespace com.brettnamba.MlbTheShowForecaster.Performance.Domain.Tests.PerformanceAssessment.Services.
    MinMaxNormalization;

public class MinMaxStatCriteriaTests
{
    [Theory]
    [InlineData(-0.01)]
    [InlineData(1.01)]
    public void Constructor_InvalidWeight_ThrowsException(double weight)
    {
        // Arrange
        var invalidWeight = (decimal)weight;
        var action = () => new MinMaxBattingStatCriteria("Hits", invalidWeight, false, 1, 100);

        // Act
        var actual = Record.Exception(action);

        // Assert
        Assert.NotNull(actual);
        Assert.IsType<InvalidMinMaxStatCriteriaException>(actual);
    }

    [Theory]
    [InlineData(10, 1)]
    [InlineData(10, 10)]
    public void Constructor_InvalidMinMax_ThrowsException(double min, double max)
    {
        // Arrange
        var invalidMin = (decimal)min;
        var invalidMax = (decimal)max;
        var action = () => new MinMaxBattingStatCriteria("Hits", 0.5m, false, invalidMin, invalidMax);

        // Act
        var actual = Record.Exception(action);

        // Assert
        Assert.NotNull(actual);
        Assert.IsType<InvalidMinMaxStatCriteriaException>(actual);
    }

    [Fact]
    public void Constructor_NonBattingStats_ThrowsException()
    {
        // Arrange
        const string invalidStatName = nameof(PitchingStats.EarnedRuns);
        var action = () => new MinMaxBattingStatCriteria(invalidStatName, 0.5m, false, 1, 100);

        // Act
        var actual = Record.Exception(action);

        // Assert
        Assert.NotNull(actual);
        Assert.IsType<InvalidMinMaxStatCriteriaException>(actual);
    }

    [Fact]
    public void Constructor_NonPitchingStats_ThrowsException()
    {
        // Arrange
        const string invalidStatName = nameof(BattingStats.BattingAverage);
        var action = () => new MinMaxPitchingStatCriteria(invalidStatName, 0.5m, false, 1, 100);

        // Act
        var actual = Record.Exception(action);

        // Assert
        Assert.NotNull(actual);
        Assert.IsType<InvalidMinMaxStatCriteriaException>(actual);
    }

    [Fact]
    public void Constructor_NonFieldingStats_ThrowsException()
    {
        // Arrange
        const string invalidStatName = nameof(BattingStats.BattingAverage);
        var action = () => new MinMaxFieldingStatCriteria(invalidStatName, 0.5m, false, 1, 100);

        // Act
        var actual = Record.Exception(action);

        // Assert
        Assert.NotNull(actual);
        Assert.IsType<InvalidMinMaxStatCriteriaException>(actual);
    }
}