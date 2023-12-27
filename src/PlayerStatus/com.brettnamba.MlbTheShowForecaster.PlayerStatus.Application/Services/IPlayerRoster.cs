namespace com.brettnamba.MlbTheShowForecaster.PlayerStatus.Application.Services;

/// <summary>
/// Defines a service that returns the status of all players in the MLB
/// </summary>
public interface IPlayerRoster
{
    /// <summary>
    /// Returns the status of all players in the MLB
    /// </summary>
    /// <param name="cancellationToken">A <see cref="CancellationToken" /> to observe while waiting for the task to complete</param>
    /// <returns>The status of all MLB players</returns>
    Task<IEnumerable<Dtos.PlayerStatus>> GetPlayerStatuses(CancellationToken cancellationToken = default);
}