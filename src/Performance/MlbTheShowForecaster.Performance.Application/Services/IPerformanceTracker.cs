using com.brettnamba.MlbTheShowForecaster.Common.Domain.ValueObjects;
using com.brettnamba.MlbTheShowForecaster.Performance.Application.Services.Results;

namespace com.brettnamba.MlbTheShowForecaster.Performance.Application.Services;

/// <summary>
/// Defines a service that will track player performance during a specified time period
///
/// <para>By tracking performance, it will keep the stats in this system up-to-date with live MLB stats</para>
/// </summary>
public interface IPerformanceTracker : IDisposable
{
    /// <summary>
    /// Should keep track of player performance for the specified season by making sure this system has stats that match
    /// the live MLB stats
    /// </summary>
    /// <param name="seasonYear">The season to track performance for</param>
    /// <param name="cancellationToken">A <see cref="CancellationToken" /> to observe while waiting for the task to complete</param>
    /// <returns>The completed task</returns>
    Task<PerformanceTrackerResult> TrackPlayerPerformance(SeasonYear seasonYear,
        CancellationToken cancellationToken = default);
}