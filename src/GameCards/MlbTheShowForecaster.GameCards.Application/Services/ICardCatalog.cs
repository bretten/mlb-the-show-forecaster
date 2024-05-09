using com.brettnamba.MlbTheShowForecaster.Common.Domain.ValueObjects;
using com.brettnamba.MlbTheShowForecaster.GameCards.Application.Dtos;
using com.brettnamba.MlbTheShowForecaster.GameCards.Application.Services.Exceptions;
using com.brettnamba.MlbTheShowForecaster.GameCards.Domain.Cards.ValueObjects;

namespace com.brettnamba.MlbTheShowForecaster.GameCards.Application.Services;

/// <summary>
/// Defines a service that will retrieve cards from an external source
/// </summary>
public interface ICardCatalog : IDisposable
{
    /// <summary>
    /// Should return active roster MLB player cards for the specified season
    /// </summary>
    /// <param name="seasonYear">The season</param>
    /// <param name="cancellationToken">A <see cref="CancellationToken" /> to observe while waiting for the task to complete</param>
    /// <returns><see cref="MlbPlayerCard"/> for the specified season</returns>
    /// <exception cref="ActiveRosterMlbPlayerCardsNotFoundInCatalogException">Thrown when no active roster cards found</exception>
    Task<IEnumerable<MlbPlayerCard>> GetActiveRosterMlbPlayerCards(SeasonYear seasonYear,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Should return a single MLB player card with the specified <see cref="CardExternalId"/> and season
    /// </summary>
    /// <param name="seasonYear">The season</param>
    /// <param name="cardExternalId">The <see cref="CardExternalId"/></param>
    /// <param name="cancellationToken">A <see cref="CancellationToken" /> to observe while waiting for the task to complete</param>
    /// <returns><see cref="MlbPlayerCard"/> with the same <see cref="CardExternalId"/> and season year</returns>
    /// <exception cref="MlbPlayerCardNotFoundInCatalogException">Thrown if the card cannot be found</exception>
    Task<MlbPlayerCard> GetMlbPlayerCard(SeasonYear seasonYear, CardExternalId cardExternalId,
        CancellationToken cancellationToken = default);
}