using com.brettnamba.MlbTheShowForecaster.Common.Domain.ValueObjects;
using com.brettnamba.MlbTheShowForecaster.GameCards.Domain.Cards.ValueObjects;
using com.brettnamba.MlbTheShowForecaster.GameCards.Domain.Marketplace.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace com.brettnamba.MlbTheShowForecaster.GameCards.Infrastructure.Marketplace.EntityFrameworkCore;

/// <summary>
/// Configures <see cref="Listing"/> for EF Core
/// </summary>
public sealed class ListingEntityTypeConfiguration : IEntityTypeConfiguration<Listing>
{
    /// <summary>
    /// Configures <see cref="Listing"/> for EF Core
    /// </summary>
    /// <param name="builder">The builder that configures the entity type</param>
    public void Configure(EntityTypeBuilder<Listing> builder)
    {
        builder.ToTable(Constants.Listings.TableName, Constants.Schema);

        builder.HasKey(e => e.Id)
            .HasName(Constants.Listings.Keys.PrimaryKey);

        // Unique constraint and index for Year and External ID
        builder.HasAlternateKey(nameof(Listing.Year), nameof(Listing.CardExternalId))
            .HasName(Constants.Listings.Keys.YearAndExternalId);

        var columnOrder = 0;

        builder.Property(e => e.Id)
            .IsRequired()
            .HasColumnType("uuid")
            .HasColumnName(Constants.Listings.Id)
            .HasColumnOrder(columnOrder++);

        builder.Property(e => e.Year)
            .IsRequired()
            .HasColumnType("smallint")
            .HasColumnName(Constants.Listings.Year)
            .HasColumnOrder(columnOrder++)
            .HasConversion(v => v.Value,
                v => SeasonYear.Create(v));

        builder.Property(e => e.CardExternalId)
            .IsRequired()
            .HasColumnType("uuid")
            .HasColumnName(Constants.Listings.CardExternalId)
            .HasColumnOrder(columnOrder++)
            .HasConversion(v => v.Value,
                v => CardExternalId.Create(v));

        builder.Property(e => e.BuyPrice)
            .IsRequired()
            .HasColumnType("integer")
            .HasColumnName(Constants.Listings.BuyPrice)
            .HasColumnOrder(columnOrder++)
            .HasConversion(v => v.Value,
                v => NaturalNumber.Create(v));

        builder.Property(e => e.SellPrice)
            .IsRequired()
            .HasColumnType("integer")
            .HasColumnName(Constants.Listings.SellPrice)
            .HasColumnOrder(columnOrder++)
            .HasConversion(v => v.Value,
                v => NaturalNumber.Create(v));

        // Ignore these properties. They are not relationships/navigation properties, but just convenience methods for other members of the class
        builder.Ignore(x => x.HistoricalPricesChronologically);
        builder.Ignore(x => x.HistoricalPrices);
        builder.Ignore(x => x.OrdersChronologically);
        builder.Ignore(x => x.Orders);

        // Relation is defined on ListingHistoricalPrice end
        // builder.HasMany<ListingHistoricalPrice>("_historicalPrices")
        //     .WithOne()
        //     .HasForeignKey(Constants.ListingHistoricalPrices.ListingId)
        //     .IsRequired();
    }
}