using com.brettnamba.MlbTheShowForecaster.Common.Domain.SeedWork;
using com.brettnamba.MlbTheShowForecaster.GameCards.Domain.Cards.Enums;
using com.brettnamba.MlbTheShowForecaster.GameCards.Domain.Cards.ValueObjects;

namespace com.brettnamba.MlbTheShowForecaster.GameCards.Domain.Cards.Entities;

/// <summary>
/// Represents a Card in MLB The Show
/// </summary>
public sealed class Card : AggregateRoot
{
    /// <summary>
    /// The ID from MLB The Show
    /// </summary>
    public CardId TheShowId { get; }

    /// <summary>
    /// The card type
    /// </summary>
    public CardType Type { get; }

    /// <summary>
    /// The card image
    /// </summary>
    public CardImage Image { get; }

    /// <summary>
    /// The name of the card
    /// </summary>
    public string Name { get; }

    /// <summary>
    /// The rarity of the card
    /// </summary>
    public Rarity Rarity { get; }

    /// <summary>
    /// The short name of the team associated with the card
    /// </summary>
    public TeamShortName? TeamShortName { get; }

    /// <summary>
    /// The overall rating of the card
    /// </summary>
    public OverallRating? OverallRating { get; }

    /// <summary>
    /// The series of the card
    /// </summary>
    public CardSeries Series { get; }

    public Card(Guid id) : base(id)
    {
    }
}