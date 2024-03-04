using com.brettnamba.MlbTheShowForecaster.Performance.Domain.PlayerSeasons.Entities;
using com.brettnamba.MlbTheShowForecaster.Performance.Domain.PlayerSeasons.ValueObjects;

namespace com.brettnamba.MlbTheShowForecaster.Performance.Domain.PlayerSeasons.Services;

/// <summary>
/// Defines a service that will score a player's season -- it will log new games for the season and assess the player's
/// performance to date
/// </summary>
public interface IPlayerSeasonScorekeeper
{
    /// <summary>
    /// Should score a player's season by logging new games and assessing the player's performance
    /// </summary>
    /// <param name="playerStatsBySeason">The player's season stats</param>
    /// <param name="performanceComparisonDate">The date used in the player's performance assessment</param>
    /// <param name="playerBattingStatsByGamesToDate">The player's batting stats by games to date</param>
    /// <param name="playerPitchingStatsByGamesToDate">The player's pitching stats by games to date</param>
    /// <param name="playerFieldingStatsByGamesToDate">The player's fielding stats by games to date</param>
    /// <returns>The updated <see cref="PlayerStatsBySeason"/></returns>
    PlayerStatsBySeason ScoreSeason(PlayerStatsBySeason playerStatsBySeason, DateTime performanceComparisonDate,
        IEnumerable<PlayerBattingStatsByGame> playerBattingStatsByGamesToDate,
        IEnumerable<PlayerPitchingStatsByGame> playerPitchingStatsByGamesToDate,
        IEnumerable<PlayerFieldingStatsByGame> playerFieldingStatsByGamesToDate);
}