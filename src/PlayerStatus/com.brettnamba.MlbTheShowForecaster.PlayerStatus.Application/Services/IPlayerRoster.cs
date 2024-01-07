using com.brettnamba.MlbTheShowForecaster.PlayerStatus.Application.Dtos;

namespace com.brettnamba.MlbTheShowForecaster.PlayerStatus.Application.Services;

/// <summary>
/// Defines a service that returns the status of all players in the MLB
/// </summary>
public interface IPlayerRoster
{
    /// <summary>
    /// Returns information on all players in the MLB
    /// </summary>
    /// <param name="seasonYear">The season to get get roster entries for</param>
    /// <param name="cancellationToken">A <see cref="CancellationToken" /> to observe while waiting for the task to complete</param>
    /// <returns>Information on all players in the MLB</returns>
    Task<IEnumerable<RosterEntry>> GetRosterEntries(int seasonYear, CancellationToken cancellationToken = default);
}