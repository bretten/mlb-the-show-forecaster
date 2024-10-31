using System.Collections.Concurrent;
using com.brettnamba.MlbTheShowForecaster.Common.Domain.ValueObjects;
using com.brettnamba.MlbTheShowForecaster.ExternalApis.MlbTheShowApi;
using com.brettnamba.MlbTheShowForecaster.ExternalApis.MlbTheShowApi.Dtos.Enums;
using com.brettnamba.MlbTheShowForecaster.ExternalApis.MlbTheShowApi.Dtos.Items;
using com.brettnamba.MlbTheShowForecaster.ExternalApis.MlbTheShowApi.Requests.Items;
using com.brettnamba.MlbTheShowForecaster.GameCards.Application.Dtos;
using com.brettnamba.MlbTheShowForecaster.GameCards.Application.Services;
using com.brettnamba.MlbTheShowForecaster.GameCards.Application.Services.Exceptions;
using com.brettnamba.MlbTheShowForecaster.GameCards.Domain.Cards.ValueObjects;
using com.brettnamba.MlbTheShowForecaster.GameCards.Infrastructure.Dtos.Mapping;

namespace com.brettnamba.MlbTheShowForecaster.GameCards.Infrastructure.Services;

/// <summary>
/// Service that retrieves cards from MLB The Show using the <see cref="IMlbTheShowApi"/> client
/// </summary>
public sealed class MlbTheShowApiCardCatalog : ICardCatalog
{
    /// <summary>
    /// Client factory for <see cref="IMlbTheShowApi"/>. Provides a client based on a season year
    /// </summary>
    private readonly IMlbTheShowApiFactory _mlbTheShowApiFactory;

    /// <summary>
    /// Maps MLB The Show DTOs to application-layer DTOs
    /// </summary>
    private readonly IMlbTheShowItemMapper _itemMapper;

    /// <summary>
    /// Constructor
    /// </summary>
    /// <param name="mlbTheShowApiFactory">Client factory for <see cref="IMlbTheShowApi"/>. Provides a client based on a season year</param>
    /// <param name="itemMapper">Maps MLB The Show DTOs to application-layer DTOs</param>
    public MlbTheShowApiCardCatalog(IMlbTheShowApiFactory mlbTheShowApiFactory, IMlbTheShowItemMapper itemMapper)
    {
        _mlbTheShowApiFactory = mlbTheShowApiFactory;
        _itemMapper = itemMapper;
    }

    /// <summary>
    /// Retrieves active roster MLB player cards for the specified season using <see cref="IMlbTheShowApi"/>
    /// </summary>
    /// <param name="seasonYear">The season</param>
    /// <param name="cancellationToken">A <see cref="CancellationToken" /> to observe while waiting for the task to complete</param>
    /// <returns><see cref="MlbPlayerCard"/> for the specified season</returns>
    /// <exception cref="ActiveRosterMlbPlayerCardsNotFoundInCatalogException">Thrown when no active roster cards found</exception>
    public async Task<IEnumerable<MlbPlayerCard>> GetActiveRosterMlbPlayerCards(SeasonYear seasonYear,
        CancellationToken cancellationToken = default)
    {
        // Get the client for the specified year
        var mlbTheShowApiForSeason = _mlbTheShowApiFactory.GetClient((Year)seasonYear.Value);

        // Holds the resulting player cards
        var theShowCards = new ConcurrentBag<MlbPlayerCard>();

        // Get the first page of cards so that the total pages is known
        var pageOneResponse = await mlbTheShowApiForSeason.GetItems(new GetItemsRequest(1, ItemType.MlbCard));
        var pageOneCards = FilterLiveMlbCards(pageOneResponse.Items)
            .Select(x => _itemMapper.Map(seasonYear, x));
        foreach (var card in pageOneCards)
        {
            theShowCards.Add(card);
        }

        // The different pages to request
        var pages = new List<int>();
        for (var i = 2; i <= pageOneResponse.TotalPages; i++)
        {
            pages.Add(i);
        }

        // Get cards from each page
        await Parallel.ForEachAsync(pages, new ParallelOptions { MaxDegreeOfParallelism = 10 }, async (page, ct) =>
        {
            var r = await mlbTheShowApiForSeason.GetItems(new GetItemsRequest(page, ItemType.MlbCard));

            // Map the items in the response to player cards
            if (r.Items.Any())
            {
                var cards = FilterLiveMlbCards(r.Items)
                    .Select(x => _itemMapper.Map(seasonYear, x));
                foreach (var card in cards)
                {
                    theShowCards.Add(card);
                }
            }
        });

        if (theShowCards.IsEmpty)
        {
            throw new ActiveRosterMlbPlayerCardsNotFoundInCatalogException(
                $"No active roster found for {seasonYear.Value}");
        }

        return theShowCards;
    }

    /// <summary>
    /// Retrieves a single MLB player card with the specified <see cref="CardExternalId"/> and season using the
    /// <see cref="IMlbTheShowApi"/>
    /// </summary>
    /// <param name="seasonYear">The season</param>
    /// <param name="cardExternalId">The <see cref="CardExternalId"/></param>
    /// <param name="cancellationToken">A <see cref="CancellationToken" /> to observe while waiting for the task to complete</param>
    /// <returns><see cref="MlbPlayerCard"/> with the same <see cref="CardExternalId"/> and season year</returns>
    /// <exception cref="MlbPlayerCardNotFoundInCatalogException">Thrown if the card cannot be found</exception>
    public async Task<MlbPlayerCard> GetMlbPlayerCard(SeasonYear seasonYear, CardExternalId cardExternalId,
        CancellationToken cancellationToken = default)
    {
        // Get the client for the specified year
        var mlbTheShowApiForSeason = _mlbTheShowApiFactory.GetClient((Year)seasonYear.Value);

        var request = new GetItemRequest(Uuid: cardExternalId.AsStringDigits);
        var item = await mlbTheShowApiForSeason.GetItem(request);

        if (item == null)
        {
            throw new MlbPlayerCardNotFoundInCatalogException(
                $"No MLB The Show Card found for UUID: {cardExternalId.Value}");
        }

        return _itemMapper.Map(seasonYear, item);
    }

    /// <summary>
    /// Filters out all <see cref="ItemDto"/>s except Live <see cref="MlbCardDto"/>s from the specified collection
    /// </summary>
    /// <param name="items">The collection to filter</param>
    /// <returns>The filtered collection</returns>
    private static IEnumerable<MlbCardDto> FilterLiveMlbCards(IEnumerable<ItemDto> items)
    {
        return items.Where(x => x.Type == Constants.ItemTypes.MlbCard)
            .Cast<MlbCardDto>()
            .Where(x => x.Series == Constants.Series.Live);
    }

    /// <summary>
    /// Dispose
    /// </summary>
    public void Dispose()
    {
        // MLB The Show API HTTP client is handled by Refit
    }
}