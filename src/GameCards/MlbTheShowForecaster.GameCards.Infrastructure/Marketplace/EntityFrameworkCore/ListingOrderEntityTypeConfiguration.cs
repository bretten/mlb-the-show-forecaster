using com.brettnamba.MlbTheShowForecaster.Common.Domain.ValueObjects;
using com.brettnamba.MlbTheShowForecaster.GameCards.Domain.Marketplace.Entities;
using com.brettnamba.MlbTheShowForecaster.GameCards.Domain.Marketplace.ValueObjects;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace com.brettnamba.MlbTheShowForecaster.GameCards.Infrastructure.Marketplace.EntityFrameworkCore;

/// <summary>
/// Configures <see cref="ListingOrder"/> for EF Core
/// </summary>
public class ListingOrderEntityTypeConfiguration : IEntityTypeConfiguration<ListingOrder>
{
    /// <summary>
    /// Configures <see cref="ListingOrder"/> for EF Core
    /// </summary>
    /// <param name="builder">The builder that configures the entity type</param>
    public void Configure(EntityTypeBuilder<ListingOrder> builder)
    {
        builder.ToTable(Constants.ListingOrders.TableName, Constants.Schema);

        builder.HasOne<Listing>()
            .WithMany(Constants.ListingOrders.FieldName)
            .HasForeignKey(Constants.ListingOrders.ListingId)
            .HasConstraintName(Constants.ListingOrders.Keys.ListingsForeignKeyConstraint);

        // Index for the foreign key relationship
        builder.HasIndex([Constants.ListingOrders.ListingId], Constants.ListingOrders.Indexes.ListingIdIndex)
            .HasMethod("btree");

        var columnOrder = 0;

        builder.Property<string>(Constants.ListingOrders.Hash)
            .IsRequired()
            .HasColumnType("text")
            .HasColumnName(Constants.ListingOrders.Hash)
            .HasColumnOrder(columnOrder++);

        // Must be after property declaration so it knows the type
        builder.HasKey(Constants.ListingOrders.Hash)
            .HasName(Constants.ListingOrders.Keys.PrimaryKey);

        builder.Property(Constants.ListingOrders.ListingId)
            .HasColumnOrder(columnOrder++);

        builder.Property(e => e.Date)
            .IsRequired()
            .HasColumnType("timestamp with time zone")
            .HasColumnName(Constants.ListingOrders.Date)
            .HasColumnOrder(columnOrder++);

        builder.Property(e => e.Price)
            .IsRequired()
            .HasColumnType("integer")
            .HasColumnName(Constants.ListingOrders.Price)
            .HasColumnOrder(columnOrder++)
            .HasConversion(v => v.Value,
                v => NaturalNumber.Create(v));

        builder.Property(e => e.Quantity)
            .IsRequired()
            .HasColumnType("integer")
            .HasColumnName(Constants.ListingOrders.Quantity)
            .HasColumnOrder(columnOrder++)
            .HasConversion(v => v.Value,
                v => NaturalNumber.Create(v));
    }
}