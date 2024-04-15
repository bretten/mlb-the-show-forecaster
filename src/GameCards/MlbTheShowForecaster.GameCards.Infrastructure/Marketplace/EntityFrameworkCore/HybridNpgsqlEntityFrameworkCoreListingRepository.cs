using System.Data;
using System.Data.Common;
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
/// Hybrid implementation of <see cref="IListingRepository"/> that uses EF Core directly but also uses Npgsql for
/// transactions and binary imports
/// </summary>
public sealed class HybridNpgsqlEntityFrameworkCoreListingRepository : IListingRepository
{
    /// <summary>
    /// The DB context for <see cref="Listing"/>
    /// </summary>
    private readonly MarketplaceDbContext _dbContext;

    /// <summary>
    /// Data source for getting Npgsql connections
    /// </summary>
    private readonly NpgsqlDataSource _dbDataSource;

    /// <summary>
    /// Constructor
    /// </summary>
    /// <param name="dbContext">The DB context for <see cref="Listing"/></param>
    /// <param name="dbDataSource">Data source for getting Npgsql connections</param>
    /// <exception cref="InvalidNpgsqlDataSourceForListingRepositoryException">Thrown when the <see cref="NpgsqlDataSource"/> is invalid</exception>
    public HybridNpgsqlEntityFrameworkCoreListingRepository(MarketplaceDbContext dbContext, DbDataSource dbDataSource)
    {
        _dbContext = dbContext;
        _dbDataSource = dbDataSource as NpgsqlDataSource ??
                        throw new InvalidNpgsqlDataSourceForListingRepositoryException(
                            $"No valid Npgsql datasource provided for {nameof(HybridNpgsqlEntityFrameworkCoreListingRepository)}");
    }

    /// <summary>
    /// Adds a <see cref="Listing"/>
    /// </summary>
    /// <param name="listing">The <see cref="Listing"/> to add</param>
    /// <param name="cancellationToken">A <see cref="CancellationToken" /> to observe while waiting for the task to complete</param>
    public async Task Add(Listing listing, CancellationToken cancellationToken = default)
    {
        // Open a connection and transaction
        await using var connection = await _dbDataSource.OpenConnectionAsync(cancellationToken);
        await using var transaction = await connection.BeginTransactionAsync(cancellationToken);

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

        // Commit
        await transaction.CommitAsync(cancellationToken);
    }

    /// <summary>
    /// Updates a <see cref="Listing"/>
    /// </summary>
    /// <param name="listing">The <see cref="Listing"/> to update</param>
    /// <param name="cancellationToken">A <see cref="CancellationToken" /> to observe while waiting for the task to complete</param>
    public async Task Update(Listing listing, CancellationToken cancellationToken = default)
    {
        // Open a connection and transaction
        await using var connection = await _dbDataSource.OpenConnectionAsync(cancellationToken);
        await using var transaction = await connection.BeginTransactionAsync(cancellationToken);

        // UPDATE the Listing
        await using var command = new NpgsqlCommand(ListingsUpdateCommand, connection, transaction);
        command.Parameters.Add(new NpgsqlParameter { Value = listing.Id, DbType = DbType.Guid });
        command.Parameters.Add(new NpgsqlParameter { Value = listing.BuyPrice.Value, DbType = DbType.Int32 });
        command.Parameters.Add(new NpgsqlParameter { Value = listing.SellPrice.Value, DbType = DbType.Int32 });
        await command.ExecuteScalarAsync(cancellationToken);

        // Bulk upsert the Listing's historical prices
        await BulkUpsertHistoricalPrices(connection, transaction, listing, _historicalPriceWriterDelegate,
            cancellationToken);

        // Commit
        await transaction.CommitAsync(cancellationToken);
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
        return await _dbContext.ListingsWithHistoricalPrices().FirstOrDefaultAsync(x => x.CardExternalId == externalId,
            cancellationToken: cancellationToken);
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
    /// Delegate for writing a <see cref="Listing"/>'s <see cref="ListingHistoricalPrice"/>s to the Npgsql binary
    /// importer
    /// </summary>
    private readonly Action<Listing, NpgsqlBinaryImporter> _historicalPriceWriterDelegate = (listing, importer) =>
    {
        foreach (var historicalPrice in listing.HistoricalPricesChronologically)
        {
            importer.StartRow();
            importer.Write(listing.Id, NpgsqlDbType.Uuid);
            importer.Write(historicalPrice.Date, NpgsqlDbType.Date);
            importer.Write(historicalPrice.BuyPrice.Value, NpgsqlDbType.Integer);
            importer.Write(historicalPrice.SellPrice.Value, NpgsqlDbType.Integer);
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
}