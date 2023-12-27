namespace com.brettnamba.MlbTheShowForecaster.PlayerStatus.Application.Services;

/// <summary>
/// Defines a service that returns the status of all players in the MLB
/// </summary>
public interface IPlayerRoster
{
    /// <summary>
    /// Returns the status of all players in the MLB
    /// </summary>
    /// <returns>The status of all MLB players</returns>
    Task<IEnumerable<Dtos.PlayerStatus>> GetPlayerStatuses();
}