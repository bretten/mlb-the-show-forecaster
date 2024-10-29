using com.brettnamba.MlbTheShowForecaster.Common.Domain.Enums;
using com.brettnamba.MlbTheShowForecaster.GameCards.Domain.Cards.Entities;
using com.brettnamba.MlbTheShowForecaster.GameCards.Domain.Cards.ValueObjects;

namespace com.brettnamba.MlbTheShowForecaster.GameCards.Application.Dtos;

/// <summary>
/// Represents a player's primary position changing in MLB The Show
/// </summary>
/// <param name="Date">The date of the change</param>
/// <param name="CardExternalId">The ID of the player card</param>
/// <param name="NewPosition">The player's new position</param>
public readonly record struct PlayerPositionChange(
    DateOnly Date,
    CardExternalId CardExternalId,
    Position NewPosition
)
{
    /// <summary>
    /// Returns true if the new position has already been applied to the <see cref="PlayerCard"/>, otherwise false
    /// </summary>
    /// <param name="playerCard">The <see cref="PlayerCard"/> to check</param>
    /// <returns>True if the new position has already been applied to the <see cref="PlayerCard"/>, otherwise false</returns>
    public bool IsApplied(PlayerCard playerCard) => playerCard.Position == NewPosition;
};