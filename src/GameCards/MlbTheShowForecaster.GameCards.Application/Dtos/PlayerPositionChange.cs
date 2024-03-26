using com.brettnamba.MlbTheShowForecaster.Common.Domain.Enums;
using com.brettnamba.MlbTheShowForecaster.GameCards.Domain.Cards.ValueObjects;

namespace com.brettnamba.MlbTheShowForecaster.GameCards.Application.Dtos;

/// <summary>
/// Represents a player's primary position changing in MLB The Show
/// </summary>
/// <param name="CardExternalId">The ID of the player card</param>
/// <param name="NewPosition">The player's new position</param>
public readonly record struct PlayerPositionChange(
    CardExternalId CardExternalId,
    Position NewPosition
);