using com.brettnamba.MlbTheShowForecaster.Performance.Domain.PerformanceAssessment.Services.MinMaxNormalization;
using com.brettnamba.MlbTheShowForecaster.Performance.Domain.PerformanceAssessment.Services.MinMaxNormalization.Exceptions;

namespace com.brettnamba.MlbTheShowForecaster.Performance.Domain.Tests.PerformanceAssessment.Services.
    MinMaxNormalization;

public class MinMaxNormalizationCriteriaTests
{
    [Fact]
    public void Constructor_IncorrectWeightSums_ThrowsException()
    {
        // Arrange
        var invalidBattingCriteria = new List<MinMaxBattingStatCriteria>()
        {
            new("Hits", 0.5m, false, min: 1, max: 100),
            new("HomeRuns", 0.6m, false, min: 1, max: 100), // Weights sum > 1
        };
        var action = () =>
            new MinMaxNormalizationCriteria(0.1m, invalidBattingCriteria, ValidPitchingCriteria, ValidFieldingCriteria);

        // Act
        var actual = Record.Exception(action);

        // Assert
        Assert.NotNull(actual);
        Assert.IsType<InvalidMinMaxNormalizationCriteriaException>(actual);
    }

    private static readonly IReadOnlyList<MinMaxPitchingStatCriteria> ValidPitchingCriteria =
        new List<MinMaxPitchingStatCriteria>()
        {
            new("EarnedRunAverage", 1m, false, min: 1, max: 100),
        };

    private static readonly IReadOnlyList<MinMaxFieldingStatCriteria> ValidFieldingCriteria =
        new List<MinMaxFieldingStatCriteria>()
        {
            new("FieldingPercentage", 1m, false, min: 1, max: 100),
        };
}