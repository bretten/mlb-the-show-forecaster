using com.brettnamba.MlbTheShowForecaster.Common.Application.Jobs;
using com.brettnamba.MlbTheShowForecaster.GameCards.Application.Services.Results;

namespace com.brettnamba.MlbTheShowForecaster.GameCards.Apps.MarketplaceWatcher.Jobs.Io;

/// <summary>
/// Result of <see cref="PlayerCardTrackerJob"/>
/// </summary>
public sealed record PlayerCardTrackerJobResult(PlayerCardTrackerResult Result) : IJobOutput;