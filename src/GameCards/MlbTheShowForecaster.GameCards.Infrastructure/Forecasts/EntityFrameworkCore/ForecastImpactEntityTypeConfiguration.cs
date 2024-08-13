using com.brettnamba.MlbTheShowForecaster.GameCards.Domain.Forecasts.Entities;
using com.brettnamba.MlbTheShowForecaster.GameCards.Domain.Forecasts.ValueObjects;
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

        builder.HasOne<PlayerCardForecast>()
            .WithMany(Constants.PlayerCardForecasts.Relationships.ForecastImpactsField)
            .HasForeignKey(Constants.ForecastImpacts.PlayerCardForecastId)
            .HasConstraintName(Constants.ForecastImpacts.Keys.PlayerCardForecastsForeignKeyConstraint);

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
            Constants.PlayerCardForecasts.Relationships.DiscriminatorName, nameof(ForecastImpact.EndDate)
        ]).HasName(Constants.ForecastImpacts.Keys.PrimaryKey);

        var columnOrder = 0;

        builder.Property(e => e.EndDate)
            .IsRequired()
            .HasColumnType("date")
            .HasColumnName(Constants.ForecastImpacts.EndDate)
            .HasColumnOrder(columnOrder++);

        builder.Property(e => e.Demand)
            .IsRequired()
            .HasColumnType("smallint")
            .HasColumnName(Constants.ForecastImpacts.Demand)
            .HasColumnOrder(columnOrder++)
            .HasConversion(v => v.Value,
                v => Demand.Create(v));
    }

    private static string DiscriminatorValue(Type type)
    {
        return type.Name.Replace("ForecastImpact", "");
    }
}