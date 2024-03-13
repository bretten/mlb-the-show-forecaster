using com.brettnamba.MlbTheShowForecaster.Common.Domain.SeedWork;
using com.brettnamba.MlbTheShowForecaster.GameCards.Domain.Cards.Entities;
using com.brettnamba.MlbTheShowForecaster.GameCards.Domain.Cards.ValueObjects.Exceptions;

namespace com.brettnamba.MlbTheShowForecaster.GameCards.Domain.Cards.ValueObjects;

/// <summary>
/// Represents an ID for a <see cref="Card"/>
/// </summary>
public sealed class CardId : ValueObject
{
    /// <summary>
    /// The underlying card ID value
    /// </summary>
    public string Value { get; }

    /// <summary>
    /// Constructor
    /// </summary>
    /// <param name="value">The card ID</param>
    private CardId(string value)
    {
        Value = value;
    }

    /// <summary>
    /// Creates a <see cref="CardId"/>
    /// </summary>
    /// <param name="cardId">The card ID</param>
    /// <returns><see cref="CardId"/></returns>
    public static CardId Create(string cardId)
    {
        if (string.IsNullOrWhiteSpace(cardId))
        {
            throw new EmptyCardIdException("A card ID is required");
        }

        return new CardId(cardId);
    }
}