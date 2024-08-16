﻿using com.brettnamba.MlbTheShowForecaster.GameCards.Domain.Forecasts.ValueObjects.StatImpacts;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace com.brettnamba.MlbTheShowForecaster.GameCards.Infrastructure.Forecasts.EntityFrameworkCore.DerivedEntities;

/// <summary>
/// Configures <see cref="FieldingStatsForecastImpact"/> for EF Core
/// </summary>
public sealed class
    FieldingStatsForecastImpactEntityTypeConfiguration : IEntityTypeConfiguration<FieldingStatsForecastImpact>
{
    /// <summary>
    /// Configures <see cref="FieldingStatsForecastImpact"/> for EF Core
    /// </summary>
    /// <param name="builder">The builder that configures the entity type</param>
    public void Configure(EntityTypeBuilder<FieldingStatsForecastImpact> builder)
    {
        builder.Property(e => e.OldScore)
            .IsRequired()
            .HasColumnType("decimal(5,4)")
            .HasColumnName(Constants.ForecastImpacts.Stats.OldScore);

        builder.Property(e => e.NewScore)
            .IsRequired()
            .HasColumnType("decimal(5,4)")
            .HasColumnName(Constants.ForecastImpacts.Stats.NewScore);
    }
}