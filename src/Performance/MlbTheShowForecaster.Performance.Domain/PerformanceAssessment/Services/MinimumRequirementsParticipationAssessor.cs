using com.brettnamba.MlbTheShowForecaster.Performance.Domain.Statistics.ValueObjects.Batting;
using com.brettnamba.MlbTheShowForecaster.Performance.Domain.Statistics.ValueObjects.Fielding;
using com.brettnamba.MlbTheShowForecaster.Performance.Domain.Statistics.ValueObjects.Pitching;

namespace com.brettnamba.MlbTheShowForecaster.Performance.Domain.PerformanceAssessment.Services;

/// <summary>
/// Assesses participation based off of expected minimum requirements
/// </summary>
public class MinimumRequirementsParticipationAssessor : IParticipationAssessor
{
    /// <summary>
    /// Historical average of the period of time a season covers
    /// </summary>
    private const int DaysInASeason = 189;

    /// <summary>
    /// Historical average of the number of games in a season
    /// </summary>
    private const int GamesInASeason = 162;

    /// <summary>
    /// The estimated number of games per day since every team has different schedules with off days. Used to estimate
    /// the number of games played when in the middle of the season and when games played is not available
    /// </summary>
    private const double GamesPerDay = (double)GamesInASeason / DaysInASeason;

    /// <summary>
    /// For a batter, the minimum number of plate appearances per game required for participation to be significant
    /// enough to warrant evaluation
    /// Defined by https://baseballsavant.mlb.com/statcast_leaderboard
    /// </summary>
    private const double MinimumPlateAppearancesPerGame = 2.1;

    /// <summary>
    /// For a pitcher, the minimum number of batters faced per game required for participation to be significant
    /// enough to warrant evaluation
    /// Defined by https://baseballsavant.mlb.com/statcast_leaderboard
    /// </summary>
    private const double MinimumBattersFacedPerGame = 1.25;

    /// <summary>
    /// For a fielder, the minimum number of fielding attempts per game required for participation to be significant
    /// enough to warrant evaluation
    /// Defined by https://baseballsavant.mlb.com/leaderboard/outs_above_average
    /// </summary>
    private const double MinimumFieldingAttemptsPerGame = 0.5;

    /// <inheritdoc />
    public bool AssessBatting(DateOnly start, DateOnly end, BattingStats stats)
    {
        var estimatedGamesPlayed = EstimateGamesPlayed(start, end);
        var requiredPlateAppearances = MinimumPlateAppearancesPerGame * estimatedGamesPlayed;

        return stats.PlateAppearances.Value >= requiredPlateAppearances;
    }

    /// <inheritdoc />
    public bool AssessPitching(DateOnly start, DateOnly end, PitchingStats stats)
    {
        var estimatedGamesPlayed = EstimateGamesPlayed(start, end);
        var requiredBattersFaced = MinimumBattersFacedPerGame * estimatedGamesPlayed;

        return stats.BattersFaced.Value >= requiredBattersFaced;
    }

    /// <inheritdoc />
    public bool AssessFielding(DateOnly start, DateOnly end, FieldingStats stats)
    {
        var estimatedGamesPlayed = EstimateGamesPlayed(start, end);
        var requiredFieldingAttempts = MinimumFieldingAttemptsPerGame * estimatedGamesPlayed;

        return stats.TotalChances.Value >= (decimal)requiredFieldingAttempts;
    }

    /// <summary>
    /// Estimates the number of games played for a time period
    /// </summary>
    /// <param name="start">The start of the time period, inclusive</param>
    /// <param name="end">The end of the time period, inclusive</param>
    /// <returns>The estimated number of games played for the time period</returns>
    private static double EstimateGamesPlayed(DateOnly start, DateOnly end)
    {
        var numberOfDays = end.DayNumber - start.DayNumber + 1; // +1 to include end of last date
        return numberOfDays * GamesPerDay;
    }
}