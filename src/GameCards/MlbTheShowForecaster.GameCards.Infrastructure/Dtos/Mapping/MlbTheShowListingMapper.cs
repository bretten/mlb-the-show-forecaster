using com.brettnamba.MlbTheShowForecaster.Common.Domain.ValueObjects;
using com.brettnamba.MlbTheShowForecaster.ExternalApis.MlbTheShowApi.Dtos.Items;
using com.brettnamba.MlbTheShowForecaster.ExternalApis.MlbTheShowApi.Dtos.Listings;
using com.brettnamba.MlbTheShowForecaster.GameCards.Application.Dtos;
using com.brettnamba.MlbTheShowForecaster.GameCards.Domain.Cards.ValueObjects;
using com.brettnamba.MlbTheShowForecaster.GameCards.Infrastructure.Dtos.Mapping.Exceptions;

namespace com.brettnamba.MlbTheShowForecaster.GameCards.Infrastructure.Dtos.Mapping;

/// <summary>
/// Mapper that maps <see cref="ListingDto{T}"/>s from MLB The Show to application-level DTOs
/// </summary>
public sealed class MlbTheShowListingMapper : IMlbTheShowListingMapper
{
    /// <summary>
    /// Maps a <see cref="ListingDto{T}"/> to a <see cref="CardListing"/>
    /// </summary>
    /// <param name="year">The year of the card that the listing is for</param>
    /// <param name="listing">The <see cref="ListingDto{T}"/> to map</param>
    /// <returns><see cref="CardListing"/></returns>
    public CardListing Map(SeasonYear year, ListingDto<ItemDto> listing)
    {
        return new CardListing(listing.ListingName,
            BestBuyPrice: NaturalNumber.Create(listing.BestBuyPrice),
            BestSellPrice: NaturalNumber.Create(listing.BestSellPrice),
            CardExternalId: CardExternalId.Create(listing.Item.Uuid),
            HistoricalPrices: listing.PriceHistory?.Select(x => MapPrice(year, x)).ToList() ??
                              new List<CardListingPrice>()
        );
    }

    /// <summary>
    /// Maps a <see cref="ListingPriceDto"/> to a <see cref="CardListingPrice"/>
    /// </summary>
    /// <param name="year">The year of the card that the listing is for</param>
    /// <param name="price">The <see cref="ListingPriceDto"/> to map</param>
    /// <returns><see cref="CardListingPrice"/></returns>
    /// <exception cref="InvalidTheShowListingPriceDateFormatException">Thrown when the date string from MLB The Show on the <see cref="ListingPriceDto"/> has an invalid format</exception>
    public CardListingPrice MapPrice(SeasonYear year, ListingPriceDto price)
    {
        if (!DateOnly.TryParse(price.Date, out var date))
        {
            throw new InvalidTheShowListingPriceDateFormatException(
                $"Listing price date could not be parsed: {price.Date}");
        }

        return new CardListingPrice(new DateOnly(year.Value, date.Month, date.Day),
            BestBuyPrice: NaturalNumber.Create(price.BestBuyPrice),
            BestSellPrice: NaturalNumber.Create(price.BestSellPrice)
        );
    }
}