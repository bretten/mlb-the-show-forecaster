using com.brettnamba.MlbTheShowForecaster.PlayerStatus.Domain.Players.Entities;
using com.brettnamba.MlbTheShowForecaster.PlayerStatus.Domain.Players.Services.Exceptions;
using com.brettnamba.MlbTheShowForecaster.PlayerStatus.Domain.Teams.ValueObjects;

namespace com.brettnamba.MlbTheShowForecaster.PlayerStatus.Domain.Players.Services;

/// <summary>
/// Defines a service that searches for a <see cref="Player"/> based on certain criteria
/// </summary>
public interface IPlayerSearchService
{
    /// <summary>
    /// Should look for a player based on their name and team
    /// </summary>
    /// <param name="name">The player's name</param>
    /// <param name="team">The player's team</param>
    /// <returns>The <see cref="Player"/> or null if no player was found</returns>
    /// <exception cref="PlayerSearchCouldNotBeRefinedException">Thrown when there are multiple matches for a <see cref="Player"/></exception>
    Task<Player?> FindPlayer(string name, Team team);
}