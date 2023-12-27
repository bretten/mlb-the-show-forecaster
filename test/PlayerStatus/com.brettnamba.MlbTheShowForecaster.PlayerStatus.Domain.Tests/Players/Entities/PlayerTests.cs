using com.brettnamba.MlbTheShowForecaster.PlayerStatus.Domain.Common.ValueObjects;
using com.brettnamba.MlbTheShowForecaster.PlayerStatus.Domain.Players.Entities;
using com.brettnamba.MlbTheShowForecaster.PlayerStatus.Domain.Players.Enums;
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
        var team = TeamMocker.Mock();
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
        Assert.Equal(TeamMocker.DefaultMlbId, actual.Team?.MlbId.Value);
        Assert.Equal(TeamMocker.DefaultTeamName, actual.Team?.Name.Value);
        Assert.Equal(TeamMocker.DefaultTeamAbbreviation, actual.Team?.Abbreviation.Value);
        Assert.True(actual.Active);
    }

    [Fact]
    public void SignContractWithTeam_NoPreviousTeam_SignsWithNewTeam()
    {
        // Arrange
        Team? currentTeam = null;
        var newTeam = TeamMocker.Mock();
        var player = PlayerMocker.Mock(team: currentTeam);

        // Act
        player.SignContractWithTeam(newTeam);

        // Assert
        Assert.NotNull(player.Team);
        Assert.Equal(newTeam, player.Team);
    }

    [Fact]
    public void SignContractWithTeam_PlayerOnAnotherTeam_LeavesCurrentTeamAndSignsWithNewTeam()
    {
        // Arrange
        var currentTeam = TeamMocker.Mock();
        var newTeam = TeamMocker.Mock(1000, "New Team", "NEW");
        var player = PlayerMocker.Mock(team: currentTeam);

        // Act
        player.SignContractWithTeam(newTeam);

        // Assert
        Assert.NotNull(player.Team);
        Assert.Equal(newTeam, player.Team);
    }

    [Fact]
    public void EnterFreeAgency_PlayerOnATeam_LeavesTeam()
    {
        // Arrange
        var currentTeam = TeamMocker.Mock();
        var player = PlayerMocker.Mock(team: currentTeam);

        // Act
        player.EnterFreeAgency();

        // Assert
        Assert.Null(player.Team);
        Assert.NotEqual(currentTeam, player.Team);
    }

    [Fact]
    public void Activate_InactivePlayer_NowActive()
    {
        // Arrange
        var player = PlayerMocker.Mock(active: false);

        // Act
        player.Activate();

        // Assert
        Assert.True(player.Active);
    }

    [Fact]
    public void Inactivate_ActivePlayer_NowInactive()
    {
        // Arrange
        var player = PlayerMocker.Mock(active: true);

        // Act
        player.Inactivate();

        // Assert
        Assert.False(player.Active);
    }
}