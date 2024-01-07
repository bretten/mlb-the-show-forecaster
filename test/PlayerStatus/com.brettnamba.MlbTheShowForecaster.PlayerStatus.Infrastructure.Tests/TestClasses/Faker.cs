using com.brettnamba.MlbTheShowForecaster.ExternalApis.MlbApi.Dtos;
using com.brettnamba.MlbTheShowForecaster.PlayerStatus.Application.Dtos;
using com.brettnamba.MlbTheShowForecaster.PlayerStatus.Domain.Common.ValueObjects;
using com.brettnamba.MlbTheShowForecaster.PlayerStatus.Domain.Players.Enums;
using com.brettnamba.MlbTheShowForecaster.PlayerStatus.Domain.Players.ValueObjects;
using com.brettnamba.MlbTheShowForecaster.PlayerStatus.Domain.Teams.ValueObjects;
using Position = com.brettnamba.MlbTheShowForecaster.PlayerStatus.Domain.Players.Enums.Position;

namespace com.brettnamba.MlbTheShowForecaster.PlayerStatus.Infrastructure.Tests.TestClasses;

/// <summary>
/// Fakes players and teams
/// </summary>
public static class Faker
{
    public const int DefaultMlbId = 1;
    public const int DefaultTeamMlbId = 136;
    public const string DefaultTeamName = "Seattle Mariners";
    public const string DefaultTeamAbbreviation = "SEA";
    public const string DefaultFirstName = "First";
    public const string DefaultLastName = "Last";

    public static Player FakePlayerDto(int? mlbId = null, CurrentTeam? team = null,
        bool active = false)
    {
        return new Player()
        {
            Id = mlbId ?? DefaultMlbId,
            FirstName = DefaultFirstName,
            LastName = DefaultLastName,
            Birthdate = new DateTime(1990, 1, 1),
            Position = new ExternalApis.MlbApi.Dtos.Position("Catcher", "C"),
            MlbDebutDate = new DateTime(2010, 1, 1),
            BatSide = new ArmSide("L", "Left"),
            ThrowArm = new ArmSide("L", "Left"),
            CurrentTeam = team ?? new CurrentTeam(DefaultTeamMlbId),
            Active = active
        };
    }

    public static RosterEntry FakeRosterEntry(int? mlbId = null, string? firstName = null, string? lastName = null,
        DateTime birthdate = default, Position position = Position.Catcher, DateTime mlbDebutDate = default,
        BatSide batSide = BatSide.Left, ThrowArm throwArm = ThrowArm.Left, int? teamMlbId = null, bool active = false)
    {
        return new RosterEntry(
            mlbId == null ? MlbId.Create(DefaultMlbId) : MlbId.Create(mlbId.Value),
            firstName == null ? PersonName.Create(DefaultFirstName) : PersonName.Create(firstName),
            lastName == null ? PersonName.Create(DefaultLastName) : PersonName.Create(lastName),
            birthdate == default ? new DateTime(1990, 1, 1) : birthdate,
            position,
            mlbDebutDate == default ? new DateTime(2010, 1, 1) : mlbDebutDate,
            batSide,
            throwArm,
            teamMlbId == null ? MlbId.Create(DefaultTeamMlbId) : MlbId.Create(teamMlbId.Value),
            active
        );
    }

    public static Domain.Players.Entities.Player FakePlayer(int? mlbId = null, Team? team = null, bool active = false)
    {
        return Domain.Players.Entities.Player.Create(
            mlbId == null ? MlbId.Create(DefaultMlbId) : MlbId.Create(mlbId.Value),
            PersonName.Create(DefaultFirstName),
            PersonName.Create(DefaultLastName),
            new DateTime(1990, 1, 1),
            Position.Catcher,
            new DateTime(2010, 1, 1),
            BatSide.Left,
            ThrowArm.Left,
            team,
            active
        );
    }

    public static Team? NoTeam => null;

    public static Team FakeTeam(int? mlbId = DefaultMlbId, string? teamName = DefaultTeamName,
        string? abbreviation = DefaultTeamAbbreviation)
    {
        return Team.Create(
            mlbId == null ? MlbId.Create(DefaultTeamMlbId) : MlbId.Create(mlbId.Value),
            teamName == null ? TeamName.Create(DefaultTeamName) : TeamName.Create(teamName),
            abbreviation == null
                ? TeamAbbreviation.Create(DefaultTeamAbbreviation)
                : TeamAbbreviation.Create(abbreviation)
        );
    }
}