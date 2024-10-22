using com.brettnamba.MlbTheShowForecaster.Common.Application.Jobs;

namespace com.brettnamba.MlbTheShowForecaster.Performance.Apps.PerformanceTracker.Jobs.Io;

/// <summary>
/// Result of <see cref="PerformanceTrackerJob"/>
/// </summary>
public sealed record PerformanceTrackerJobResult(
    int TotalPlayerSeasons,
    int TotalNewPlayerSeasons,
    int TotalPlayerSeasonUpdates) : IJobOutput;