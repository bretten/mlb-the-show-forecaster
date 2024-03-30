using System.Collections.Immutable;
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
public readonly record struct CardListing(
    string ListingName,
    NaturalNumber BestBuyPrice,
    NaturalNumber BestSellPrice,
    CardExternalId CardExternalId,
    IReadOnlyList<CardListingPrice> HistoricalPrices
)
{
    /// <summary>
    /// Returns true if this <see cref="CardListing"/> has historical prices that the domain <see cref="Listing"/>
    /// does not have, otherwise false
    /// </summary>
    /// <param name="domainListing">The domain <see cref="Listing"/></param>
    /// <returns>True if there are new historical prices for the <see cref="Listing"/></returns>
    public bool HasNewHistoricalPrices(Listing domainListing) =>
        domainListing.HistoricalPricesChronologically.Select(x => x.Date).OrderBy(x => x)
        != HistoricalPrices.Select(x => x.Date).OrderBy(x => x);

    /// <summary>
    /// Returns new historical prices for the <see cref="Listing"/>
    /// </summary>
    /// <param name="domainListing">The domain <see cref="Listing"/></param>
    /// <returns>The new historical prices for the <see cref="Listing"/></returns>
    public IReadOnlyList<CardListingPrice> GetNewHistoricalPrices(Listing domainListing)
    {
        return HistoricalPrices
            .Where(x => !domainListing.HistoricalPricesChronologically.Select(y => y.Date).Contains(x.Date))
            .ToImmutableList();
    }
};