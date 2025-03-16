using System.Collections.ObjectModel;
using System.Data;
using com.brettnamba.MlbTheShowForecaster.Common.Domain.ValueObjects;
using com.brettnamba.MlbTheShowForecaster.GameCards.Domain.Cards.ValueObjects;
using com.brettnamba.MlbTheShowForecaster.GameCards.Domain.Marketplace.Entities;
using com.brettnamba.MlbTheShowForecaster.GameCards.Domain.Marketplace.Repositories;
using com.brettnamba.MlbTheShowForecaster.GameCards.Domain.Marketplace.ValueObjects;
using com.brettnamba.MlbTheShowForecaster.GameCards.Infrastructure.Marketplace.EntityFrameworkCore;
using Npgsql;
using NpgsqlTypes;

namespace com.brettnamba.MlbTheShowForecaster.GameCards.Infrastructure.Marketplace.Npgsql;

/// <summary>
/// Npgsql implementation of <see cref="IListingRepository"/>
/// </summary>
public sealed class NpgsqlListingRepository : IListingRepository
{
    /// <summary>
    /// Data source for getting connections
    /// </summary>
    private readonly NpgsqlDataSource _dataSource;

    /// <summary>
    /// Constructor
    /// </summary>
    /// <param name="dataSource">Data source for getting connections</param>
    public NpgsqlListingRepository(NpgsqlDataSource dataSource)
    {
        _dataSource = dataSource;
    }

    /// <inheritdoc />
    public async Task Add(Listing listing, CancellationToken cancellationToken = default)
    {
        // Open a connection
        await using var connection = await _dataSource.OpenConnectionAsync(cancellationToken);

        // INSERT the Listing
        await using var command = new NpgsqlCommand(ListingsInsertCommand, connection);
        command.Parameters.Add(new NpgsqlParameter { Value = listing.Id, DbType = DbType.Guid });
        command.Parameters.Add(new NpgsqlParameter { Value = (short)listing.Year.Value, DbType = DbType.Int16 });
        command.Parameters.Add(new NpgsqlParameter { Value = listing.CardExternalId.Value, DbType = DbType.Guid });
        command.Parameters.Add(new NpgsqlParameter { Value = listing.BuyPrice.Value, DbType = DbType.Int32 });
        command.Parameters.Add(new NpgsqlParameter { Value = listing.SellPrice.Value, DbType = DbType.Int32 });
        await command.ExecuteScalarAsync(cancellationToken);
    }

    /// <inheritdoc />
    public async Task Update(Listing listing, CancellationToken cancellationToken = default)
    {
        // Open a connection
        await using var connection = await _dataSource.OpenConnectionAsync(cancellationToken);

        // UPDATE the Listing
        await using var command = new NpgsqlCommand(ListingsUpdateCommand, connection);
        command.Parameters.Add(new NpgsqlParameter { Value = listing.Id, DbType = DbType.Guid });
        command.Parameters.Add(new NpgsqlParameter { Value = listing.BuyPrice.Value, DbType = DbType.Int32 });
        command.Parameters.Add(new NpgsqlParameter { Value = listing.SellPrice.Value, DbType = DbType.Int32 });
        await command.ExecuteScalarAsync(cancellationToken);
    }

    /// <inheritdoc />
    public async Task<Listing?> GetByExternalId(SeasonYear year, CardExternalId externalId, bool includeRelated,
        CancellationToken cancellationToken = default)
    {
        await using var connection = await _dataSource.OpenConnectionAsync(cancellationToken);

        await using var command = new NpgsqlCommand(ListingSelectByCardExternalIdCommand, connection);
        command.Parameters.AddWithValue("year", (short)year.Value);
        command.Parameters.AddWithValue("id", externalId.Value);

        // Read the Listing
        await using var reader = await command.ExecuteReaderAsync(cancellationToken);
        if (!await reader.ReadAsync(cancellationToken))
        {
            // No corresponding Listing
            return null;
        }

        var id = reader.GetGuid(0);
        var buyPrice = reader.GetInt32(1);
        var sellPrice = reader.GetInt32(2);

        await reader.CloseAsync();

        // Get the related entities
        var prices = new List<ListingHistoricalPrice>();
        var orders = new List<ListingOrder>();
        if (includeRelated)
        {
            prices = await QueryPrices(connection, id, cancellationToken);
            orders = await QueryOrders(connection, id, cancellationToken);
        }

        await connection.CloseAsync();

        return Listing.Create(year, externalId, NaturalNumber.Create(buyPrice), NaturalNumber.Create(sellPrice), prices,
            orders, id);
    }

