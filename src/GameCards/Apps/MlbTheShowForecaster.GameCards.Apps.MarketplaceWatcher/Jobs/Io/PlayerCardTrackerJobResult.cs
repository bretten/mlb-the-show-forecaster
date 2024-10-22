using com.brettnamba.MlbTheShowForecaster.Common.Application.Jobs;

namespace com.brettnamba.MlbTheShowForecaster.GameCards.Apps.MarketplaceWatcher.Jobs.Io;

/// <summary>
/// Result of <see cref="PlayerCardTrackerJob"/>
/// </summary>
public sealed record PlayerCardTrackerJobResult(
    int TotalCatalogCards,
    int TotalNewCatalogCards,
    int TotalUpdatedPlayerCards) : IJobOutput;