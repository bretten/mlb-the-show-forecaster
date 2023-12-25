using com.brettnamba.MlbTheShowForecaster.PlayerStatus.Common.ValueObjects;
using com.brettnamba.MlbTheShowForecaster.PlayerStatus.Teams.Enums;
using com.brettnamba.MlbTheShowForecaster.PlayerStatus.Teams.Exceptions;
using com.brettnamba.MlbTheShowForecaster.PlayerStatus.Teams.ValueObjects;

namespace com.brettnamba.MlbTheShowForecaster.PlayerStatus.Teams.Services;

/// <summary>
/// Service that provides Teams based on different parameters
/// </summary>
public sealed class TeamProvider : ITeamProvider
{
    /// <summary>
    /// Maps <see cref="MlbId"/> to the corresponding <see cref="Team"/>
    /// </summary>
    private readonly Dictionary<MlbId, Team> _mlbIdToTeam;

    /// <summary>
    /// Maps <see cref="TeamAbbreviation"/> to the corresponding <see cref="Team"/>
    /// </summary>
    private readonly Dictionary<TeamAbbreviation, Team> _abbreviationToTeam;

    /// <summary>
    /// Constructor
    /// </summary>
    public TeamProvider()
    {
        // Create lookups based on keys
        var teams = Enum.GetValues(typeof(TeamInfo)).Cast<TeamInfo>().Select(Team.Create).ToList();
        _mlbIdToTeam = teams.ToDictionary(k => k.MlbId, v => v);
        _abbreviationToTeam = teams.ToDictionary(k => k.Abbreviation, v => v);
    }

    /// <summary>
    /// Gets a <see cref="Team"/> by <see cref="MlbId"/>
    /// </summary>
    /// <param name="mlbId">The <see cref="MlbId"/></param>
    /// <returns>The corresponding <see cref="Team"/></returns>
    /// <exception cref="UnknownTeamMlbIdException">Thrown when the <see cref="MlbId"/> does not correspond to a <see cref="Team"/></exception>
    public Team GetBy(MlbId mlbId)
    {
        if (!_mlbIdToTeam.TryGetValue(mlbId, out var value))
        {
            throw new UnknownTeamMlbIdException($"Cannot get Team due to unknown Team MLB ID: {mlbId.Value}");
        }

        return value;
    }

    /// <summary>
    /// Gets a <see cref="Team"/> by <see cref="TeamAbbreviation"/>
    /// </summary>
    /// <param name="abbreviation">The <see cref="TeamAbbreviation"/></param>
    /// <returns>The corresponding <see cref="Team"/></returns>
    /// <exception cref="UnknownTeamAbbreviationException">Thrown when the <see cref="TeamAbbreviation"/> does not correspond to a <see cref="Team"/></exception>
    public Team GetBy(TeamAbbreviation abbreviation)
    {
        if (!_abbreviationToTeam.TryGetValue(abbreviation, out var value))
        {
            throw new UnknownTeamAbbreviationException(
                $"Cannot get Team due to unknown Team abbreviation: {abbreviation}");
        }

        return value;
    }
}