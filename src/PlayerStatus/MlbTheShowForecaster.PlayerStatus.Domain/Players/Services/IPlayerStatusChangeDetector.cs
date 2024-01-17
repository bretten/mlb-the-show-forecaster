using com.brettnamba.MlbTheShowForecaster.PlayerStatus.Domain.Players.Entities;
using com.brettnamba.MlbTheShowForecaster.PlayerStatus.Domain.Players.ValueObjects;
using com.brettnamba.MlbTheShowForecaster.PlayerStatus.Domain.Teams.ValueObjects;

namespace com.brettnamba.MlbTheShowForecaster.PlayerStatus.Domain.Players.Services;

/// <summary>
/// Defines a service that will detect a <see cref="Player"/>'s status changes by comparing the <see cref="Player"/>
/// to the current MLB reported states
/// </summary>
public interface IPlayerStatusChangeDetector
{
    /// <summary>
    /// Detects a <see cref="Player"/>'s status changes
    /// </summary>
    /// <param name="player">The <see cref="Player"/></param>
    /// <param name="mlbReportedActiveStatus">True if the MLB reports the <see cref="Player"/> as active, otherwise false</param>
    /// <param name="mlbReportedTeam">The <see cref="Team"/> that MLB reports as the <see cref="Player"/>'s current one</param>
    /// <returns>Any status changes</returns>
    PlayerStatusChanges DetectChanges(Player player, bool mlbReportedActiveStatus, Team? mlbReportedTeam);
}