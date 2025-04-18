using System.Globalization;
using com.brettnamba.MlbTheShowForecaster.Common.Application.FileSystems;
using com.brettnamba.MlbTheShowForecaster.Common.DateAndTime;
using com.brettnamba.MlbTheShowForecaster.Common.Domain.ValueObjects;
using com.brettnamba.MlbTheShowForecaster.GameCards.Application.Services.EventStores;
using Parquet;
using Parquet.Data;
using Parquet.Schema;
using StackExchange.Redis;

namespace com.brettnamba.MlbTheShowForecaster.GameCards.Infrastructure.Services.EventStores;

/// <summary>
/// Listing data sink that stores data in Parquet files
/// </summary>
public sealed class ParquetListingDataSink : IListingDataSink
{
    /// <summary>
    /// Connects to the Listing data source in Redis
    /// </summary>
    private readonly IConnectionMultiplexer _redisConnection;

    /// <summary>
    /// File system used to store the Parquet files
    /// </summary>
    private readonly IFileSystem _fileSystem;

    /// <summary>
    /// Filters orders by date
    /// </summary>
    private readonly ICalendar _calendar;

    /// <summary>
    /// Settings
    /// </summary>
    private readonly Settings _settings;

    /// <summary>
    /// Constructor
    /// </summary>
    /// <param name="redisConnection">Connects to the Listing data source in Redis</param>
    /// <param name="fileSystem">File system used to store the Parquet files</param>
    /// <param name="calendar">Filters orders by date</param>
    /// <param name="settings">Settings</param>
    public ParquetListingDataSink(IConnectionMultiplexer redisConnection, IFileSystem fileSystem, ICalendar calendar,
        Settings settings)
    {
        _redisConnection = redisConnection;
        _fileSystem = fileSystem;
        _calendar = calendar;
        _settings = settings;
    }

    /// <summary>
    /// Key for the chronologically last acknowledged Listing order in the event store
    /// </summary>
    /// <param name="year">The year of the listing</param>
    private static string LastAcknowledgedOrderSinkKey(SeasonYear year) =>
        $"{RedisListingEventStore.KeyPrefix(year)}:orders:last_acknowledged_sink";

    /// <summary>
    /// The Parquet schema
    /// </summary>
    private static readonly ParquetSchema Schema = new ParquetSchema(
        new DataField<string>("CardExternalId"),
        new DataField<DateTime>("Date"),
        new DataField<int>("Price"));

    /// <inheritdoc />
    public async Task Write(SeasonYear year, int count)
    {
        var db = _redisConnection.GetDatabase();

        // Get the last acknowledged ID
        var checkpoint = await GetCheckpoint(LastAcknowledgedOrderSinkKey(year));

        // Read new orders that were appended after the last acknowledged ID
        var entries =
            await db.StreamReadAsync(RedisListingEventStore.OrdersEventStoreKey(year), checkpoint, count: count);
        var orders = entries.Select(entry =>
        {
            // Parse the order
            var externalId = entry["card_external_id"].ToString();
            var date = DateTime.ParseExact(entry["date"].ToString(), RedisListingEventStore.DateTimeFormat,
                CultureInfo.InvariantCulture,
                DateTimeStyles.AssumeUniversal).ToUniversalTime();
            entry["price"].TryParse(out int price);
            return new ParquetListingOrder(entry.Id.ToString(), externalId, date, price);
        });

        // Group the orders by date and store in Parquet files
        var ordersByDate = orders.GroupBy(x => DateOnly.FromDateTime(x.Date))
            .Where(x => x.Key <= _calendar.Today().AddDays(-_settings.EndDateDaysBackOffset))
            .OrderBy(x => x.Key);
        foreach (var allOrdersForDay in ordersByDate)
        {
            // Date for all orders
            var date = allOrdersForDay.Key;
            // Make sure all orders for the day are in chronological order
            var allOrdersForDayChronological = allOrdersForDay.OrderBy(x => x.Date);
            // In batches, write to Parquet
            foreach (var orderBatchForDay in allOrdersForDayChronological.Chunk(_settings.CountPerFile))
            {
                // Map the data to columns for Parquet
                var externalIdColumn = new DataColumn(Schema.DataFields[0],
                    orderBatchForDay.Select(x => x.CardExternalId).ToArray());
                var dateColumn = new DataColumn(Schema.DataFields[1],
                    orderBatchForDay.Select(x => x.Date).ToArray());
                var priceColumn = new DataColumn(Schema.DataFields[2],
                    orderBatchForDay.Select(x => x.Price).ToArray());

                // Write the columns to a temp Parquet file
                var tmpFileName = Path.GetTempFileName();
                await using (Stream fs = File.OpenWrite(tmpFileName))
                await using (var writer = await ParquetWriter.CreateAsync(Schema, fs))
                {
                    using var groupWriter = writer.CreateRowGroup();
                    await groupWriter.WriteColumnAsync(externalIdColumn);
                    await groupWriter.WriteColumnAsync(dateColumn);
                    await groupWriter.WriteColumnAsync(priceColumn);
                }

                // Copy the Parquet file to the destination file system
                var destinationFileName = GetFilePath(date);
                await using (var readStream = File.OpenRead(tmpFileName))
                {
                    await _fileSystem.StoreFile(readStream, destinationFileName, true);
                }

                // Delete the temp file
                File.Delete(tmpFileName);

                // If writing to Parquet was successful, update the checkpoint
                var lastItemId = allOrdersForDay.Last().Id;
                await UpdateCheckpoint(LastAcknowledgedOrderSinkKey(year), lastItemId);
            }
        }

        // Trim old data
        await Trim(year);
    }

