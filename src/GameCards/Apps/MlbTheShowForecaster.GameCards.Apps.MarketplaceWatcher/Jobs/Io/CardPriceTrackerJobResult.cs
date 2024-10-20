using com.brettnamba.MlbTheShowForecaster.Common.Application.Jobs;
using com.brettnamba.MlbTheShowForecaster.GameCards.Application.Services.Results;

namespace com.brettnamba.MlbTheShowForecaster.GameCards.Apps.MarketplaceWatcher.Jobs.Io;

/// <summary>
/// Result of <see cref="CardPriceTrackerJob"/>
/// </summary>
public sealed record CardPriceTrackerJobResult(CardPriceTrackerResult Result) : IJobOutput;