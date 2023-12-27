using com.brettnamba.MlbTheShowForecaster.Common.Application.Cqrs;
using com.brettnamba.MlbTheShowForecaster.PlayerStatus.Application.Commands.CreatePlayer;
using com.brettnamba.MlbTheShowForecaster.PlayerStatus.Application.Commands.UpdatePlayer;
using com.brettnamba.MlbTheShowForecaster.PlayerStatus.Application.Queries.GetPlayerByMlbId;
using com.brettnamba.MlbTheShowForecaster.PlayerStatus.Domain.Players.Entities;
using com.brettnamba.MlbTheShowForecaster.PlayerStatus.Domain.Players.Services;
using com.brettnamba.MlbTheShowForecaster.PlayerStatus.Domain.Teams.Services;

namespace com.brettnamba.MlbTheShowForecaster.PlayerStatus.Application.Services;

/// <summary>
/// Updates the status of all <see cref="Player"/>s
/// </summary>
public sealed class PlayerStatusUpdater : IPlayerStatusUpdater
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
    /// Constructor
    /// </summary>
    /// <param name="playerRoster">The roster provides a list of MLB players</param>
    /// <param name="querySender">Sends queries to retrieve state from the system</param>
    /// <param name="commandSender">Sends commands to mutate the system</param>
    /// <param name="playerStatusChangeDetector">Detects if there are any changes in a player's status</param>
    /// <param name="teamProvider">Provides information on teams</param>
    public PlayerStatusUpdater(IPlayerRoster playerRoster, IQuerySender querySender, ICommandSender commandSender,
        IPlayerStatusChangeDetector playerStatusChangeDetector, ITeamProvider teamProvider)
    {
        _playerRoster = playerRoster;
        _querySender = querySender;
        _commandSender = commandSender;
        _playerStatusChangeDetector = playerStatusChangeDetector;
        _teamProvider = teamProvider;
    }

    /// <summary>
    /// Updates player statuses
    /// </summary>
    /// <param name="cancellationToken">A <see cref="CancellationToken" /> to observe while waiting for the task to complete</param>
    public async Task UpdatePlayerStatuses(CancellationToken cancellationToken = default)
    {
        var playerStatuses = await _playerRoster.GetPlayerStatuses();

        foreach (var playerStatus in playerStatuses)
        {
            // Check if the player exists
            var existingPlayer =
                await _querySender.Send(new GetPlayerByMlbIdQuery(playerStatus.MlbId), cancellationToken);

            // Create the player if there is no existing status
            if (existingPlayer == null)
            {
                await _commandSender.Send(new CreatePlayerCommand(playerStatus), cancellationToken);
                continue;
            }

            // The player exists, so see if there are any status changes
            var detectedChanges = _playerStatusChangeDetector.DetectChanges(existingPlayer, playerStatus.Active,
                _teamProvider.GetBy(playerStatus.CurrentTeamMlbId));
            if (detectedChanges.Any())
            {
                await _commandSender.Send(new UpdatePlayerCommand(existingPlayer, detectedChanges), cancellationToken);
            }
        }
    }
}