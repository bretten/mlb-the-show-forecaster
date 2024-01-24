using com.brettnamba.MlbTheShowForecaster.Common.Domain.Enums;
using com.brettnamba.MlbTheShowForecaster.Common.Domain.ValueObjects;
using com.brettnamba.MlbTheShowForecaster.ExternalApis.MlbApi.Dtos;
using com.brettnamba.MlbTheShowForecaster.PlayerStatus.Application.Dtos;
using com.brettnamba.MlbTheShowForecaster.PlayerStatus.Domain.Players.Entities;
using com.brettnamba.MlbTheShowForecaster.PlayerStatus.Domain.Players.ValueObjects;
using com.brettnamba.MlbTheShowForecaster.PlayerStatus.Domain.Teams.ValueObjects;

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

    public static PlayerDto FakePlayerDto(int? mlbId = null, string? firstName = null, string? lastName = null,
        DateTime birthdate = default, PositionDto? position = null, DateTime mlbDebutDate = default,
        ArmSideDto? batSide = null, ArmSideDto? throwArm = null, CurrentTeamDto? team = null, bool active = false)
    {
        return new PlayerDto()
        {
            Id = mlbId ?? DefaultMlbId,
            FirstName = firstName ?? DefaultFirstName,
            LastName = lastName ?? DefaultLastName,
            Birthdate = birthdate == default ? new DateTime(1990, 1, 1) : birthdate,
            Position = position ?? new PositionDto("Catcher", "C"),
            MlbDebutDate = mlbDebutDate == default ? new DateTime(2010, 1, 1) : mlbDebutDate,
            BatSide = batSide ?? new ArmSideDto("L", "Left"),
            ThrowArm = throwArm ?? new ArmSideDto("L", "Left"),
            CurrentTeam = team ?? new CurrentTeamDto(DefaultTeamMlbId),
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

    public static Player FakePlayer(int? mlbId = null, Team? team = null, bool active = false)
    {
        return Player.Create(
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