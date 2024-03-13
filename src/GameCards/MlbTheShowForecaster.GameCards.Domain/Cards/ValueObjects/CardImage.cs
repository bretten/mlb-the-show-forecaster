using com.brettnamba.MlbTheShowForecaster.GameCards.Domain.Cards.Entities;
using com.brettnamba.MlbTheShowForecaster.GameCards.Domain.Cards.ValueObjects.Exceptions;

namespace com.brettnamba.MlbTheShowForecaster.GameCards.Domain.Cards.ValueObjects;

/// <summary>
/// Represents an image for a <see cref="Card"/>
/// </summary>
public sealed class CardImage
{
    /// <summary>
    /// The underlying card image
    /// </summary>
    public string Value { get; }

    /// <summary>
    /// Constructor
    /// </summary>
    /// <param name="value">The card image</param>
    private CardImage(string value)
    {
        Value = value;
    }

    /// <summary>
    /// Creates a <see cref="CardImage"/>
    /// </summary>
    /// <param name="cardImage">The card image</param>
    /// <returns><see cref="CardImage"/></returns>
    public static CardImage Create(string cardImage)
    {
        if (string.IsNullOrWhiteSpace(cardImage))
        {
            throw new EmptyCardImageException("A card image is required");
        }

        return new CardImage(cardImage);
    }
}