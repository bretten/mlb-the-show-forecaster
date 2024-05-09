using com.brettnamba.MlbTheShowForecaster.Common.Domain.ValueObjects;
using com.brettnamba.MlbTheShowForecaster.ExternalApis.MlbApi;
using com.brettnamba.MlbTheShowForecaster.ExternalApis.MlbApi.Requests;
using com.brettnamba.MlbTheShowForecaster.Performance.Application.Dtos;
using com.brettnamba.MlbTheShowForecaster.Performance.Application.Services;
using com.brettnamba.MlbTheShowForecaster.Performance.Infrastructure.Dtos.Mapping;
using com.brettnamba.MlbTheShowForecaster.Performance.Infrastructure.Services.Exceptions;

namespace com.brettnamba.MlbTheShowForecaster.Performance.Infrastructure.Services;

/// <summary>
/// MLB API implementation of <see cref="IPlayerStats"/>
///
/// <para>Pulls live MLB stats from the MLB API</para>
/// </summary>
public sealed class MlbApiPlayerStats : IPlayerStats
{
    /// <summary>
    /// The <see cref="IMlbApi"/>
    /// </summary>
    private readonly IMlbApi _mlbApi;

    /// <summary>
    /// Maps the MLB API data to application level DTOs
    /// </summary>
    private readonly IMlbApiPlayerStatsMapper _mlbApiPlayerStatsMapper;

    /// <summary>
    /// Constructor
    /// </summary>
    /// <param name="mlbApi">The <see cref="IMlbApi"/></param>
    /// <param name="mlbApiPlayerStatsMapper">Maps the MLB API data to application level DTOs</param>
    public MlbApiPlayerStats(IMlbApi mlbApi, IMlbApiPlayerStatsMapper mlbApiPlayerStatsMapper)
    {
        _mlbApi = mlbApi;
        _mlbApiPlayerStatsMapper = mlbApiPlayerStatsMapper;
    }

    /// <summary>
    /// Gets a player's season stats
    /// </summary>
    /// <param name="playerMlbId">The MLB ID of the player</param>
    /// <param name="seasonYear">The season year</param>
    /// <returns>The player's season stats</returns>
    /// <exception cref="PlayerSeasonNotFoundException">Thrown if the MLB API cannot find the specified player's season stats</exception>
    public async Task<PlayerSeason> GetPlayerSeason(MlbId playerMlbId, SeasonYear seasonYear)
    {
        var request = new GetPlayerSeasonStatsByGameRequest(playerMlbId.Value, seasonYear.Value);
        var response = await _mlbApi.GetPlayerSeasonStatsByGame(request);
        var player = response.People?.FirstOrDefault(x => x.Id == playerMlbId.Value);
        if (player == null || player.Value.Id == 0) // If the DTO is a record struct, default value will be 0 on ID
        {
            throw new PlayerSeasonNotFoundException(
                $"Player season not found for player ID = {playerMlbId.Value} and season = {seasonYear.Value}");
        }

        return _mlbApiPlayerStatsMapper.Map(player.Value);
    }

    /// <summary>
    /// Dispose
    /// </summary>
    public void Dispose()
    {
        // MLB API HTTP client is handled by Refit
    }
}