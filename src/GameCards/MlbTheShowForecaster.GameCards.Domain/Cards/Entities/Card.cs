using com.brettnamba.MlbTheShowForecaster.Common.Domain.SeedWork;
using com.brettnamba.MlbTheShowForecaster.GameCards.Domain.Cards.Enums;
using com.brettnamba.MlbTheShowForecaster.GameCards.Domain.Cards.ValueObjects;

namespace com.brettnamba.MlbTheShowForecaster.GameCards.Domain.Cards.Entities;

/// <summary>
/// Represents a Card in MLB The Show
/// </summary>
public abstract class Card : AggregateRoot
{
    /// <summary>
    /// The card ID from MLB The Show
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
    /// The series of the card
    /// </summary>
    public CardSeries Series { get; }

    /// <summary>
    /// Constructor
    /// </summary>
    /// <param name="theShowId">The card ID from MLB The Show</param>
    /// <param name="type">The card type</param>
    /// <param name="image">The card image</param>
    /// <param name="name">The name of the card</param>
    /// <param name="rarity">The rarity of the card</param>
    /// <param name="series">The series of the card</param>
    protected Card(CardId theShowId, CardType type, CardImage image, string name, Rarity rarity, CardSeries series) :
        base(Guid.NewGuid())
    {
        TheShowId = theShowId;
        Type = type;
        Image = image;
        Name = name;
        Rarity = rarity;
        Series = series;
    }
}