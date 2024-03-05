namespace com.brettnamba.MlbTheShowForecaster.ExternalApis.MlbApi.Requests;

/// <summary>
/// Request for getting a player's season stats by game
/// </summary>
/// <param name="PlayerMlbId">The MLB ID of the player</param>
/// <param name="Season">The season</param>
public sealed record GetPlayerSeasonStatsByGameRequest(int PlayerMlbId, int Season);