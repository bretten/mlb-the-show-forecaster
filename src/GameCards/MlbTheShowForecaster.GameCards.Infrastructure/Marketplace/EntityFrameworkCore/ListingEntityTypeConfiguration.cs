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

        // Index for querying by the card's external ID
        builder.HasIndex(e => e.CardExternalId, Constants.Listings.Indexes.ExternalId)
            .HasMethod("btree");

        var columnOrder = 0;

        builder.Property(e => e.Id)
            .IsRequired()
            .HasColumnType("uuid")
            .HasColumnName(Constants.Listings.Id)
            .HasColumnOrder(columnOrder++);

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

        // Relation is defined on ListingHistoricalPrice end
        // builder.HasMany<ListingHistoricalPrice>("_historicalPrices")
        //     .WithOne()
        //     .HasForeignKey(Constants.ListingHistoricalPrices.ListingId)
        //     .IsRequired();
    }
}