using com.brettnamba.MlbTheShowForecaster.Common.Domain.SeedWork;
using com.brettnamba.MlbTheShowForecaster.GameCards.Domain.Cards.Entities;

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
    public string AsStringDigits => Value.ToString("N");

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
    public static CardExternalId Create(Guid externalId)
    {
        return new CardExternalId(externalId);
    }
}