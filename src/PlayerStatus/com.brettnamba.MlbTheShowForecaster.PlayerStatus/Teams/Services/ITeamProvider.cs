﻿using com.brettnamba.MlbTheShowForecaster.PlayerStatus.Common.ValueObjects;
using com.brettnamba.MlbTheShowForecaster.PlayerStatus.Teams.ValueObjects;

namespace com.brettnamba.MlbTheShowForecaster.PlayerStatus.Teams.Services;

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
}