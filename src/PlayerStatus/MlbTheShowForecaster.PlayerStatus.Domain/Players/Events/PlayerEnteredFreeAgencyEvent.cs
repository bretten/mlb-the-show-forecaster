using com.brettnamba.MlbTheShowForecaster.Common.Domain.Events;
using com.brettnamba.MlbTheShowForecaster.Common.Domain.ValueObjects;

namespace com.brettnamba.MlbTheShowForecaster.PlayerStatus.Domain.Players.Events;

/// <summary>
/// Published when a Player enters free agency
/// </summary>
/// <param name="PlayerMlbId">The MLB ID of the player</param>
public record PlayerEnteredFreeAgencyEvent(MlbId PlayerMlbId) : IDomainEvent;