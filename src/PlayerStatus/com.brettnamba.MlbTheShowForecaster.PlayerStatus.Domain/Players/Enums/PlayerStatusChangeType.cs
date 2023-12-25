using com.brettnamba.MlbTheShowForecaster.PlayerStatus.Domain.Players.Entities;

namespace com.brettnamba.MlbTheShowForecaster.PlayerStatus.Domain.Players.Enums;

/// <summary>
/// Enum representing the different status changes that may happen for a <see cref="Player"/>
/// </summary>
public enum PlayerStatusChangeType
{
    EnteredFreeAgency,
    SignedWithNewTeam,
    Traded,
    Activated,
    Inactivated
}