using com.brettnamba.MlbTheShowForecaster.GameCards.Domain.Cards.Entities;

namespace com.brettnamba.MlbTheShowForecaster.GameCards.Application.Services.Results;

/// <summary>
/// Represents the result of <see cref="IPlayerRatingHistoryService.SyncHistory"/>
/// </summary>
/// <param name="UpdatedPlayerCards">The PlayerCards that were updated</param>
public readonly record struct PlayerRatingHistoryResult(IEnumerable<PlayerCard> UpdatedPlayerCards);