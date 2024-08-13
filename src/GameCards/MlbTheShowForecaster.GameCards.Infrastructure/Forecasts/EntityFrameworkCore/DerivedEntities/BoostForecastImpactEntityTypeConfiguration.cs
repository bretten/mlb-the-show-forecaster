using com.brettnamba.MlbTheShowForecaster.GameCards.Domain.Forecasts.ValueObjects;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace com.brettnamba.MlbTheShowForecaster.GameCards.Infrastructure.Forecasts.EntityFrameworkCore.DerivedEntities;

/// <summary>
/// Configures <see cref="BoostForecastImpact"/> for EF Core
/// </summary>
public sealed class BoostForecastImpactEntityTypeConfiguration : IEntityTypeConfiguration<BoostForecastImpact>
{
    /// <summary>
    /// Configures <see cref="BoostForecastImpact"/> for EF Core
    /// </summary>
    /// <param name="builder">The builder that configures the entity type</param>
    public void Configure(EntityTypeBuilder<BoostForecastImpact> builder)
    {
        builder.Property(e => e.BoostReason)
            .IsRequired()
            .HasColumnType("text")
            .HasColumnName(Constants.ForecastImpacts.Boost.BoostReason);
    }
}