using com.brettnamba.MlbTheShowForecaster.Common.Domain.ValueObjects;
using com.brettnamba.MlbTheShowForecaster.Performance.Application.Dtos;

namespace com.brettnamba.MlbTheShowForecaster.Performance.Application.Services;

/// <summary>
/// Defines a service that will provide a player's season stats
/// </summary>
public interface IPlayerStats : IDisposable
{
    /// <summary>
    /// Gets a player's season stats
    /// </summary>
    /// <param name="playerMlbId">The MLB ID of the player</param>
    /// <param name="seasonYear">The season year</param>
    /// <returns>The player's season stats</returns>
    Task<PlayerSeason> GetPlayerSeason(MlbId playerMlbId, SeasonYear seasonYear);
}