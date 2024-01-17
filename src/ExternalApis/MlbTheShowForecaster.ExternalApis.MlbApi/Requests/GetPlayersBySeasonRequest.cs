using com.brettnamba.MlbTheShowForecaster.ExternalApis.MlbApi.Dtos.Enums;

namespace com.brettnamba.MlbTheShowForecaster.ExternalApis.MlbApi.Requests;

/// <summary>
/// Request for getting all players by season
/// /v1/sports/1/players?season={request.Season}&gameType={request.GameType}
/// </summary>
/// <param name="Season">The season year (eg, 2023)</param>
/// <param name="GameType">The type of game: R = regular season, P = playoffs</param>
public sealed record GetPlayersBySeasonRequest(int Season, GameType GameType);