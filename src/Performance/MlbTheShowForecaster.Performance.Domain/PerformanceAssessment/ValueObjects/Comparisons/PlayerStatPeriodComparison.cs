﻿using com.brettnamba.MlbTheShowForecaster.Common.Domain.ValueObjects;
using com.brettnamba.MlbTheShowForecaster.Common.Domain.ValueObjects.Contracts;

namespace com.brettnamba.MlbTheShowForecaster.Performance.Domain.PerformanceAssessment.ValueObjects.Comparisons;

/// <summary>
/// Represents a comparison between a stat before a specified date to the same stat since the specified date
/// </summary>
public abstract class PlayerStatPeriodComparison : PercentageChange
{
    /// <summary>
    /// The player's stat before the comparison date
    /// </summary>
    protected readonly IStat StatBeforeComparisonDate;

    /// <summary>
    /// The player's stat since the comparison date
    /// </summary>
    protected readonly IStat StatSinceComparisonDate;

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
    protected PlayerStatPeriodComparison(MlbId playerMlbId, DateTime comparisonDate, IStat statBeforeComparisonDate,
        IStat statSinceComparisonDate) : base(statBeforeComparisonDate.Value, statSinceComparisonDate.Value)
    {
        PlayerMlbId = playerMlbId;
        ComparisonDate = comparisonDate;
        StatBeforeComparisonDate = statBeforeComparisonDate;
        StatSinceComparisonDate = statSinceComparisonDate;
    }
}