﻿namespace com.brettnamba.MlbTheShowForecaster.Performance.Application.Services.Results;

/// <summary>
/// Represents the result of <see cref="IPerformanceTracker.TrackPlayerPerformance"/>
/// </summary>
/// <param name="TotalPlayerSeasons">The total number of player seasons in the domain</param>
/// <param name="TotalNewPlayerSeasons">The number of player seasons that were created</param>
/// <param name="TotalPlayerSeasonUpdates">The number of player seasons that were updated</param>
public readonly record struct PerformanceTrackerResult(
    int TotalPlayerSeasons,
    int TotalNewPlayerSeasons,
    int TotalPlayerSeasonUpdates)
{
    /// <summary>
    /// The number of player seasons that were already up-to-date
    /// </summary>
    public int TotalUpToDatePlayerSeasons => TotalPlayerSeasons - TotalNewPlayerSeasons - TotalPlayerSeasonUpdates;
}