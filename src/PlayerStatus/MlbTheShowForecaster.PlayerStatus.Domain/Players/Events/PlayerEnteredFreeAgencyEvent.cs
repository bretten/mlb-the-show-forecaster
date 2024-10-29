using com.brettnamba.MlbTheShowForecaster.Common.Domain.Events;
using com.brettnamba.MlbTheShowForecaster.Common.Domain.ValueObjects;

namespace com.brettnamba.MlbTheShowForecaster.PlayerStatus.Domain.Players.Events;

/// <summary>
/// Published when a Player enters free agency
/// </summary>
/// <param name="Year">The year the player entered free agency</param>
/// <param name="PlayerMlbId">The MLB ID of the player</param>
/// <param name="Date">The date</param>
public sealed record PlayerEnteredFreeAgencyEvent(SeasonYear Year, MlbId PlayerMlbId, DateOnly Date) : IDomainEvent;