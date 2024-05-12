using com.brettnamba.MlbTheShowForecaster.Common.Domain.ValueObjects;
using com.brettnamba.MlbTheShowForecaster.Performance.Domain.PlayerSeasons.Entities;
using com.brettnamba.MlbTheShowForecaster.Performance.Domain.PlayerSeasons.ValueObjects;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace com.brettnamba.MlbTheShowForecaster.Performance.Infrastructure.PlayerSeasons.EntityFrameworkCore;

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

        builder.HasKey(e => e.Id)
            .HasName(Constants.PlayerStatsBySeasons.Keys.PrimaryKey);

        // Index for querying by season year
        builder.HasIndex(e => new { e.SeasonYear }, Constants.PlayerStatsBySeasons.Indexes.Year)
            .HasMethod("btree");

        var columnOrder = 0;

        builder.Property(e => e.Id)
            .IsRequired()
            .HasColumnName(Constants.PlayerStatsBySeasons.Id)
            .HasColumnOrder(columnOrder++);

        builder.Property(e => e.PlayerMlbId)
            .IsRequired()
            .HasColumnName(Constants.PlayerStatsBySeasons.PlayerMlbId)
            .HasColumnOrder(columnOrder++)
            .HasConversion(v => v.Value,
                v => MlbId.Create(v));

        builder.Property(e => e.SeasonYear)
            .IsRequired()
            .HasColumnType("smallint")
            .HasColumnName(Constants.PlayerStatsBySeasons.Season)
            .HasColumnOrder(columnOrder++)
            .HasConversion(v => v.Value,
                v => SeasonYear.Create(v));

        // Ignore these properties. They are not relationships/navigation properties, but just convenience methods for other members of the class
        builder.Ignore(x => x.BattingStatsByGamesChronologically);
        builder.Ignore(x => x.PitchingStatsByGamesChronologically);
        builder.Ignore(x => x.FieldingStatsByGamesChronologically);

        builder.HasMany<PlayerBattingStatsByGame>("_battingStatsByGames")
            .WithOne()
            .HasForeignKey(Constants.PlayerBattingStatsByGames.PlayerStatsBySeasonId)
            .HasConstraintName(Constants.PlayerBattingStatsByGames.Keys.PlayerStatsBySeasonsForeignKeyConstraint)
            .IsRequired();
        builder.HasMany<PlayerPitchingStatsByGame>("_pitchingStatsByGames")
            .WithOne()
            .HasForeignKey(Constants.PlayerPitchingStatsByGames.PlayerStatsBySeasonId)
            .HasConstraintName(Constants.PlayerPitchingStatsByGames.Keys.PlayerStatsBySeasonsForeignKeyConstraint)
            .IsRequired();
        builder.HasMany<PlayerFieldingStatsByGame>("_fieldingStatsByGames")
            .WithOne()
            .HasForeignKey(Constants.PlayerFieldingStatsByGames.PlayerStatsBySeasonId)
            .HasConstraintName(Constants.PlayerFieldingStatsByGames.Keys.PlayerStatsBySeasonsForeignKeyConstraint)
            .IsRequired();
    }
}