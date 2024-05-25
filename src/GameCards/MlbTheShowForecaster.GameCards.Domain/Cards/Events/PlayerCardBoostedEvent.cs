using com.brettnamba.MlbTheShowForecaster.Common.Domain.Events;
using com.brettnamba.MlbTheShowForecaster.GameCards.Domain.Cards.Entities;
using com.brettnamba.MlbTheShowForecaster.GameCards.Domain.Cards.ValueObjects;
using com.brettnamba.MlbTheShowForecaster.GameCards.Domain.Cards.ValueObjects.PlayerCards;

namespace com.brettnamba.MlbTheShowForecaster.GameCards.Domain.Cards.Events;

/// <summary>
/// Raised when a <see cref="PlayerCard"/> gets a significant rating and attribute increase
/// </summary>
/// <param name="CardExternalId">The card ID from MLB The Show</param>
/// <param name="NewOverallRating">The new overall rating</param>
/// <param name="NewPlayerCardAttributes">The new player attributes</param>
public sealed record PlayerCardBoostedEvent(
    CardExternalId CardExternalId,
    OverallRating NewOverallRating,
    PlayerCardAttributes NewPlayerCardAttributes
) : IDomainEvent;