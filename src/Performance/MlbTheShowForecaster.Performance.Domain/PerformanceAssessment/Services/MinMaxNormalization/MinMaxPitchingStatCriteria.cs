using com.brettnamba.MlbTheShowForecaster.Performance.Domain.PerformanceAssessment.Services.MinMaxNormalization.Exceptions;
using com.brettnamba.MlbTheShowForecaster.Performance.Domain.Statistics.ValueObjects.Pitching;

namespace com.brettnamba.MlbTheShowForecaster.Performance.Domain.PerformanceAssessment.Services.MinMaxNormalization;

/// <summary>
/// Criteria for pitching stats
/// </summary>
public sealed record MinMaxPitchingStatCriteria : MinMaxStatCriteria
{
    /// <summary>
    /// Quick reference for <see cref="PitchingStats"/> type
    /// </summary>
    private static readonly Type PitchingStatsType = typeof(PitchingStats);

    /// <inheritdoc />
    public MinMaxPitchingStatCriteria(string name, decimal weight, bool isLowerValueBetter, decimal min, decimal max) :
        base(name, weight, isLowerValueBetter, min, max)
    {
        // Uses reflection, but only runs on startup
        if (PitchingStatsType.GetProperty(name) == null)
        {
            throw new InvalidMinMaxStatCriteriaException(
                $"{nameof(MinMaxPitchingStatCriteria)}: Stat '{name}' does not exist");
        }
    }
}