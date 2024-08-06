using com.brettnamba.MlbTheShowForecaster.Common.Domain.Events;
using com.brettnamba.MlbTheShowForecaster.Common.Domain.ValueObjects;
using com.brettnamba.MlbTheShowForecaster.GameCards.Domain.Cards.Entities;
using com.brettnamba.MlbTheShowForecaster.GameCards.Domain.Cards.ValueObjects;
using com.brettnamba.MlbTheShowForecaster.GameCards.Domain.Cards.ValueObjects.PlayerCards;

namespace com.brettnamba.MlbTheShowForecaster.GameCards.Domain.Cards.Events;

/// <summary>
/// Raised when a <see cref="PlayerCard"/> gets a significant rating and attribute increase
/// </summary>
/// <param name="Year">The year of MLB The Show</param>
/// <param name="CardExternalId">The card ID from MLB The Show</param>
/// <param name="NewOverallRating">The new overall rating</param>
/// <param name="NewPlayerCardAttributes">The new player attributes</param>
/// <param name="BoostReason">The reason the card is being boosted</param>
/// <param name="BoostEndDate">The end date of the boost</param>
public sealed record PlayerCardBoostedEvent(
    SeasonYear Year,
    CardExternalId CardExternalId,
    OverallRating NewOverallRating,
    PlayerCardAttributes NewPlayerCardAttributes,
    string BoostReason,
    DateOnly BoostEndDate
) : IDomainEvent;