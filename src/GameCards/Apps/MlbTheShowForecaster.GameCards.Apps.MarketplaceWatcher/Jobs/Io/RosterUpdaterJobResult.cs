using com.brettnamba.MlbTheShowForecaster.Common.Application.Jobs;

namespace com.brettnamba.MlbTheShowForecaster.GameCards.Apps.MarketplaceWatcher.Jobs.Io;

/// <summary>
/// Result of <see cref="RosterUpdaterJob"/>
/// </summary>
public sealed record RosterUpdaterJobResult(int TotalHistoricalUpdatesApplied, int TotalRosterUpdatesApplied)
    : IJobOutput;