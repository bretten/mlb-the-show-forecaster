using com.brettnamba.MlbTheShowForecaster.GameCards.Domain.Cards.Entities;
using com.brettnamba.MlbTheShowForecaster.GameCards.Domain.Cards.ValueObjects;

namespace com.brettnamba.MlbTheShowForecaster.GameCards.Domain.Cards.Events;

/// <summary>
/// Raised when a <see cref="PlayerCard"/> gets a higher, temporary <see cref="OverallRating"/>
/// </summary>
/// <param name="CardExternalId">The card ID from MLB The Show</param>
/// <param name="NewOverallRating">The new overall rating</param>
/// <param name="PreviousOverallRating">The previous overall rating being replaced</param>
public sealed record PlayerCardOverallRatingTemporarilyImprovedEvent(
    CardExternalId CardExternalId,
    OverallRating NewOverallRating,
    OverallRating PreviousOverallRating) : IPlayerCardOverallRatingTemporarilyChangedEvent;