using com.brettnamba.MlbTheShowForecaster.Common.Application.Jobs;
using com.brettnamba.MlbTheShowForecaster.GameCards.Application.Services.Results;

namespace com.brettnamba.MlbTheShowForecaster.GameCards.Apps.MarketplaceWatcher.Jobs.Io;

/// <summary>
/// Result of <see cref="RosterUpdaterJob"/>
/// </summary>
public sealed record RosterUpdaterJobResult(IEnumerable<RosterUpdateOrchestratorResult> Result) : IJobOutput;