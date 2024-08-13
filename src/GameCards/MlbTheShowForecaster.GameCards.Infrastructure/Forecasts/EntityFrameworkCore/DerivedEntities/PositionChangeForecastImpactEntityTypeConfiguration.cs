using System.ComponentModel;
using com.brettnamba.MlbTheShowForecaster.Common.Domain.Enums;
using com.brettnamba.MlbTheShowForecaster.Common.Extensions;
using com.brettnamba.MlbTheShowForecaster.GameCards.Domain.Forecasts.ValueObjects;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace com.brettnamba.MlbTheShowForecaster.GameCards.Infrastructure.Forecasts.EntityFrameworkCore.DerivedEntities;

/// <summary>
/// Configures <see cref="PositionChangeForecastImpact"/> for EF Core
/// </summary>
public sealed class
    PositionChangeForecastImpactEntityTypeConfiguration : IEntityTypeConfiguration<PositionChangeForecastImpact>
{
    /// <summary>
    /// Configures <see cref="PositionChangeForecastImpact"/> for EF Core
    /// </summary>
    /// <param name="builder">The builder that configures the entity type</param>
    public void Configure(EntityTypeBuilder<PositionChangeForecastImpact> builder)
    {
        builder.Property(e => e.OldPosition)
            .IsRequired()
            .HasColumnType("varchar(4)")
            .HasColumnName(Constants.PlayerCardForecasts.Position)
            .HasConversion(v => v.GetDisplayName(),
                v => (Position)TypeDescriptor.GetConverter(typeof(Position)).ConvertFrom(v)!);

        builder.Property(e => e.NewPosition)
            .IsRequired()
            .HasColumnType("varchar(4)")
            .HasColumnName(Constants.PlayerCardForecasts.Position)
            .HasConversion(v => v.GetDisplayName(),
                v => (Position)TypeDescriptor.GetConverter(typeof(Position)).ConvertFrom(v)!);
    }
}