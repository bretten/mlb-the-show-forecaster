using com.brettnamba.MlbTheShowForecaster.PlayerStatus.Domain.Players.Enums;
using com.brettnamba.MlbTheShowForecaster.PlayerStatus.Domain.Players.Services;
using com.brettnamba.MlbTheShowForecaster.PlayerStatus.Domain.Tests.Players.TestClasses;
using com.brettnamba.MlbTheShowForecaster.PlayerStatus.Domain.Tests.Teams.TestClasses;

namespace com.brettnamba.MlbTheShowForecaster.PlayerStatus.Domain.Tests.Players.Services;

public class PlayerStatusChangeDetectorTests
{
    [Fact]
    public void DetectChanges_InactivePlayerReportedAsActive_DetectsActive()
    {
        // Arrange
        var detector = new PlayerStatusChangeDetector();
        var player = PlayerMocker.Mock(active: false);
        var mlbReportedActiveStatus = true;

        // Act
        var actual = detector.DetectChanges(player, mlbReportedActiveStatus, null);

        // Assert
        Assert.Contains(PlayerStatusChangeType.Activated, actual.Changes);
    }

    [Fact]
    public void DetectChanges_ActivePlayerReportedAsInactive_DetectsInactive()
    {
        // Arrange
        var detector = new PlayerStatusChangeDetector();
        var player = PlayerMocker.Mock(active: true);
        var mlbReportedActiveStatus = false;

        // Act
        var actual = detector.DetectChanges(player, mlbReportedActiveStatus, null);

        // Assert
        Assert.Contains(PlayerStatusChangeType.Inactivated, actual.Changes);
    }

    [Fact]
    public void DetectChanges_PlayerOnTeamReportedAsFreeAgent_DetectsFreeAgency()
    {
        // Arrange
        var detector = new PlayerStatusChangeDetector();
        var currentTeam = TeamMocker.Mock();
        var player = PlayerMocker.Mock(team: currentTeam);
        var mlbReportedActiveStatus = true;
        var mlbReportedTeam = TeamMocker.NoTeam;

        // Act
        var actual = detector.DetectChanges(player, mlbReportedActiveStatus, mlbReportedTeam);

        // Assert
        Assert.Contains(PlayerStatusChangeType.EnteredFreeAgency, actual.Changes);
    }

    [Fact]
    public void DetectChanges_FreeAgentPlayerReportedOnANewTeam_DetectsNewTeamSigning()
    {
        // Arrange
        var detector = new PlayerStatusChangeDetector();
        var currentTeam = TeamMocker.NoTeam;
        var player = PlayerMocker.Mock(team: currentTeam);
        var mlbReportedActiveStatus = true;
        var mlbReportedTeam = TeamMocker.Mock();

        // Act
        var actual = detector.DetectChanges(player, mlbReportedActiveStatus, mlbReportedTeam);

        // Assert
        Assert.Contains(PlayerStatusChangeType.SignedContractWithTeam, actual.Changes);
    }
}