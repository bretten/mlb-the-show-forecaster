using com.brettnamba.MlbTheShowForecaster.Common.Domain.ValueObjects;
using com.brettnamba.MlbTheShowForecaster.GameCards.Application.Dtos;
using com.brettnamba.MlbTheShowForecaster.GameCards.Application.Services.Exceptions;
using com.brettnamba.MlbTheShowForecaster.GameCards.Domain.Cards.ValueObjects;

namespace com.brettnamba.MlbTheShowForecaster.GameCards.Application.Services;

/// <summary>
/// Defines a service that can retrieve marketplace information on cards such as pricing
/// </summary>
public interface ICardMarketplace
{
    /// <summary>
    /// Should return the prices for a card by its <see cref="CardExternalId"/>
    /// </summary>
    /// <param name="seasonYear">The year of the card</param>
    /// <param name="cardExternalId">The <see cref="CardExternalId"/> of the card</param>
    /// <param name="cancellationToken">A <see cref="CancellationToken" /> to observe while waiting for the task to complete</param>
    /// <returns>The prices for the card</returns>
    /// <exception cref="CardListingNotFoundInMarketplaceException">Thrown when the specified listing cannot be found</exception>
    Task<CardListing> GetCardPrice(SeasonYear seasonYear, CardExternalId cardExternalId,
        CancellationToken cancellationToken = default);
}