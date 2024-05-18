using com.brettnamba.MlbTheShowForecaster.GameCards.Domain.Cards.Entities;
using com.brettnamba.MlbTheShowForecaster.GameCards.Domain.Cards.ValueObjects.Exceptions;

namespace com.brettnamba.MlbTheShowForecaster.GameCards.Domain.Cards.ValueObjects;

/// <summary>
/// Represents an image location for a <see cref="Card"/>
/// </summary>
public sealed class CardImageLocation
{
    /// <summary>
    /// The underlying card image location
    /// </summary>
    public Uri Value { get; }

    /// <summary>
    /// Constructor
    /// </summary>
    /// <param name="value">The card image location</param>
    private CardImageLocation(Uri value)
    {
        Value = value;
    }

    /// <summary>
    /// Creates a <see cref="CardImageLocation"/>
    /// </summary>
    /// <param name="cardImageLocation">The card image location</param>
    /// <returns><see cref="CardImageLocation"/></returns>
    public static CardImageLocation Create(string cardImageLocation)
    {
        if (string.IsNullOrWhiteSpace(cardImageLocation))
        {
            throw new EmptyCardImageException("A card image location is required");
        }

        var created = Uri.TryCreate(cardImageLocation, UriKind.RelativeOrAbsolute, out var uri);
        if (!created || uri == null)
        {
            throw new InvalidCardImageLocationException(
                $"The card image location must be a valid URI. Invalid value given: {cardImageLocation}");
        }

        return new CardImageLocation(uri);
    }
}