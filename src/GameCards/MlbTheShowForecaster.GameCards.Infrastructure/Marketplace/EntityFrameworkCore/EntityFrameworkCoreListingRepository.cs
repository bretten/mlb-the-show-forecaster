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
public sealed class EntityFrameworkCoreListingRepository : IListingRepository
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
    public EntityFrameworkCoreListingRepository(MarketplaceDbContext dbContext, DbDataSource dbDataSource)
    {
        _dbContext = dbContext;
        _dbDataSource = dbDataSource as NpgsqlDataSource ??
                        throw new InvalidNpgsqlDataSourceForListingRepositoryException(
                            $"No valid Npgsql datasource provided for {nameof(EntityFrameworkCoreListingRepository)}");
    }

    /// <summary>
    /// Adds a <see cref="Listing"/>
    /// </summary>
    /// <param name="listing">The <see cref="Listing"/> to add</param>
    public async Task Add(Listing listing)
    {
        await BulkInsert(listing, (listingToAdd, importer) =>
        {
            foreach (var historicalPrice in listing.HistoricalPricesChronologically)
            {
                importer.StartRow();
                importer.Write(listingToAdd.Id, NpgsqlDbType.Uuid);
                importer.Write(historicalPrice.Date, NpgsqlDbType.Date);
                importer.Write(historicalPrice.BuyPrice.Value, NpgsqlDbType.Integer);
                importer.Write(historicalPrice.SellPrice.Value, NpgsqlDbType.Integer);
            }
        }, CancellationToken.None);
    }

    /// <summary>
    /// Updates a <see cref="Listing"/>
    /// </summary>
    /// <param name="listing">The <see cref="Listing"/> to update</param>
    public async Task Update(Listing listing)
    {
        await BulkUpsert(listing, (listingToUpdate, importer) =>
        {
            foreach (var historicalPrice in listing.HistoricalPricesChronologically)
            {
                importer.StartRow();
                importer.Write(listingToUpdate.Id, NpgsqlDbType.Uuid);
                importer.Write(historicalPrice.Date, NpgsqlDbType.Date);
                importer.Write(historicalPrice.BuyPrice.Value, NpgsqlDbType.Integer);
                importer.Write(historicalPrice.SellPrice.Value, NpgsqlDbType.Integer);
            }
        }, CancellationToken.None);
    }

    /// <summary>
    /// Returns a <see cref="Listing"/> for the specified <see cref="CardExternalId"/>
    /// </summary>
    /// <param name="externalId">The <see cref="CardExternalId"/> of the <see cref="Listing"/></param>
    /// <returns>The corresponding <see cref="Listing"/></returns>
    public async Task<Listing?> GetByExternalId(CardExternalId externalId)
    {
        return await _dbContext.ListingsWithHistoricalPrices()
            .FirstOrDefaultAsync(x => x.CardExternalId == externalId);
    }

    private async Task BulkInsert(Listing listing, Action<Listing, NpgsqlBinaryImporter> historicalPriceWriterDelegate,
        CancellationToken cancellationToken)
    {
        // Open a connection and transaction
        await using var connection = await _dbDataSource.OpenConnectionAsync(cancellationToken);
        await using var transaction = await connection.BeginTransactionAsync(cancellationToken);

        // Insert the Listing
        await using var insertCmd = new NpgsqlCommand(ListingsInsertCommand, connection, transaction);
        insertCmd.Parameters.Add(new NpgsqlParameter { Value = listing.Id, DbType = DbType.Guid });
        insertCmd.Parameters.Add(new NpgsqlParameter { Value = listing.CardExternalId.Value, DbType = DbType.Guid });
        insertCmd.Parameters.Add(new NpgsqlParameter { Value = listing.BuyPrice.Value, DbType = DbType.Int32 });
        insertCmd.Parameters.Add(new NpgsqlParameter { Value = listing.SellPrice.Value, DbType = DbType.Int32 });
        await insertCmd.ExecuteScalarAsync(cancellationToken);

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

        await transaction.CommitAsync(cancellationToken);
    }

    private async Task BulkUpsert(Listing listing, Action<Listing, NpgsqlBinaryImporter> historicalPriceWriterDelegate,
        CancellationToken cancellationToken)
    {
        // Open a connection and transaction
        await using var connection = await _dbDataSource.OpenConnectionAsync(cancellationToken);
        await using var transaction = await connection.BeginTransactionAsync(cancellationToken);

        // Insert the Listing
        await using var updateCmd = new NpgsqlCommand(ListingsUpdateCommand, connection, transaction);
        updateCmd.Parameters.Add(new NpgsqlParameter { Value = listing.Id, DbType = DbType.Guid });
        updateCmd.Parameters.Add(new NpgsqlParameter { Value = listing.BuyPrice.Value, DbType = DbType.Int32 });
        updateCmd.Parameters.Add(new NpgsqlParameter { Value = listing.SellPrice.Value, DbType = DbType.Int32 });
        await updateCmd.ExecuteScalarAsync(cancellationToken);

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

        await transaction.CommitAsync(cancellationToken);
    }

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