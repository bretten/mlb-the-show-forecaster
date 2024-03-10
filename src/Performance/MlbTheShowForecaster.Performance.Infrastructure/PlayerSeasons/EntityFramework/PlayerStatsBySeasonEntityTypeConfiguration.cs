using com.brettnamba.MlbTheShowForecaster.Common.Domain.ValueObjects;
using com.brettnamba.MlbTheShowForecaster.Performance.Domain.PlayerSeasons.Entities;
using com.brettnamba.MlbTheShowForecaster.Performance.Domain.PlayerSeasons.ValueObjects;
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

        // Ignore these properties. They are not relationships/navigation properties, but just convenience methods for other members of the class
        builder.Ignore(x => x.BattingStatsByGamesChronologically);
        builder.Ignore(x => x.PitchingStatsByGamesChronologically);
        builder.Ignore(x => x.FieldingStatsByGamesChronologically);

        builder.HasMany<PlayerBattingStatsByGame>("_battingStatsByGames")
            .WithOne("_playerStatsBySeason")
            .HasForeignKey("player_stats_by_season_id")
            .IsRequired();
        builder.HasMany<PlayerPitchingStatsByGame>("_pitchingStatsByGames")
            .WithOne("_playerStatsBySeason")
            .HasForeignKey("player_stats_by_season_id")
            .IsRequired();
        builder.HasMany<PlayerFieldingStatsByGame>("_fieldingStatsByGames")
            .WithOne("_playerStatsBySeason")
            .HasForeignKey("player_stats_by_season_id")
            .IsRequired();
    }
}