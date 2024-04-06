using com.brettnamba.MlbTheShowForecaster.Common.Domain.ValueObjects;
using com.brettnamba.MlbTheShowForecaster.ExternalApis.MlbTheShowApi.Dtos.Items;
using com.brettnamba.MlbTheShowForecaster.ExternalApis.MlbTheShowApi.Dtos.Listings;
using com.brettnamba.MlbTheShowForecaster.GameCards.Application.Dtos;
using com.brettnamba.MlbTheShowForecaster.GameCards.Infrastructure.Dtos.Mapping.Exceptions;

namespace com.brettnamba.MlbTheShowForecaster.GameCards.Infrastructure.Dtos.Mapping;

/// <summary>
/// Defines a mapper that maps <see cref="ListingDto{T}"/>s from MLB The Show to application-level DTOs
/// </summary>
public interface IMlbTheShowListingMapper
{
    /// <summary>
    /// Should map a <see cref="ListingDto{T}"/> to a <see cref="CardListing"/>
    /// </summary>
    /// <param name="year">The year of the card that the listing is for</param>
    /// <param name="listing">The <see cref="ListingDto{T}"/> to map</param>
    /// <returns><see cref="CardListing"/></returns>
    CardListing Map(SeasonYear year, ListingDto<ItemDto> listing);

    /// <summary>
    /// Should map a <see cref="ListingPriceDto"/> to a <see cref="CardListingPrice"/>
    /// </summary>
    /// <param name="year">The year of the card that the listing is for</param>
    /// <param name="price">The <see cref="ListingPriceDto"/> to map</param>
    /// <returns><see cref="CardListingPrice"/></returns>
    /// <exception cref="InvalidTheShowListingPriceDateFormatException">Thrown when the date string from MLB The Show on the <see cref="ListingPriceDto"/> has an invalid format</exception>
    CardListingPrice MapPrice(SeasonYear year, ListingPriceDto price);
}