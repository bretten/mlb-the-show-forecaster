using com.brettnamba.MlbTheShowForecaster.Common.Application.Jobs;
using com.brettnamba.MlbTheShowForecaster.PlayerStatus.Application.Services.Results;

namespace com.brettnamba.MlbTheShowForecaster.PlayerStatus.Apps.PlayerTracker.Jobs.Io;

/// <summary>
/// Result of <see cref="PlayerStatusTrackerJob"/>
/// </summary>
public sealed record PlayerStatusTrackerJobResult(PlayerStatusTrackerResult Result) : IJobOutput;