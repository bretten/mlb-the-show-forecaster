using com.brettnamba.MlbTheShowForecaster.GameCards.Domain.Marketplace.Entities;
using com.brettnamba.MlbTheShowForecaster.GameCards.Domain.Marketplace.ValueObjects;

namespace com.brettnamba.MlbTheShowForecaster.GameCards.Application.Dtos.Mapping;

/// <summary>
/// Defines a mapper that maps the Application-level listing to the Domain's <see cref="Listing"/>
/// </summary>
public interface IListingMapper
{
    /// <summary>
    /// Should map <see cref="CardListing"/> to <see cref="Listing"/>
    /// </summary>
    /// <param name="listing">The listing to map</param>
    /// <returns><see cref="Listing"/></returns>
    Listing Map(CardListing listing);

    /// <summary>
    /// Should map <see cref="CardListingPrice"/> to <see cref="ListingHistoricalPrice"/>
    /// </summary>
    /// <param name="cardListingPrice">The previous prices for the listing</param>
    /// <returns><see cref="ListingHistoricalPrice"/></returns>
    ListingHistoricalPrice Map(CardListingPrice cardListingPrice);
}