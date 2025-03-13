using System.Globalization;
using System.Text.RegularExpressions;
using AngleSharp;
using AngleSharp.Dom;
using com.brettnamba.MlbTheShowForecaster.Common.Domain.ValueObjects;
using com.brettnamba.MlbTheShowForecaster.ExternalApis.MlbTheShowApi.Dtos.Items;
using com.brettnamba.MlbTheShowForecaster.ExternalApis.MlbTheShowApi.Dtos.Listings;
using com.brettnamba.MlbTheShowForecaster.GameCards.Application.Dtos;
using com.brettnamba.MlbTheShowForecaster.GameCards.Application.Services;
using com.brettnamba.MlbTheShowForecaster.GameCards.Application.Services.Exceptions;
using com.brettnamba.MlbTheShowForecaster.GameCards.Domain.Cards.ValueObjects;
using com.brettnamba.MlbTheShowForecaster.GameCards.Infrastructure.Dtos.Mapping;
using com.brettnamba.MlbTheShowForecaster.GameCards.Infrastructure.Services.Exceptions;
using Polly;

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
    /// Retry policy for the web client
    /// </summary>
    private readonly ResiliencePipeline<IDocument> _resiliencePipeline;

    /// <summary>
    /// Maps MLB The Show DTOs to application-layer DTOs
    /// </summary>
    private readonly IMlbTheShowListingMapper _listingMapper;

    /// <summary>
    /// Constructor
    /// </summary>
    /// <param name="context"><see cref="IBrowsingContext"/> for <see cref="AngleSharp"/></param>
    /// <param name="resiliencePipeline">Retry policy for the web client</param>
    /// <param name="listingMapper">Maps MLB The Show DTOs to application-layer DTOs</param>
    public MlbTheShowComCardMarketplace(IBrowsingContext context, ResiliencePipeline<IDocument> resiliencePipeline,
        IMlbTheShowListingMapper listingMapper)
    {
        _context = context;
        _resiliencePipeline = resiliencePipeline;
        _listingMapper = listingMapper;
    }

    /// <inheritdoc />
    public async Task<CardListing> GetCardPrice(SeasonYear seasonYear, CardExternalId cardExternalId,
        CancellationToken cancellationToken = default)
    {
        // The url format is mlb[yy].theshow.com, eg, mlb24.theshow.com
        var seasonDate = new DateOnly(seasonYear.Value, 1, 1);
        var url = $"https://mlb{seasonDate.ToString("yy")}.theshow.com/items/{cardExternalId.AsStringDigits}";

        var doc = await _resiliencePipeline.ExecuteAsync(
            async token => await _context.OpenAsync(url, token), cancellationToken);

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

        // Parse the historical prices
        var prices = ParseHistoricalPrices(doc, cardExternalId);
        // Parse the orders
        var orders = ParseCompletedOrders(doc, cardExternalId);

        // Map to the MLB The Show API DTOs so we can use the same mapper
        var listingDto = new ListingDto<ItemDto>(listingName,
            BestSellPrice: prices.LastOrDefault()?.BestSellPrice ?? 0,
            BestBuyPrice: prices.LastOrDefault()?.BestBuyPrice ?? 0,
            // The ItemDto type doesn't matter, just the UUID
            new UnlockableDto(Uuid: new UuidDto(cardExternalId.AsStringDigits), "", "", "", "", false, 0, 0),
            prices, orders);

        // The mapper parsed out the erroneous prices, so use the correct prices
        var tempListing = _listingMapper.Map(seasonYear, listingDto);
        return tempListing with
        {
            BestBuyPrice = NaturalNumber.Create(tempListing.HistoricalPrices.FirstOrDefault().BestBuyPrice.Value),
            BestSellPrice = NaturalNumber.Create(tempListing.HistoricalPrices.FirstOrDefault().BestSellPrice.Value)
        };
    }

    /// <summary>
    /// Parses the historical prices from the HTML document
    /// </summary>
    /// <param name="doc">The HTML document</param>
    /// <param name="cardExternalId">The <see cref="CardExternalId"/> of the card</param>
    /// <returns>Historical prices</returns>
    /// <exception cref="MlbTheShowComCardMarketplaceParsingException">Thrown when a row does not have enough cells</exception>
    private List<ListingPriceDto> ParseHistoricalPrices(IDocument doc, CardExternalId cardExternalId)
    {
        var prices = new List<ListingPriceDto>();
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
            // Buy price is the first number, sell price is the second
            var buyNowPrice = int.Parse(cells[1].TextContent.Trim());
            var sellNowPrice = int.Parse(cells[2].TextContent.Trim());
            prices.Add(new ListingPriceDto(dateString, buyNowPrice, sellNowPrice));
        }

        return prices;
    }

    /// <summary>
    /// Parses the completed orders from the HTML document
    /// </summary>
    /// <param name="doc">The HTML document</param>
    /// <param name="cardExternalId">The <see cref="CardExternalId"/> of the card</param>
    /// <returns>The orders for the Listing</returns>
    /// <exception cref="MlbTheShowComCardMarketplaceParsingException">Thrown when a row does not have the right number of cells</exception>
    private List<ListingOrderDto> ParseCompletedOrders(IDocument doc, CardExternalId cardExternalId)
    {
        var rows = doc.QuerySelectorAll("#table-completed-orders tbody tr");
        return rows.Select(row =>
        {
            var cells = row.QuerySelectorAll("td");
            if (cells.Length != 2)
            {
                throw new MlbTheShowComCardMarketplaceParsingException(
                    $"The completed order row was not formatted correctly for {cardExternalId.AsStringDigits}");
            }

            var parsedPrice = Regex.Replace(cells[0].TextContent.Trim(), @"[^\d]", "");
            var price = int.Parse(parsedPrice);

            // Parse the date string which is in the format 1/16/2025 3:38AM PST
            var dateString = cells[1].TextContent.Trim().Replace(" PST", "").Replace(" PDT", "");
            var rawDateTime = DateTime.ParseExact(dateString, "M/d/yyyy h:mmtt", CultureInfo.InvariantCulture);

            var tzInfo = TimeZoneInfo.FindSystemTimeZoneById("America/Los_Angeles");
            var dateOffset = new DateTimeOffset(rawDateTime, tzInfo.BaseUtcOffset);

            // Exclude seconds
            var utcDate = new DateTime(dateOffset.UtcDateTime.Year, dateOffset.UtcDateTime.Month,
                dateOffset.UtcDateTime.Day, dateOffset.UtcDateTime.Hour, dateOffset.UtcDateTime.Minute, 0,
                DateTimeKind.Utc);

            return new ListingOrderDto(utcDate.ToString("MM/dd/yyyy HH:mm:ss"), price.ToString());
        }).ToList();
    }

    /// <summary>
    /// Dispose
    /// </summary>
    public void Dispose()
    {
        _context.Dispose();
    }
}