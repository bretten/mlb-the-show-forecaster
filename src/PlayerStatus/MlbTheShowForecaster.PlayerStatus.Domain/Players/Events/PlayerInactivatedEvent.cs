using com.brettnamba.MlbTheShowForecaster.Core.Events;
using com.brettnamba.MlbTheShowForecaster.PlayerStatus.Domain.Common.ValueObjects;

namespace com.brettnamba.MlbTheShowForecaster.PlayerStatus.Domain.Players.Events;

/// <summary>
/// Published when a Player is inactivated
/// </summary>
/// <param name="PlayerMlbId">The MLB ID of the player</param>
public record PlayerInactivatedEvent(MlbId PlayerMlbId) : IDomainEvent;