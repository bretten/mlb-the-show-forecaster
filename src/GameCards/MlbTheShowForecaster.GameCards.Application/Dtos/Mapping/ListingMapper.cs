﻿using com.brettnamba.MlbTheShowForecaster.GameCards.Domain.Marketplace.Entities;
using com.brettnamba.MlbTheShowForecaster.GameCards.Domain.Marketplace.ValueObjects;

namespace com.brettnamba.MlbTheShowForecaster.GameCards.Application.Dtos.Mapping;

/// <summary>
/// Maps the Application-level listing to the Domain's <see cref="Listing"/>
/// </summary>
public sealed class ListingMapper : IListingMapper
{
    /// <summary>
    /// Maps <see cref="CardListing"/> to <see cref="Listing"/>
    /// </summary>
    /// <param name="listing">The listing to map</param>
    /// <returns><see cref="Listing"/></returns>
    public Listing Map(CardListing listing)
    {
        return Listing.Create(
            listing.Year,
            listing.CardExternalId,
            listing.BestBuyPrice,
            listing.BestSellPrice,
            listing.HistoricalPrices.Select(Map).ToList(),
            listing.RecentOrders.Select(Map).ToList()
        );
    }

    /// <summary>
    /// Maps <see cref="CardListingPrice"/> to <see cref="ListingHistoricalPrice"/>
    /// </summary>
    /// <param name="cardListingPrice">The previous prices for the listing</param>
    /// <returns><see cref="ListingHistoricalPrice"/></returns>
    public ListingHistoricalPrice Map(CardListingPrice cardListingPrice)
    {
        return ListingHistoricalPrice.Create(cardListingPrice.Date,
            buyPrice: cardListingPrice.BestBuyPrice,
            sellPrice: cardListingPrice.BestSellPrice
        );
    }

    /// <summary>
    /// Maps <see cref="CardListingOrder"/> to <see cref="ListingOrder"/>
    /// </summary>
    /// <param name="cardListingOrder"><see cref="CardListingOrder"/></param>
    /// <returns><see cref="ListingOrder"/></returns>
    public ListingOrder Map(CardListingOrder cardListingOrder)
    {
        return ListingOrder.Create(cardListingOrder.Date, price: cardListingOrder.Price);
    }
}