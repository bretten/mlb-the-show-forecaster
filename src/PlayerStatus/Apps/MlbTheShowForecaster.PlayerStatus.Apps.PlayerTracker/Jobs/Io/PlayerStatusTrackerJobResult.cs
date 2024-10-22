using com.brettnamba.MlbTheShowForecaster.Common.Application.Jobs;

namespace com.brettnamba.MlbTheShowForecaster.PlayerStatus.Apps.PlayerTracker.Jobs.Io;

/// <summary>
/// Result of <see cref="PlayerStatusTrackerJob"/>
/// </summary>
public sealed record PlayerStatusTrackerJobResult(
    int TotalRosterEntries,
    int TotalNewPlayers,
    int TotalUpdatedPlayers) : IJobOutput;