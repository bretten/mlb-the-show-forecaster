using com.brettnamba.MlbTheShowForecaster.PlayerStatus.Domain.Players.Enums;
using com.brettnamba.MlbTheShowForecaster.PlayerStatus.Domain.Players.Services;
using Faker = com.brettnamba.MlbTheShowForecaster.PlayerStatus.Domain.Tests.Teams.TestClasses.Faker;

namespace com.brettnamba.MlbTheShowForecaster.PlayerStatus.Domain.Tests.Players.Services;

public class PlayerStatusChangeDetectorTests
{
    [Fact]
    public void DetectChanges_InactivePlayerReportedAsActive_DetectsActive()
    {
        // Arrange
        var detector = new PlayerStatusChangeDetector();
        var player = TestClasses.Faker.FakePlayer(active: false);
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
        var player = TestClasses.Faker.FakePlayer(active: true);
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
        var currentTeam = Faker.FakeTeam();
        var player = TestClasses.Faker.FakePlayer(team: currentTeam);
        var mlbReportedActiveStatus = true;
        var mlbReportedTeam = Faker.NoTeam;

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
        var currentTeam = Faker.NoTeam;
        var player = TestClasses.Faker.FakePlayer(team: currentTeam);
        var mlbReportedActiveStatus = true;
        var mlbReportedTeam = Faker.FakeTeam();

        // Act
        var actual = detector.DetectChanges(player, mlbReportedActiveStatus, mlbReportedTeam);

        // Assert
        Assert.Contains(PlayerStatusChangeType.SignedContractWithTeam, actual.Changes);
    }
}