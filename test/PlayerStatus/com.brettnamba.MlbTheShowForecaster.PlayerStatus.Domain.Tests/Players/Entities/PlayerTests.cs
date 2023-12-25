using com.brettnamba.MlbTheShowForecaster.PlayerStatus.Domain.Common.ValueObjects;
using com.brettnamba.MlbTheShowForecaster.PlayerStatus.Domain.Players.Entities;
using com.brettnamba.MlbTheShowForecaster.PlayerStatus.Domain.Players.Enums;
using com.brettnamba.MlbTheShowForecaster.PlayerStatus.Domain.Players.ValueObjects;
using com.brettnamba.MlbTheShowForecaster.PlayerStatus.Domain.Teams.ValueObjects;

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
        var team = Team.Create(MlbId.Create(2), TeamName.Create("Mariners"), TeamAbbreviation.Create("SEA"));
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
        Assert.Equal(2, actual.Team?.MlbId.Value);
        Assert.True(actual.Active);
    }

    [Fact]
    public void SignContractWithTeam_NoPreviousTeam_SignsWithNewTeam()
    {
        // Arrange
        Team? currentTeam = null;
        var newTeam = Team.Create(MlbId.Create(2), TeamName.Create("Seattle Mariners"), TeamAbbreviation.Create("SEA"));
        var player = CreatePlayer(team: currentTeam);

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
        var currentTeam = Team.Create(MlbId.Create(2), TeamName.Create("Seattle Mariners"),
            TeamAbbreviation.Create("SEA"));
        var newTeam = Team.Create(MlbId.Create(3), TeamName.Create("New York Yankees"),
            TeamAbbreviation.Create("NYY"));
        var player = CreatePlayer(team: currentTeam);

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
        var currentTeam = Team.Create(MlbId.Create(2), TeamName.Create("Seattle Mariners"),
            TeamAbbreviation.Create("SEA"));
        var player = CreatePlayer(team: currentTeam);

        // Act
        player.EnterFreeAgency();

        // Assert
        Assert.Null(player.Team);
        Assert.NotEqual(currentTeam, player.Team);
    }

    private Player CreatePlayer(MlbId? mlbId = null, PersonName? firstName = null, PersonName? lastName = null,
        DateTime birthdate = default, Position position = Position.Catcher, DateTime mlbDebutDate = default,
        BatSide batSide = BatSide.Left, ThrowArm throwArm = ThrowArm.Left, Team? team = null, bool active = false)
    {
        return Player.Create(
            mlbId == null ? MlbId.Create(1) : mlbId,
            firstName == null ? PersonName.Create("First") : firstName,
            lastName == null ? PersonName.Create("Last") : lastName,
            birthdate == default ? new DateTime(1990, 1, 1) : birthdate,
            position,
            mlbDebutDate == default ? new DateTime(2010, 1, 1) : mlbDebutDate,
            batSide,
            throwArm,
            team,
            active
        );
    }
}