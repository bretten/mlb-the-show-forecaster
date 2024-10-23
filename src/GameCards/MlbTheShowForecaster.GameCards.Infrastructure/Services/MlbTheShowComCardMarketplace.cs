using AngleSharp;
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
    /// Constructor
    /// </summary>
    /// <param name="context"><see cref="IBrowsingContext"/> for <see cref="AngleSharp"/></param>
    public MlbTheShowComCardMarketplace(IBrowsingContext context)
    {
        _context = context;
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

        // Parse historical prices
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

            // No year is provided with the date
            var dateWithAutoYear = DateOnly.ParseExact(cells[0].TextContent.Trim(), "MM/dd");
            var date = new DateOnly(seasonYear.Value, dateWithAutoYear.Month, dateWithAutoYear.Day);

            // Buy price is the first number, sell price is the second
            var buyNowPrice = int.Parse(cells[1].TextContent.Trim());
            var sellNowPrice = int.Parse(cells[2].TextContent.Trim());

            historicalPrices.Add(new CardListingPrice(date, NaturalNumber.Create(buyNowPrice),
                NaturalNumber.Create(sellNowPrice)));
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