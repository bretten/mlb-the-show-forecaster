using com.brettnamba.MlbTheShowForecaster.GameCards.Domain.Cards.ValueObjects;

namespace com.brettnamba.MlbTheShowForecaster.GameCards.Application.Dtos;

/// <summary>
/// Represents a player being added to MLB The Show
/// </summary>
/// <param name="CardExternalId">The ID of the player card</param>
public readonly record struct PlayerAddition(
    CardExternalId CardExternalId
);