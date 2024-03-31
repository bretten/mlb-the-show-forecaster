using com.brettnamba.MlbTheShowForecaster.Common.Domain.ValueObjects;
using com.brettnamba.MlbTheShowForecaster.GameCards.Application.Dtos;

namespace com.brettnamba.MlbTheShowForecaster.GameCards.Application.Services;

/// <summary>
/// Defines a service that will retrieve cards from an external source
/// </summary>
public interface ICardCatalog
{
    /// <summary>
    /// Should return all player cards for the specified season
    /// </summary>
    /// <param name="seasonYear">The season</param>
    /// <param name="cancellationToken">A <see cref="CancellationToken" /> to observe while waiting for the task to complete</param>
    /// <returns><see cref="MlbPlayerCard"/> for the specified season</returns>
    Task<IReadOnlyList<MlbPlayerCard>?>
        GetAllMlbPlayerCards(SeasonYear seasonYear, CancellationToken cancellationToken);
}