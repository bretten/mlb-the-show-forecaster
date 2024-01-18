using com.brettnamba.MlbTheShowForecaster.PlayerStatus.Application.Dtos;

namespace com.brettnamba.MlbTheShowForecaster.PlayerStatus.Application.Services;

/// <summary>
/// Defines a service that returns the status of all players in the MLB
/// </summary>
public interface IPlayerRoster
{
    /// <summary>
    /// Returns roster information on all players in the MLB for the specified season year
    /// </summary>
    /// <param name="seasonYear">The season to get roster entries for</param>
    /// <param name="cancellationToken">A <see cref="CancellationToken" /> to observe while waiting for the task to complete</param>
    /// <returns>Roster information on all players in the MLB for the specified season year</returns>
    Task<IEnumerable<RosterEntry>> GetRosterEntries(int seasonYear, CancellationToken cancellationToken = default);
}