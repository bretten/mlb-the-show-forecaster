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
    /// Service that assesses the performance of the player's season
    /// </summary>
    private readonly IPerformanceAssessor _performanceAssessor;

    /// <summary>
    /// Constructor
    /// </summary>
    /// <param name="performanceAssessor">Service that assesses the performance of the player's season</param>
    public PlayerSeasonScorekeeper(IPerformanceAssessor performanceAssessor)
    {
        _performanceAssessor = performanceAssessor;
    }

    /// <summary>
    /// Scores a player's season by logging new games and assessing the player's performance
    /// </summary>
    /// <param name="playerStatsBySeason">The player's season stats</param>
    /// <param name="playerBattingStatsByGamesToDate">The player's batting stats by games to date</param>
    /// <param name="playerPitchingStatsByGamesToDate">The player's pitching stats by games to date</param>
    /// <param name="playerFieldingStatsByGamesToDate">The player's fielding stats by games to date</param>
    /// <returns>The updated <see cref="PlayerStatsBySeason"/></returns>
    public PlayerStatsBySeason ScoreSeason(PlayerStatsBySeason playerStatsBySeason,
        IEnumerable<PlayerBattingStatsByGame> playerBattingStatsByGamesToDate,
        IEnumerable<PlayerPitchingStatsByGame> playerPitchingStatsByGamesToDate,
        IEnumerable<PlayerFieldingStatsByGame> playerFieldingStatsByGamesToDate)
    {
        ScoreBatting(ref playerStatsBySeason, playerBattingStatsByGamesToDate);
        ScorePitching(ref playerStatsBySeason, playerPitchingStatsByGamesToDate);
        ScoreFielding(ref playerStatsBySeason, playerFieldingStatsByGamesToDate);
        return playerStatsBySeason;
    }

    /// <summary>
    /// Logs new batting games
    /// </summary>
    /// <param name="playerStatsBySeason">The player season stats as it exists in the system currently</param>
    /// <param name="statsByGamesToDate">The most up-to-date player season stats from the external MLB source</param>
    private void ScoreBatting(ref PlayerStatsBySeason playerStatsBySeason,
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
        foreach (var statsByGame in newStatsByGames.OrderBy(x => x.GameDate))
        {
            playerStatsBySeason.LogBattingGame(statsByGame);
            if (statsByGame.GameDate.DayOfWeek == DayOfWeek.Thursday) // Assess one day before roster update day
            {
                // Assess performance to date
                playerStatsBySeason.AssessBattingPerformance(_performanceAssessor);
            }
        }
    }

    /// <summary>
    /// Logs new pitching games
    /// </summary>
    /// <param name="playerStatsBySeason">The player season stats as it exists in the system currently</param>
    /// <param name="statsByGamesToDate">The most up-to-date player season stats from the external MLB source</param>
    private void ScorePitching(ref PlayerStatsBySeason playerStatsBySeason,
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

        foreach (var statsByGame in newStatsByGames.OrderBy(x => x.GameDate))
        {
            playerStatsBySeason.LogPitchingGame(statsByGame);
            if (statsByGame.GameDate.DayOfWeek == DayOfWeek.Thursday) // Assess one day before roster update day
            {
                // Assess performance to date
                playerStatsBySeason.AssessPitchingPerformance(_performanceAssessor);
            }
        }
    }

    /// <summary>
    /// Logs new fielding games
    /// </summary>
    /// <param name="playerStatsBySeason">The player season stats as it exists in the system currently</param>
    /// <param name="statsByGamesToDate">The most up-to-date player season stats from the external MLB source</param>
    private void ScoreFielding(ref PlayerStatsBySeason playerStatsBySeason,
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

        foreach (var statsByGame in newStatsByGames.OrderBy(x => x.GameDate))
        {
            playerStatsBySeason.LogFieldingGame(statsByGame);
            if (statsByGame.GameDate.DayOfWeek == DayOfWeek.Thursday) // Assess one day before roster update day
            {
                // Assess performance to date
                playerStatsBySeason.AssessFieldingPerformance(_performanceAssessor);
            }
        }
    }
}