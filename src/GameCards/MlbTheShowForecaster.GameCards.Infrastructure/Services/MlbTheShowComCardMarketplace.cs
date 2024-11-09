using AngleSharp;
using com.brettnamba.MlbTheShowForecaster.Common.DateAndTime;
using com.brettnamba.MlbTheShowForecaster.Common.Domain.ValueObjects;
using com.brettnamba.MlbTheShowForecaster.GameCards.Application.Dtos;
using com.brettnamba.MlbTheShowForecaster.GameCards.Application.Services;
using com.brettnamba.MlbTheShowForecaster.GameCards.Application.Services.Exceptions;
using com.brettnamba.MlbTheShowForecaster.GameCards.Domain.Cards.ValueObjects;
using com.brettnamba.MlbTheShowForecaster.GameCards.Infrastructure.Services.Exceptions;

namespace com.brettnamba.MlbTheShowForecaster.GameCards.Infrastructure.Services;

/// <summary>
/// Service that reads current and historical card prices from the MLB The Show website
///
/// The implementation that uses the MLB The Show API, <see cref="MlbTheShowApiCardMarketplace"/>, cannot provide full
/// historical pricing because the API limits the data to 2 months. The website is able to provide the full history
/// </summary>
public sealed class MlbTheShowComCardMarketplace : ICardMarketplace
{
    /// <summary>
    /// <see cref="IBrowsingContext"/> for <see cref="AngleSharp"/>
    /// </summary>
    private readonly IBrowsingContext _context;

    /// <summary>
    /// Gets the current date
    /// </summary>
    private readonly ICalendar _calendar;

    /// <summary>
    /// Constructor
    /// </summary>
    /// <param name="context"><see cref="IBrowsingContext"/> for <see cref="AngleSharp"/></param>
    /// <param name="calendar">Gets the current date</param>
    public MlbTheShowComCardMarketplace(IBrowsingContext context, ICalendar calendar)
    {
        _context = context;
        _calendar = calendar;
    }

    /// <inheritdoc />
    public async Task<CardListing> GetCardPrice(SeasonYear seasonYear, CardExternalId cardExternalId,
        CancellationToken cancellationToken = default)
    {
        // The url format is mlb[yy].theshow.com, eg, mlb24.theshow.com
        var seasonDate = new DateOnly(seasonYear.Value, 1, 1);
        var url = $"https://mlb{seasonDate.ToString("yy")}.theshow.com/items/{cardExternalId.AsStringDigits}";

        var doc = await _context.OpenAsync(url, cancellationToken);

        // The site redirects if the player does not exist
        if (doc.Title == "Home - The Show Account")
        {
            throw new CardListingNotFoundInMarketplaceException($"No MLB The Show Card Listing found at {url}");
        }

        // Parse the listing name whose format is: "MLB The Show 24 - [Listing Name]"
        var titleParts = doc.Title?.Split('-');
        if (titleParts == null || titleParts.Length < 2)
        {
            throw new MlbTheShowComCardMarketplaceParsingException(
                $"The listing name could not be parsed for {cardExternalId.AsStringDigits}: {doc.Title}");
        }

        var listingName = titleParts[1].Trim();

        // MLB The Show's website has an issue where games from previous years will start listing prices at 12/31
        // These seem to be erroneous and can be ignored until the current date's pricing is found
        // You can then start at the current calendar year and go back to the day that the game launched
        var today = _calendar.Today();
        var todayDateString = today.ToString("MM/dd"); // Used to skip erroneous dates
        var passedErroneousDates = false; // Will be set to true once erroneous rows have been passed
        var currentYear = today.Year; // 1st price is for the current year. When 12/31 is reached, decrement year
        var previousDateString = todayDateString; // Used to handle case where data has no 12/31 date, only 12/30

        // Parse historical prices from the table body rows
        var historicalPrices = new List<CardListingPrice>();
        var rows = doc.QuerySelectorAll("#table-trends tbody tr");
        foreach (var row in rows)
        {
            var cells = row.QuerySelectorAll("td");
            if (cells.Length < 3)
            {
                throw new MlbTheShowComCardMarketplaceParsingException(
                    $"The historical price row was not formatted correctly for {cardExternalId.AsStringDigits}");
            }

            // The dates are in the format MM/dd with no year. It starts at the current calendar year and goes back to the year the game launched
            var dateString = cells[0].TextContent.Trim();

            // Skip erroneous dates
            if (!passedErroneousDates && dateString != todayDateString)
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

            // Parse the date and use the current year counter
            var dateWithAutoYear = DateOnly.ParseExact(dateString, "MM/dd");
            var date = new DateOnly(currentYear, dateWithAutoYear.Month, dateWithAutoYear.Day);

            // Buy price is the first number, sell price is the second
            var buyNowPrice = int.Parse(cells[1].TextContent.Trim());
            var sellNowPrice = int.Parse(cells[2].TextContent.Trim());

            historicalPrices.Add(new CardListingPrice(date, NaturalNumber.Create(buyNowPrice),
                NaturalNumber.Create(sellNowPrice)));

            previousDateString = dateString;
        }

        var mostRecentPrice = historicalPrices.OrderByDescending(x => x.Date).First();
        return new CardListing(listingName, BestBuyPrice: mostRecentPrice.BestBuyPrice,
            BestSellPrice: mostRecentPrice.BestSellPrice, cardExternalId, historicalPrices);
    }

    /// <summary>
    /// Dispose
    /// </summary>
    public void Dispose()
    {
        _context.Dispose();
    }
}