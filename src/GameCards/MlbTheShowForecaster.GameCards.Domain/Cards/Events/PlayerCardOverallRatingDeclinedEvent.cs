﻿using com.brettnamba.MlbTheShowForecaster.Common.Domain.Events;
using com.brettnamba.MlbTheShowForecaster.GameCards.Domain.Cards.Entities;
using com.brettnamba.MlbTheShowForecaster.GameCards.Domain.Cards.ValueObjects;
using com.brettnamba.MlbTheShowForecaster.GameCards.Domain.Cards.ValueObjects.PlayerCards;

namespace com.brettnamba.MlbTheShowForecaster.GameCards.Domain.Cards.Events;

/// <summary>
/// Raised when a <see cref="PlayerCard"/> gets a lower <see cref="OverallRating"/>
/// </summary>
/// <param name="CardId">The card ID from MLB The Show</param>
/// <param name="NewOverallRating">The new overall rating</param>
/// <param name="NewPlayerCardAttributes">The new player attributes</param>
/// <param name="PreviousOverallRating">The previous overall rating being replaced</param>
/// <param name="PreviousPlayerCardAttributes">The previous player attributes being replaced</param>
public record PlayerCardOverallRatingDeclinedEvent(
    CardId CardId,
    OverallRating NewOverallRating,
    PlayerCardAttributes NewPlayerCardAttributes,
    OverallRating PreviousOverallRating,
    PlayerCardAttributes PreviousPlayerCardAttributes) : IDomainEvent;