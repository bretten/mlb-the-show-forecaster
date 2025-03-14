﻿using com.brettnamba.MlbTheShowForecaster.Common.Domain.Events;
using com.brettnamba.MlbTheShowForecaster.Common.Domain.ValueObjects;
using com.brettnamba.MlbTheShowForecaster.GameCards.Domain.Cards.Entities;
using com.brettnamba.MlbTheShowForecaster.GameCards.Domain.Cards.ValueObjects;
using com.brettnamba.MlbTheShowForecaster.GameCards.Domain.Cards.ValueObjects.PlayerCards;

namespace com.brettnamba.MlbTheShowForecaster.GameCards.Domain.Cards.Events;

/// <summary>
/// Defines the <see cref="IDomainEvent"/> that is published when a <see cref="PlayerCard"/>'s
/// <see cref="OverallRating"/> changes
/// </summary>
public interface IPlayerCardOverallRatingChangedEvent : IDomainEvent
{
    /// <summary>
    /// The year of MLB The Show
    /// </summary>
    public SeasonYear Year { get; }

    /// <summary>
    /// The card ID from MLB The Show
    /// </summary>
    CardExternalId CardExternalId { get; }

    /// <summary>
    /// The new overall rating
    /// </summary>
    OverallRating NewOverallRating { get; }

    /// <summary>
    /// The new player attributes
    /// </summary>
    PlayerCardAttributes NewPlayerCardAttributes { get; }

    /// <summary>
    /// The previous overall rating being replaced
    /// </summary>
    OverallRating PreviousOverallRating { get; }

    /// <summary>
    /// The previous player attributes being replaced
    /// </summary>
    PlayerCardAttributes PreviousPlayerCardAttributes { get; }

    /// <summary>
    /// True if the card rarity changed, otherwise false
    /// </summary>
    bool RarityChanged { get; }

    /// <summary>
    /// The date
    /// </summary>
    DateOnly Date { get; }
}