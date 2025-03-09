using System.Collections.ObjectModel;
using System.Data;
using System.Security.Cryptography;
using System.Text;
using com.brettnamba.MlbTheShowForecaster.Common.Infrastructure.Database;
using com.brettnamba.MlbTheShowForecaster.GameCards.Domain.Cards.ValueObjects;
using com.brettnamba.MlbTheShowForecaster.GameCards.Domain.Marketplace.Entities;
using com.brettnamba.MlbTheShowForecaster.GameCards.Domain.Marketplace.Repositories;
using com.brettnamba.MlbTheShowForecaster.GameCards.Domain.Marketplace.ValueObjects;
using com.brettnamba.MlbTheShowForecaster.GameCards.Infrastructure.Marketplace.EntityFrameworkCore.Exceptions;
using Microsoft.EntityFrameworkCore;
using Npgsql;
using NpgsqlTypes;

namespace com.brettnamba.MlbTheShowForecaster.GameCards.Infrastructure.Marketplace.EntityFrameworkCore;

/// <summary>
/// Hybrid implementation of <see cref="IListingRepository"/> that uses EF Core but also uses Npgsql directly for
/// transactions and binary imports
/// </summary>
public sealed class HybridNpgsqlEntityFrameworkCoreListingRepository : IListingRepository
{
    /// <summary>
    /// The DB context for <see cref="Listing"/>
    /// </summary>
    private readonly MarketplaceDbContext _dbContext;

    /// <summary>
    /// The current, active database transaction
    /// </summary>
    private readonly IAtomicDatabaseOperation _atomicDatabaseOperation;

    /// <summary>
    /// Constructor
    /// </summary>
    /// <param name="dbContext">The DB context for <see cref="Listing"/></param>
    /// <param name="atomicDatabaseOperation">The current, active database transaction</param>
    public HybridNpgsqlEntityFrameworkCoreListingRepository(MarketplaceDbContext dbContext,
        IAtomicDatabaseOperation atomicDatabaseOperation)
    {
        _dbContext = dbContext;
        _atomicDatabaseOperation = atomicDatabaseOperation;
    }

    /// <summary>
    /// Adds a <see cref="Listing"/>
    /// </summary>
    /// <param name="listing">The <see cref="Listing"/> to add</param>
    /// <param name="cancellationToken">A <see cref="CancellationToken" /> to observe while waiting for the task to complete</param>
    public async Task Add(Listing listing, CancellationToken cancellationToken = default)
    {
        // Open a connection and begin a transaction
        var connection = await GetConnection(cancellationToken);
        var transaction = await GetTransaction(cancellationToken);

        // INSERT the Listing
        await using var command = new NpgsqlCommand(ListingsInsertCommand, connection, transaction);
        command.Parameters.Add(new NpgsqlParameter { Value = listing.Id, DbType = DbType.Guid });
        command.Parameters.Add(new NpgsqlParameter { Value = listing.CardExternalId.Value, DbType = DbType.Guid });
        command.Parameters.Add(new NpgsqlParameter { Value = listing.BuyPrice.Value, DbType = DbType.Int32 });
        command.Parameters.Add(new NpgsqlParameter { Value = listing.SellPrice.Value, DbType = DbType.Int32 });
        await command.ExecuteScalarAsync(cancellationToken);

        // Bulk upsert the Listing's historical prices
        await BulkUpsertHistoricalPrices(connection, transaction, listing, _historicalPriceWriterDelegate,
            cancellationToken);

        // Bulk upsert the Listing's orders
        await BulkUpsertOrders(connection, transaction, listing, _orderWriterDelegate, cancellationToken);
    }

