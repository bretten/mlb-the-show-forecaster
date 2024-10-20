using com.brettnamba.MlbTheShowForecaster.Common.Application.Jobs;

namespace com.brettnamba.MlbTheShowForecaster.GameCards.Apps.MarketplaceWatcher.Jobs.Io;

/// <summary>
/// Result of <see cref="TrendReporterJob"/>
/// </summary>
public sealed record TrendReporterJobResult(int TotalPlayerCards, int UpdatedReports) : IJobOutput;