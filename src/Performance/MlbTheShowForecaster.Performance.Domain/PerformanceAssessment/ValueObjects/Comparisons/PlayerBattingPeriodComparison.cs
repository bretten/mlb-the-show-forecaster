﻿using com.brettnamba.MlbTheShowForecaster.Common.Domain.ValueObjects;

namespace com.brettnamba.MlbTheShowForecaster.Performance.Domain.PerformanceAssessment.ValueObjects.Comparisons;

/// <summary>
/// Compares a player's batting performance before a given date to their batting performance since that date
/// </summary>
public sealed class PlayerBattingPeriodComparison : PlayerStatPeriodComparison
{
    /// <summary>
    /// The number of plate appearances before the comparison date
    /// </summary>
    public NaturalNumber PlateAppearancesBeforeComparisonDate { get; }

    /// <summary>
    /// The player's OPS before the comparison date
    /// </summary>
    public RawStat OnBasePlusSluggingBeforeComparisonDate => StatBeforeComparisonDate;

    /// <summary>
    /// The number of plate appearances since the comparison date
    /// </summary>
    public NaturalNumber PlateAppearancesSinceComparisonDate { get; }

    /// <summary>
    /// The player's OPS since the comparison date
    /// </summary>
    public RawStat OnBasePlusSluggingSinceComparisonDate => StatSinceComparisonDate;

    /// <summary>
    /// Constructor
    /// </summary>
    /// <param name="playerMlbId">The MLB ID of the player</param>
    /// <param name="comparisonDate">The date of comparison -- the stat before this date will be compared to the stat since this date</param>
    /// <param name="plateAppearancesBeforeComparisonDate">The number of plate appearances before the comparison date</param>
    /// <param name="onBasePlusSluggingBeforeComparisonDate">The player's OPS before the comparison date</param>
    /// <param name="plateAppearancesSinceComparisonDate">The number of plate appearances since the comparison date</param>
    /// <param name="onBasePlusSluggingSinceComparisonDate">The player's OPS since the comparison date</param>
    private PlayerBattingPeriodComparison(MlbId playerMlbId, DateTime comparisonDate,
        NaturalNumber plateAppearancesBeforeComparisonDate, RawStat onBasePlusSluggingBeforeComparisonDate,
        NaturalNumber plateAppearancesSinceComparisonDate, RawStat onBasePlusSluggingSinceComparisonDate) : base(
        playerMlbId, comparisonDate, onBasePlusSluggingBeforeComparisonDate, onBasePlusSluggingSinceComparisonDate)
    {
        PlateAppearancesBeforeComparisonDate = plateAppearancesBeforeComparisonDate;
        PlateAppearancesSinceComparisonDate = plateAppearancesSinceComparisonDate;
    }

    /// <summary>
    /// Creates <see cref="PlayerBattingPeriodComparison"/>
    /// </summary>
    /// <param name="playerMlbId">The MLB ID of the player</param>
    /// <param name="comparisonDate">The date of comparison -- the stat before this date will be compared to the stat since this date</param>
    /// <param name="plateAppearancesBeforeComparisonDate">The number of plate appearances before the comparison date</param>
    /// <param name="onBasePlusSluggingBeforeComparisonDate">The player's OPS before the comparison date</param>
    /// <param name="plateAppearancesSinceComparisonDate">The number of plate appearances since the comparison date</param>
    /// <param name="onBasePlusSluggingSinceComparisonDate">The player's OPS since the comparison date</param>
    /// <returns><see cref="PlayerBattingPeriodComparison"/></returns>
    public static PlayerBattingPeriodComparison Create(MlbId playerMlbId, DateTime comparisonDate,
        int plateAppearancesBeforeComparisonDate, decimal onBasePlusSluggingBeforeComparisonDate,
        int plateAppearancesSinceComparisonDate, decimal onBasePlusSluggingSinceComparisonDate)
    {
        return new PlayerBattingPeriodComparison(playerMlbId, comparisonDate,
            plateAppearancesBeforeComparisonDate: NaturalNumber.Create(plateAppearancesBeforeComparisonDate),
            onBasePlusSluggingBeforeComparisonDate: RawStat.Create(onBasePlusSluggingBeforeComparisonDate),
            plateAppearancesSinceComparisonDate: NaturalNumber.Create(plateAppearancesSinceComparisonDate),
            onBasePlusSluggingSinceComparisonDate: RawStat.Create(onBasePlusSluggingSinceComparisonDate)
        );
    }
}