    /// <summary>
    /// Updates a <see cref="Listing"/>
    /// </summary>
    /// <param name="listing">The <see cref="Listing"/> to update</param>
    /// <param name="cancellationToken">A <see cref="CancellationToken" /> to observe while waiting for the task to complete</param>
    public async Task Update(Listing listing, CancellationToken cancellationToken = default)
    {
        // Open a connection and begin a transaction
        var connection = await GetConnection(cancellationToken);
        var transaction = await GetTransaction(cancellationToken);

        // UPDATE the Listing
        await using var command = new NpgsqlCommand(ListingsUpdateCommand, connection, transaction);
        command.Parameters.Add(new NpgsqlParameter { Value = listing.Id, DbType = DbType.Guid });
        command.Parameters.Add(new NpgsqlParameter { Value = listing.BuyPrice.Value, DbType = DbType.Int32 });
        command.Parameters.Add(new NpgsqlParameter { Value = listing.SellPrice.Value, DbType = DbType.Int32 });
        await command.ExecuteScalarAsync(cancellationToken);

        // Bulk upsert the Listing's historical prices
        await BulkUpsertHistoricalPrices(connection, transaction, listing, _historicalPriceWriterDelegate,
            cancellationToken);

        // Bulk upsert the Listing's orders
        await BulkUpsertOrders(connection, transaction, listing, _orderWriterDelegate, cancellationToken);
    }

    /// <summary>
    /// Returns a <see cref="Listing"/> for the specified <see cref="CardExternalId"/>
    /// </summary>
    /// <param name="externalId">The <see cref="CardExternalId"/> of the <see cref="Listing"/></param>
    /// <param name="cancellationToken">A <see cref="CancellationToken" /> to observe while waiting for the task to complete</param>
    /// <returns>The corresponding <see cref="Listing"/></returns>
    public async Task<Listing?> GetByExternalId(CardExternalId externalId,
        CancellationToken cancellationToken = default)
    {
        return await _dbContext.ListingsWithHistoricalPrices()
            .AsNoTracking() // Entities will be updated without using EF Core, so no tracking needed
            .FirstOrDefaultAsync(x => x.CardExternalId == externalId, cancellationToken: cancellationToken);
    }

    public Task Add(Dictionary<CardExternalId, Listing> listings,
        Dictionary<CardExternalId, ReadOnlyCollection<ListingHistoricalPrice>> prices,
        CancellationToken cancellationToken)
    {
        // Will be implemented in #478
        throw new NotImplementedException();
    }

    public Task Add(Dictionary<CardExternalId, Listing> listings,
        Dictionary<CardExternalId, ReadOnlyCollection<ListingOrder>> orders, CancellationToken cancellationToken)
    {
        // Will be implemented in #478
        throw new NotImplementedException();
    }

    /// <summary>
    /// Gets the current DB connection
    /// </summary>
    /// <param name="cancellationToken">A <see cref="CancellationToken" /> to observe while waiting for the task to complete</param>
    /// <returns>The current DB connection</returns>
    /// <exception cref="InvalidNpgsqlDataSourceForListingRepositoryException">Thrown when the connection is not a <see cref="NpgsqlConnection"/></exception>
    private async Task<NpgsqlConnection> GetConnection(CancellationToken cancellationToken = default)
    {
        if (await _atomicDatabaseOperation.GetCurrentActiveConnection(cancellationToken) is not NpgsqlConnection c)
        {
            throw new InvalidNpgsqlDataSourceForListingRepositoryException(
                $"Connection was not a {nameof(NpgsqlConnection)} for {nameof(HybridNpgsqlEntityFrameworkCoreListingRepository)}");
        }

        return c;
    }

    /// <summary>
    /// Gets the current DB transaction
    /// </summary>
    /// <param name="cancellationToken">A <see cref="CancellationToken" /> to observe while waiting for the task to complete</param>
    /// <returns>The current DB transaction</returns>
    /// <exception cref="InvalidNpgsqlDataSourceForListingRepositoryException">Thrown when the transaction is not a <see cref="NpgsqlTransaction"/></exception>
    private async Task<NpgsqlTransaction> GetTransaction(CancellationToken cancellationToken = default)
    {
        if (await _atomicDatabaseOperation.GetCurrentActiveTransaction(cancellationToken) is not NpgsqlTransaction t)
        {
            throw new InvalidNpgsqlDataSourceForListingRepositoryException(
                $"Transaction was not a {nameof(NpgsqlTransaction)} for {nameof(HybridNpgsqlEntityFrameworkCoreListingRepository)}");
        }

        return t;
    }

