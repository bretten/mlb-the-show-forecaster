using com.brettnamba.MlbTheShowForecaster.Common.Domain.ValueObjects;
using com.brettnamba.MlbTheShowForecaster.GameCards.Application.Services.Results;

namespace com.brettnamba.MlbTheShowForecaster.GameCards.Application.Services;

/// <summary>
/// Defines a service that applies roster updates to the domain
/// </summary>
public interface IRosterUpdateOrchestrator
{
    /// <summary>
    /// Should apply roster updates to the domain
    /// </summary>
    /// <param name="seasonYear">The season to apply roster updates for</param>
    /// <param name="cancellationToken">A <see cref="CancellationToken" /> to observe while waiting for the task to complete</param>
    /// <returns>The completed task</returns>
    Task<IEnumerable<RosterUpdateOrchestratorResult>> SyncRosterUpdates(SeasonYear seasonYear,
        CancellationToken cancellationToken = default);
}