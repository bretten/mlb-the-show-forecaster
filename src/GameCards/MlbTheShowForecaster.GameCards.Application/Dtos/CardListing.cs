using com.brettnamba.MlbTheShowForecaster.Common.Domain.ValueObjects;
using com.brettnamba.MlbTheShowForecaster.GameCards.Domain.Cards.ValueObjects;
using com.brettnamba.MlbTheShowForecaster.GameCards.Domain.Marketplace.Entities;

namespace com.brettnamba.MlbTheShowForecaster.GameCards.Application.Dtos;

/// <summary>
/// Represents a Listing from the MLB The Show
/// </summary>
/// <param name="ListingName">The name of the Listing</param>
/// <param name="BestBuyPrice">The current, best buy price</param>
/// <param name="BestSellPrice">The current, best sell price</param>
/// <param name="CardExternalId">The external ID (MLB The Show UUID) of the card</param>
/// <param name="HistoricalPrices">The prices on the card for previous days</param>
/// <param name="RecentOrders">Recent orders</param>
public readonly record struct CardListing(
    string ListingName,
    NaturalNumber BestBuyPrice,
    NaturalNumber BestSellPrice,
    CardExternalId CardExternalId,
    IReadOnlyList<CardListingPrice> HistoricalPrices,
    IReadOnlyList<CardListingOrder> RecentOrders
)
{
    /// <summary>
    /// Returns true if the <see cref="BestBuyPrice"/> or <see cref="BestSellPrice"/> is different from the prices
    /// on the domain <see cref="Listing"/>
    /// </summary>
    /// <param name="domainListing">The domain <see cref="Listing"/> to check against</param>
    /// <returns>True if the prices are different than the <see cref="Listing"/></returns>
    public bool HasNewPrices(Listing domainListing) =>
        BestBuyPrice != domainListing.BuyPrice || BestSellPrice != domainListing.SellPrice;
};