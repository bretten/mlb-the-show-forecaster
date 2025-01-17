using System.Collections.Immutable;
using System.ComponentModel.DataAnnotations.Schema;
using com.brettnamba.MlbTheShowForecaster.Common.Domain.SeedWork;
using com.brettnamba.MlbTheShowForecaster.Common.Domain.ValueObjects;
using com.brettnamba.MlbTheShowForecaster.GameCards.Domain.Cards.Entities;
using com.brettnamba.MlbTheShowForecaster.GameCards.Domain.Cards.ValueObjects;
using com.brettnamba.MlbTheShowForecaster.GameCards.Domain.Marketplace.Entities.Exceptions;
using com.brettnamba.MlbTheShowForecaster.GameCards.Domain.Marketplace.Events;
using com.brettnamba.MlbTheShowForecaster.GameCards.Domain.Marketplace.ValueObjects;

namespace com.brettnamba.MlbTheShowForecaster.GameCards.Domain.Marketplace.Entities;

/// <summary>
/// Represents a listing on the marketplace for a <see cref="Card"/>
/// </summary>
public sealed class Listing : AggregateRoot
{
    /// <summary>
    /// The price history of this listing
    /// </summary>
    private readonly List<ListingHistoricalPrice> _historicalPrices;

    /// <summary>
    /// Orders for the listing
    /// </summary>
    private readonly List<ListingOrder> _orders;

    /// <summary>
    /// The external ID of the card that this listing is for
    /// </summary>
    public CardExternalId CardExternalId { get; }

    /// <summary>
    /// The current, best buy price
    /// </summary>
    public NaturalNumber BuyPrice { get; private set; }

    /// <summary>
    /// The current, best sell price
    /// </summary>
    public NaturalNumber SellPrice { get; private set; }

    /// <summary>
    /// The price history of this listing in chronological order
    /// </summary>
    public IReadOnlyList<ListingHistoricalPrice> HistoricalPricesChronologically =>
        _historicalPrices.OrderBy(x => x.Date).ToImmutableList();

    /// <summary>
    /// Orders for the listing in chronological order
    /// </summary>
    [NotMapped]
    public IReadOnlyList<ListingOrder> OrdersChronologically =>
        _orders.OrderBy(x => x.Date).ThenByDescending(x => x.Price).ToImmutableList();

    /// <summary>
    /// The total number of orders for the specified date
    /// </summary>
    /// <param name="date">The date to get orders for</param>
    /// <returns>Total number of orders for the specified date</returns>
    public NaturalNumber TotalOrdersFor(DateOnly date) => NaturalNumber.Create(_orders
        .Where(x => DateOnly.FromDateTime(x.Date) == date)
        .Sum(x => x.Quantity.Value));

    /// <summary>
    /// Constructor
    /// </summary>
    /// <param name="cardExternalId">The external ID of the card that this listing is for</param>
    /// <param name="buyPrice">The current, best buy price</param>
    /// <param name="sellPrice">The current, best sell price</param>
    /// <param name="historicalPrices">The price history of this listing</param>
    /// <param name="orders">Orders for the listing</param>
    private Listing(CardExternalId cardExternalId, NaturalNumber buyPrice, NaturalNumber sellPrice,
        List<ListingHistoricalPrice> historicalPrices, List<ListingOrder> orders) : base(Guid.NewGuid())
    {
        CardExternalId = cardExternalId;
        BuyPrice = buyPrice;
        SellPrice = sellPrice;
        _historicalPrices = historicalPrices;
        _orders = orders;
    }

    /// <summary>
    /// Constructor
    /// </summary>
    /// <param name="cardExternalId">The external ID of the card that this listing is for</param>
    /// <param name="buyPrice">The current, best buy price</param>
    /// <param name="sellPrice">The current, best sell price</param>
    private Listing(CardExternalId cardExternalId, NaturalNumber buyPrice, NaturalNumber sellPrice)
        : this(cardExternalId, buyPrice, sellPrice, new List<ListingHistoricalPrice>(), new List<ListingOrder>())
    {
    }

    /// <summary>
    /// Logs the specified prices by adding them to the historical price collection under the specified date
    /// </summary>
    /// <param name="date">The date to archive the prices under</param>
    /// <param name="buyPrice">The best buy price for the date</param>
    /// <param name="sellPrice">The best sell price for the date</param>
    /// <exception cref="ListingHistoricalPriceExistsException">Thrown when there is already a historical price for the specified date</exception>
    public void LogHistoricalPrice(DateOnly date, NaturalNumber buyPrice, NaturalNumber sellPrice)
    {
        if (_historicalPrices.Any(x => x.Date == date))
        {
            throw new ListingHistoricalPriceExistsException(
                $"Listing historical price already exists for card = {CardExternalId.Value} and date = {date.ToShortDateString()}");
        }

        _historicalPrices.Add(ListingHistoricalPrice.Create(date, buyPrice, sellPrice));
    }

