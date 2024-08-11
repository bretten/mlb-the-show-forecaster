using com.brettnamba.MlbTheShowForecaster.Common.Domain.ValueObjects;
using com.brettnamba.MlbTheShowForecaster.GameCards.Domain.Cards.Entities;
using com.brettnamba.MlbTheShowForecaster.GameCards.Domain.Cards.ValueObjects;

namespace com.brettnamba.MlbTheShowForecaster.GameCards.Application.Services;

/// <summary>
/// Defines a service that matches a <see cref="PlayerCard"/> to a player in the MLB
/// </summary>
public interface IPlayerMatcher
{
    /// <summary>
    /// Should match the specified <see cref="CardName"/> to a MLB player
    /// </summary>
    /// <param name="name"><see cref="CardName"/></param>
    /// <param name="teamShortName"><see cref="TeamShortName"/></param>
    /// <returns>The player's <see cref="MlbId"/> or null if no matching player was found</returns>
    Task<MlbId?> GetPlayerByName(CardName name, TeamShortName teamShortName);
}