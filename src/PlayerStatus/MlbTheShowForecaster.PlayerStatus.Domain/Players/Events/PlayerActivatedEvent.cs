using com.brettnamba.MlbTheShowForecaster.Common.Domain.Events;
using com.brettnamba.MlbTheShowForecaster.Common.Domain.ValueObjects;

namespace com.brettnamba.MlbTheShowForecaster.PlayerStatus.Domain.Players.Events;

/// <summary>
/// Published when a Player is activated
/// </summary>
/// <param name="PlayerMlbId">The MLB ID of the player</param>
public record PlayerActivatedEvent(MlbId PlayerMlbId) : IDomainEvent;