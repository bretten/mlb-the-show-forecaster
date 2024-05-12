using com.brettnamba.MlbTheShowForecaster.Performance.Domain.PerformanceAssessment.Services;
using com.brettnamba.MlbTheShowForecaster.Performance.Domain.PlayerSeasons.Entities;
using com.brettnamba.MlbTheShowForecaster.Performance.Domain.PlayerSeasons.ValueObjects;

namespace com.brettnamba.MlbTheShowForecaster.Performance.Domain.PlayerSeasons.Services;

/// <summary>
/// Service that will score a player's season -- it will log new games for the season and assess the player's
/// performance to date
/// </summary>
public sealed class PlayerSeasonScorekeeper : IPlayerSeasonScorekeeper
{
    /// <summary>
    /// Performance assessment requirements
    /// </summary>
    private readonly IPerformanceAssessmentRequirements _performanceAssessmentRequirements;

    /// <summary>
    /// Constructor
    /// </summary>
    /// <param name="performanceAssessmentRequirements">Performance assessment requirements</param>
    public PlayerSeasonScorekeeper(IPerformanceAssessmentRequirements performanceAssessmentRequirements)
    {
        _performanceAssessmentRequirements = performanceAssessmentRequirements;
    }

    /// <summary>
    /// Scores a player's season by logging new games and assessing the player's performance
    /// </summary>
    /// <param name="playerStatsBySeason">The player's season stats</param>
    /// <param name="performanceComparisonDate">The date used in the player's performance assessment</param>
    /// <param name="playerBattingStatsByGamesToDate">The player's batting stats by games to date</param>
    /// <param name="playerPitchingStatsByGamesToDate">The player's pitching stats by games to date</param>
    /// <param name="playerFieldingStatsByGamesToDate">The player's fielding stats by games to date</param>
    /// <returns>The updated <see cref="PlayerStatsBySeason"/></returns>
    public PlayerStatsBySeason ScoreSeason(PlayerStatsBySeason playerStatsBySeason, DateOnly performanceComparisonDate,
        IEnumerable<PlayerBattingStatsByGame> playerBattingStatsByGamesToDate,
        IEnumerable<PlayerPitchingStatsByGame> playerPitchingStatsByGamesToDate,
        IEnumerable<PlayerFieldingStatsByGame> playerFieldingStatsByGamesToDate)
    {
        ScoreBatting(ref playerStatsBySeason, performanceComparisonDate, playerBattingStatsByGamesToDate);
        ScorePitching(ref playerStatsBySeason, performanceComparisonDate, playerPitchingStatsByGamesToDate);
        ScoreFielding(ref playerStatsBySeason, performanceComparisonDate, playerFieldingStatsByGamesToDate);
        return playerStatsBySeason;
    }

    /// <summary>
    /// Logs new batting games
    /// </summary>
    /// <param name="playerStatsBySeason">The player season stats as it exists in the system currently</param>
    /// <param name="performanceComparisonDate">The date used in the player's performance assessment</param>
    /// <param name="statsByGamesToDate">The most up-to-date player season stats from the external MLB source</param>
    private void ScoreBatting(ref PlayerStatsBySeason playerStatsBySeason, DateOnly performanceComparisonDate,
        IEnumerable<PlayerBattingStatsByGame> statsByGamesToDate)
    {
        // The stats by games before the most recent update
        var previousStatsByGames = playerStatsBySeason.BattingStatsByGamesChronologically;

        // Get stats by games that have not yet been recorded by the scorekeeper
        var newStatsByGames = statsByGamesToDate.Except(previousStatsByGames).ToList();
        if (!newStatsByGames.Any())
        {
            return;
        }

        // Log new games
        foreach (var statsByGame in newStatsByGames)
        {
            playerStatsBySeason.LogBattingGame(statsByGame);
        }

        // Assess performance to date
        playerStatsBySeason.AssessBattingPerformance(performanceComparisonDate, _performanceAssessmentRequirements);
    }

    /// <summary>
    /// Logs new pitching games
    /// </summary>
    /// <param name="playerStatsBySeason">The player season stats as it exists in the system currently</param>
    /// <param name="performanceComparisonDate">The date used in the player's performance assessment</param>
    /// <param name="statsByGamesToDate">The most up-to-date player season stats from the external MLB source</param>
    private void ScorePitching(ref PlayerStatsBySeason playerStatsBySeason, DateOnly performanceComparisonDate,
        IEnumerable<PlayerPitchingStatsByGame> statsByGamesToDate)
    {
        // The stats by games before the most recent update
        var previousStatsByGames = playerStatsBySeason.PitchingStatsByGamesChronologically;

        // Get stats by games that have not yet been recorded by the scorekeeper
        var newStatsByGames = statsByGamesToDate.Except(previousStatsByGames).ToList();
        if (!newStatsByGames.Any())
        {
            return;
        }

        // Log new games
        foreach (var statsByGame in newStatsByGames)
        {
            playerStatsBySeason.LogPitchingGame(statsByGame);
        }

        // Assess performance to date
        playerStatsBySeason.AssessPitchingPerformance(performanceComparisonDate, _performanceAssessmentRequirements);
    }

    /// <summary>
    /// Logs new fielding games
    /// </summary>
    /// <param name="playerStatsBySeason">The player season stats as it exists in the system currently</param>
    /// <param name="performanceComparisonDate">The date used in the player's performance assessment</param>
    /// <param name="statsByGamesToDate">The most up-to-date player season stats from the external MLB source</param>
    private void ScoreFielding(ref PlayerStatsBySeason playerStatsBySeason, DateOnly performanceComparisonDate,
        IEnumerable<PlayerFieldingStatsByGame> statsByGamesToDate)
    {
        // The stats by games before the most recent update
        var previousStatsByGames = playerStatsBySeason.FieldingStatsByGamesChronologically;

        // Get stats by games that have not yet been recorded by the scorekeeper
        var newStatsByGames = statsByGamesToDate.Except(previousStatsByGames).ToList();
        if (!newStatsByGames.Any())
        {
            return;
        }

        // Log new games
        foreach (var statsByGame in newStatsByGames)
        {
            playerStatsBySeason.LogFieldingGame(statsByGame);
        }

        // Assess performance to date
        playerStatsBySeason.AssessFieldingPerformance(performanceComparisonDate, _performanceAssessmentRequirements);
    }
}