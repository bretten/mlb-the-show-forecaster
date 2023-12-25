using com.brettnamba.MlbTheShowForecaster.PlayerStatus.Domain.Players.Entities;
using com.brettnamba.MlbTheShowForecaster.PlayerStatus.Domain.Players.Enums;
using com.brettnamba.MlbTheShowForecaster.PlayerStatus.Domain.Players.ValueObjects;
using com.brettnamba.MlbTheShowForecaster.PlayerStatus.Domain.Teams.ValueObjects;

namespace com.brettnamba.MlbTheShowForecaster.PlayerStatus.Domain.Players.Services;

/// <summary>
/// Detects <see cref="Player"/> status changes by comparing the <see cref="Player"/> to the current MLB reported states
/// </summary>
public sealed class PlayerStatusChangeDetector : IPlayerStatusChangeDetector
{
    /// <summary>
    /// Detects status changes for the <see cref="Player"/> based on the specified parameters
    /// </summary>
    /// <param name="player">The <see cref="Player"/></param>
    /// <param name="mlbReportedActiveStatus">True if MLB reports the <see cref="Player"/> as active</param>
    /// <param name="mlbReportedTeam">The <see cref="Team"/> that the MLB reports the <see cref="Player"/> is currently on</param>
    /// <returns>Any status changes</returns>
    public PlayerStatusChanges DetectChanges(Player player, bool mlbReportedActiveStatus,
        Team? mlbReportedTeam)
    {
        var changes = new List<PlayerStatusChangeType>();

        CheckForActiveStatusChanges(player, mlbReportedActiveStatus, ref changes);

        CheckForTeamChanges(player, mlbReportedTeam, ref changes);

        return new PlayerStatusChanges(changes, mlbReportedTeam);
    }

    /// <summary>
    /// Checks if the <see cref="Player"/>'s active status has changed and if so, adds any status changes
    /// to the collection of changes
    /// </summary>
    /// <param name="player">The <see cref="Player"/></param>
    /// <param name="mlbReportedActiveStatus">True if MLB reports the <see cref="Player"/> as active</param>
    /// <param name="changes">A collection of changes to add to</param>
    private static void CheckForActiveStatusChanges(Player player, bool mlbReportedActiveStatus,
        ref List<PlayerStatusChangeType> changes)
    {
        switch (player.Active)
        {
            case false when mlbReportedActiveStatus:
                changes.Add(PlayerStatusChangeType.Activated);
                break;
            case true when !mlbReportedActiveStatus:
                changes.Add(PlayerStatusChangeType.Inactivated);
                break;
        }
    }

    /// <summary>
    /// Checks if the <see cref="Player"/>'s <see cref="Team"/> has changed. If so, any status changes are recorded
    /// in the collection of changes
    /// </summary>
    /// <param name="player">The <see cref="Player"/></param>
    /// <param name="mlbReportedTeam">The <see cref="Team"/> that the MLB reports the <see cref="Player"/> is currently on</param>
    /// <param name="changes">A collection of changes to add to</param>
    private static void CheckForTeamChanges(Player player, Team? mlbReportedTeam,
        ref List<PlayerStatusChangeType> changes)
    {
        if (!player.IsFreeAgent() && mlbReportedTeam == null)
        {
            changes.Add(PlayerStatusChangeType.EnteredFreeAgency);
        }
        else if (player.IsFreeAgent() && mlbReportedTeam != null)
        {
            changes.Add(PlayerStatusChangeType.SignedWithNewTeam);
        }
        else if (player.Team != mlbReportedTeam)
        {
            changes.Add(PlayerStatusChangeType.Traded);
        }
    }
}