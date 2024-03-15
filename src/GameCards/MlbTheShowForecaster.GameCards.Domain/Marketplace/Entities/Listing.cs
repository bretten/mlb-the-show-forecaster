﻿using System.Collections.Immutable;
using com.brettnamba.MlbTheShowForecaster.Common.Domain.SeedWork;
using com.brettnamba.MlbTheShowForecaster.Common.Domain.ValueObjects;
using com.brettnamba.MlbTheShowForecaster.GameCards.Domain.Cards.Entities;
using com.brettnamba.MlbTheShowForecaster.GameCards.Domain.Cards.ValueObjects;
using com.brettnamba.MlbTheShowForecaster.GameCards.Domain.Marketplace.Entities.Exceptions;
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
    /// The ID of the card that this listing is for
    /// </summary>
    public CardId CardId { get; }

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
    /// Constructor
    /// </summary>
    /// <param name="cardId">The ID of the card that this listing is for</param>
    /// <param name="buyPrice">The current, best buy price</param>
    /// <param name="sellPrice">The current, best sell price</param>
    private Listing(CardId cardId, NaturalNumber buyPrice, NaturalNumber sellPrice) : base(Guid.NewGuid())
    {
        CardId = cardId;
        BuyPrice = buyPrice;
        SellPrice = sellPrice;
        _historicalPrices = new List<ListingHistoricalPrice>();
    }

    /// <summary>
    /// Archives the current prices by adding them to the historical price collection under the specified date
    /// </summary>
    /// <param name="date">The date to archive the prices under</param>
    /// <exception cref="ListingHistoricalPriceExistsException">Thrown when there is already a historical price for the specified date</exception>
    public void ArchivePrice(DateOnly date)
    {
        if (_historicalPrices.Any(x => x.Date == date))
        {
            throw new ListingHistoricalPriceExistsException(
                $"Listing historical price already exists for card = {CardId.Value} and date = {date.ToShortDateString()}");
        }

        _historicalPrices.Add(ListingHistoricalPrice.Create(date, BuyPrice, SellPrice));
    }

    /// <summary>
    /// Updates the prices for this listing
    /// </summary>
    /// <param name="buyPrice">The new best buy price</param>
    /// <param name="sellPrice">The new best sell price</param>
    public void UpdatePrices(NaturalNumber buyPrice, NaturalNumber sellPrice)
    {
        BuyPrice = buyPrice;
        SellPrice = sellPrice;
    }

    /// <summary>
    /// Creates a <see cref="Listing"/>
    /// </summary>
    /// <param name="cardId">The ID of the card that this listing is for</param>
    /// <param name="buyPrice">The current, best buy price</param>
    /// <param name="sellPrice">The current, best sell price</param>
    /// <returns><see cref="Listing"/></returns>
    public static Listing Create(CardId cardId, NaturalNumber buyPrice, NaturalNumber sellPrice)
    {
        return new Listing(cardId, buyPrice, sellPrice);
    }
}