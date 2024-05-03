﻿using System.Collections.Immutable;
using com.brettnamba.MlbTheShowForecaster.Common.Application.Cqrs;
using com.brettnamba.MlbTheShowForecaster.Common.Domain.ValueObjects;
using com.brettnamba.MlbTheShowForecaster.Performance.Application.Commands.UpdatePlayerStatsBySeason;
using com.brettnamba.MlbTheShowForecaster.Performance.Application.Dtos;
using com.brettnamba.MlbTheShowForecaster.Performance.Application.Queries.GetAllPlayerStatsBySeason;
using com.brettnamba.MlbTheShowForecaster.Performance.Application.Services.Exceptions;
using com.brettnamba.MlbTheShowForecaster.Performance.Domain.PlayerSeasons.Entities;

namespace com.brettnamba.MlbTheShowForecaster.Performance.Application.Services;

/// <summary>
/// Keeps track of live MLB stats and ensures this system has up-to-date stats
/// </summary>
public sealed class PerformanceTracker : IPerformanceTracker
{
    /// <summary>
    /// Sends queries to retrieve state from the system
    /// </summary>
    private readonly IQuerySender _querySender;

    /// <summary>
    /// Sends commands to mutate the system
    /// </summary>
    private readonly ICommandSender _commandSender;

    /// <summary>
    /// Live MLB player stats
    /// </summary>
    private readonly IPlayerStats _playerStats;

    /// <summary>
    /// Constructor
    /// </summary>
    /// <param name="querySender">Sends queries to retrieve state from the system</param>
    /// <param name="commandSender">Sends commands to mutate the system</param>
    /// <param name="playerStats">Live MLB player stats</param>
    public PerformanceTracker(IQuerySender querySender, ICommandSender commandSender, IPlayerStats playerStats)
    {
        _querySender = querySender;
        _commandSender = commandSender;
        _playerStats = playerStats;
    }

    /// <summary>
    /// Keeps track of player performance for the specified season by using live MLB data <see cref="IPlayerStats"/>.
    /// If any player's stats by season are not up-to-date with the live data, it will be updated to match the live stats
    /// </summary>
    /// <param name="seasonYear">The season to track performance for</param>
    /// <param name="cancellationToken">A <see cref="CancellationToken" /> to observe while waiting for the task to complete</param>
    /// <exception cref="PerformanceTrackerFoundNoPlayerSeasonsException">Thrown when there are no player seasons in the domain</exception>
    public async Task TrackPlayerPerformance(SeasonYear seasonYear, CancellationToken cancellationToken = default)
    {
        // Get all player seasons that are stored in the domain for the specified season
        var playerStatsBySeasons =
            (await _querySender.Send(new GetAllPlayerStatsBySeasonQuery(seasonYear), cancellationToken) ?? Array.Empty<PlayerStatsBySeason>()).ToImmutableList();

        // There should always be PlayerSeasons in the domain, or else the system has not been properly populated
        if (playerStatsBySeasons.IsEmpty)
        {
            throw new PerformanceTrackerFoundNoPlayerSeasonsException($"No PlayerSeasons found for {seasonYear.Value}");
        }

        // Make sure each player's stats by season is up-to-date with the most recent stats
        foreach (var playerStatsBySeason in playerStatsBySeasons)
        {
            // Get the most recent season stats
            var seasonToDate = await _playerStats.GetPlayerSeason(playerStatsBySeason.PlayerMlbId, seasonYear);
            // Are the stats in this system up-to-date?
            if (IsSeasonUpToDate(playerStatsBySeason, seasonToDate))
            {
                // No action or domain event needed
                continue;
            }

            // The player's season stats are not up-to-date, so update it with the new live stats
            await _commandSender.Send(new UpdatePlayerStatsBySeasonCommand(playerStatsBySeason, seasonToDate),
                cancellationToken);
        }
    }

    /// <summary>
    /// Checks to make sure if the player's stats by season (this system) is up-to-date with live stats
    /// </summary>
    /// <param name="playerStatsBySeason">The player stats by season as it exists in this system</param>
    /// <param name="seasonToDate">The player's live season stats to date</param>
    /// <returns>True if the <see cref="PlayerStatsBySeason"/> needs to be updated</returns>
    private static bool IsSeasonUpToDate(PlayerStatsBySeason playerStatsBySeason, PlayerSeason seasonToDate)
    {
        return playerStatsBySeason.BattingStatsByGamesChronologically.Count == seasonToDate.GameBattingStats.Count
               && playerStatsBySeason.PitchingStatsByGamesChronologically.Count == seasonToDate.GamePitchingStats.Count
               && playerStatsBySeason.FieldingStatsByGamesChronologically.Count == seasonToDate.GameFieldingStats.Count;
    }
}