using com.brettnamba.MlbTheShowForecaster.Common.Domain.Events;
using com.brettnamba.MlbTheShowForecaster.Common.Domain.ValueObjects;

namespace com.brettnamba.MlbTheShowForecaster.PlayerStatus.Domain.Players.Events;

/// <summary>
/// Published when a Player signs a contract with a Team
/// </summary>
/// <param name="PlayerMlbId">The MLB ID of the player</param>
/// <param name="TeamMlbId">The MLB ID of the team</param>
public record PlayerSignedContractWithTeamEvent(MlbId PlayerMlbId, MlbId TeamMlbId) : IDomainEvent;