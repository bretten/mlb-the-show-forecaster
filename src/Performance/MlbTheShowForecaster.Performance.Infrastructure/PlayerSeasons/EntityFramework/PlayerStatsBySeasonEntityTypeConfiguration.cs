using com.brettnamba.MlbTheShowForecaster.Common.Domain.ValueObjects;
using com.brettnamba.MlbTheShowForecaster.Performance.Domain.PlayerSeasons.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace com.brettnamba.MlbTheShowForecaster.Performance.Infrastructure.PlayerSeasons.EntityFramework;

/// <summary>
/// Configures <see cref="PlayerStatsBySeason"/> for EF
/// </summary>
public sealed class PlayerStatsBySeasonEntityTypeConfiguration : IEntityTypeConfiguration<PlayerStatsBySeason>
{
    /// <summary>
    /// Configures <see cref="PlayerStatsBySeason"/> for EF
    /// </summary>
    /// <param name="builder">The builder that configures the entity type</param>
    public void Configure(EntityTypeBuilder<PlayerStatsBySeason> builder)
    {
        builder.ToTable(Constants.PlayerStatsBySeasons.TableName, Constants.Schema);

        builder.Property(e => e.Id)
            .IsRequired()
            .HasColumnName(Constants.PlayerStatsBySeasons.Id);

        builder.Property(e => e.PlayerMlbId)
            .IsRequired()
            .HasColumnName(Constants.PlayerStatsBySeasons.PlayerMlbId)
            .HasConversion(v => v.Value,
                v => MlbId.Create(v));

        builder.Property(e => e.SeasonYear)
            .IsRequired()
            .HasColumnName(Constants.PlayerStatsBySeasons.Season)
            .HasConversion(v => v.Value,
                v => SeasonYear.Create(v));
    }
}