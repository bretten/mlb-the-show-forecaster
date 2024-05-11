using com.brettnamba.MlbTheShowForecaster.Common.Domain.ValueObjects;
using com.brettnamba.MlbTheShowForecaster.Performance.Domain.PlayerSeasons.ValueObjects;
using com.brettnamba.MlbTheShowForecaster.Performance.Domain.Statistics.ValueObjects.Shared;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace com.brettnamba.MlbTheShowForecaster.Performance.Infrastructure.PlayerSeasons.EntityFrameworkCore;

/// <summary>
/// Configures <see cref="PlayerPitchingStatsByGame"/> for EF
/// </summary>
public sealed class
    PlayerPitchingStatsByGameEntityTypeConfiguration : IEntityTypeConfiguration<PlayerPitchingStatsByGame>
{
    /// <summary>
    /// Configures <see cref="PlayerPitchingStatsByGame"/> for EF
    /// </summary>
    /// <param name="builder">The builder that configures the entity type</param>
    public void Configure(EntityTypeBuilder<PlayerPitchingStatsByGame> builder)
    {
        builder.ToTable(Constants.PlayerPitchingStatsByGames.TableName, Constants.Schema);

        builder.HasKey([
                Constants.PlayerPitchingStatsByGames.PlayerStatsBySeasonId, nameof(PlayerPitchingStatsByGame.GameMlbId)
            ])
            .HasName(Constants.PlayerPitchingStatsByGames.Keys.PrimaryKey);

        var columnOrder = 0;

        builder.Property(Constants.PlayerPitchingStatsByGames.PlayerStatsBySeasonId)
            .HasColumnOrder(columnOrder++);

        builder.Property(e => e.GameMlbId)
            .IsRequired()
            .HasColumnName(Constants.PlayerPitchingStatsByGames.GameMlbId)
            .HasColumnOrder(columnOrder++)
            .HasConversion(v => v.Value,
                v => MlbId.Create(v));

        builder.Property(e => e.PlayerMlbId)
            .IsRequired()
            .HasColumnName(Constants.PlayerPitchingStatsByGames.PlayerMlbId)
            .HasColumnOrder(columnOrder++)
            .HasConversion(v => v.Value,
                v => MlbId.Create(v));

        builder.Property(e => e.SeasonYear)
            .IsRequired()
            .HasColumnType("smallint")
            .HasColumnName(Constants.PlayerPitchingStatsByGames.Season)
            .HasColumnOrder(columnOrder++)
            .HasConversion(v => v.Value,
                v => SeasonYear.Create(v));

        builder.Property(e => e.GameDate)
            .IsRequired()
            .HasColumnType("date")
            .HasColumnName(Constants.PlayerPitchingStatsByGames.GameDate)
            .HasColumnOrder(columnOrder++);

        builder.Property(e => e.TeamMlbId)
            .IsRequired()
            .HasColumnName(Constants.PlayerPitchingStatsByGames.TeamMlbId)
            .HasColumnOrder(columnOrder++)
            .HasConversion(v => v.Value,
                v => MlbId.Create(v));

        builder.Property(e => e.Wins)
            .IsRequired()
            .HasColumnType("integer")
            .HasColumnName(Constants.PlayerPitchingStatsByGames.Wins)
            .HasColumnOrder(columnOrder++)
            .HasConversion(v => v.Value,
                v => NaturalNumber.Create(v));

        builder.Property(e => e.Losses)
            .IsRequired()
            .HasColumnType("integer")
            .HasColumnName(Constants.PlayerPitchingStatsByGames.Losses)
            .HasColumnOrder(columnOrder++)
            .HasConversion(v => v.Value,
                v => NaturalNumber.Create(v));

        builder.Property(e => e.GamesStarted)
            .IsRequired()
            .HasColumnType("integer")
            .HasColumnName(Constants.PlayerPitchingStatsByGames.GamesStarted)
            .HasColumnOrder(columnOrder++)
            .HasConversion(v => v.Value,
                v => NaturalNumber.Create(v));

        builder.Property(e => e.GamesFinished)
            .IsRequired()
            .HasColumnType("integer")
            .HasColumnName(Constants.PlayerPitchingStatsByGames.GamesFinished)
            .HasColumnOrder(columnOrder++)
            .HasConversion(v => v.Value,
                v => NaturalNumber.Create(v));

        builder.Property(e => e.CompleteGames)
            .IsRequired()
            .HasColumnType("integer")
            .HasColumnName(Constants.PlayerPitchingStatsByGames.CompleteGames)
            .HasColumnOrder(columnOrder++)
            .HasConversion(v => v.Value,
                v => NaturalNumber.Create(v));

        builder.Property(e => e.Shutouts)
            .IsRequired()
            .HasColumnType("integer")
            .HasColumnName(Constants.PlayerPitchingStatsByGames.Shutouts)
            .HasColumnOrder(columnOrder++)
            .HasConversion(v => v.Value,
                v => NaturalNumber.Create(v));

        builder.Property(e => e.Holds)
            .IsRequired()
            .HasColumnType("integer")
            .HasColumnName(Constants.PlayerPitchingStatsByGames.Holds)
            .HasColumnOrder(columnOrder++)
            .HasConversion(v => v.Value,
                v => NaturalNumber.Create(v));

        builder.Property(e => e.Saves)
            .IsRequired()
            .HasColumnType("integer")
            .HasColumnName(Constants.PlayerPitchingStatsByGames.Saves)
            .HasColumnOrder(columnOrder++)
            .HasConversion(v => v.Value,
                v => NaturalNumber.Create(v));

        builder.Property(e => e.BlownSaves)
            .IsRequired()
            .HasColumnType("integer")
            .HasColumnName(Constants.PlayerPitchingStatsByGames.BlownSaves)
            .HasColumnOrder(columnOrder++)
            .HasConversion(v => v.Value,
                v => NaturalNumber.Create(v));

        builder.Property(e => e.SaveOpportunities)
            .IsRequired()
            .HasColumnType("integer")
            .HasColumnName(Constants.PlayerPitchingStatsByGames.SaveOpportunities)
            .HasColumnOrder(columnOrder++)
            .HasConversion(v => v.Value,
                v => NaturalNumber.Create(v));

        builder.Property(e => e.InningsPitched)
            .IsRequired()
            .HasColumnType("decimal(8,3)")
            .HasColumnName(Constants.PlayerPitchingStatsByGames.InningsPitched)
            .HasColumnOrder(columnOrder++)
            .HasConversion(v => v.Value,
                v => InningsCount.Create(v));

        builder.Property(e => e.Hits)
            .IsRequired()
            .HasColumnType("integer")
            .HasColumnName(Constants.PlayerPitchingStatsByGames.Hits)
            .HasColumnOrder(columnOrder++)
            .HasConversion(v => v.Value,
                v => NaturalNumber.Create(v));

        builder.Property(e => e.Doubles)
            .IsRequired()
            .HasColumnType("integer")
            .HasColumnName(Constants.PlayerPitchingStatsByGames.Doubles)
            .HasColumnOrder(columnOrder++)
            .HasConversion(v => v.Value,
                v => NaturalNumber.Create(v));

        builder.Property(e => e.Triples)
            .IsRequired()
            .HasColumnType("integer")
            .HasColumnName(Constants.PlayerPitchingStatsByGames.Triples)
            .HasColumnOrder(columnOrder++)
            .HasConversion(v => v.Value,
                v => NaturalNumber.Create(v));

        builder.Property(e => e.HomeRuns)
            .IsRequired()
            .HasColumnType("integer")
            .HasColumnName(Constants.PlayerPitchingStatsByGames.HomeRuns)
            .HasColumnOrder(columnOrder++)
            .HasConversion(v => v.Value,
                v => NaturalNumber.Create(v));

        builder.Property(e => e.Runs)
            .IsRequired()
            .HasColumnType("integer")
            .HasColumnName(Constants.PlayerPitchingStatsByGames.Runs)
            .HasColumnOrder(columnOrder++)
            .HasConversion(v => v.Value,
                v => NaturalNumber.Create(v));

        builder.Property(e => e.EarnedRuns)
            .IsRequired()
            .HasColumnType("integer")
            .HasColumnName(Constants.PlayerPitchingStatsByGames.EarnedRuns)
            .HasColumnOrder(columnOrder++)
            .HasConversion(v => v.Value,
                v => NaturalNumber.Create(v));

        builder.Property(e => e.Strikeouts)
            .IsRequired()
            .HasColumnType("integer")
            .HasColumnName(Constants.PlayerPitchingStatsByGames.Strikeouts)
            .HasColumnOrder(columnOrder++)
            .HasConversion(v => v.Value,
                v => NaturalNumber.Create(v));

        builder.Property(e => e.BaseOnBalls)
            .IsRequired()
            .HasColumnType("integer")
            .HasColumnName(Constants.PlayerPitchingStatsByGames.BaseOnBalls)
            .HasColumnOrder(columnOrder++)
            .HasConversion(v => v.Value,
                v => NaturalNumber.Create(v));

        builder.Property(e => e.IntentionalWalks)
            .IsRequired()
            .HasColumnType("integer")
            .HasColumnName(Constants.PlayerPitchingStatsByGames.IntentionalWalks)
            .HasColumnOrder(columnOrder++)
            .HasConversion(v => v.Value,
                v => NaturalNumber.Create(v));

        builder.Property(e => e.HitBatsmen)
            .IsRequired()
            .HasColumnType("integer")
            .HasColumnName(Constants.PlayerPitchingStatsByGames.HitBatsmen)
            .HasColumnOrder(columnOrder++)
            .HasConversion(v => v.Value,
                v => NaturalNumber.Create(v));

        builder.Property(e => e.Outs)
            .IsRequired()
            .HasColumnType("integer")
            .HasColumnName(Constants.PlayerPitchingStatsByGames.Outs)
            .HasColumnOrder(columnOrder++)
            .HasConversion(v => v.Value,
                v => NaturalNumber.Create(v));

        builder.Property(e => e.GroundOuts)
            .IsRequired()
            .HasColumnType("integer")
            .HasColumnName(Constants.PlayerPitchingStatsByGames.GroundOuts)
            .HasColumnOrder(columnOrder++)
            .HasConversion(v => v.Value,
                v => NaturalNumber.Create(v));

        builder.Property(e => e.AirOuts)
            .IsRequired()
            .HasColumnType("integer")
            .HasColumnName(Constants.PlayerPitchingStatsByGames.AirOuts)
            .HasColumnOrder(columnOrder++)
            .HasConversion(v => v.Value,
                v => NaturalNumber.Create(v));

        builder.Property(e => e.GroundIntoDoublePlays)
            .IsRequired()
            .HasColumnType("integer")
            .HasColumnName(Constants.PlayerPitchingStatsByGames.GroundIntoDoublePlays)
            .HasColumnOrder(columnOrder++)
            .HasConversion(v => v.Value,
                v => NaturalNumber.Create(v));

        builder.Property(e => e.NumberOfPitches)
            .IsRequired()
            .HasColumnType("integer")
            .HasColumnName(Constants.PlayerPitchingStatsByGames.NumberOfPitches)
            .HasColumnOrder(columnOrder++)
            .HasConversion(v => v.Value,
                v => NaturalNumber.Create(v));

        builder.Property(e => e.Strikes)
            .IsRequired()
            .HasColumnType("integer")
            .HasColumnName(Constants.PlayerPitchingStatsByGames.Strikes)
            .HasColumnOrder(columnOrder++)
            .HasConversion(v => v.Value,
                v => NaturalNumber.Create(v));

        builder.Property(e => e.WildPitches)
            .IsRequired()
            .HasColumnType("integer")
            .HasColumnName(Constants.PlayerPitchingStatsByGames.WildPitches)
            .HasColumnOrder(columnOrder++)
            .HasConversion(v => v.Value,
                v => NaturalNumber.Create(v));

        builder.Property(e => e.Balks)
            .IsRequired()
            .HasColumnType("integer")
            .HasColumnName(Constants.PlayerPitchingStatsByGames.Balks)
            .HasColumnOrder(columnOrder++)
            .HasConversion(v => v.Value,
                v => NaturalNumber.Create(v));

        builder.Property(e => e.BattersFaced)
            .IsRequired()
            .HasColumnType("integer")
            .HasColumnName(Constants.PlayerPitchingStatsByGames.BattersFaced)
            .HasColumnOrder(columnOrder++)
            .HasConversion(v => v.Value,
                v => NaturalNumber.Create(v));

        builder.Property(e => e.AtBats)
            .IsRequired()
            .HasColumnType("integer")
            .HasColumnName(Constants.PlayerPitchingStatsByGames.AtBats)
            .HasColumnOrder(columnOrder++)
            .HasConversion(v => v.Value,
                v => NaturalNumber.Create(v));

        builder.Property(e => e.StolenBases)
            .IsRequired()
            .HasColumnType("integer")
            .HasColumnName(Constants.PlayerPitchingStatsByGames.StolenBases)
            .HasColumnOrder(columnOrder++)
            .HasConversion(v => v.Value,
                v => NaturalNumber.Create(v));

        builder.Property(e => e.CaughtStealing)
            .IsRequired()
            .HasColumnType("integer")
            .HasColumnName(Constants.PlayerPitchingStatsByGames.CaughtStealing)
            .HasColumnOrder(columnOrder++)
            .HasConversion(v => v.Value,
                v => NaturalNumber.Create(v));

        builder.Property(e => e.Pickoffs)
            .IsRequired()
            .HasColumnType("integer")
            .HasColumnName(Constants.PlayerPitchingStatsByGames.Pickoffs)
            .HasColumnOrder(columnOrder++)
            .HasConversion(v => v.Value,
                v => NaturalNumber.Create(v));

        builder.Property(e => e.InheritedRunners)
            .IsRequired()
            .HasColumnType("integer")
            .HasColumnName(Constants.PlayerPitchingStatsByGames.InheritedRunners)
            .HasColumnOrder(columnOrder++)
            .HasConversion(v => v.Value,
                v => NaturalNumber.Create(v));

        builder.Property(e => e.InheritedRunnersScored)
            .IsRequired()
            .HasColumnType("integer")
            .HasColumnName(Constants.PlayerPitchingStatsByGames.InheritedRunnersScored)
            .HasColumnOrder(columnOrder++)
            .HasConversion(v => v.Value,
                v => NaturalNumber.Create(v));

        builder.Property(e => e.CatcherInterferences)
            .IsRequired()
            .HasColumnType("integer")
            .HasColumnName(Constants.PlayerPitchingStatsByGames.CatcherInterferences)
            .HasColumnOrder(columnOrder++)
            .HasConversion(v => v.Value,
                v => NaturalNumber.Create(v));

        builder.Property(e => e.SacrificeBunts)
            .IsRequired()
            .HasColumnType("integer")
            .HasColumnName(Constants.PlayerPitchingStatsByGames.SacrificeBunts)
            .HasColumnOrder(columnOrder++)
            .HasConversion(v => v.Value,
                v => NaturalNumber.Create(v));

        builder.Property(e => e.SacrificeFlies)
            .IsRequired()
            .HasColumnType("integer")
            .HasColumnName(Constants.PlayerPitchingStatsByGames.SacrificeFlies)
            .HasColumnOrder(columnOrder++)
            .HasConversion(v => v.Value,
                v => NaturalNumber.Create(v));
    }
}