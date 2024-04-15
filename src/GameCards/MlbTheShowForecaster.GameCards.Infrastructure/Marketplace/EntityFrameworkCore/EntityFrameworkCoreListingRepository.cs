using System.Data;
using System.Data.Common;
using com.brettnamba.MlbTheShowForecaster.GameCards.Domain.Cards.ValueObjects;
using com.brettnamba.MlbTheShowForecaster.GameCards.Domain.Marketplace.Entities;
using com.brettnamba.MlbTheShowForecaster.GameCards.Domain.Marketplace.Repositories;
using Microsoft.EntityFrameworkCore;
using Npgsql;
using NpgsqlTypes;

namespace com.brettnamba.MlbTheShowForecaster.GameCards.Infrastructure.Marketplace.EntityFrameworkCore;

public sealed class EntityFrameworkCoreListingRepository : IListingRepository
{
    private readonly MarketplaceDbContext _dbContext;
    private readonly NpgsqlDataSource _dbDataSource;

    public EntityFrameworkCoreListingRepository(MarketplaceDbContext dbContext, DbDataSource dbDataSource)
    {
        _dbContext = dbContext;
        _dbDataSource = dbDataSource as NpgsqlDataSource ??
                        throw new ArgumentException(
                            $"No Npgsql datasource provided for {nameof(EntityFrameworkCoreListingRepository)}");
    }

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
        await using var insertCmd = new NpgsqlCommand(LISTINGS_INSERT_COMMAND, connection, transaction);
        insertCmd.Parameters.Add(new NpgsqlParameter { Value = listing.Id, DbType = DbType.Guid });
        insertCmd.Parameters.Add(new NpgsqlParameter { Value = listing.CardExternalId.Value, DbType = DbType.Guid });
        insertCmd.Parameters.Add(new NpgsqlParameter { Value = listing.BuyPrice.Value, DbType = DbType.Int32 });
        insertCmd.Parameters.Add(new NpgsqlParameter { Value = listing.SellPrice.Value, DbType = DbType.Int32 });
        await insertCmd.ExecuteScalarAsync(cancellationToken);

        // Create a temporary table to insert the Listing's historical prices into
        await using var tempTableCmd =
            new NpgsqlCommand(LISTING_HISTORICAL_PRICES_CREATE_TEMP_TABLE_COMMAND, connection, transaction);
        await tempTableCmd.ExecuteNonQueryAsync(cancellationToken);

        // COPY the Listing's historical prices into the temporary table using a binary import
        await using var importer =
            await connection.BeginBinaryImportAsync(LISTING_HISTORICAL_PRICES_BINARY_COPY_COMMAND, cancellationToken);
        historicalPriceWriterDelegate(listing, importer);
        await importer.CompleteAsync(cancellationToken);
        await importer.CloseAsync(cancellationToken);

        // Merge the temporary table historical prices into the actual historical prices table
        await using var mergeCmd = new NpgsqlCommand(LISTING_HISTORICAL_PRICES_MERGE_COMMAND, connection, transaction);
        await mergeCmd.ExecuteNonQueryAsync(cancellationToken);

        // Remove the temporary table
        await using var dropTempTableCmd =
            new NpgsqlCommand(LISTING_HISTORICAL_PRICES_DROP_TEMP_TABLE_COMMAND, connection, transaction);
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
        await using var updateCmd = new NpgsqlCommand(LISTINGS_UPDATE_COMMAND, connection, transaction);
        updateCmd.Parameters.Add(new NpgsqlParameter { Value = listing.Id, DbType = DbType.Guid });
        updateCmd.Parameters.Add(new NpgsqlParameter { Value = listing.BuyPrice.Value, DbType = DbType.Int32 });
        updateCmd.Parameters.Add(new NpgsqlParameter { Value = listing.SellPrice.Value, DbType = DbType.Int32 });
        await updateCmd.ExecuteScalarAsync(cancellationToken);

