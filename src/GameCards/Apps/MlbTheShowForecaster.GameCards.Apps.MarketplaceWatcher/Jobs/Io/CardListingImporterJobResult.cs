using com.brettnamba.MlbTheShowForecaster.Common.Application.Jobs;

namespace com.brettnamba.MlbTheShowForecaster.GameCards.Apps.MarketplaceWatcher.Jobs.Io;

/// <summary>
/// Result of <see cref="CardListingImporterJob"/>
/// </summary>
/// <param name="TotalListings">The number of updated Listings</param>
public sealed record CardListingImporterJobResult(int TotalListings) : IJobOutput;