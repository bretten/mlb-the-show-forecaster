using com.brettnamba.MlbTheShowForecaster.Common.Domain.ValueObjects;
using com.brettnamba.MlbTheShowForecaster.GameCards.Application.Dtos;

namespace com.brettnamba.MlbTheShowForecaster.GameCards.Application.Services;

/// <summary>
/// Should define a service that tracks which roster updates are available and which have yet to be applied
/// to the domain
/// </summary>
public interface IRosterUpdateFeed : IDisposable
{
    /// <summary>
    /// Should provide a collection of roster updates that have not been applied to the domain yet
    /// </summary>
    /// <param name="seasonYear">The season to check for roster updates</param>
    /// <param name="cancellationToken">A <see cref="CancellationToken" /> to observe while waiting for the task to complete</param>
    /// <returns>New roster updates</returns>
    Task<AllRosterUpdates> GetNewRosterUpdates(SeasonYear seasonYear, CancellationToken cancellationToken = default);

    /// <summary>
    /// Should mark a roster update as complete, so it will no longer be considered a new roster update
    /// </summary>
    /// <param name="rosterUpdate">The roster update to mark as complete</param>
    /// <param name="cancellationToken">A <see cref="CancellationToken" /> to observe while waiting for the task to complete</param>
    /// <returns>The completed task</returns>
    Task CompleteRosterUpdate(RosterUpdate rosterUpdate, CancellationToken cancellationToken = default);
}