    /// <inheritdoc />
    public async Task Add(Dictionary<CardExternalId, Listing> listings,
        Dictionary<CardExternalId, ReadOnlyCollection<ListingHistoricalPrice>> prices,
        CancellationToken cancellationToken = default)
    {
        // Open a connection and begin a transaction
        await using var connection = await _dataSource.OpenConnectionAsync(cancellationToken);
        await using var transaction = await connection.BeginTransactionAsync(cancellationToken);

        await using var importer =
            await connection.BeginBinaryImportAsync(ListingHistoricalPricesBinaryCopyCommand, cancellationToken);
        _batchHistoricalPriceWriterDelegate(listings, prices, importer);
        await importer.CompleteAsync(cancellationToken);
        await importer.CloseAsync(cancellationToken);

        await transaction.CommitAsync(cancellationToken);
    }

    /// <inheritdoc />
    public async Task Add(Dictionary<CardExternalId, Listing> listings,
        Dictionary<CardExternalId, ReadOnlyCollection<ListingOrder>> orders,
        CancellationToken cancellationToken = default)
    {
        // Open a connection and begin a transaction
        await using var connection = await _dataSource.OpenConnectionAsync(cancellationToken);
        await using var transaction = await connection.BeginTransactionAsync(cancellationToken);

        await using var importer =
            await connection.BeginBinaryImportAsync(ListingOrdersBinaryCopyCommand, cancellationToken);
        _batchOrderWriterDelegate(listings, orders, importer);
        await importer.CompleteAsync(cancellationToken);
        await importer.CloseAsync(cancellationToken);

        await transaction.CommitAsync(cancellationToken);
    }

    /// <summary>
    /// Queries the database for a <see cref="Listing"/>'s <see cref="ListingHistoricalPrice"/>s
    /// </summary>
    /// <param name="connection">The connection to use</param>
    /// <param name="listingId">The ID of the <see cref="Listing"/></param>
    /// <param name="cancellationToken">A <see cref="CancellationToken" /> to observe while waiting for the task to complete</param>
    /// <returns>All <see cref="ListingHistoricalPrice"/> for the specified <see cref="Listing"/></returns>
    private async Task<List<ListingHistoricalPrice>> QueryPrices(NpgsqlConnection connection, Guid listingId,
        CancellationToken cancellationToken = default)
    {
        await using var command = new NpgsqlCommand(ListingPricesSelectByListingIdCommand, connection);
        command.Parameters.AddWithValue("listing_id", listingId);

        await using var reader = await command.ExecuteReaderAsync(cancellationToken);

        var prices = new List<ListingHistoricalPrice>();
        while (await reader.ReadAsync(cancellationToken))
        {
            var date = reader.GetDateTime(0);
            var buyPrice = reader.GetInt32(1);
            var sellPrice = reader.GetInt32(2);
            var price = ListingHistoricalPrice.Create(DateOnly.FromDateTime(date), NaturalNumber.Create(buyPrice),
                NaturalNumber.Create(sellPrice));
            prices.Add(price);
        }

        await reader.CloseAsync();

        return prices;
    }

    /// <summary>
    /// Queries the database for a <see cref="Listing"/>'s <see cref="ListingOrder"/>s
    /// </summary>
    /// <param name="connection">The connection to use</param>
    /// <param name="listingId">The ID of the <see cref="Listing"/></param>
    /// <param name="cancellationToken">A <see cref="CancellationToken" /> to observe while waiting for the task to complete</param>
    /// <returns>All <see cref="ListingOrder"/> for the specified <see cref="Listing"/></returns>
    private async Task<List<ListingOrder>> QueryOrders(NpgsqlConnection connection, Guid listingId,
        CancellationToken cancellationToken = default)
    {
        await using var command = new NpgsqlCommand(ListingOrdersSelectByListingIdCommand, connection);
        command.Parameters.AddWithValue("listing_id", listingId);

        await using var reader = await command.ExecuteReaderAsync(cancellationToken);

        var orders = new List<ListingOrder>();
        while (await reader.ReadAsync(cancellationToken))
        {
            var date = reader.GetDateTime(0);
            var price = reader.GetInt32(1);
            var order = ListingOrder.Create(date, NaturalNumber.Create(price));
            orders.Add(order);
        }

        await reader.CloseAsync();

        return orders;
    }

    /// <summary>
    /// Delegate for writing a batch of <see cref="ListingHistoricalPrice"/>s to the Npgsql binary importer
    /// </summary>
    private readonly
        Action<Dictionary<CardExternalId, Listing>,
            Dictionary<CardExternalId, ReadOnlyCollection<ListingHistoricalPrice>>,
            NpgsqlBinaryImporter> _batchHistoricalPriceWriterDelegate = (listings, pricesMap, importer) =>
        {
            foreach (var (cardExternalId, prices) in pricesMap)
            {
                foreach (var historicalPrice in prices)
                {
                    // Get the corresponding listing
                    var listing = listings[cardExternalId];

                    importer.StartRow();
                    importer.Write(listing.Id, NpgsqlDbType.Uuid);
                    importer.Write(historicalPrice.Date, NpgsqlDbType.Date);
                    importer.Write(historicalPrice.BuyPrice.Value, NpgsqlDbType.Integer);
                    importer.Write(historicalPrice.SellPrice.Value, NpgsqlDbType.Integer);
                }
            }
        };