    /// <summary>
    /// Uses an existing connection and transaction to bulk upsert a <see cref="Listing"/>'s <see cref="ListingHistoricalPrice"/>s
    /// </summary>
    /// <param name="connection">The existing connection</param>
    /// <param name="transaction">The existing transaction</param>
    /// <param name="listing">The <see cref="Listing"/> to upsert <see cref="ListingHistoricalPrice"/>s for</param>
    /// <param name="historicalPriceWriterDelegate">Delegate for writing a <see cref="Listing"/>'s <see cref="ListingHistoricalPrice"/>s to the Npgsql binary importer</param>
    /// <param name="cancellationToken">A <see cref="CancellationToken" /> to observe while waiting for the task to complete</param>
    private async Task BulkUpsertHistoricalPrices(NpgsqlConnection connection, NpgsqlTransaction transaction,
        Listing listing, Action<Listing, NpgsqlBinaryImporter> historicalPriceWriterDelegate,
        CancellationToken cancellationToken)
    {
        // Create a temporary table to insert the Listing's historical prices into
        await using var tempTableCmd =
            new NpgsqlCommand(ListingHistoricalPricesCreateTempTableCommand, connection, transaction);
        await tempTableCmd.ExecuteNonQueryAsync(cancellationToken);

        // COPY the Listing's historical prices into the temporary table using a binary import
        await using var importer =
            await connection.BeginBinaryImportAsync(ListingHistoricalPricesBinaryCopyCommand, cancellationToken);
        historicalPriceWriterDelegate(listing, importer);
        await importer.CompleteAsync(cancellationToken);
        await importer.CloseAsync(cancellationToken);

        // Bulk update the actual historical prices table with data from the temporary table
        await using var bulkUpdateCmd =
            new NpgsqlCommand(ListingHistoricalPricesBulkUpdateCommand, connection, transaction);
        await bulkUpdateCmd.ExecuteNonQueryAsync(cancellationToken);

        // Remove the temporary table
        await using var dropTempTableCmd =
            new NpgsqlCommand(ListingHistoricalPricesDropTempTableCommand, connection, transaction);
        await dropTempTableCmd.ExecuteNonQueryAsync(cancellationToken);
    }

    /// <summary>
    /// Uses an existing connection and transaction to bulk upsert a <see cref="Listing"/>'s <see cref="ListingOrder"/>s
    /// </summary>
    /// <param name="connection">The existing connection</param>
    /// <param name="transaction">The existing transaction</param>
    /// <param name="listing">The <see cref="Listing"/> to upsert <see cref="ListingOrder"/>s for</param>
    /// <param name="orderWriterDelegate">Delegate for writing a <see cref="Listing"/>'s <see cref="ListingOrder"/>s to the Npgsql binary importer</param>
    /// <param name="cancellationToken">A <see cref="CancellationToken" /> to observe while waiting for the task to complete</param>
    private async Task BulkUpsertOrders(NpgsqlConnection connection, NpgsqlTransaction transaction, Listing listing,
        Action<Listing, NpgsqlBinaryImporter> orderWriterDelegate, CancellationToken cancellationToken)
    {
        // Create a temporary table to insert the Listing's orders into
        await using var tempTableCmd = new NpgsqlCommand(ListingOrdersCreateTempTableCommand, connection, transaction);
        await tempTableCmd.ExecuteNonQueryAsync(cancellationToken);

        // COPY the Listing's orders into the temporary table using a binary import
        await using var importer =
            await connection.BeginBinaryImportAsync(ListingOrdersBinaryCopyCommand, cancellationToken);
        orderWriterDelegate(listing, importer);
        await importer.CompleteAsync(cancellationToken);
        await importer.CloseAsync(cancellationToken);

        // Bulk update the actual orders table with data from the temporary table
        await using var bulkUpdateCmd = new NpgsqlCommand(ListingOrdersBulkUpdateCommand, connection, transaction);
        await bulkUpdateCmd.ExecuteNonQueryAsync(cancellationToken);

        // Remove the temporary table
        await using var dropTempTableCmd =
            new NpgsqlCommand(ListingOrdersDropTempTableCommand, connection, transaction);
        await dropTempTableCmd.ExecuteNonQueryAsync(cancellationToken);
    }

