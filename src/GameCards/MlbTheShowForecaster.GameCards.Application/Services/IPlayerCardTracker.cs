using com.brettnamba.MlbTheShowForecaster.Common.Domain.ValueObjects;
using com.brettnamba.MlbTheShowForecaster.GameCards.Domain.Cards.Entities;

namespace com.brettnamba.MlbTheShowForecaster.GameCards.Application.Services;

/// <summary>
/// Defines a service that retrieves player cards from an external source and creates a corresponding
/// <see cref="PlayerCard"/> in this domain if it does not yet exist
/// </summary>
public interface IPlayerCardTracker
{
    /// <summary>
    /// Should retrieve player cards from an external source and create a corresponding <see cref="PlayerCard"/> in
    /// this domain if it does not yet exist
    /// </summary>
    /// <param name="seasonYear">The year to retrieve cards for</param>
    /// <param name="cancellationToken">A <see cref="CancellationToken" /> to observe while waiting for the task to complete</param>
    /// <returns>The completed task</returns>
    Task TrackPlayerCards(SeasonYear seasonYear, CancellationToken cancellationToken = default);
}