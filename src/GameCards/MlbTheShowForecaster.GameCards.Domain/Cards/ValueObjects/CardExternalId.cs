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
    public Guid Value { get; }

    /// <summary>
    /// The value with only digits: 00000000000000000000000000000000
    /// </summary>
    public string ValueStringDigits => Value.ToString("N");

    /// <summary>
    /// Constructor
    /// </summary>
    /// <param name="value">The card external ID</param>
    private CardExternalId(Guid value)
    {
        Value = value;
    }

    /// <summary>
    /// Creates a <see cref="CardExternalId"/>
    /// </summary>
    /// <param name="externalId">The card external ID</param>
    /// <returns><see cref="CardExternalId"/></returns>
    /// <exception cref="EmptyCardExternalIdException">Thrown when the external ID value is empty</exception>
    /// <exception cref="InvalidCardExternalIdException">Thrown when the the external ID value is not a valid GUID</exception>
    public static CardExternalId Create(string externalId)
    {
        if (string.IsNullOrWhiteSpace(externalId))
        {
            throw new EmptyCardExternalIdException("A card external ID is required");
        }

        if (!Guid.TryParse(externalId, out var guid))
        {
            throw new InvalidCardExternalIdException($"The card external ID is not a valid GUID: {externalId}");
        }

        return new CardExternalId(guid);
    }

    /// <summary>
    /// Creates a <see cref="CardExternalId"/>
    /// </summary>
    /// <param name="externalId">The card external ID</param>
    /// <returns><see cref="CardExternalId"/></returns>
    public static CardExternalId Create(Guid externalId)
    {
        return new CardExternalId(externalId);
    }
}