using com.brettnamba.MlbTheShowForecaster.ExternalApis.MlbApi.Requests;
using com.brettnamba.MlbTheShowForecaster.ExternalApis.MlbApi.Responses;
using Refit;

namespace com.brettnamba.MlbTheShowForecaster.ExternalApis.MlbApi;

/// <summary>
/// MLB Stats API client - https://statsapi.mlb.com/api
///
/// <para>Provides access to live MLB data</para>
/// </summary>
public interface IMlbApi
{
    /// <summary>
    /// Gets all players that participated in a season
    /// </summary>
    /// <param name="request">The request containing the season and game type</param>
    /// <returns>Any MLB player that participated in the specified season</returns>
    [Get("/v1/sports/1/players?season={request.Season}&gameType={request.GameType}")]
    Task<GetPlayersBySeasonResponse> GetPlayersBySeason(GetPlayersBySeasonRequest request);

    /// <summary>
    /// Gets a player's season stats by each game
    /// </summary>
    /// <param name="request">The request containing the MLB ID of the player and the season</param>
    /// <returns>The player's season stats by each game</returns>
    [Get(
        "/v1/people/{request.PlayerMlbId}?hydrate=stats(group=[hitting,pitching,fielding],type=[gameLog],season={request.Season})")]
    Task<GetPlayerSeasonStatsByGameResponse> GetPlayerSeasonStatsByGame(GetPlayerSeasonStatsByGameRequest request);
}