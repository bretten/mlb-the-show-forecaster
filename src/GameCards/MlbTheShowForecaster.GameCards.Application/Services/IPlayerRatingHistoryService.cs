using com.brettnamba.MlbTheShowForecaster.Common.Domain.ValueObjects;
using com.brettnamba.MlbTheShowForecaster.GameCards.Application.Services.Results;
using com.brettnamba.MlbTheShowForecaster.GameCards.Domain.Cards.Entities;
using com.brettnamba.MlbTheShowForecaster.GameCards.Domain.Cards.ValueObjects.PlayerCards;

namespace com.brettnamba.MlbTheShowForecaster.GameCards.Application.Services;

/// <summary>
/// Defines a service that creates a history of <see cref="PlayerCardHistoricalRating"/>s for a <see cref="PlayerCard"/>
/// </summary>
public interface IPlayerRatingHistoryService : IDisposable
{
    /// <summary>
    /// Should update all <see cref="PlayerCard"/>s with their entire history of <see cref="PlayerCardHistoricalRating"/>s
    /// </summary>
    /// <param name="seasonYear">The season to sync rating changes for</param>
    /// <param name="cancellationToken">A <see cref="CancellationToken" /> to observe while waiting for the task to complete</param>
    /// <returns><see cref="PlayerRatingHistoryResult"/></returns>
    Task<PlayerRatingHistoryResult> SyncHistory(SeasonYear seasonYear, CancellationToken cancellationToken = default);
}