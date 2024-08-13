using com.brettnamba.MlbTheShowForecaster.GameCards.Domain;
using com.brettnamba.MlbTheShowForecaster.GameCards.Domain.Forecasts.Entities;
using Microsoft.EntityFrameworkCore;

namespace com.brettnamba.MlbTheShowForecaster.GameCards.Infrastructure.Forecasts.EntityFrameworkCore;

/// <summary>
/// DB context for <see cref="PlayerCardForecast"/>
/// </summary>
public sealed class ForecastsDbContext : DbContext, IForecastWork
{
    /// <summary>
    /// Constructor
    /// </summary>
    /// <param name="options">DB Context options</param>
    public ForecastsDbContext(DbContextOptions<ForecastsDbContext> options) : base(options)
    {
    }

    /// <summary>
    /// DB set for <see cref="PlayerCardForecast"/>
    /// </summary>
    public DbSet<PlayerCardForecast> PlayerCardForecasts { get; private init; } = null!;

    /// <summary>
    /// Model configuration for <see cref="PlayerCardForecast"/>
    /// </summary>
    /// <param name="modelBuilder">Model builder</param>
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.HasDefaultSchema(Constants.Schema);
        modelBuilder.ApplyConfiguration(new PlayerCardForecastEntityTypeConfiguration());
        modelBuilder.ApplyConfiguration(new ForecastImpactEntityTypeConfiguration());
    }
}