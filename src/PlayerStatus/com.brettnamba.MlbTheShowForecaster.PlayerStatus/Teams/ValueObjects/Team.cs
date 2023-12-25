using com.brettnamba.MlbTheShowForecaster.Common.Extensions;
using com.brettnamba.MlbTheShowForecaster.Core.SeedWork;
using com.brettnamba.MlbTheShowForecaster.PlayerStatus.Common.ValueObjects;
using com.brettnamba.MlbTheShowForecaster.PlayerStatus.Players.Entities;
using com.brettnamba.MlbTheShowForecaster.PlayerStatus.Teams.Enums;

namespace com.brettnamba.MlbTheShowForecaster.PlayerStatus.Teams.ValueObjects;

/// <summary>
/// Represents a MLB team
///
/// <para>
/// Currently, we will not consider a Team an entity as the domain is only focusing on
/// the perspective of the <see cref="Player"/>
/// </para>
/// </summary>
public sealed class Team : ValueObject
{
    /// <summary>
    /// The ID of the Team from MLB
    /// </summary>
    public MlbId MlbId { get; }

    /// <summary>
    /// The name of the Team
    /// </summary>
    public TeamName Name { get; }

    /// <summary>
    /// The Team's abbreviation
    /// </summary>
    public TeamAbbreviation Abbreviation { get; }

    /// <summary>
    /// Constructor
    /// </summary>
    /// <param name="mlbId">The MLB ID</param>
    /// <param name="name">The Team's name</param>
    /// <param name="abbreviation">The Team's abbreviation</param>
    private Team(MlbId mlbId, TeamName name, TeamAbbreviation abbreviation)
    {
        MlbId = mlbId;
        Name = name;
        Abbreviation = abbreviation;
    }

    /// <summary>
    /// Creates a <see cref="Team"/>
    /// </summary>
    /// <param name="mlbId">The MLB ID</param>
    /// <param name="name">The Team's name</param>
    /// <param name="abbreviation">The Team's abbreviation</param>
    /// <returns>The created <see cref="Team"/></returns>
    public static Team Create(MlbId mlbId, TeamName name, TeamAbbreviation abbreviation)
    {
        return new Team(mlbId, name, abbreviation);
    }

    /// <summary>
    /// Creates a <see cref="Team"/>
    /// </summary>
    /// <param name="teamInfo">A <see cref="TeamInfo"/> enum</param>
    /// <returns>The created <see cref="Team"/></returns>
    public static Team Create(TeamInfo teamInfo)
    {
        return new Team(MlbId.Create((int)teamInfo), TeamName.Create(teamInfo.GetDisplayName()),
            TeamAbbreviation.Create(teamInfo.ToString()));
    }
}