using com.brettnamba.MlbTheShowForecaster.Common.Domain.Events;
using com.brettnamba.MlbTheShowForecaster.Common.Domain.ValueObjects;

namespace com.brettnamba.MlbTheShowForecaster.PlayerStatus.Domain.Players.Events;

/// <summary>
/// Published when a Player signs a contract with a Team
/// </summary>
/// <param name="Year">The year the player signed with the team</param>
/// <param name="PlayerMlbId">The MLB ID of the player</param>
/// <param name="TeamMlbId">The MLB ID of the team</param>
public sealed record PlayerSignedContractWithTeamEvent(SeasonYear Year, MlbId PlayerMlbId, MlbId TeamMlbId)
    : IDomainEvent;