    /// <summary>
    /// Gets the last acknowledged ID for a given key
    /// </summary>
    /// <param name="key">The key</param>
    /// <returns>The last acknowledged ID</returns>
    private async Task<string> GetCheckpoint(string key)
    {
        var db = _redisConnection.GetDatabase();

        // Get the last acknowledged ID or if null, start at the beginning
        var lastAcknowledgedId = await db.StringGetAsync(key);
        return lastAcknowledgedId.HasValue ? lastAcknowledgedId.ToString() : "0";
    }

    /// <summary>
    /// Updates the last acknowledged ID for a given key
    /// </summary>
    /// <param name="key">The key</param>
    /// <param name="lastAcknowledgedId">The new last acknowledged ID</param>
    private async Task UpdateCheckpoint(string key, string lastAcknowledgedId)
    {
        var db = _redisConnection.GetDatabase();
        await db.StringSetAsync(key, lastAcknowledgedId);
    }

    /// <summary>
    /// Trims the Redis data source to remove old, unneeded data
    /// </summary>
    /// <param name="year">The season to trim data for</param>
    private async Task Trim(SeasonYear year)
    {
        var db = _redisConnection.GetDatabase();

        // Get the current Unix timestamp to determine the trim ID
        var daysBack = -1 * (_settings.EndDateDaysBackOffset + 7); // end date offset + a week
        var now = new DateTimeOffset(_calendar.Today().AddDays(-daysBack), TimeOnly.MinValue, TimeSpan.Zero)
            .ToUnixTimeMilliseconds();

        // Trim everything chronologically before this ID
        var id = $"{now}-0";

        await db.ExecuteAsync("XTRIM", RedisListingEventStore.OrdersEventStoreKey(year), "MINID", id);
    }

    /// <summary>
    /// Gets the Parquet file path for a given date
    /// </summary>
    /// <param name="date">The date</param>
    /// <returns>Parquet file path for date</returns>
    private static string GetFilePath(DateOnly date)
    {
        return
            $"parquet/year={date.Year}/month={date.Month:D2}/day={date.Day:D2}/{date:yyyy-MM-dd}-{Guid.NewGuid():N}.parquet";
    }

    /// <summary>
    /// Represents a listing order for Parquet
    /// </summary>
    private sealed record ParquetListingOrder(string Id, string CardExternalId, DateTime Date, int Price);

    /// <summary>
    /// Settings
    /// </summary>
    /// <param name="CountPerFile">The number of entries per Parquet file</param>
    /// <param name="EndDateDaysBackOffset">How many days in the past to make the end date of the data. This prevents spreading data over smaller files throughout the day and instead waits until the day is complete</param>
    public sealed record Settings(int CountPerFile, int EndDateDaysBackOffset);
}