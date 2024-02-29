using com.brettnamba.MlbTheShowForecaster.Common.Domain.ValueObjects;

namespace com.brettnamba.MlbTheShowForecaster.Performance.Application.Dtos;

/// <summary>
/// A player's season and stats
/// </summary>
/// <param name="PlayerMlbId">The MLB ID of the player</param>
/// <param name="SeasonYear">The season year</param>
/// <param name="GameBattingStats">The player's batting stats by game</param>
/// <param name="GamePitchingStats">The player's pitching stats by game</param>
/// <param name="GameFieldingStats">The player's fielding stats by game</param>
public readonly record struct PlayerSeason(
    MlbId PlayerMlbId,
    SeasonYear SeasonYear,
    List<PlayerGameBattingStats> GameBattingStats,
    List<PlayerGamePitchingStats> GamePitchingStats,
    List<PlayerGameFieldingStats> GameFieldingStats);