using com.brettnamba.MlbTheShowForecaster.PlayerStatus.Domain.Players.Entities;
using com.brettnamba.MlbTheShowForecaster.PlayerStatus.Domain.Players.Enums;
using com.brettnamba.MlbTheShowForecaster.PlayerStatus.Domain.Teams.ValueObjects;

namespace com.brettnamba.MlbTheShowForecaster.PlayerStatus.Domain.Players.ValueObjects;

/// <summary>
/// Represents status changes for a <see cref="Player"/>, who may have more than one change
/// at any given time
/// </summary>
/// <param name="Changes">All the status changes for the <see cref="Player"/></param>
/// <param name="NewTeam">The <see cref="Team"/> if the status change involves a new <see cref="Team"/></param>
public readonly record struct PlayerStatusChanges(List<PlayerStatusChangeType> Changes, Team? NewTeam)
{
    /// <summary>
    /// True if the player signed a contract with a team, otherwise false
    /// </summary>
    public bool SignedContractWithTeam => Changes.Contains(PlayerStatusChangeType.SignedContractWithTeam);

    /// <summary>
    /// True if the player entered free agency, otherwise false
    /// </summary>
    public bool EnteredFreeAgency => Changes.Contains(PlayerStatusChangeType.EnteredFreeAgency);

    /// <summary>
    /// True if the player was activated, otherwise false
    /// </summary>
    public bool Activated => Changes.Contains(PlayerStatusChangeType.Activated);

    /// <summary>
    /// True if the player was inactivated
    /// </summary>
    public bool Inactivated => Changes.Contains(PlayerStatusChangeType.Inactivated);

    /// <summary>
    /// Checks if there are any changes
    /// </summary>
    /// <returns>True if there are changes, otherwise false</returns>
    public bool Any() => Changes != null && Changes.Any();
};