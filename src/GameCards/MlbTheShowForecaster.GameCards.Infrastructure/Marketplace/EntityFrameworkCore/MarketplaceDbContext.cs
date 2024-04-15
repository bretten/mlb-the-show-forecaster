using com.brettnamba.MlbTheShowForecaster.GameCards.Domain.Marketplace.Entities;
using com.brettnamba.MlbTheShowForecaster.GameCards.Domain.Marketplace.ValueObjects;
using Microsoft.EntityFrameworkCore;

namespace com.brettnamba.MlbTheShowForecaster.GameCards.Infrastructure.Marketplace.EntityFrameworkCore;

/// <summary>
/// DB context for <see cref="Listing"/>
/// </summary>
public sealed class MarketplaceDbContext : DbContext
{
    /// <summary>
    /// Constructor
    /// </summary>
    /// <param name="options">DB Context options</param>
    public MarketplaceDbContext(DbContextOptions<MarketplaceDbContext> options) : base(options)
    {
    }

    /// <summary>
    /// DB set for <see cref="Listing"/>
    /// </summary>
    public DbSet<Listing> Listings { get; private init; } = null!;

    /// <summary>
    /// DB set for <see cref="ListingHistoricalPrice"/>
    /// </summary>
    public DbSet<ListingHistoricalPrice> ListingHistoricalPrices { get; private init; } = null!;

    /// <summary>
    /// Model configuration for <see cref="Listing"/>
    /// </summary>
    /// <param name="modelBuilder">Model builder</param>
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.HasDefaultSchema(Constants.Schema);
        modelBuilder.ApplyConfiguration(new ListingEntityTypeConfiguration());
        modelBuilder.ApplyConfiguration(new ListingHistoricalPriceEntityTypeConfiguration());
    }
}