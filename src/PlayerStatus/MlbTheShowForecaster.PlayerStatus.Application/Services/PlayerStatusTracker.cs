using System.Collections.Immutable;
using com.brettnamba.MlbTheShowForecaster.Common.Application.Cqrs;
using com.brettnamba.MlbTheShowForecaster.Common.DateAndTime;
using com.brettnamba.MlbTheShowForecaster.Common.Domain.ValueObjects;
using com.brettnamba.MlbTheShowForecaster.PlayerStatus.Application.Commands.CreatePlayer;
using com.brettnamba.MlbTheShowForecaster.PlayerStatus.Application.Commands.UpdatePlayer;
using com.brettnamba.MlbTheShowForecaster.PlayerStatus.Application.Queries.GetPlayerByMlbId;
using com.brettnamba.MlbTheShowForecaster.PlayerStatus.Application.Services.Exceptions;
using com.brettnamba.MlbTheShowForecaster.PlayerStatus.Application.Services.Results;
using com.brettnamba.MlbTheShowForecaster.PlayerStatus.Domain.Players.Entities;
using com.brettnamba.MlbTheShowForecaster.PlayerStatus.Domain.Players.Services;
using com.brettnamba.MlbTheShowForecaster.PlayerStatus.Domain.Teams.Services;

namespace com.brettnamba.MlbTheShowForecaster.PlayerStatus.Application.Services;

/// <summary>
/// Updates the status of all <see cref="Player"/>s
/// </summary>
public sealed class PlayerStatusTracker : IPlayerStatusTracker
{
    /// <summary>
    /// The roster provides a list of MLB players
    /// </summary>
    private readonly IPlayerRoster _playerRoster;

    /// <summary>
    /// Sends queries to retrieve state from the system
    /// </summary>
    private readonly IQuerySender _querySender;

    /// <summary>
    /// Sends commands to mutate the system
    /// </summary>
    private readonly ICommandSender _commandSender;

    /// <summary>
    /// Detects if there are any changes in a player's status
    /// </summary>
    private readonly IPlayerStatusChangeDetector _playerStatusChangeDetector;

    /// <summary>
    /// Provides information on teams
    /// </summary>
    private readonly ITeamProvider _teamProvider;

    /// <summary>
    /// Gets today's date
    /// </summary>
    private readonly ICalendar _calendar;

    /// <summary>
    /// Constructor
    /// </summary>
    /// <param name="playerRoster">The roster provides a list of MLB players</param>
    /// <param name="querySender">Sends queries to retrieve state from the system</param>
    /// <param name="commandSender">Sends commands to mutate the system</param>
    /// <param name="playerStatusChangeDetector">Detects if there are any changes in a player's status</param>
    /// <param name="teamProvider">Provides information on teams</param>
    /// <param name="calendar">Gets today's date</param>
    public PlayerStatusTracker(IPlayerRoster playerRoster, IQuerySender querySender, ICommandSender commandSender,
        IPlayerStatusChangeDetector playerStatusChangeDetector, ITeamProvider teamProvider, ICalendar calendar)
    {
        _playerRoster = playerRoster;
        _querySender = querySender;
        _commandSender = commandSender;
        _playerStatusChangeDetector = playerStatusChangeDetector;
        _teamProvider = teamProvider;
        _calendar = calendar;
    }

    /// <summary>
    /// Updates players
    /// </summary>
    /// <param name="seasonYear">The season that the players participated in</param>
    /// <param name="cancellationToken">A <see cref="CancellationToken" /> to observe while waiting for the task to complete</param>
    /// <exception cref="PlayerStatusTrackerFoundNoRosterEntriesException">Thrown when no roster entries were found from the external source <see cref="IPlayerRoster"/></exception>
    public async Task<PlayerStatusTrackerResult> TrackPlayers(SeasonYear seasonYear,
        CancellationToken cancellationToken = default)
    {
        // Get roster entries from the external player roster source
        var rosterEntries = (await _playerRoster.GetRosterEntries(seasonYear, cancellationToken)).ToImmutableList();

        // It is not a real world scenario for the external source to have no roster entries
        if (rosterEntries.IsEmpty)
        {
            throw new PlayerStatusTrackerFoundNoRosterEntriesException(
                $"No roster entries were found for {seasonYear}");
        }

        // Make sure each player exists in the domain and is up-to-date
        var newPlayers = 0;
        var updatedPlayers = 0;
        foreach (var rosterEntry in rosterEntries)
        {
            // Check if the player exists for the roster entry
            var existingPlayer =
                await _querySender.Send(new GetPlayerByMlbIdQuery(rosterEntry.MlbId), cancellationToken);

            // Create the player if they don't exist
            if (existingPlayer == null)
            {
                await _commandSender.Send(new CreatePlayerCommand(rosterEntry), cancellationToken);
                newPlayers++;
                continue;
            }

            // The player exists, so see if there are any status changes
            var detectedChanges = _playerStatusChangeDetector.DetectChanges(existingPlayer, rosterEntry.Active,
                _teamProvider.GetBy(rosterEntry.CurrentTeamMlbId));
            if (detectedChanges.Any())
            {
                await _commandSender.Send(
                    new UpdatePlayerCommand(seasonYear, existingPlayer, detectedChanges, _calendar.Today()),
                    cancellationToken);
                updatedPlayers++;
            }
        }

        return new PlayerStatusTrackerResult(TotalRosterEntries: rosterEntries.Count,
            TotalNewPlayers: newPlayers,
            TotalUpdatedPlayers: updatedPlayers
        );
    }

    /// <summary>
    /// Dispose
    /// </summary>
    public void Dispose()
    {
        _playerRoster.Dispose();
    }
}