using com.brettnamba.MlbTheShowForecaster.Common.Domain.ValueObjects;
using com.brettnamba.MlbTheShowForecaster.GameCards.Domain.Cards.Entities;
using com.brettnamba.MlbTheShowForecaster.GameCards.Domain.Cards.ValueObjects;
using com.brettnamba.MlbTheShowForecaster.GameCards.Domain.Cards.ValueObjects.PlayerCards;

namespace com.brettnamba.MlbTheShowForecaster.GameCards.Domain.Cards.Events;

/// <summary>
/// Raised when a <see cref="PlayerCard"/> gets a lower <see cref="OverallRating"/>
/// </summary>
/// <param name="Year">The year of MLB The Show</param>
/// <param name="CardExternalId">The card ID from MLB The Show</param>
/// <param name="NewOverallRating">The new overall rating</param>
/// <param name="NewPlayerCardAttributes">The new player attributes</param>
/// <param name="PreviousOverallRating">The previous overall rating being replaced</param>
/// <param name="PreviousPlayerCardAttributes">The previous player attributes being replaced</param>
/// <param name="RarityChanged">True if the card rarity changed, otherwise false</param>
/// <param name="Date">The date</param>
public sealed record PlayerCardOverallRatingDeclinedEvent(
    SeasonYear Year,
    CardExternalId CardExternalId,
    OverallRating NewOverallRating,
    PlayerCardAttributes NewPlayerCardAttributes,
    OverallRating PreviousOverallRating,
    PlayerCardAttributes PreviousPlayerCardAttributes,
    bool RarityChanged,
    DateOnly Date) : IPlayerCardOverallRatingChangedEvent;