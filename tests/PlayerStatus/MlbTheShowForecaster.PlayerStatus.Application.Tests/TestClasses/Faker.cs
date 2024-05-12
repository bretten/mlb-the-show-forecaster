using com.brettnamba.MlbTheShowForecaster.Common.Domain.Enums;
using com.brettnamba.MlbTheShowForecaster.Common.Domain.ValueObjects;
using com.brettnamba.MlbTheShowForecaster.PlayerStatus.Application.Dtos;
using com.brettnamba.MlbTheShowForecaster.PlayerStatus.Domain.Players.Entities;
using com.brettnamba.MlbTheShowForecaster.PlayerStatus.Domain.Players.ValueObjects;
using com.brettnamba.MlbTheShowForecaster.PlayerStatus.Domain.Teams.ValueObjects;

namespace com.brettnamba.MlbTheShowForecaster.PlayerStatus.Application.Tests.TestClasses;

/// <summary>
/// Fakes a roster entry
/// </summary>
public static class Faker
{
    public const int DefaultMlbId = 1;
    public const int DefaultTeamMlbId = 136;
    public const string DefaultTeamName = "Seattle Mariners";
    public const string DefaultTeamAbbreviation = "SEA";
    public const string DefaultFirstName = "First";
    public const string DefaultLastName = "Last";

    public static RosterEntry FakeRosterEntry(int? mlbId = null, string? firstName = null, string? lastName = null,
        DateOnly birthdate = default, Position position = Position.Catcher, DateOnly mlbDebutDate = default,
        BatSide batSide = BatSide.Left, ThrowArm throwArm = ThrowArm.Left, int? teamMlbId = null, bool active = false)
    {
        return new RosterEntry(
            mlbId == null ? MlbId.Create(DefaultMlbId) : MlbId.Create(mlbId.Value),
            firstName == null ? PersonName.Create(DefaultFirstName) : PersonName.Create(firstName),
            lastName == null ? PersonName.Create(DefaultLastName) : PersonName.Create(lastName),
            birthdate == default ? new DateOnly(1990, 1, 1) : birthdate,
            position,
            mlbDebutDate == default ? new DateOnly(2010, 1, 1) : mlbDebutDate,
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
            new DateOnly(1990, 1, 1),
            Position.Catcher,
            new DateOnly(2010, 1, 1),
            BatSide.Left,
            ThrowArm.Left,
            team,
            active
        );
    }

    public static Team? NoTeam => null;

    public static Team FakeTeam(int? mlbId = null, string? teamName = null, string? abbreviation = null)
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