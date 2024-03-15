using com.brettnamba.MlbTheShowForecaster.Common.Domain.SeedWork;
using com.brettnamba.MlbTheShowForecaster.Common.Domain.ValueObjects;

namespace com.brettnamba.MlbTheShowForecaster.GameCards.Domain.Marketplace.ValueObjects;

/// <summary>
/// Listing prices for a card on a specific date
/// </summary>
public sealed class ListingHistoricalPrice : ValueObject
{
    /// <summary>
    /// The date of the card's listing prices
    /// </summary>
    public DateOnly Date { get; set; }

    /// <summary>
    /// The best buy price for the day
    /// </summary>
    public NaturalNumber BuyPrice { get; }

    /// <summary>
    /// The best sell price for the day
    /// </summary>
    public NaturalNumber SellPrice { get; }

    /// <summary>
    /// Constructor
    /// </summary>
    /// <param name="date">The date of the card's listing prices</param>
    /// <param name="buyPrice">The best buy price for the day</param>
    /// <param name="sellPrice">The best sell price for the day</param>
    private ListingHistoricalPrice(DateOnly date, NaturalNumber buyPrice, NaturalNumber sellPrice)
    {
        Date = date;
        BuyPrice = buyPrice;
        SellPrice = sellPrice;
    }

    /// <summary>
    /// Creates a <see cref="ListingHistoricalPrice"/>
    /// </summary>
    /// <param name="date">The date of the card's listing prices</param>
    /// <param name="buyPrice">The best buy price for the day</param>
    /// <param name="sellPrice">The best sell price for the day</param>
    /// <returns><see cref="ListingHistoricalPrice"/></returns>
    public static ListingHistoricalPrice Create(DateOnly date, NaturalNumber buyPrice, NaturalNumber sellPrice)
    {
        return new ListingHistoricalPrice(date, buyPrice, sellPrice);
    }
}