        // Create a temporary table to insert the Listing's historical prices into
        await using var tempTableCmd =
            new NpgsqlCommand(LISTING_HISTORICAL_PRICES_CREATE_TEMP_TABLE_COMMAND, connection, transaction);
        await tempTableCmd.ExecuteNonQueryAsync(cancellationToken);

        // COPY the Listing's historical prices into the temporary table using a binary import
        await using var importer =
            await connection.BeginBinaryImportAsync(LISTING_HISTORICAL_PRICES_BINARY_COPY_COMMAND, cancellationToken);
        historicalPriceWriterDelegate(listing, importer);
        await importer.CompleteAsync(cancellationToken);
        await importer.CloseAsync(cancellationToken);

        // Merge the temporary table historical prices into the actual historical prices table
        await using var mergeCmd = new NpgsqlCommand(LISTING_HISTORICAL_PRICES_MERGE_COMMAND, connection, transaction);
        await mergeCmd.ExecuteNonQueryAsync(cancellationToken);

        // Remove the temporary table
        await using var dropTempTableCmd =
            new NpgsqlCommand(LISTING_HISTORICAL_PRICES_DROP_TEMP_TABLE_COMMAND, connection, transaction);
        await dropTempTableCmd.ExecuteNonQueryAsync(cancellationToken);

        await transaction.CommitAsync(cancellationToken);
    }

    private static readonly string LISTINGS_INSERT_COMMAND = $@"
        INSERT INTO {Constants.Schema}.{Constants.Listings.TableName} (
                {Constants.Listings.Id},
                {Constants.Listings.CardExternalId},
                {Constants.Listings.BuyPrice},
                {Constants.Listings.SellPrice}
        )
        VALUES ($1, $2, $3, $4)
        RETURNING {Constants.Listings.Id}";

    private static readonly string LISTINGS_UPDATE_COMMAND = $@"
        UPDATE {Constants.Schema}.{Constants.Listings.TableName}
        SET (
                {Constants.Listings.BuyPrice},
                {Constants.Listings.SellPrice}
        )
        = ($2, $3)
        WHERE {Constants.Listings.Id} = $1";

    private static readonly string LISTING_HISTORICAL_PRICES_CREATE_TEMP_TABLE_COMMAND = $@"
        create table {Constants.Schema}.{Constants.ListingHistoricalPrices.TableName}_temp
        (
            listing_id uuid    not null,
            date       date    not null,
            buy_price  integer not null,
            sell_price integer not null,
            primary key (listing_id, date)
        );
    ";

    private static readonly string LISTING_HISTORICAL_PRICES_DROP_TEMP_TABLE_COMMAND = $@"
        drop table {Constants.Schema}.{Constants.ListingHistoricalPrices.TableName}_temp
    ";

    private static readonly string LISTING_HISTORICAL_PRICES_BINARY_COPY_COMMAND = $@"
            COPY {Constants.Schema}.{Constants.ListingHistoricalPrices.TableName}_temp (
                {Constants.ListingHistoricalPrices.ListingId},
                {Constants.ListingHistoricalPrices.Date},
                {Constants.ListingHistoricalPrices.BuyPrice},
                {Constants.ListingHistoricalPrices.SellPrice}
            )
            FROM STDIN (FORMAT BINARY)";

    private static readonly string LISTING_HISTORICAL_PRICES_MERGE_COMMAND = $@"
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
            FROM {Constants.Schema}.{Constants.ListingHistoricalPrices.TableName}_temp
            ON CONFLICT ON CONSTRAINT {Constants.ListingHistoricalPrices.Keys.PrimaryKey} DO UPDATE
            SET {Constants.ListingHistoricalPrices.BuyPrice} = EXCLUDED.{Constants.ListingHistoricalPrices.BuyPrice},
                {Constants.ListingHistoricalPrices.SellPrice} = EXCLUDED.{Constants.ListingHistoricalPrices.SellPrice};
        ";
}