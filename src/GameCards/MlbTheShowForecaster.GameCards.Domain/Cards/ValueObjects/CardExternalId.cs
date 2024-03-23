using com.brettnamba.MlbTheShowForecaster.Common.Domain.SeedWork;
using com.brettnamba.MlbTheShowForecaster.GameCards.Domain.Cards.Entities;
using com.brettnamba.MlbTheShowForecaster.GameCards.Domain.Cards.ValueObjects.Exceptions;

namespace com.brettnamba.MlbTheShowForecaster.GameCards.Domain.Cards.ValueObjects;

/// <summary>
/// Represents an external ID for a <see cref="Card"/>
/// </summary>
public sealed class CardExternalId : ValueObject
{
    /// <summary>
    /// The underlying card external ID value
    /// </summary>
    public string Value { get; }

    /// <summary>
    /// Constructor
    /// </summary>
    /// <param name="value">The card external ID</param>
    private CardExternalId(string value)
    {
        Value = value;
    }

    /// <summary>
    /// Creates a <see cref="CardExternalId"/>
    /// </summary>
    /// <param name="externalId">The card external ID</param>
    /// <returns><see cref="CardExternalId"/></returns>
    public static CardExternalId Create(string externalId)
    {
        if (string.IsNullOrWhiteSpace(externalId))
        {
            throw new EmptyCardExternalIdException("A card external ID is required");
        }

        return new CardExternalId(externalId);
    }
}