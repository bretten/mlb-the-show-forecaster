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

public sealed class MlbTheShowApiCardCatalog : ICardCatalog
{
    private readonly IMlbTheShowApiFactory _mlbTheShowApiFactory;


    private readonly IMlbTheShowItemMapper _itemMapper;

    public MlbTheShowApiCardCatalog(IMlbTheShowApiFactory mlbTheShowApiFactory, IMlbTheShowItemMapper itemMapper)
    {
        _mlbTheShowApiFactory = mlbTheShowApiFactory;
        _itemMapper = itemMapper;
    }

    public async Task<IReadOnlyList<MlbPlayerCard>?> GetActiveRosterMlbPlayerCards(SeasonYear seasonYear,
        CancellationToken cancellationToken = default)
    {
        var mlbTheShowApiForSeason = _mlbTheShowApiFactory.GetClient((Year)seasonYear.Value);

        var theShowCards = new List<MlbPlayerCard>();

        GetItemsPaginatedResponse response;
        var page = 1;
        do
        {
            response = await mlbTheShowApiForSeason.GetItems(new GetItemsRequest(page, ItemType.MlbCard));
            if (response.Items.Any()) theShowCards.AddRange(response.Items.Select(x => _itemMapper.Map(seasonYear, x)));

            page++;
        } while (response.Items.Any());

        if (!theShowCards.Any())
        {
            throw new MlbPlayerCardActiveRosterEmptyException($"No active roster found");
        }

        return theShowCards;
    }

    public async Task<MlbPlayerCard> GetMlbPlayerCard(SeasonYear seasonYear, CardExternalId cardExternalId,
        CancellationToken cancellationToken = default)
    {
        var mlbTheShowApiForSeason = _mlbTheShowApiFactory.GetClient((Year)seasonYear.Value);

        var request = new GetItemRequest(Uuid: cardExternalId.Value);
        var item = await mlbTheShowApiForSeason.GetItem(request);
        if (item == null)
        {
            throw new MlbPlayerCardNotFoundException($"No MLB The Show Card found for UUID: {cardExternalId.Value}");
        }

        return _itemMapper.Map(seasonYear, item);
    }
}