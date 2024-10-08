﻿using com.brettnamba.MlbTheShowForecaster.GameCards.Domain.Forecasts.ValueObjects;
using com.brettnamba.MlbTheShowForecaster.GameCards.Domain.Forecasts.ValueObjects.AdministrativeImpacts;
using com.brettnamba.MlbTheShowForecaster.GameCards.Domain.Forecasts.ValueObjects.StatImpacts;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace com.brettnamba.MlbTheShowForecaster.GameCards.Infrastructure.Forecasts.EntityFrameworkCore;

/// <summary>
/// Configures <see cref="ForecastImpact"/> for EF Core
/// </summary>
public sealed class ForecastImpactEntityTypeConfiguration : IEntityTypeConfiguration<ForecastImpact>
{
    /// <summary>
    /// Configures <see cref="ForecastImpact"/> for EF Core
    /// </summary>
    /// <param name="builder">The builder that configures the entity type</param>
    public void Configure(EntityTypeBuilder<ForecastImpact> builder)
    {
        builder.ToTable(Constants.ForecastImpacts.TableName, Constants.Schema);

        // Table-per-hierarchy configuration
        builder.HasDiscriminator<string>(Constants.PlayerCardForecasts.Relationships.DiscriminatorName)
            .HasValue<PlayerActivationForecastImpact>(DiscriminatorValue(typeof(PlayerActivationForecastImpact)))
            .HasValue<PlayerDeactivationForecastImpact>(DiscriminatorValue(typeof(PlayerDeactivationForecastImpact)))
            .HasValue<PlayerFreeAgencyForecastImpact>(DiscriminatorValue(typeof(PlayerFreeAgencyForecastImpact)))
            .HasValue<PlayerTeamSigningForecastImpact>(DiscriminatorValue(typeof(PlayerTeamSigningForecastImpact)))
            .HasValue<BattingStatsForecastImpact>(DiscriminatorValue(typeof(BattingStatsForecastImpact)))
            .HasValue<FieldingStatsForecastImpact>(DiscriminatorValue(typeof(FieldingStatsForecastImpact)))
            .HasValue<PitchingStatsForecastImpact>(DiscriminatorValue(typeof(PitchingStatsForecastImpact)))
            .HasValue<BoostForecastImpact>(DiscriminatorValue(typeof(BoostForecastImpact)))
            .HasValue<OverallRatingChangeForecastImpact>(DiscriminatorValue(typeof(OverallRatingChangeForecastImpact)))
            .HasValue<PositionChangeForecastImpact>(DiscriminatorValue(typeof(PositionChangeForecastImpact)));

        builder.HasKey([
            Constants.ForecastImpacts.PlayerCardForecastId,
            Constants.PlayerCardForecasts.Relationships.DiscriminatorName,
            nameof(ForecastImpact.StartDate), nameof(ForecastImpact.EndDate)
        ]).HasName(Constants.ForecastImpacts.Keys.PrimaryKey);

        // Index for querying by the start and end dates
        builder.HasIndex(e => new { e.StartDate, e.EndDate }, Constants.ForecastImpacts.Indexes.StartAndEndDates)
            .HasMethod("btree");

        var columnOrder = 0;

        builder.Property(Constants.ForecastImpacts.PlayerCardForecastId)
            .HasColumnOrder(columnOrder++);

        builder.Property(e => e.StartDate)
            .IsRequired()
            .HasColumnType("date")
            .HasColumnName(Constants.ForecastImpacts.StartDate)
            .HasColumnOrder(columnOrder++);

        builder.Property(e => e.EndDate)
            .IsRequired()
            .HasColumnType("date")
            .HasColumnName(Constants.ForecastImpacts.EndDate)
            .HasColumnOrder(columnOrder++);
    }

    private static string DiscriminatorValue(Type type)
    {
        return type.Name.Replace("ForecastImpact", "");
    }
}