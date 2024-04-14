using com.brettnamba.MlbTheShowForecaster.Common.Domain.ValueObjects;
using com.brettnamba.MlbTheShowForecaster.GameCards.Domain.Marketplace.Entities;
using com.brettnamba.MlbTheShowForecaster.GameCards.Domain.Marketplace.ValueObjects;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace com.brettnamba.MlbTheShowForecaster.GameCards.Infrastructure.Marketplace.EntityFrameworkCore;

/// <summary>
/// Configures <see cref="ListingHistoricalPrice"/> for EF Core
/// </summary>
public sealed class ListingHistoricalPriceEntityTypeConfiguration : IEntityTypeConfiguration<ListingHistoricalPrice>
{
    /// <summary>
    /// Configures <see cref="ListingHistoricalPrice"/> for EF Core
    /// </summary>
    /// <param name="builder">The builder that configures the entity type</param>
    public void Configure(EntityTypeBuilder<ListingHistoricalPrice> builder)
    {
        builder.ToTable(Constants.ListingHistoricalPrices.TableName, Constants.Schema);

        builder.HasOne<Listing>()
            .WithMany("_historicalPrices")
            .HasForeignKey(Constants.ListingHistoricalPrices.ListingId)
            .HasConstraintName(Constants.ListingHistoricalPrices.Keys.ListingsForeignKeyConstraint);

        builder.HasKey([Constants.ListingHistoricalPrices.ListingId, nameof(ListingHistoricalPrice.Date)])
            .HasName(Constants.ListingHistoricalPrices.Keys.PrimaryKey);

        var columnOrder = 0;

        builder.Property(Constants.ListingHistoricalPrices.ListingId)
            .HasColumnOrder(columnOrder++);

        builder.Property(e => e.Date)
            .IsRequired()
            .HasColumnType("date")
            .HasColumnName(Constants.ListingHistoricalPrices.Date)
            .HasColumnOrder(columnOrder++);

        builder.Property(e => e.BuyPrice)
            .IsRequired()
            .HasColumnType("integer")
            .HasColumnName(Constants.ListingHistoricalPrices.BuyPrice)
            .HasColumnOrder(columnOrder++)
            .HasConversion(v => v.Value,
                v => NaturalNumber.Create(v));

        builder.Property(e => e.SellPrice)
            .IsRequired()
            .HasColumnType("integer")
            .HasColumnName(Constants.ListingHistoricalPrices.SellPrice)
            .HasColumnOrder(columnOrder++)
            .HasConversion(v => v.Value,
                v => NaturalNumber.Create(v));
    }
}