    /// <summary>
    /// Delegate for writing a <see cref="Listing"/>'s <see cref="ListingHistoricalPrice"/>s to the Npgsql binary
    /// importer
    /// </summary>
    private readonly Action<Listing, NpgsqlBinaryImporter> _historicalPriceWriterDelegate = (listing, importer) =>
    {
        foreach (var historicalPrice in listing.HistoricalPrices)
        {
            importer.StartRow();
            importer.Write(listing.Id, NpgsqlDbType.Uuid);
            importer.Write(historicalPrice.Date, NpgsqlDbType.Date);
            importer.Write(historicalPrice.BuyPrice.Value, NpgsqlDbType.Integer);
            importer.Write(historicalPrice.SellPrice.Value, NpgsqlDbType.Integer);
        }
    };

    /// <summary>
    /// Delegate for writing a <see cref="Listing"/>'s <see cref="ListingOrder"/>s to the Npgsql binary importer
    /// </summary>
    private readonly Action<Listing, NpgsqlBinaryImporter> _orderWriterDelegate = (listing, importer) =>
    {
        foreach (var orderGroup in listing.Orders.GroupBy(x => new { x.Date, x.Price.Value }))
        {
            // Get the order with the highest quantity
            var order = orderGroup.OrderByDescending(x => x.Quantity.Value).First();
            // Generate a hash
            var hashBytes =
                MD5.HashData(Encoding.UTF8.GetBytes($"{listing.Id:N}-{order.Date:yyyyMMddHHmm}-{order.Price.Value}"));
            var hashHex = Convert.ToHexString(hashBytes);

            importer.StartRow();
            importer.Write(hashHex, NpgsqlDbType.Text);
            importer.Write(listing.Id, NpgsqlDbType.Uuid);
            importer.Write(order.Date, NpgsqlDbType.TimestampTz);
            importer.Write(order.Price.Value, NpgsqlDbType.Integer);
            importer.Write(order.Quantity.Value, NpgsqlDbType.Integer);
        }
    };

    /// <summary>
    /// Inserts a new <see cref="Listing"/>
    /// </summary>
    private const string ListingsInsertCommand = $@"
        INSERT INTO {Constants.Schema}.{Constants.Listings.TableName} (
            {Constants.Listings.Id},
            {Constants.Listings.CardExternalId},
            {Constants.Listings.BuyPrice},
            {Constants.Listings.SellPrice}
        )
        VALUES ($1, $2, $3, $4)
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
    /// Name of the temporary table for <see cref="ListingHistoricalPrice"/> for bulk binary imports
    /// </summary>
    private const string ListingHistoricalPricesTempTableName = $"{Constants.ListingHistoricalPrices.TableName}_TEMP";

    /// <summary>
    /// Creates a temporary table for <see cref="ListingHistoricalPrice"/> for a bulk binary import
    /// </summary>
    private const string ListingHistoricalPricesCreateTempTableCommand = $@"
        CREATE TABLE {Constants.Schema}.{ListingHistoricalPricesTempTableName}
        (
            {Constants.ListingHistoricalPrices.ListingId}          uuid    not null,
            {Constants.ListingHistoricalPrices.Date}               date    not null,
            {Constants.ListingHistoricalPrices.BuyPrice}           integer not null,
            {Constants.ListingHistoricalPrices.SellPrice}          integer not null,
            primary key ({Constants.ListingHistoricalPrices.ListingId}, {Constants.ListingHistoricalPrices.Date})
        );";

    /// <summary>
    /// Drops the temporary table for <see cref="ListingHistoricalPrice"/>
    /// </summary>
    private const string ListingHistoricalPricesDropTempTableCommand = $@"
        DROP TABLE {Constants.Schema}.{ListingHistoricalPricesTempTableName}";

    /// <summary>
    /// Bulk binary import for <see cref="ListingHistoricalPrice"/>
    /// </summary>
    private const string ListingHistoricalPricesBinaryCopyCommand = $@"
        COPY {Constants.Schema}.{ListingHistoricalPricesTempTableName} (
            {Constants.ListingHistoricalPrices.ListingId},
            {Constants.ListingHistoricalPrices.Date},
            {Constants.ListingHistoricalPrices.BuyPrice},
            {Constants.ListingHistoricalPrices.SellPrice}
        )
        FROM STDIN (FORMAT BINARY)";

