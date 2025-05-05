using com.brettnamba.MlbTheShowForecaster.Common.Domain.ValueObjects;

namespace com.brettnamba.MlbTheShowForecaster.GameCards.Application.Services.EventStores;

/// <summary>
/// Defines a data sink for Listing data
/// </summary>
public interface IListingDataSink
{
    /// <summary>
    /// Writes Listing data to the sink for the specified season
    /// </summary>
    /// <param name="year">The season to write data for</param>
    /// <param name="count">The number of entries to write</param>
    /// <returns>The completed task</returns>
    Task Write(SeasonYear year, int count);
}