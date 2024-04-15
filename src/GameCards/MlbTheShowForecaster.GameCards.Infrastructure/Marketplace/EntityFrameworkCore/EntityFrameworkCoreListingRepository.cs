using System.Data;
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

    public EntityFrameworkCoreListingRepository(MarketplaceDbContext dbContext)
    {
        _dbContext = dbContext;
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

    public Task Update(Listing listing)
    {
        throw new NotImplementedException();
    }

    public async Task<Listing?> GetByExternalId(CardExternalId externalId)
    {
        return await _dbContext.ListingsWithHistoricalPrices()
            .FirstOrDefaultAsync(x => x.CardExternalId == externalId);
    }

    private async Task BulkInsert(Listing listing, Action<Listing, NpgsqlBinaryImporter> copyCommandMapper,
        CancellationToken cancellationToken)
    {
        await using var dataSource = NpgsqlDataSource.Create("");
        await using var connection = await dataSource.OpenConnectionAsync(cancellationToken);
        await using var transaction = await connection.BeginTransactionAsync(cancellationToken);

        await using var command = new NpgsqlCommand(LISTINGS_INSERT_COMMAND, connection, transaction);
        command.Parameters.Add(new NpgsqlParameter { Value = listing.Id, DbType = DbType.Guid });
        command.Parameters.Add(new NpgsqlParameter { Value = listing.CardExternalId.Value, DbType = DbType.Guid });
        command.Parameters.Add(new NpgsqlParameter { Value = listing.BuyPrice.Value, DbType = DbType.Int32 });
        command.Parameters.Add(new NpgsqlParameter { Value = listing.SellPrice.Value, DbType = DbType.Int32 });
        await command.ExecuteScalarAsync(cancellationToken);

        await using var tempTableCommand =
            new NpgsqlCommand(LISTING_HISTORICAL_PRICES_TEMP_TABLE_COMMAND, connection, transaction);
        await tempTableCommand.ExecuteNonQueryAsync(cancellationToken);

        // INSERT the new entities using a binary copy
        await using var importer =
            await connection.BeginBinaryImportAsync(LISTING_HISTORICAL_PRICES_BINARY_COPY_COMMAND, cancellationToken);
        copyCommandMapper(listing, importer);
        await importer.CompleteAsync(cancellationToken);
        await importer.CloseAsync(cancellationToken);

        await using var mergeCommand =
            new NpgsqlCommand(LISTING_HISTORICAL_PRICES_MERGE_COMMAND, connection, transaction);
        await mergeCommand.ExecuteNonQueryAsync(cancellationToken);

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

    private static readonly string LISTING_HISTORICAL_PRICES_TEMP_TABLE_COMMAND = $@"
        create table {Constants.Schema}.{Constants.ListingHistoricalPrices.TableName}_temp
        (
            listing_id uuid    not null,
            date       date    not null,
            buy_price  integer not null,
            sell_price integer not null,
            primary key (listing_id, date)
        );
    ";

    private static readonly string LISTING_HISTORICAL_PRICES_BINARY_COPY_COMMAND = $@"
            COPY {Constants.Schema}.{Constants.ListingHistoricalPrices.TableName} (
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