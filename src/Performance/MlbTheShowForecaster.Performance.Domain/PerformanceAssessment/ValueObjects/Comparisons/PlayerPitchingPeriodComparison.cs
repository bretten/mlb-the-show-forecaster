using com.brettnamba.MlbTheShowForecaster.Common.Domain.ValueObjects;
using com.brettnamba.MlbTheShowForecaster.Common.Domain.ValueObjects.Contracts;
using com.brettnamba.MlbTheShowForecaster.Performance.Domain.Statistics.ValueObjects.Pitching;
using com.brettnamba.MlbTheShowForecaster.Performance.Domain.Statistics.ValueObjects.Shared;

namespace com.brettnamba.MlbTheShowForecaster.Performance.Domain.PerformanceAssessment.ValueObjects.Comparisons;

/// <summary>
/// Compares a player's pitching performance before a given date to their pitching performance since that date
/// </summary>
public sealed class PlayerPitchingPeriodComparison : PlayerStatPeriodComparison
{
    /// <summary>
    /// The number of innings pitched before the comparison date
    /// </summary>
    public InningsCount InningsPitchedBeforeComparisonDate { get; }

    /// <summary>
    /// The number of batters faced before the comparison date
    /// </summary>
    public NaturalNumber BattersFacedBeforeComparisonDate { get; }

    /// <summary>
    /// The player's ERA before the comparison date
    /// </summary>
    public IStat EarnedRunAverageBeforeComparisonDate => StatBeforeComparisonDate;

    /// <summary>
    /// The number of innings pitched since the comparison date
    /// </summary>
    public InningsCount InningsPitchedSinceComparisonDate { get; }

    /// <summary>
    /// The number of batters faced since the comparison date
    /// </summary>
    public NaturalNumber BattersFacedSinceComparisonDate { get; }

    /// <summary>
    /// The player's ERA since the comparison date
    /// </summary>
    public IStat EarnedRunAverageSinceComparisonDate => StatSinceComparisonDate;

    /// <summary>
    /// Constructor
    /// </summary>
    /// <param name="playerMlbId">The MLB ID of the player</param>
    /// <param name="comparisonDate">The date of comparison -- the stat before this date will be compared to the stat since this date</param>
    /// <param name="inningsPitchedBeforeComparisonDate">The number of innings pitched before the comparison date</param>
    /// <param name="battersFacedBeforeComparisonDate">The number of batters faced before the comparison date</param>
    /// <param name="earnedRunAverageBeforeComparisonDate">The player's ERA before the comparison date</param>
    /// <param name="inningsPitchedSinceComparisonDate">The number of innings pitched since the comparison date</param>
    /// <param name="battersFacedSinceComparisonDate">The number of batters faced since the comparison date</param>
    /// <param name="earnedRunAverageSinceComparisonDate">The player's ERA since the comparison date</param>
    private PlayerPitchingPeriodComparison(MlbId playerMlbId, DateOnly comparisonDate,
        InningsCount inningsPitchedBeforeComparisonDate, NaturalNumber battersFacedBeforeComparisonDate,
        IStat earnedRunAverageBeforeComparisonDate, InningsCount inningsPitchedSinceComparisonDate,
        NaturalNumber battersFacedSinceComparisonDate, IStat earnedRunAverageSinceComparisonDate) : base(playerMlbId,
        comparisonDate, earnedRunAverageBeforeComparisonDate, earnedRunAverageSinceComparisonDate)
    {
        InningsPitchedBeforeComparisonDate = inningsPitchedBeforeComparisonDate;
        BattersFacedBeforeComparisonDate = battersFacedBeforeComparisonDate;
        InningsPitchedSinceComparisonDate = inningsPitchedSinceComparisonDate;
        BattersFacedSinceComparisonDate = battersFacedSinceComparisonDate;
    }

    /// <summary>
    /// Creates <see cref="PlayerPitchingPeriodComparison"/>
    /// </summary>
    /// <param name="playerMlbId">The MLB ID of the player</param>
    /// <param name="comparisonDate">The date of comparison -- the stat before this date will be compared to the stat since this date</param>
    /// <param name="inningsPitchedBeforeComparisonDate">The number of innings pitched before the comparison date</param>
    /// <param name="battersFacedBeforeComparisonDate">The number of batters faced before the comparison date</param>
    /// <param name="earnedRunAverageBeforeComparisonDate">The player's ERA before the comparison date</param>
    /// <param name="inningsPitchedSinceComparisonDate">The number of innings pitched since the comparison date</param>
    /// <param name="battersFacedSinceComparisonDate">The number of batters faced since the comparison date</param>
    /// <param name="earnedRunAverageSinceComparisonDate">The player's ERA since the comparison date</param>
    /// <returns><see cref="PlayerPitchingPeriodComparison"/></returns>
    public static PlayerPitchingPeriodComparison Create(MlbId playerMlbId, DateOnly comparisonDate,
        decimal inningsPitchedBeforeComparisonDate, int battersFacedBeforeComparisonDate,
        decimal earnedRunAverageBeforeComparisonDate, decimal inningsPitchedSinceComparisonDate,
        int battersFacedSinceComparisonDate, decimal earnedRunAverageSinceComparisonDate)
    {
        return new PlayerPitchingPeriodComparison(playerMlbId, comparisonDate,
            inningsPitchedBeforeComparisonDate: InningsCount.Create(inningsPitchedBeforeComparisonDate),
            battersFacedBeforeComparisonDate: NaturalNumber.Create(battersFacedBeforeComparisonDate),
            earnedRunAverageBeforeComparisonDate: RawStat.Create(earnedRunAverageBeforeComparisonDate),
            inningsPitchedSinceComparisonDate: InningsCount.Create(inningsPitchedSinceComparisonDate),
            battersFacedSinceComparisonDate: NaturalNumber.Create(battersFacedSinceComparisonDate),
            earnedRunAverageSinceComparisonDate: RawStat.Create(earnedRunAverageSinceComparisonDate)
        );
    }

    /// <summary>
    /// Creates <see cref="PlayerPitchingPeriodComparison"/>
    /// </summary>
    /// <param name="playerMlbId">The MLB ID of the player</param>
    /// <param name="comparisonDate">The date of comparison -- stats before this date will be compared to stats since this date</param>
    /// <param name="statsBeforeComparisonDate">Stats from before the comparison date</param>
    /// <param name="statsSinceComparisonDate">Stats since the comparison date</param>
    /// <returns><see cref="PlayerPitchingPeriodComparison"/></returns>
    public static PlayerPitchingPeriodComparison Create(MlbId playerMlbId, DateOnly comparisonDate,
        PitchingStats statsBeforeComparisonDate, PitchingStats statsSinceComparisonDate)
    {
        return new PlayerPitchingPeriodComparison(playerMlbId, comparisonDate,
            inningsPitchedBeforeComparisonDate: statsBeforeComparisonDate.InningsPitched,
            battersFacedBeforeComparisonDate: statsBeforeComparisonDate.BattersFaced,
            earnedRunAverageBeforeComparisonDate: statsBeforeComparisonDate.EarnedRunAverage,
            inningsPitchedSinceComparisonDate: statsSinceComparisonDate.InningsPitched,
            battersFacedSinceComparisonDate: statsSinceComparisonDate.BattersFaced,
            earnedRunAverageSinceComparisonDate: statsSinceComparisonDate.EarnedRunAverage
        );
    }
}