    /// <summary>
    /// Updates the prices for this listing
    /// </summary>
    /// <param name="newBuyPrice">The new best buy price</param>
    /// <param name="newSellPrice">The new best sell price</param>
    /// <param name="changeThreshold">The percentage change threshold that determines significant listing price changes</param>
    public void UpdatePrices(NaturalNumber newBuyPrice, NaturalNumber newSellPrice,
        IListingPriceSignificantChangeThreshold changeThreshold)
    {
        CheckNewBuyPriceForSignificantChange(newBuyPrice, changeThreshold);
        CheckNewSellPriceForSignificantChange(newSellPrice, changeThreshold);

        BuyPrice = newBuyPrice;
        SellPrice = newSellPrice;
    }

    /// <summary>
    /// Updates the Listing's orders with new orders
    /// </summary>
    /// <param name="newOrders">New orders</param>
    public void UpdateOrders(List<ListingOrder> newOrders)
    {
        _orders.AddRange(newOrders);
    }

    /// <summary>
    /// Checks for significant changes between the old buy price and the specified new buy price
    /// </summary>
    /// <param name="newBuyPrice">The new buy price</param>
    /// <param name="t">The percentage change threshold that a price must change by for it to be deemed significant</param>
    private void CheckNewBuyPriceForSignificantChange(NaturalNumber newBuyPrice,
        IListingPriceSignificantChangeThreshold t)
    {
        // Calculate the percentage difference between the previous buy price and the new buy price
        var priceDiffPercentage = PercentageChange.Create(referenceValue: BuyPrice, newValue: newBuyPrice, true);

        // Check if the price increased or decreased significantly
        if (priceDiffPercentage.HasIncreasedBy(t.BuyPricePercentageChangeThreshold))
        {
            RaiseDomainEvent(
                new ListingBuyPriceIncreasedEvent(CardExternalId, BuyPrice, newBuyPrice, priceDiffPercentage));
        }
        else if (priceDiffPercentage.HasDecreasedBy(t.BuyPricePercentageChangeThreshold))
        {
            RaiseDomainEvent(
                new ListingBuyPriceDecreasedEvent(CardExternalId, BuyPrice, newBuyPrice, priceDiffPercentage));
        }

        // If the price percentage change threshold was not crossed, this is a negligible event
    }

    /// <summary>
    /// Checks for significant changes between the old sell price and the specified new sell price
    /// </summary>
    /// <param name="newSellPrice">The new sell price</param>
    /// <param name="t">The percentage change threshold that a price must change by for it to be deemed significant</param>
    private void CheckNewSellPriceForSignificantChange(NaturalNumber newSellPrice,
        IListingPriceSignificantChangeThreshold t)
    {
        // Calculate the percentage difference between the previous sell price and the new sell price
        var priceDiffPercentage = PercentageChange.Create(referenceValue: SellPrice, newValue: newSellPrice, true);

        // Check if the price increased or decreased significantly
        if (priceDiffPercentage.HasIncreasedBy(t.SellPricePercentageChangeThreshold))
        {
            RaiseDomainEvent(new ListingSellPriceIncreasedEvent(CardExternalId, SellPrice, newSellPrice,
                priceDiffPercentage));
        }
        else if (priceDiffPercentage.HasDecreasedBy(t.SellPricePercentageChangeThreshold))
        {
            RaiseDomainEvent(new ListingSellPriceDecreasedEvent(CardExternalId, SellPrice, newSellPrice,
                priceDiffPercentage));
        }

        // If the price percentage change threshold was not crossed, this is a negligible event
    }

    /// <summary>
    /// Creates a <see cref="Listing"/>
    /// </summary>
    /// <param name="cardExternalId">The external ID of the card that this listing is for</param>
    /// <param name="buyPrice">The current, best buy price</param>
    /// <param name="sellPrice">The current, best sell price</param>
    /// <param name="historicalPrices">The price history of this listing</param>
    /// <param name="orders">Orders for the listing</param>
    /// <returns><see cref="Listing"/></returns>
    public static Listing Create(CardExternalId cardExternalId, NaturalNumber buyPrice, NaturalNumber sellPrice,
        List<ListingHistoricalPrice>? historicalPrices = null, List<ListingOrder>? orders = null)
    {
        return new Listing(cardExternalId, buyPrice, sellPrice, historicalPrices ?? new List<ListingHistoricalPrice>(),
            orders ?? new List<ListingOrder>());
    }
}