    /// <summary>
    /// Delegate for writing a batch of <see cref="ListingOrder"/>s to the Npgsql binary importer
    /// </summary>
    private readonly
        Action<Dictionary<CardExternalId, Listing>, Dictionary<CardExternalId, ReadOnlyCollection<ListingOrder>>,
            NpgsqlBinaryImporter>
        _batchOrderWriterDelegate = (listings, ordersMap, importer) =>
        {
            foreach (var (cardExternalId, orders) in ordersMap)
            {
                foreach (var order in orders)
                {
                    // Get the corresponding listing
                    var listing = listings[cardExternalId];

                    importer.StartRow();
                    importer.Write(listing.Id, NpgsqlDbType.Uuid);
                    importer.Write(order.Date, NpgsqlDbType.TimestampTz);
                    importer.Write(order.Price.Value, NpgsqlDbType.Integer);
                }
            }
        };

    /// <summary>
    /// Selects a <see cref="Listing"/> by <see cref="CardExternalId"/>
    /// </summary>
    private const string ListingSelectByCardExternalIdCommand = $@"SELECT
            {Constants.Listings.Id},
            {Constants.Listings.BuyPrice},
            {Constants.Listings.SellPrice}
        FROM {Constants.Schema}.{Constants.Listings.TableName}
        WHERE {Constants.Listings.Year} = @year AND {Constants.Listings.CardExternalId} = @id LIMIT 1";

    /// <summary>
    /// Inserts a new <see cref="Listing"/>
    /// </summary>
    private const string ListingsInsertCommand = $@"
        INSERT INTO {Constants.Schema}.{Constants.Listings.TableName} (
            {Constants.Listings.Id},
            {Constants.Listings.Year},
            {Constants.Listings.CardExternalId},
            {Constants.Listings.BuyPrice},
            {Constants.Listings.SellPrice}
        )
        VALUES ($1, $2, $3, $4, $5)
        RETURNING {Constants.Listings.Id};";

    /// <summary>
    /// Updates an existing <see cref="Listing"/>
    /// </summary>
    private const string ListingsUpdateCommand = $@"
        UPDATE {Constants.Schema}.{Constants.Listings.TableName}
        SET (
            {Constants.Listings.BuyPrice},
            {Constants.Listings.SellPrice}
        )
        = ($2, $3)
        WHERE {Constants.Listings.Id} = $1;";

    /// <summary>
    /// Selects <see cref="ListingHistoricalPrice"/>s by a <see cref="Listing"/> ID
    /// </summary>
    private const string ListingPricesSelectByListingIdCommand = $@"SELECT
            {Constants.ListingHistoricalPrices.Date},
            {Constants.ListingHistoricalPrices.BuyPrice},
            {Constants.ListingHistoricalPrices.SellPrice} 
        FROM {Constants.Schema}.{Constants.ListingHistoricalPrices.TableName}
        WHERE {Constants.ListingHistoricalPrices.ListingId} = @listing_id";

    /// <summary>
    /// Bulk binary import for <see cref="ListingHistoricalPrice"/>
    /// </summary>
    private const string ListingHistoricalPricesBinaryCopyCommand = $@"
        COPY {Constants.Schema}.{Constants.ListingHistoricalPrices.TableName} (
            {Constants.ListingHistoricalPrices.ListingId},
            {Constants.ListingHistoricalPrices.Date},
            {Constants.ListingHistoricalPrices.BuyPrice},
            {Constants.ListingHistoricalPrices.SellPrice}
        )
        FROM STDIN (FORMAT BINARY)";

    /// <summary>
    /// Selects <see cref="ListingOrder"/>s by a <see cref="Listing"/> ID
    /// </summary>
    private const string ListingOrdersSelectByListingIdCommand = $@"SELECT
            {Constants.ListingOrders.Date},
            {Constants.ListingOrders.Price}
        FROM {Constants.Schema}.{Constants.ListingOrders.TableName}
        WHERE {Constants.ListingOrders.ListingId} = @listing_id";

    /// <summary>
    /// Bulk binary import for <see cref="ListingOrder"/>
    /// </summary>
    private const string ListingOrdersBinaryCopyCommand = $@"
        COPY {Constants.Schema}.{Constants.ListingOrders.TableName} (
            {Constants.ListingOrders.ListingId},
            {Constants.ListingOrders.Date},
            {Constants.ListingOrders.Price}
        )
        FROM STDIN (FORMAT BINARY)";
}