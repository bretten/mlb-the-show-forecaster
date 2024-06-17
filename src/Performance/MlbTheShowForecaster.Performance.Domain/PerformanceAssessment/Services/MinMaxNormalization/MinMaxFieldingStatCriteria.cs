using com.brettnamba.MlbTheShowForecaster.Performance.Domain.PerformanceAssessment.Services.MinMaxNormalization.Exceptions;
using com.brettnamba.MlbTheShowForecaster.Performance.Domain.Statistics.ValueObjects.Fielding;

namespace com.brettnamba.MlbTheShowForecaster.Performance.Domain.PerformanceAssessment.Services.MinMaxNormalization;

/// <summary>
/// Criteria for fielding stats
/// </summary>
public sealed record MinMaxFieldingStatCriteria : MinMaxStatCriteria
{
    /// <summary>
    /// Quick reference for <see cref="FieldingStats"/> type
    /// </summary>
    private static readonly Type FieldingStatsType = typeof(FieldingStats);

    /// <inheritdoc />
    public MinMaxFieldingStatCriteria(string name, decimal weight, bool isLowerValueBetter, decimal min, decimal max) :
        base(name, weight, isLowerValueBetter, min, max)
    {
        // Uses reflection, but only runs on startup
        if (FieldingStatsType.GetProperty(name) == null)
        {
            throw new InvalidMinMaxStatCriteriaException(
                $"{nameof(MinMaxFieldingStatCriteria)}: Stat '{name}' does not exist");
        }
    }
}