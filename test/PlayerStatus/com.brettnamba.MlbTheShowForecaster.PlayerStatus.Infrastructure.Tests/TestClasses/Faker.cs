using com.brettnamba.MlbTheShowForecaster.PlayerStatus.Domain.Common.ValueObjects;
using com.brettnamba.MlbTheShowForecaster.PlayerStatus.Domain.Players.Entities;
using com.brettnamba.MlbTheShowForecaster.PlayerStatus.Domain.Players.Enums;
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