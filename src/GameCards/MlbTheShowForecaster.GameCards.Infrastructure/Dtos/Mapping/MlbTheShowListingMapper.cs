using com.brettnamba.MlbTheShowForecaster.Common.DateAndTime;
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
    /// Calendar used to determine the date of a historical price
    /// </summary>
    private readonly ICalendar _calendar;

    /// <summary>
    /// Constructor
    /// </summary>
    /// <param name="calendar">Calendar used to determine the date of a historical price</param>
    public MlbTheShowListingMapper(ICalendar calendar)
    {
        _calendar = calendar;
    }

    /// <summary>
    /// Maps a <see cref="ListingDto{T}"/> to a <see cref="CardListing"/>
    /// </summary>
    /// <param name="listing">The <see cref="ListingDto{T}"/> to map</param>
    /// <returns><see cref="CardListing"/></returns>
    public CardListing Map(ListingDto<ItemDto> listing)
    {
        return new CardListing(listing.ListingName,
            BestBuyPrice: NaturalNumber.Create(listing.BestBuyPrice),
            BestSellPrice: NaturalNumber.Create(listing.BestSellPrice),
            CardExternalId: CardExternalId.Create(listing.Item.Uuid.Value ??
                                                  throw new InvalidTheShowUuidException(
                                                      $"Could not map the {nameof(ListingDto<ItemDto>)}'s UUID since it is not valid: ${listing.Item.Uuid.RawValue}")),
            HistoricalPrices: listing.PriceHistory != null
                ? MapPrices(listing.PriceHistory)
                : new List<CardListingPrice>()
        );
    }

    /// <summary>
    /// Maps a collection of price DTOs
    /// </summary>
    /// <param name="prices">The prices to map</param>
    /// <returns><see cref="CardListingPrice"/> collection</returns>
    /// <exception cref="InvalidTheShowListingPriceDateFormatException">Thrown if a price has an invalid date string</exception>
    private List<CardListingPrice> MapPrices(IReadOnlyCollection<ListingPriceDto> prices)
    {
        var historicalPrices = new List<CardListingPrice>();

        // MLB The Show's API has an issue where games from previous years will start listing prices at 12/31
        // These seem to be erroneous and can be ignored until the current date's pricing is found
        // You can then start at the current calendar year and go back to the day that the game launched
        var today = _calendar.TodayPst();
        var todayDateString = today.ToString("MM/dd"); // Used to skip erroneous dates
        var yesterdayDateString = today.AddDays(-1).ToString("MM/dd"); // Used to skip erroneous dates
        var passedErroneousDates = false; // Will be set to true once erroneous rows have been passed
        var currentYear = today.Year; // 1st price is for the current year. When 12/31 is reached, decrement year
        var previousDateString = todayDateString; // Used to handle case where data has no 12/31 date, only 12/30

        foreach (var price in prices)
        {
            // The dates are in the format MM/dd with no year. It starts at the current calendar year and goes back to the year the game launched
            var dateString = price.Date;

            // Skip erroneous dates
            if (!passedErroneousDates && dateString != todayDateString && dateString != yesterdayDateString)
            {
                continue;
            }

            // The current date has been reached, so no more skipping needed
            passedErroneousDates = true;

            // When the previous year has been reached, change the year
            // NOTE: When looking at 2021 data for Aaron Judge, some 12/31's were missing, so use 12/30 as a fallback
            if (previousDateString == "01/01" && (dateString == "12/31" || dateString == "12/30"))
            {
                currentYear--;
            }

            try
            {
                // Parse the date and use the current year counter
                var date = DateOnly.ParseExact($"{dateString}/{currentYear}", "MM/dd/yyyy");

                // Buy price is the first number, sell price is the second
                var buyNowPrice = price.BestBuyPrice;
                var sellNowPrice = price.BestSellPrice;

                historicalPrices.Add(new CardListingPrice(date, NaturalNumber.Create(buyNowPrice),
                    NaturalNumber.Create(sellNowPrice)));

                previousDateString = dateString;
            }
            catch (FormatException)
            {
                throw new InvalidTheShowListingPriceDateFormatException(
                    $"Listing price date could not be parsed: {price.Date}");
            }
        }

        return historicalPrices;
    }
}