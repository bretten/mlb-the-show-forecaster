using com.brettnamba.MlbTheShowForecaster.Common.Domain.SeedWork;
using com.brettnamba.MlbTheShowForecaster.Common.Domain.ValueObjects;
using com.brettnamba.MlbTheShowForecaster.GameCards.Domain.Marketplace.Entities;

namespace com.brettnamba.MlbTheShowForecaster.GameCards.Domain.Marketplace.ValueObjects;

/// <summary>
/// The sale of a <see cref="Listing"/>
/// </summary>
public sealed class ListingOrder : ValueObject
{
    /// <summary>
    /// The date of the order
    /// </summary>
    public DateTime Date { get; }

    /// <summary>
    /// The price of the order
    /// </summary>
    public NaturalNumber Price { get; }

    /// <summary>
    /// The quantity of the order
    /// </summary>
    public NaturalNumber Quantity { get; }

    /// <summary>
    /// Constructor
    /// </summary>
    /// <param name="date">The date of the order</param>
    /// <param name="price">The price of the order</param>
    /// <param name="quantity">The quantity of the order</param>
    private ListingOrder(DateTime date, NaturalNumber price, NaturalNumber quantity)
    {
        Date = date;
        Price = price;
        Quantity = quantity;
    }

    /// <summary>
    /// Creates a <see cref="ListingOrder"/>
    /// </summary>
    /// <param name="date">The date of the order</param>
    /// <param name="price">The price of the order</param>
    /// <param name="quantity">The quantity of the order</param>
    /// <returns><see cref="ListingOrder"/></returns>
    public static ListingOrder Create(DateTime date, NaturalNumber price, NaturalNumber quantity)
    {
        return new ListingOrder(date, price, quantity);
    }
}