using com.brettnamba.MlbTheShowForecaster.GameCards.Domain.Cards.Entities;
using com.brettnamba.MlbTheShowForecaster.GameCards.Domain.Cards.ValueObjects;
using com.brettnamba.MlbTheShowForecaster.GameCards.Domain.Cards.ValueObjects.PlayerCards;

namespace com.brettnamba.MlbTheShowForecaster.GameCards.Domain.Cards.Events;

/// <summary>
/// Raised when a <see cref="PlayerCard"/> gets a higher <see cref="OverallRating"/>
/// </summary>
/// <param name="CardId">The card ID from MLB The Show</param>
/// <param name="NewOverallRating">The new overall rating</param>
/// <param name="NewPlayerCardAttributes">The new player attributes</param>
/// <param name="PreviousOverallRating">The previous overall rating being replaced</param>
/// <param name="PreviousPlayerCardAttributes">The previous player attributes being replaced</param>
/// <param name="RarityChanged">True if the card rarity changed, otherwise false</param>
public record PlayerCardOverallRatingImprovedEvent(
    CardId CardId,
    OverallRating NewOverallRating,
    PlayerCardAttributes NewPlayerCardAttributes,
    OverallRating PreviousOverallRating,
    PlayerCardAttributes PreviousPlayerCardAttributes,
    bool RarityChanged) : IPlayerCardOverallRatingChangedEvent;