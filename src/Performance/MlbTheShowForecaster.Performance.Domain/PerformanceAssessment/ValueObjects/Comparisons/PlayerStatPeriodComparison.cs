using com.brettnamba.MlbTheShowForecaster.Common.Domain.SeedWork;
using com.brettnamba.MlbTheShowForecaster.Common.Domain.ValueObjects;

namespace com.brettnamba.MlbTheShowForecaster.Performance.Domain.PerformanceAssessment.ValueObjects.Comparisons;

/// <summary>
/// Represents a comparison between a stat before a specified date to the same stat since the specified date
/// </summary>
public abstract class PlayerStatPeriodComparison : ValueObject
{
    /// <summary>
    /// The underlying percentage change
    /// </summary>
    private decimal? _percentageChange;

    /// <summary>
    /// The player's stat before the comparison date
    /// </summary>
    protected readonly RawStat StatBeforeComparisonDate;

    /// <summary>
    /// The player's stat since the comparison date
    /// </summary>
    protected readonly RawStat StatSinceComparisonDate;

    /// <summary>
    /// The MLB ID of the player
    /// </summary>
    public MlbId PlayerMlbId { get; }

    /// <summary>
    /// The date of comparison for the stat. We will be comparing the stat before this date to the stat since this date.
    /// In other words, how the player has performed in this stat previously compared to recently
    /// </summary>
    public DateTime ComparisonDate { get; }

    /// <summary>
    /// Constructor
    /// </summary>
    /// <param name="playerMlbId">The MLB ID of the player</param>
    /// <param name="comparisonDate">The date of comparison -- the stat before this date will be compared to the stat since this date</param>
    /// <param name="statBeforeComparisonDate">The player's stat before the comparison date</param>
    /// <param name="statSinceComparisonDate">The player's stat since the comparison date</param>
    protected PlayerStatPeriodComparison(MlbId playerMlbId, DateTime comparisonDate, RawStat statBeforeComparisonDate,
        RawStat statSinceComparisonDate)
    {
        PlayerMlbId = playerMlbId;
        ComparisonDate = comparisonDate;
        StatBeforeComparisonDate = statBeforeComparisonDate;
        StatSinceComparisonDate = statSinceComparisonDate;
    }

    /// <summary>
    /// The percentage of change over the previous value
    /// </summary>
    public decimal PercentageChange
    {
        get
        {
            if (!_percentageChange.HasValue)
            {
                _percentageChange = Math.Round(CalculatePercentageChange(), 2, MidpointRounding.AwayFromZero);
            }

            return _percentageChange.Value;
        }
    }

    /// <summary>
    /// Calculates the percentage of change over the previous value
    /// </summary>
    private decimal CalculatePercentageChange()
    {
        return 100 * ((StatSinceComparisonDate.Value - StatBeforeComparisonDate.Value) /
                      StatBeforeComparisonDate.Value);
    }
}