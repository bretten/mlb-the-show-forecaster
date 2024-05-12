using com.brettnamba.MlbTheShowForecaster.Common.Domain.ValueObjects;
using com.brettnamba.MlbTheShowForecaster.Common.Domain.ValueObjects.Contracts;
using com.brettnamba.MlbTheShowForecaster.Performance.Domain.Statistics.ValueObjects.Fielding;

namespace com.brettnamba.MlbTheShowForecaster.Performance.Domain.PerformanceAssessment.ValueObjects.Comparisons;

/// <summary>
/// Compares a player's fielding performance before a given date to their fielding performance since that date
/// </summary>
public sealed class PlayerFieldingPeriodComparison : PlayerStatPeriodComparison
{
    /// <summary>
    /// The number of fielding chances before the comparison date
    /// </summary>
    public NaturalNumber TotalChancesBeforeComparisonDate { get; }

    /// <summary>
    /// The player's fielding percentage before the comparison date
    /// </summary>
    public IStat FieldingPercentageBeforeComparisonDate => StatBeforeComparisonDate;

    /// <summary>
    /// The number of fielding chances since the comparison date
    /// </summary>
    public NaturalNumber TotalChancesSinceComparisonDate { get; }

    /// <summary>
    /// The player's fielding percentage since the comparison date
    /// </summary>
    public IStat FieldingPercentageSinceComparisonDate => StatSinceComparisonDate;

    /// <summary>
    /// Constructor
    /// </summary>
    /// <param name="playerMlbId">The MLB ID of the player</param>
    /// <param name="comparisonDate">The date of comparison -- the stat before this date will be compared to the stat since this date</param>
    /// <param name="totalChancesBeforeComparisonDate">The number of fielding chances before the comparison date</param>
    /// <param name="fieldingPercentageBeforeComparisonDate">The player's fielding percentage before the comparison date</param>
    /// <param name="totalChancesSinceComparisonDate">The number of fielding chances since the comparison date</param>
    /// <param name="fieldingPercentageSinceComparisonDate">The player's fielding percentage since the comparison date</param>
    private PlayerFieldingPeriodComparison(MlbId playerMlbId, DateOnly comparisonDate,
        NaturalNumber totalChancesBeforeComparisonDate, IStat fieldingPercentageBeforeComparisonDate,
        NaturalNumber totalChancesSinceComparisonDate, IStat fieldingPercentageSinceComparisonDate) : base(
        playerMlbId, comparisonDate, fieldingPercentageBeforeComparisonDate, fieldingPercentageSinceComparisonDate)
    {
        TotalChancesBeforeComparisonDate = totalChancesBeforeComparisonDate;
        TotalChancesSinceComparisonDate = totalChancesSinceComparisonDate;
    }

    /// <summary>
    /// Creates <see cref="PlayerFieldingPeriodComparison"/>
    /// </summary>
    /// <param name="playerMlbId">The MLB ID of the player</param>
    /// <param name="comparisonDate">The date of comparison -- the stat before this date will be compared to the stat since this date</param>
    /// <param name="totalChancesBeforeComparisonDate">The number of fielding chances before the comparison date</param>
    /// <param name="fieldingPercentageBeforeComparisonDate">The player's fielding percentage before the comparison date</param>
    /// <param name="totalChancesSinceComparisonDate">The number of fielding chances since the comparison date</param>
    /// <param name="fieldingPercentageSinceComparisonDate">The player's fielding percentage since the comparison date</param>
    /// <returns><see cref="PlayerFieldingPeriodComparison"/></returns>
    public static PlayerFieldingPeriodComparison Create(MlbId playerMlbId, DateOnly comparisonDate,
        int totalChancesBeforeComparisonDate, decimal fieldingPercentageBeforeComparisonDate,
        int totalChancesSinceComparisonDate, decimal fieldingPercentageSinceComparisonDate)
    {
        return new PlayerFieldingPeriodComparison(playerMlbId, comparisonDate,
            totalChancesBeforeComparisonDate: NaturalNumber.Create(totalChancesBeforeComparisonDate),
            fieldingPercentageBeforeComparisonDate: RawStat.Create(fieldingPercentageBeforeComparisonDate),
            totalChancesSinceComparisonDate: NaturalNumber.Create(totalChancesSinceComparisonDate),
            fieldingPercentageSinceComparisonDate: RawStat.Create(fieldingPercentageSinceComparisonDate)
        );
    }

    /// <summary>
    /// Creates <see cref="PlayerFieldingPeriodComparison"/>
    /// </summary>
    /// <param name="playerMlbId">The MLB ID of the player</param>
    /// <param name="comparisonDate">The date of comparison -- stats before this date will be compared to stats since this date</param>
    /// <param name="statsBeforeComparisonDate">Stats from before the comparison date</param>
    /// <param name="statsSinceComparisonDate">Stats since the comparison date</param>
    /// <returns><see cref="PlayerFieldingPeriodComparison"/></returns>
    public static PlayerFieldingPeriodComparison Create(MlbId playerMlbId, DateOnly comparisonDate,
        FieldingStats statsBeforeComparisonDate, FieldingStats statsSinceComparisonDate)
    {
        return new PlayerFieldingPeriodComparison(playerMlbId, comparisonDate,
            totalChancesBeforeComparisonDate: statsBeforeComparisonDate.TotalChances.ToNaturalNumber(),
            fieldingPercentageBeforeComparisonDate: statsBeforeComparisonDate.FieldingPercentage,
            totalChancesSinceComparisonDate: statsSinceComparisonDate.TotalChances.ToNaturalNumber(),
            fieldingPercentageSinceComparisonDate: statsSinceComparisonDate.FieldingPercentage
        );
    }
}