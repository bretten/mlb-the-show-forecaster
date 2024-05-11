using com.brettnamba.MlbTheShowForecaster.Common.Domain.ValueObjects;
using com.brettnamba.MlbTheShowForecaster.Performance.Domain.PlayerSeasons.ValueObjects;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace com.brettnamba.MlbTheShowForecaster.Performance.Infrastructure.PlayerSeasons.EntityFrameworkCore;

/// <summary>
/// Configures <see cref="PlayerBattingStatsByGame"/> for EF
/// </summary>
public sealed class PlayerBattingStatsByGameEntityTypeConfiguration : IEntityTypeConfiguration<PlayerBattingStatsByGame>
{
    /// <summary>
    /// Configures <see cref="PlayerBattingStatsByGame"/> for EF
    /// </summary>
    /// <param name="builder">The builder that configures the entity type</param>
    public void Configure(EntityTypeBuilder<PlayerBattingStatsByGame> builder)
    {
        builder.ToTable(Constants.PlayerBattingStatsByGames.TableName, Constants.Schema);

        builder.HasKey([
                Constants.PlayerBattingStatsByGames.PlayerStatsBySeasonId, nameof(PlayerBattingStatsByGame.GameMlbId)
            ])
            .HasName(Constants.PlayerBattingStatsByGames.Keys.PrimaryKey);

        var columnOrder = 0;

        builder.Property(Constants.PlayerBattingStatsByGames.PlayerStatsBySeasonId)
            .HasColumnOrder(columnOrder++);

        builder.Property(e => e.GameMlbId)
            .IsRequired()
            .HasColumnName(Constants.PlayerBattingStatsByGames.GameMlbId)
            .HasColumnOrder(columnOrder++)
            .HasConversion(v => v.Value,
                v => MlbId.Create(v));

        builder.Property(e => e.PlayerMlbId)
            .IsRequired()
            .HasColumnName(Constants.PlayerBattingStatsByGames.PlayerMlbId)
            .HasColumnOrder(columnOrder++)
            .HasConversion(v => v.Value,
                v => MlbId.Create(v));

        builder.Property(e => e.SeasonYear)
            .IsRequired()
            .HasColumnType("smallint")
            .HasColumnName(Constants.PlayerBattingStatsByGames.Season)
            .HasColumnOrder(columnOrder++)
            .HasConversion(v => v.Value,
                v => SeasonYear.Create(v));

        builder.Property(e => e.GameDate)
            .IsRequired()
            .HasColumnType("date")
            .HasColumnName(Constants.PlayerBattingStatsByGames.GameDate)
            .HasColumnOrder(columnOrder++);

        builder.Property(e => e.TeamMlbId)
            .IsRequired()
            .HasColumnName(Constants.PlayerBattingStatsByGames.TeamMlbId)
            .HasColumnOrder(columnOrder++)
            .HasConversion(v => v.Value,
                v => MlbId.Create(v));

        builder.Property(e => e.PlateAppearances)
            .IsRequired()
            .HasColumnType("integer")
            .HasColumnName(Constants.PlayerBattingStatsByGames.PlateAppearances)
            .HasColumnOrder(columnOrder++)
            .HasConversion(v => v.Value,
                v => NaturalNumber.Create(v));

        builder.Property(e => e.AtBats)
            .IsRequired()
            .HasColumnType("integer")
            .HasColumnName(Constants.PlayerBattingStatsByGames.AtBats)
            .HasColumnOrder(columnOrder++)
            .HasConversion(v => v.Value,
                v => NaturalNumber.Create(v));

        builder.Property(e => e.Runs)
            .IsRequired()
            .HasColumnType("integer")
            .HasColumnName(Constants.PlayerBattingStatsByGames.Runs)
            .HasColumnOrder(columnOrder++)
            .HasConversion(v => v.Value,
                v => NaturalNumber.Create(v));

        builder.Property(e => e.Hits)
            .IsRequired()
            .HasColumnType("integer")
            .HasColumnName(Constants.PlayerBattingStatsByGames.Hits)
            .HasColumnOrder(columnOrder++)
            .HasConversion(v => v.Value,
                v => NaturalNumber.Create(v));

        builder.Property(e => e.Doubles)
            .IsRequired()
            .HasColumnType("integer")
            .HasColumnName(Constants.PlayerBattingStatsByGames.Doubles)
            .HasColumnOrder(columnOrder++)
            .HasConversion(v => v.Value,
                v => NaturalNumber.Create(v));

        builder.Property(e => e.Triples)
            .IsRequired()
            .HasColumnType("integer")
            .HasColumnName(Constants.PlayerBattingStatsByGames.Triples)
            .HasColumnOrder(columnOrder++)
            .HasConversion(v => v.Value,
                v => NaturalNumber.Create(v));

        builder.Property(e => e.HomeRuns)
            .IsRequired()
            .HasColumnType("integer")
            .HasColumnName(Constants.PlayerBattingStatsByGames.HomeRuns)
            .HasColumnOrder(columnOrder++)
            .HasConversion(v => v.Value,
                v => NaturalNumber.Create(v));

        builder.Property(e => e.RunsBattedIn)
            .IsRequired()
            .HasColumnType("integer")
            .HasColumnName(Constants.PlayerBattingStatsByGames.RunsBattedIn)
            .HasColumnOrder(columnOrder++)
            .HasConversion(v => v.Value,
                v => NaturalNumber.Create(v));

        builder.Property(e => e.BaseOnBalls)
            .IsRequired()
            .HasColumnType("integer")
            .HasColumnName(Constants.PlayerBattingStatsByGames.BaseOnBalls)
            .HasColumnOrder(columnOrder++)
            .HasConversion(v => v.Value,
                v => NaturalNumber.Create(v));

        builder.Property(e => e.IntentionalWalks)
            .IsRequired()
            .HasColumnType("integer")
            .HasColumnName(Constants.PlayerBattingStatsByGames.IntentionalWalks)
            .HasColumnOrder(columnOrder++)
            .HasConversion(v => v.Value,
                v => NaturalNumber.Create(v));

        builder.Property(e => e.Strikeouts)
            .IsRequired()
            .HasColumnType("integer")
            .HasColumnName(Constants.PlayerBattingStatsByGames.Strikeouts)
            .HasColumnOrder(columnOrder++)
            .HasConversion(v => v.Value,
                v => NaturalNumber.Create(v));

        builder.Property(e => e.StolenBases)
            .IsRequired()
            .HasColumnType("integer")
            .HasColumnName(Constants.PlayerBattingStatsByGames.StolenBases)
            .HasColumnOrder(columnOrder++)
            .HasConversion(v => v.Value,
                v => NaturalNumber.Create(v));

        builder.Property(e => e.CaughtStealing)
            .IsRequired()
            .HasColumnType("integer")
            .HasColumnName(Constants.PlayerBattingStatsByGames.CaughtStealing)
            .HasColumnOrder(columnOrder++)
            .HasConversion(v => v.Value,
                v => NaturalNumber.Create(v));

        builder.Property(e => e.HitByPitches)
            .IsRequired()
            .HasColumnType("integer")
            .HasColumnName(Constants.PlayerBattingStatsByGames.HitByPitches)
            .HasColumnOrder(columnOrder++)
            .HasConversion(v => v.Value,
                v => NaturalNumber.Create(v));

        builder.Property(e => e.SacrificeBunts)
            .IsRequired()
            .HasColumnType("integer")
            .HasColumnName(Constants.PlayerBattingStatsByGames.SacrificeBunts)
            .HasColumnOrder(columnOrder++)
            .HasConversion(v => v.Value,
                v => NaturalNumber.Create(v));

        builder.Property(e => e.SacrificeFlies)
            .IsRequired()
            .HasColumnType("integer")
            .HasColumnName(Constants.PlayerBattingStatsByGames.SacrificeFlies)
            .HasColumnOrder(columnOrder++)
            .HasConversion(v => v.Value,
                v => NaturalNumber.Create(v));

        builder.Property(e => e.NumberOfPitchesSeen)
            .IsRequired()
            .HasColumnType("integer")
            .HasColumnName(Constants.PlayerBattingStatsByGames.NumberOfPitchesSeen)
            .HasColumnOrder(columnOrder++)
            .HasConversion(v => v.Value,
                v => NaturalNumber.Create(v));

        builder.Property(e => e.LeftOnBase)
            .IsRequired()
            .HasColumnType("integer")
            .HasColumnName(Constants.PlayerBattingStatsByGames.LeftOnBase)
            .HasColumnOrder(columnOrder++)
            .HasConversion(v => v.Value,
                v => NaturalNumber.Create(v));

        builder.Property(e => e.GroundOuts)
            .IsRequired()
            .HasColumnType("integer")
            .HasColumnName(Constants.PlayerBattingStatsByGames.GroundOuts)
            .HasColumnOrder(columnOrder++)
            .HasConversion(v => v.Value,
                v => NaturalNumber.Create(v));

        builder.Property(e => e.GroundIntoDoublePlays)
            .IsRequired()
            .HasColumnType("integer")
            .HasColumnName(Constants.PlayerBattingStatsByGames.GroundIntoDoublePlays)
            .HasColumnOrder(columnOrder++)
            .HasConversion(v => v.Value,
                v => NaturalNumber.Create(v));

        builder.Property(e => e.GroundIntoTriplePlays)
            .IsRequired()
            .HasColumnType("integer")
            .HasColumnName(Constants.PlayerBattingStatsByGames.GroundIntoTriplePlays)
            .HasColumnOrder(columnOrder++)
            .HasConversion(v => v.Value,
                v => NaturalNumber.Create(v));

        builder.Property(e => e.AirOuts)
            .IsRequired()
            .HasColumnType("integer")
            .HasColumnName(Constants.PlayerBattingStatsByGames.AirOuts)
            .HasColumnOrder(columnOrder++)
            .HasConversion(v => v.Value,
                v => NaturalNumber.Create(v));

        builder.Property(e => e.CatcherInterferences)
            .IsRequired()
            .HasColumnType("integer")
            .HasColumnName(Constants.PlayerBattingStatsByGames.CatcherInterferences)
            .HasColumnOrder(columnOrder++)
            .HasConversion(v => v.Value,
                v => NaturalNumber.Create(v));
    }
}