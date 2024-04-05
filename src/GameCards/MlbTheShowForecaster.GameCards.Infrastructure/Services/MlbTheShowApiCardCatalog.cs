using com.brettnamba.MlbTheShowForecaster.Common.Domain.ValueObjects;
using com.brettnamba.MlbTheShowForecaster.ExternalApis.MlbTheShowApi;
using com.brettnamba.MlbTheShowForecaster.ExternalApis.MlbTheShowApi.Dtos.Enums;
using com.brettnamba.MlbTheShowForecaster.ExternalApis.MlbTheShowApi.Requests.Items;
using com.brettnamba.MlbTheShowForecaster.ExternalApis.MlbTheShowApi.Responses.Items;
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
    public async Task<IReadOnlyList<MlbPlayerCard>> GetActiveRosterMlbPlayerCards(SeasonYear seasonYear,
        CancellationToken cancellationToken = default)
    {
        // Get the client for the specified year
        var mlbTheShowApiForSeason = _mlbTheShowApiFactory.GetClient((Year)seasonYear.Value);

        // Holds the resulting player cards
        var theShowCards = new List<MlbPlayerCard>();

        GetItemsPaginatedResponse response;
        var page = 1;
        do
        {
            response = await mlbTheShowApiForSeason.GetItems(new GetItemsRequest(page, ItemType.MlbCard));

            // Map the items in the response to player cards
            if (response.Items.Any())
            {
                theShowCards.AddRange(response.Items.Select(x => _itemMapper.Map(seasonYear, x)));
            }

            page++;
        } while (response.Items.Any());

        if (!theShowCards.Any())
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

        var request = new GetItemRequest(Uuid: cardExternalId.Value);
        var item = await mlbTheShowApiForSeason.GetItem(request);

        if (item == null)
        {
            throw new MlbPlayerCardNotFoundInCatalogException(
                $"No MLB The Show Card found for UUID: {cardExternalId.Value}");
        }

        return _itemMapper.Map(seasonYear, item);
    }
}