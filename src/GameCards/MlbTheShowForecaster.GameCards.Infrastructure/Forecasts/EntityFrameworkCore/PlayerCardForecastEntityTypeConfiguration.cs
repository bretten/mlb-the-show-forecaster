﻿using System.ComponentModel;
using com.brettnamba.MlbTheShowForecaster.Common.Domain.Enums;
using com.brettnamba.MlbTheShowForecaster.Common.Domain.ValueObjects;
using com.brettnamba.MlbTheShowForecaster.Common.Extensions;
using com.brettnamba.MlbTheShowForecaster.GameCards.Domain.Cards.ValueObjects;
using com.brettnamba.MlbTheShowForecaster.GameCards.Domain.Forecasts.Entities;
using com.brettnamba.MlbTheShowForecaster.GameCards.Domain.Forecasts.ValueObjects;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace com.brettnamba.MlbTheShowForecaster.GameCards.Infrastructure.Forecasts.EntityFrameworkCore;

/// <summary>
/// Configures <see cref="PlayerCardForecast"/> for EF Core
/// </summary>
public sealed class PlayerCardForecastEntityTypeConfiguration : IEntityTypeConfiguration<PlayerCardForecast>
{
    /// <summary>
    /// Configures <see cref="PlayerCardForecast"/> for EF Core
    /// </summary>
    /// <param name="builder">The builder that configures the entity type</param>
    public void Configure(EntityTypeBuilder<PlayerCardForecast> builder)
    {
        builder.ToTable(Constants.PlayerCardForecasts.TableName, Constants.Schema);

        builder.HasKey(e => e.Id)
            .HasName(Constants.PlayerCardForecasts.Keys.PrimaryKey);

        // Unique constraint and index for Year and External ID
        builder.HasAlternateKey(nameof(PlayerCardForecast.Year), nameof(PlayerCardForecast.CardExternalId))
            .HasName(Constants.PlayerCardForecasts.Keys.YearAndExternalId);
        // Index for querying by game year and then the MLB ID
        builder.HasIndex(e => new { e.Year, e.MlbId }, Constants.PlayerCardForecasts.Indexes.YearAndMlbId)
            .IsUnique();

        var columnOrder = 0;

        builder.Property(e => e.Id)
            .IsRequired()
            .HasColumnType("uuid")
            .HasColumnName(Constants.PlayerCardForecasts.Id)
            .HasColumnOrder(columnOrder++);

        builder.Property(e => e.Year)
            .IsRequired()
            .HasColumnType("smallint")
            .HasColumnName(Constants.PlayerCardForecasts.Year)
            .HasColumnOrder(columnOrder++)
            .HasConversion(v => v.Value,
                v => SeasonYear.Create(v));

        builder.Property(e => e.CardExternalId)
            .IsRequired()
            .HasColumnType("uuid")
            .HasColumnName(Constants.PlayerCardForecasts.CardExternalId)
            .HasColumnOrder(columnOrder++)
            .HasConversion(v => v.Value,
                v => CardExternalId.Create(v));

        builder.Property(e => e.MlbId)
            .HasColumnType("integer")
            .HasColumnName(Constants.PlayerCardForecasts.PlayerMlbId)
            .HasColumnOrder(columnOrder++)
            .HasConversion(v => v != null ? v.Value : new int?(),
                v => v != null ? MlbId.Create(v.Value) : null);

        builder.Property(e => e.PrimaryPosition)
            .IsRequired()
            .HasColumnType("varchar(4)")
            .HasColumnName(Constants.PlayerCardForecasts.Position)
            .HasColumnOrder(columnOrder++)
            .HasConversion(v => v.GetDisplayName(),
                v => (Position)TypeDescriptor.GetConverter(typeof(Position)).ConvertFrom(v)!);

        builder.Property(e => e.OverallRating)
            .IsRequired()
            .HasColumnType("smallint")
            .HasColumnName(Constants.PlayerCardForecasts.OverallRating)
            .HasColumnOrder(columnOrder++)
            .HasConversion(v => v.Value,
                v => OverallRating.Create(v));

        // Ignore these properties. They are not relationships/navigation properties, but just convenience methods for other members of the class
        builder.Ignore(x => x.ForecastImpactsChronologically);

        // Has many ForecastImpacts
        builder.HasMany<ForecastImpact>(Constants.PlayerCardForecasts.Relationships.ForecastImpactsField)
            .WithOne()
            .HasForeignKey(Constants.ForecastImpacts.PlayerCardForecastId)
            .HasConstraintName(Constants.ForecastImpacts.Keys.PlayerCardForecastsForeignKeyConstraint)
            .IsRequired();
    }
}