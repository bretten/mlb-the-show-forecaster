using com.brettnamba.MlbTheShowForecaster.Common.Domain.Events;
using com.brettnamba.MlbTheShowForecaster.GameCards.Domain.Cards.Entities;
using com.brettnamba.MlbTheShowForecaster.GameCards.Domain.Cards.ValueObjects;

namespace com.brettnamba.MlbTheShowForecaster.GameCards.Domain.Cards.Events;

/// <summary>
/// Defines the <see cref="IDomainEvent"/> that is published when a <see cref="PlayerCard"/> gets a temporary
/// <see cref="OverallRating"/> (<see cref="PlayerCard.TemporaryOverallRating"/>)
/// </summary>
public interface IPlayerCardOverallRatingTemporarilyChangedEvent : IDomainEvent
{
    /// <summary>
    /// The card ID from MLB The Show
    /// </summary>
    CardExternalId CardExternalId { get; }

    /// <summary>
    /// The new overall rating
    /// </summary>
    OverallRating NewOverallRating { get; }

    /// <summary>
    /// The previous overall rating being replaced
    /// </summary>
    OverallRating PreviousOverallRating { get; }
}