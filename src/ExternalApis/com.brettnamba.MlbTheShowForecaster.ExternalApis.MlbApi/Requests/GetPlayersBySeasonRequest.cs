namespace com.brettnamba.MlbTheShowForecaster.ExternalApis.MlbApi.Requests;

/// <summary>
/// Request for getting all players by season
/// /v1/sports/1/players?season={request.Season}&gameType={request.GameType}
/// </summary>
public sealed class GetPlayersBySeasonRequest
{
    /// <summary>
    /// The season year (eg, 2023)
    /// </summary>
    public int Season { get; init; }

    /// <summary>
    /// The type of game:
    /// R = regular season
    /// P = playoffs
    /// </summary>
    public string? GameType { get; init; }
}