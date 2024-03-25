using com.brettnamba.MlbTheShowForecaster.Common.Domain.SeedWork;
using com.brettnamba.MlbTheShowForecaster.GameCards.Domain.Cards.Entities;
using com.brettnamba.MlbTheShowForecaster.GameCards.Domain.Cards.ValueObjects.Exceptions;

namespace com.brettnamba.MlbTheShowForecaster.GameCards.Domain.Cards.ValueObjects;

/// <summary>
/// Represents the name of a <see cref="Card"/>
/// </summary>
public sealed class CardName : ValueObject
{
    /// <summary>
    /// The underlying card name
    /// </summary>
    public string Value { get; }

    /// <summary>
    /// Constructor
    /// </summary>
    /// <param name="value">The card name</param>
    private CardName(string value)
    {
        Value = value;
    }

    /// <summary>
    /// Creates a <see cref="CardName"/>
    /// </summary>
    /// <param name="cardName">The card name</param>
    /// <returns><see cref="CardName"/></returns>
    public static CardName Create(string cardName)
    {
        if (string.IsNullOrWhiteSpace(cardName))
        {
            throw new EmptyCardNameException("A card requires a name");
        }

        return new CardName(cardName);
    }
}