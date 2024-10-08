﻿using System.Runtime.CompilerServices;
using System.Threading.Tasks.Dataflow;
using com.brettnamba.MlbTheShowForecaster.Common.Domain.ValueObjects;
using com.brettnamba.MlbTheShowForecaster.ExternalApis.MlbApi;
using com.brettnamba.MlbTheShowForecaster.ExternalApis.MlbApi.Dtos;
using com.brettnamba.MlbTheShowForecaster.ExternalApis.MlbApi.Dtos.Enums;
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
    /// Gets season stats for all players in the specified year
    /// </summary>
    /// <param name="seasonYear">The season to get stats for</param>
    /// <param name="cancellationToken">A <see cref="CancellationToken" /> to observe while waiting for the task to complete</param>
    /// <returns>Stats for all players in the specified year</returns>
    public async IAsyncEnumerable<PlayerSeason> GetAllPlayerStatsFor(SeasonYear seasonYear,
        [EnumeratorCancellation] CancellationToken cancellationToken)
    {
        var playersResponse =
            await _mlbApi.GetPlayersBySeason(new GetPlayersBySeasonRequest(seasonYear.Value, GameType.RegularSeason));

        if (playersResponse.Players == null || playersResponse.Players.Count == 0)
        {
            throw new NoPlayerSeasonsFoundException($"No players found for {seasonYear.Value}");
        }

        // Yield results as they become available with transform block
        var block = new TransformBlock<PlayerDto, PlayerSeason>(
            async player => await GetPlayerSeason(MlbId.Create(player.Id), seasonYear),
            new ExecutionDataflowBlockOptions()
            {
                CancellationToken = cancellationToken,
                MaxDegreeOfParallelism = Environment.ProcessorCount
            });

        // Pass the input to be used in the block
        foreach (var player in playersResponse.Players)
        {
            block.Post(player);
        }

        block.Complete();

        // Yield results as they are available
        while (await block.OutputAvailableAsync(cancellationToken))
        {
            while (block.TryReceive(out var playerSeason))
            {
                yield return playerSeason;
            }
        }

        await block.Completion;
    }

    /// <summary>
    /// Gets a player's season stats
    /// </summary>
    /// <param name="playerMlbId">The MLB ID of the player</param>
    /// <param name="seasonYear">The season year</param>
    /// <returns>The player's season stats</returns>
    /// <exception cref="PlayerSeasonNotFoundException">Thrown if the MLB API cannot find the specified player's season stats</exception>
    private async Task<PlayerSeason> GetPlayerSeason(MlbId playerMlbId, SeasonYear seasonYear)
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