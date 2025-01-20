using com.brettnamba.MlbTheShowForecaster.Common.Domain.ValueObjects;
using com.brettnamba.MlbTheShowForecaster.PlayerStatus.Domain.Teams.ValueObjects;

namespace com.brettnamba.MlbTheShowForecaster.PlayerStatus.Domain.Tests.Teams.TestClasses;

/// <summary>
/// Fakes a Team
/// </summary>
public static class Faker
{
    public const int DefaultMlbId = 136;
    public const string DefaultTeamName = "Seattle Mariners";
    public const string DefaultTeamAbbreviation = "SEA";

    public static Team? NoTeam => null;

    public static Team FakeTeam(int? mlbId = null, string? teamName = null, string? abbreviation = null)
    {
        return Team.Create(
            mlbId == null ? MlbId.Create(DefaultMlbId) : MlbId.Create(mlbId.Value),
            teamName == null ? TeamName.Create(DefaultTeamName) : TeamName.Create(teamName),
            abbreviation == null
                ? TeamAbbreviation.Create(DefaultTeamAbbreviation)
                : TeamAbbreviation.Create(abbreviation)
        );
    }
}