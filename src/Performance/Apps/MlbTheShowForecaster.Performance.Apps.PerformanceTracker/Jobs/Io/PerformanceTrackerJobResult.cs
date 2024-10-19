using com.brettnamba.MlbTheShowForecaster.Common.Application.Jobs;
using com.brettnamba.MlbTheShowForecaster.Performance.Application.Services.Results;

namespace com.brettnamba.MlbTheShowForecaster.Performance.Apps.PerformanceTracker.Jobs.Io;

/// <summary>
/// Result of <see cref="PerformanceTrackerJob"/>
/// </summary>
public sealed record PerformanceTrackerJobResult(PerformanceTrackerResult Result) : IJobOutput;