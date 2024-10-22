using com.brettnamba.MlbTheShowForecaster.Common.Application.Jobs;

namespace com.brettnamba.MlbTheShowForecaster.GameCards.Apps.MarketplaceWatcher.Jobs.Io;

/// <summary>
/// Result of <see cref="CardPriceTrackerJob"/>
/// </summary>
public sealed record CardPriceTrackerJobResult(int TotalCards, int TotalNewListings, int TotalUpdatedListings)
    : IJobOutput;