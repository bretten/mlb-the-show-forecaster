using com.brettnamba.MlbTheShowForecaster.PlayerStatus.Domain.Players.Entities;

namespace com.brettnamba.MlbTheShowForecaster.PlayerStatus.Domain.Players.Enums;

/// <summary>
/// Enum representing the different status changes that may happen for a <see cref="Player"/>
/// </summary>
public enum PlayerStatusChangeType
{
    /// <summary>
    /// When a player signs a contract with a team
    /// </summary>
    SignedContractWithTeam,

    /// <summary>
    /// When a player enters free agency and is no longer on a team
    /// </summary>
    EnteredFreeAgency,

    /// <summary>
    /// When a player is activated in the MLB
    /// </summary>
    Activated,

    /// <summary>
    /// When a player is inactivated in the MLB
    /// </summary>
    Inactivated
}