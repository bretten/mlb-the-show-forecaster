using com.brettnamba.MlbTheShowForecaster.Common.Domain.ValueObjects;
using com.brettnamba.MlbTheShowForecaster.PlayerStatus.Domain.Teams.ValueObjects;

namespace com.brettnamba.MlbTheShowForecaster.PlayerStatus.Domain.Teams.Services;

/// <summary>
/// Service that provides Teams based on different parameters
/// </summary>
public interface ITeamProvider
{
    /// <summary>
    /// Gets a <see cref="Team"/> by <see cref="MlbId"/>
    /// </summary>
    /// <param name="mlbId">The <see cref="MlbId"/></param>
    /// <returns>The corresponding <see cref="Team"/></returns>
    Team GetBy(MlbId mlbId);

    /// <summary>
    /// Gets a <see cref="Team"/> by <see cref="TeamAbbreviation"/>
    /// </summary>
    /// <param name="abbreviation">The <see cref="TeamAbbreviation"/></param>
    /// <returns>The corresponding <see cref="Team"/></returns>
    Team GetBy(TeamAbbreviation abbreviation);

    /// <summary>
    /// Retrieves a <see cref="Team"/> by its common names or abbreviations
    /// </summary>
    /// <param name="name">The name of the team</param>
    /// <returns>The corresponding <see cref="Team"/> or null</returns>
    Team? GetBy(string name);
}