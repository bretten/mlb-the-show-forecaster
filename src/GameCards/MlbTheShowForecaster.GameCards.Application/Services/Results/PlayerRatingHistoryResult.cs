using com.brettnamba.MlbTheShowForecaster.GameCards.Domain.Cards.Entities;
using com.brettnamba.MlbTheShowForecaster.GameCards.Domain.Cards.ValueObjects.PlayerCards;

namespace com.brettnamba.MlbTheShowForecaster.GameCards.Application.Services.Results;

/// <summary>
/// Represents the result of <see cref="IPlayerRatingHistoryService.SyncHistory"/>
/// </summary>
/// <param name="UpdatedPlayerCards">The PlayerCards that were updated</param>
public readonly record struct PlayerRatingHistoryResult(
    Dictionary<PlayerCard, IReadOnlyList<PlayerCardHistoricalRating>> UpdatedPlayerCards);