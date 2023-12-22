using com.brettnamba.MlbTheShowForecaster.Core.SeedWork;
using com.brettnamba.MlbTheShowForecaster.PlayerStatus.Common.ValueObjects;
using com.brettnamba.MlbTheShowForecaster.PlayerStatus.Players.Entities;

namespace com.brettnamba.MlbTheShowForecaster.PlayerStatus.Teams.ValueObjects;

/// <summary>
/// Represents a MLB team
///
/// Currently, we will not consider a Team an entity as the domain is only focusing on
/// the perspective of the <see cref="Player"/>
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

    public static Team Create(MlbId mlbId, TeamName name, TeamAbbreviation abbreviation)
    {
        return new Team(mlbId, name, abbreviation);
    }
}