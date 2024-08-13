using com.brettnamba.MlbTheShowForecaster.GameCards.Domain.Cards.ValueObjects;
using com.brettnamba.MlbTheShowForecaster.GameCards.Domain.Forecasts.ValueObjects;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace com.brettnamba.MlbTheShowForecaster.GameCards.Infrastructure.Forecasts.EntityFrameworkCore.DerivedEntities;

/// <summary>
/// Configures <see cref="OverallRatingChangeForecastImpact"/> for EF Core
/// </summary>
public sealed class
    OverallRatingChangeForecastImpactEntityTypeConfiguration : IEntityTypeConfiguration<
    OverallRatingChangeForecastImpact>
{
    /// <summary>
    /// Configures <see cref="OverallRatingChangeForecastImpact"/> for EF Core
    /// </summary>
    /// <param name="builder">The builder that configures the entity type</param>
    public void Configure(EntityTypeBuilder<OverallRatingChangeForecastImpact> builder)
    {
        builder.Property(e => e.OldRating)
            .IsRequired()
            .HasColumnType("smallint")
            .HasColumnName(Constants.PlayerCardForecasts.OverallRating)
            .HasConversion(v => v.Value,
                v => OverallRating.Create(v));

        builder.Property(e => e.NewRating)
            .IsRequired()
            .HasColumnType("smallint")
            .HasColumnName(Constants.PlayerCardForecasts.OverallRating)
            .HasConversion(v => v.Value,
                v => OverallRating.Create(v));
    }
}