using com.brettnamba.MlbTheShowForecaster.ExternalApis.MlbApi.Dtos.RosterEntries;
using com.brettnamba.MlbTheShowForecaster.ExternalApis.MlbApi.Dtos.Stats;
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
    /// <para>When <see cref="Refit"/> generates an implementation of this interface, this internal method will
    /// still be created, but it won't be publicly accessible. The method <see cref="GetPlayerSeasonStatsByGame"/> is
    /// the one that is exposed and uses the implementation of this method.</para>
    /// </summary>
    /// <param name="request">The request containing the MLB ID of the player and the season</param>
    /// <returns>The player's season stats by each game</returns>
    [Get(
        "/v1/people/{request.PlayerMlbId}?hydrate=stats(group=[hitting,pitching,fielding],type=[gameLog],season={request.Season})")]
    internal Task<GetPlayerSeasonStatsByGameResponse> GetPlayerSeasonStatsByGameInternal(
        GetPlayerSeasonStatsByGameRequest request);

    /// <summary>
    /// Gets a player's season stats by each game and adds the season
    /// </summary>
    /// <param name="request">The request containing the MLB ID of the player and the season</param>
    /// <returns>The player's season stats by each game</returns>
    public async Task<GetPlayerSeasonStatsByGameResponse> GetPlayerSeasonStatsByGame(
        GetPlayerSeasonStatsByGameRequest request)
    {
        var response = await GetPlayerSeasonStatsByGameInternal(request);
        var people = response.People ?? new List<PlayerSeasonStatsByGameDto>();
        var updatedPeople = people.Select(person => person with { SeasonYear = request.Season }).ToList();
        return new GetPlayerSeasonStatsByGameResponse(updatedPeople);
    }

    /// <summary>
    /// Gets a player's roster status history
    ///
    /// Follows internal pattern of <see cref="GetPlayerSeasonStatsByGameInternal"/>
    /// </summary>
    /// <param name="request">The request containing the MLB ID of the player</param>
    /// <returns>The player's roster status history</returns>
    [Get("/v1/people/{request.PlayerMlbId}?hydrate=rosterEntries")]
    internal Task<GetPlayerRosterEntriesResponse> GetPlayerRosterEntriesInternal(GetPlayerRosterEntriesRequest request);

    /// <summary>
    /// Gets a player's roster status history
    /// </summary>
    /// <param name="request">The request containing the MLB ID of the player</param>
    /// <returns>The player's roster status history</returns>
    public async Task<IEnumerable<RosterEntryDto>> GetPlayerRosterEntries(GetPlayerRosterEntriesRequest request)
    {
        var response = await GetPlayerRosterEntriesInternal(request);
        if (response.Players == null || response.Players.Count == 0)
        {
            return new List<RosterEntryDto>();
        }

        return response.Players.First().RosterEntries.OrderBy(x => x.StatusDate);
    }
}