using com.brettnamba.MlbTheShowForecaster.Common.Domain.Enums;
using com.brettnamba.MlbTheShowForecaster.Common.Domain.ValueObjects;
using com.brettnamba.MlbTheShowForecaster.PlayerStatus.Domain.Players.Entities;
using com.brettnamba.MlbTheShowForecaster.PlayerStatus.Domain.Players.Events;
using com.brettnamba.MlbTheShowForecaster.PlayerStatus.Domain.Players.ValueObjects;
using com.brettnamba.MlbTheShowForecaster.PlayerStatus.Domain.Teams.ValueObjects;
using Faker = com.brettnamba.MlbTheShowForecaster.PlayerStatus.Domain.Tests.Teams.TestClasses.Faker;

namespace com.brettnamba.MlbTheShowForecaster.PlayerStatus.Domain.Tests.Players.Entities;

public class PlayerTests
{
    private static readonly DateOnly Date = new DateOnly(2024, 10, 28);

    [Fact]
    public void Create_ValidValues_Created()
    {
        // Arrange
        var mlbId = MlbId.Create(1);
        var firstName = PersonName.Create("First");
        var lastName = PersonName.Create("Last");
        var birthdate = new DateOnly(1990, 1, 1);
        var position = Position.CenterField;
        var mlbDebutDate = new DateOnly(2010, 1, 1);
        var batSide = BatSide.Left;
        var throwArm = ThrowArm.Right;
        var team = Faker.FakeTeam();
        var active = true;

        // Act
        var actual = Player.Create(mlbId, firstName, lastName, birthdate, position, mlbDebutDate, batSide, throwArm,
            team, active);

        // Assert
        Assert.Equal(1, actual.MlbId.Value);
        Assert.Equal("First", actual.FirstName.Value);
        Assert.Equal("Last", actual.LastName.Value);
        Assert.Equal(new DateOnly(1990, 1, 1), actual.Birthdate);
        Assert.Equal(Position.CenterField, actual.Position);
        Assert.Equal(new DateOnly(2010, 1, 1), actual.MlbDebutDate);
        Assert.Equal(BatSide.Left, actual.BatSide);
        Assert.Equal(ThrowArm.Right, actual.ThrowArm);
        Assert.Equal(Faker.DefaultMlbId, actual.Team?.MlbId.Value);
        Assert.Equal(Faker.DefaultTeamName, actual.Team?.Name.Value);
        Assert.Equal(Faker.DefaultTeamAbbreviation, actual.Team?.Abbreviation.Value);
        Assert.True(actual.Active);
    }

    [Fact]
    public void SignContractWithTeam_NoPreviousTeam_SignsWithNewTeam()
    {
        // Arrange
        var year = SeasonYear.Create(2024);
        Team? currentTeam = null;
        var newTeam = Faker.FakeTeam();
        var player = TestClasses.Faker.FakePlayer(team: currentTeam);

        // Act
        player.SignContractWithTeam(year, newTeam, Date);

        // Assert
        Assert.NotNull(player.Team);
        Assert.Equal(newTeam, player.Team);
        Assert.Equal(1, player.DomainEvents.Count);
        Assert.Equal(new PlayerSignedContractWithTeamEvent(year, player.MlbId, newTeam.MlbId, Date),
            player.DomainEvents[0]);
    }

    [Fact]
    public void SignContractWithTeam_PlayerOnAnotherTeam_LeavesCurrentTeamAndSignsWithNewTeam()
    {
        // Arrange
        var year = SeasonYear.Create(2024);
        var currentTeam = Faker.FakeTeam();
        var newTeam = Faker.FakeTeam(1000, "New Team", "NEW");
        var player = TestClasses.Faker.FakePlayer(team: currentTeam);

        // Act
        player.SignContractWithTeam(year, newTeam, Date);

        // Assert
        Assert.NotNull(player.Team);
        Assert.Equal(newTeam, player.Team);
        Assert.Equal(1, player.DomainEvents.Count);
        Assert.Equal(new PlayerSignedContractWithTeamEvent(year, player.MlbId, newTeam.MlbId, Date),
            player.DomainEvents[0]);
    }

    [Fact]
    public void EnterFreeAgency_PlayerOnATeam_LeavesTeam()
    {
        // Arrange
        var year = SeasonYear.Create(2024);
        var currentTeam = Faker.FakeTeam();
        var player = TestClasses.Faker.FakePlayer(team: currentTeam);

        // Act
        player.EnterFreeAgency(year, Date);

        // Assert
        Assert.Null(player.Team);
        Assert.NotEqual(currentTeam, player.Team);
        Assert.Equal(1, player.DomainEvents.Count);
        Assert.Equal(new PlayerEnteredFreeAgencyEvent(year, player.MlbId, Date), player.DomainEvents[0]);
    }

    [Fact]
    public void Activate_InactivePlayer_NowActive()
    {
        // Arrange
        var year = SeasonYear.Create(2024);
        var player = TestClasses.Faker.FakePlayer(active: false);

        // Act
        player.Activate(year, Date);

        // Assert
        Assert.True(player.Active);
        Assert.Equal(1, player.DomainEvents.Count);
        Assert.Equal(new PlayerActivatedEvent(year, player.MlbId, Date), player.DomainEvents[0]);
    }

    [Fact]
    public void Inactivate_ActivePlayer_NowInactive()
    {
        // Arrange
        var year = SeasonYear.Create(2024);
        var player = TestClasses.Faker.FakePlayer(active: true);

        // Act
        player.Inactivate(year, Date);

        // Assert
        Assert.False(player.Active);
        Assert.Equal(1, player.DomainEvents.Count);
        Assert.Equal(new PlayerInactivatedEvent(year, player.MlbId, Date), player.DomainEvents[0]);
    }
}