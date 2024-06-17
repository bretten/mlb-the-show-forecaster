using com.brettnamba.MlbTheShowForecaster.Performance.Domain.PerformanceAssessment.Services.MinMaxNormalization.Exceptions;
using com.brettnamba.MlbTheShowForecaster.Performance.Domain.Statistics.ValueObjects.Batting;

namespace com.brettnamba.MlbTheShowForecaster.Performance.Domain.PerformanceAssessment.Services.MinMaxNormalization;

/// <summary>
/// Criteria for batting stats
/// </summary>
public sealed record MinMaxBattingStatCriteria : MinMaxStatCriteria
{
    /// <summary>
    /// Quick reference for <see cref="BattingStats"/> type
    /// </summary>
    private static readonly Type BattingStatsType = typeof(BattingStats);

    /// <inheritdoc />
    public MinMaxBattingStatCriteria(string name, decimal weight, bool isLowerValueBetter, decimal min, decimal max) :
        base(name, weight, isLowerValueBetter, min, max)
    {
        // Uses reflection, but only runs on startup
        if (BattingStatsType.GetProperty(name) == null)
        {
            throw new InvalidMinMaxStatCriteriaException(
                $"{nameof(MinMaxBattingStatCriteria)}: Stat '{name}' does not exist");
        }
    }
}