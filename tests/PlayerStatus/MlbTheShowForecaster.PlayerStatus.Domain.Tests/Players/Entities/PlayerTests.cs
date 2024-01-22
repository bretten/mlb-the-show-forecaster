﻿using com.brettnamba.MlbTheShowForecaster.Common.Domain.ValueObjects;
using com.brettnamba.MlbTheShowForecaster.PlayerStatus.Domain.Players.Entities;
using com.brettnamba.MlbTheShowForecaster.PlayerStatus.Domain.Players.Enums;
using com.brettnamba.MlbTheShowForecaster.PlayerStatus.Domain.Players.Events;
using com.brettnamba.MlbTheShowForecaster.PlayerStatus.Domain.Players.ValueObjects;
using com.brettnamba.MlbTheShowForecaster.PlayerStatus.Domain.Teams.ValueObjects;
using com.brettnamba.MlbTheShowForecaster.PlayerStatus.Domain.Tests.Players.TestClasses;
using com.brettnamba.MlbTheShowForecaster.PlayerStatus.Domain.Tests.Teams.TestClasses;

namespace com.brettnamba.MlbTheShowForecaster.PlayerStatus.Domain.Tests.Players.Entities;

public class PlayerTests
{
    [Fact]
    public void Create_ValidValues_Created()
    {
        // Arrange
        var mlbId = MlbId.Create(1);
        var firstName = PersonName.Create("First");
        var lastName = PersonName.Create("Last");
        var birthdate = new DateTime(1990, 1, 1);
        var position = Position.CenterField;
        var mlbDebutDate = new DateTime(2010, 1, 1);
        var batSide = BatSide.Left;
        var throwArm = ThrowArm.Right;
        var team = TeamFaker.Fake();
        var active = true;

        // Act
        var actual = Player.Create(mlbId, firstName, lastName, birthdate, position, mlbDebutDate, batSide, throwArm,
            team, active);

        // Assert
        Assert.Equal(1, actual.MlbId.Value);
        Assert.Equal("First", actual.FirstName.Value);
        Assert.Equal("Last", actual.LastName.Value);
        Assert.Equal(new DateTime(1990, 1, 1), actual.Birthdate);
        Assert.Equal(Position.CenterField, actual.Position);
        Assert.Equal(new DateTime(2010, 1, 1), actual.MlbDebutDate);
        Assert.Equal(BatSide.Left, actual.BatSide);
        Assert.Equal(ThrowArm.Right, actual.ThrowArm);
        Assert.Equal(TeamFaker.DefaultMlbId, actual.Team?.MlbId.Value);
        Assert.Equal(TeamFaker.DefaultTeamName, actual.Team?.Name.Value);
        Assert.Equal(TeamFaker.DefaultTeamAbbreviation, actual.Team?.Abbreviation.Value);
        Assert.True(actual.Active);
    }

    [Fact]
    public void SignContractWithTeam_NoPreviousTeam_SignsWithNewTeam()
    {
        // Arrange
        Team? currentTeam = null;
        var newTeam = TeamFaker.Fake();
        var player = PlayerFaker.Fake(team: currentTeam);

        // Act
        player.SignContractWithTeam(newTeam);

        // Assert
        Assert.NotNull(player.Team);
        Assert.Equal(newTeam, player.Team);
        Assert.Equal(1, player.DomainEvents.Count);
        Assert.Equal(new PlayerSignedContractWithTeamEvent(player.MlbId, newTeam.MlbId), player.DomainEvents[0]);
    }

    [Fact]
    public void SignContractWithTeam_PlayerOnAnotherTeam_LeavesCurrentTeamAndSignsWithNewTeam()
    {
        // Arrange
        var currentTeam = TeamFaker.Fake();
        var newTeam = TeamFaker.Fake(1000, "New Team", "NEW");
        var player = PlayerFaker.Fake(team: currentTeam);

        // Act
        player.SignContractWithTeam(newTeam);

        // Assert
        Assert.NotNull(player.Team);
        Assert.Equal(newTeam, player.Team);
        Assert.Equal(1, player.DomainEvents.Count);
        Assert.Equal(new PlayerSignedContractWithTeamEvent(player.MlbId, newTeam.MlbId), player.DomainEvents[0]);
    }

    [Fact]
    public void EnterFreeAgency_PlayerOnATeam_LeavesTeam()
    {
        // Arrange
        var currentTeam = TeamFaker.Fake();
        var player = PlayerFaker.Fake(team: currentTeam);

        // Act
        player.EnterFreeAgency();

        // Assert
        Assert.Null(player.Team);
        Assert.NotEqual(currentTeam, player.Team);
        Assert.Equal(1, player.DomainEvents.Count);
        Assert.Equal(new PlayerEnteredFreeAgencyEvent(player.MlbId), player.DomainEvents[0]);
    }

    [Fact]
    public void Activate_InactivePlayer_NowActive()
    {
        // Arrange
        var player = PlayerFaker.Fake(active: false);

        // Act
        player.Activate();

        // Assert
        Assert.True(player.Active);
        Assert.Equal(1, player.DomainEvents.Count);
        Assert.Equal(new PlayerActivatedEvent(player.MlbId), player.DomainEvents[0]);
    }

    [Fact]
    public void Inactivate_ActivePlayer_NowInactive()
    {
        // Arrange
        var player = PlayerFaker.Fake(active: true);

        // Act
        player.Inactivate();

        // Assert
        Assert.False(player.Active);
        Assert.Equal(1, player.DomainEvents.Count);
        Assert.Equal(new PlayerInactivatedEvent(player.MlbId), player.DomainEvents[0]);
    }
}