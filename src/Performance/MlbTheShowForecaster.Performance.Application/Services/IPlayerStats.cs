using com.brettnamba.MlbTheShowForecaster.Common.Domain.ValueObjects;
using com.brettnamba.MlbTheShowForecaster.Performance.Application.Dtos;

namespace com.brettnamba.MlbTheShowForecaster.Performance.Application.Services;

/// <summary>
/// Defines a service that will provide a player's season stats
/// </summary>
public interface IPlayerStats : IDisposable
{
    /// <summary>
    /// Should get season stats for all players in the specified year
    /// </summary>
    /// <param name="seasonYear">The season to get stats for</param>
    /// <param name="cancellationToken">A <see cref="CancellationToken" /> to observe while waiting for the task to complete</param>
    /// <returns>Stats for all players in the specified year</returns>
    IAsyncEnumerable<PlayerSeason> GetAllPlayerStatsFor(SeasonYear seasonYear, CancellationToken cancellationToken);
}