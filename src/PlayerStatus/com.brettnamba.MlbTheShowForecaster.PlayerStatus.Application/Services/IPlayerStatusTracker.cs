﻿using com.brettnamba.MlbTheShowForecaster.PlayerStatus.Domain.Players.Entities;

namespace com.brettnamba.MlbTheShowForecaster.PlayerStatus.Application.Services;

/// <summary>
/// Defines a service that updates the status of all <see cref="Player"/>s
/// </summary>
public interface IPlayerStatusTracker
{
    /// <summary>
    /// Updates the status of all players
    /// </summary>
    /// <param name="cancellationToken">A <see cref="CancellationToken" /> to observe while waiting for the task to complete</param>
    /// <returns>The completed task</returns>
    Task TrackPlayers(CancellationToken cancellationToken = default);
}