    /// <summary>
    /// Bulk update for the <see cref="ListingHistoricalPrice"/> table using the temporary table as the source
    /// </summary>
    private const string ListingHistoricalPricesBulkUpdateCommand = $@"
        INSERT INTO {Constants.Schema}.{Constants.ListingHistoricalPrices.TableName} (
            {Constants.ListingHistoricalPrices.ListingId},
            {Constants.ListingHistoricalPrices.Date},
            {Constants.ListingHistoricalPrices.BuyPrice},
            {Constants.ListingHistoricalPrices.SellPrice}
        )
        SELECT
            {Constants.ListingHistoricalPrices.ListingId},
            {Constants.ListingHistoricalPrices.Date},
            {Constants.ListingHistoricalPrices.BuyPrice},
            {Constants.ListingHistoricalPrices.SellPrice}
        FROM {Constants.Schema}.{ListingHistoricalPricesTempTableName}
        ON CONFLICT ON CONSTRAINT {Constants.ListingHistoricalPrices.Keys.PrimaryKey} DO UPDATE
        SET {Constants.ListingHistoricalPrices.BuyPrice} = EXCLUDED.{Constants.ListingHistoricalPrices.BuyPrice},
            {Constants.ListingHistoricalPrices.SellPrice} = EXCLUDED.{Constants.ListingHistoricalPrices.SellPrice};";

    /// <summary>
    /// Name of the temporary table for <see cref="ListingOrder"/> for bulk binary imports
    /// </summary>
    private const string ListingOrdersTempTableName = $"{Constants.ListingOrders.TableName}_TEMP";

    /// <summary>
    /// Creates a temporary table for <see cref="ListingOrder"/> for a bulk binary import
    /// </summary>
    private const string ListingOrdersCreateTempTableCommand = $@"
        CREATE TABLE {Constants.Schema}.{ListingOrdersTempTableName}
        (
            {Constants.ListingOrders.Hash}               text                        not null,
            {Constants.ListingOrders.ListingId}          uuid                        not null,
            {Constants.ListingOrders.Date}               timestamp with time zone    not null,
            {Constants.ListingOrders.Price}              integer                     not null,
            {Constants.ListingOrders.Quantity}           integer                     not null,
            primary key ({Constants.ListingOrders.Hash})
        );";

    /// <summary>
    /// Drops the temporary table for <see cref="ListingOrder"/>
    /// </summary>
    private const string ListingOrdersDropTempTableCommand =
        $"DROP TABLE {Constants.Schema}.{ListingOrdersTempTableName}";

    /// <summary>
    /// Bulk binary import for <see cref="ListingOrder"/>
    /// </summary>
    private const string ListingOrdersBinaryCopyCommand = $@"
        COPY {Constants.Schema}.{ListingOrdersTempTableName} (
            {Constants.ListingOrders.Hash},
            {Constants.ListingOrders.ListingId},
            {Constants.ListingOrders.Date},
            {Constants.ListingOrders.Price},
            {Constants.ListingOrders.Quantity}
        )
        FROM STDIN (FORMAT BINARY)";

    /// <summary>
    /// Bulk update for the <see cref="ListingOrder"/> table using the temporary table as the source
    /// </summary>
    private const string ListingOrdersBulkUpdateCommand = $@"
        INSERT INTO {Constants.Schema}.{Constants.ListingOrders.TableName} (
            {Constants.ListingOrders.Hash},
            {Constants.ListingOrders.ListingId},
            {Constants.ListingOrders.Date},
            {Constants.ListingOrders.Price},
            {Constants.ListingOrders.Quantity}
        )
        SELECT
            {Constants.ListingOrders.Hash},
            {Constants.ListingOrders.ListingId},
            {Constants.ListingOrders.Date},
            {Constants.ListingOrders.Price},
            {Constants.ListingOrders.Quantity}
        FROM {Constants.Schema}.{ListingOrdersTempTableName}
        ON CONFLICT ON CONSTRAINT {Constants.ListingOrders.Keys.PrimaryKey} DO UPDATE
        SET {Constants.ListingOrders.Quantity} = EXCLUDED.{Constants.ListingOrders.Quantity};";
}