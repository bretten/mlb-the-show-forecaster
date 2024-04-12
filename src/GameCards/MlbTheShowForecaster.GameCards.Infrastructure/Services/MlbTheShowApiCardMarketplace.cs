using com.brettnamba.MlbTheShowForecaster.Common.Domain.ValueObjects;
using com.brettnamba.MlbTheShowForecaster.ExternalApis.MlbTheShowApi;
using com.brettnamba.MlbTheShowForecaster.ExternalApis.MlbTheShowApi.Dtos.Enums;
using com.brettnamba.MlbTheShowForecaster.ExternalApis.MlbTheShowApi.Requests.Listings;
using com.brettnamba.MlbTheShowForecaster.GameCards.Application.Dtos;
using com.brettnamba.MlbTheShowForecaster.GameCards.Application.Services;
using com.brettnamba.MlbTheShowForecaster.GameCards.Application.Services.Exceptions;
using com.brettnamba.MlbTheShowForecaster.GameCards.Domain.Cards.ValueObjects;
using com.brettnamba.MlbTheShowForecaster.GameCards.Infrastructure.Dtos.Mapping;

namespace com.brettnamba.MlbTheShowForecaster.GameCards.Infrastructure.Services;

/// <summary>
/// Service that can retrieve marketplace information on cards such as pricing
/// </summary>
public sealed class MlbTheShowApiCardMarketplace : ICardMarketplace
{
    /// <summary>
    /// Client factory for <see cref="IMlbTheShowApi"/>. Provides a client based on a season year
    /// </summary>
    private readonly IMlbTheShowApiFactory _mlbTheShowApiFactory;

    /// <summary>
    /// Maps MLB The Show DTOs to application-layer DTOs
    /// </summary>
    private readonly IMlbTheShowListingMapper _listingMapper;

    /// <summary>
    /// Constructor
    /// </summary>
    /// <param name="mlbTheShowApiFactory">Client factory for <see cref="IMlbTheShowApi"/>. Provides a client based on a season year</param>
    /// <param name="listingMapper">Maps MLB The Show DTOs to application-layer DTOs</param>
    public MlbTheShowApiCardMarketplace(IMlbTheShowApiFactory mlbTheShowApiFactory,
        IMlbTheShowListingMapper listingMapper)
    {
        _mlbTheShowApiFactory = mlbTheShowApiFactory;
        _listingMapper = listingMapper;
    }

    /// <summary>
    /// Retrieves the prices for a card by its <see cref="CardExternalId"/>
    /// </summary>
    /// <param name="seasonYear">The year of the card</param>
    /// <param name="cardExternalId">The <see cref="CardExternalId"/> of the card</param>
    /// <param name="cancellationToken">A <see cref="CancellationToken" /> to observe while waiting for the task to complete</param>
    /// <returns>The prices for the card</returns>
    /// <exception cref="CardListingNotFoundInMarketplaceException">Thrown when the specified listing cannot be found</exception>
    public async Task<CardListing> GetCardPrice(SeasonYear seasonYear, CardExternalId cardExternalId,
        CancellationToken cancellationToken = default)
    {
        // Get the client for the specified year
        var mlbTheShowApiForSeason = _mlbTheShowApiFactory.GetClient((Year)seasonYear.Value);

        var request = new GetListingRequest(Uuid: cardExternalId.ValueStringDigits);
        var listing = await mlbTheShowApiForSeason.GetListing(request);

        if (listing == null)
        {
            throw new CardListingNotFoundInMarketplaceException(
                $"No MLB The Show Card Listing found for UUID: {cardExternalId.Value}");
        }

        return _listingMapper.Map(seasonYear, listing);
    }
}