using com.brettnamba.MlbTheShowForecaster.Common.Domain.ValueObjects;
using com.brettnamba.MlbTheShowForecaster.Performance.Domain.PlayerSeasons.ValueObjects;
using com.brettnamba.MlbTheShowForecaster.Performance.Domain.Statistics.ValueObjects.Shared;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace com.brettnamba.MlbTheShowForecaster.Performance.Infrastructure.PlayerSeasons.EntityFramework;

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

        builder.Property(e => e.PlayerMlbId)
            .IsRequired()
            .HasColumnName(Constants.PlayerPitchingStatsByGames.PlayerMlbId)
            .HasConversion(v => v.Value,
                v => MlbId.Create(v));

        builder.Property(e => e.SeasonYear)
            .IsRequired()
            .HasColumnName(Constants.PlayerPitchingStatsByGames.Season)
            .HasConversion(v => v.Value,
                v => SeasonYear.Create(v));

        builder.Property(e => e.Wins)
            .IsRequired()
            .HasColumnType("integer")
            .HasColumnName(Constants.PlayerPitchingStatsByGames.Wins)
            .HasConversion(v => v.Value,
                v => NaturalNumber.Create(v));

        builder.Property(e => e.Losses)
            .IsRequired()
            .HasColumnType("integer")
            .HasColumnName(Constants.PlayerPitchingStatsByGames.Losses)
            .HasConversion(v => v.Value,
                v => NaturalNumber.Create(v));

        builder.Property(e => e.GamesStarted)
            .IsRequired()
            .HasColumnType("integer")
            .HasColumnName(Constants.PlayerPitchingStatsByGames.GamesStarted)
            .HasConversion(v => v.Value,
                v => NaturalNumber.Create(v));

        builder.Property(e => e.GamesFinished)
            .IsRequired()
            .HasColumnType("integer")
            .HasColumnName(Constants.PlayerPitchingStatsByGames.GamesFinished)
            .HasConversion(v => v.Value,
                v => NaturalNumber.Create(v));

        builder.Property(e => e.CompleteGames)
            .IsRequired()
            .HasColumnType("integer")
            .HasColumnName(Constants.PlayerPitchingStatsByGames.CompleteGames)
            .HasConversion(v => v.Value,
                v => NaturalNumber.Create(v));

        builder.Property(e => e.Shutouts)
            .IsRequired()
            .HasColumnType("integer")
            .HasColumnName(Constants.PlayerPitchingStatsByGames.Shutouts)
            .HasConversion(v => v.Value,
                v => NaturalNumber.Create(v));

        builder.Property(e => e.Holds)
            .IsRequired()
            .HasColumnType("integer")
            .HasColumnName(Constants.PlayerPitchingStatsByGames.Holds)
            .HasConversion(v => v.Value,
                v => NaturalNumber.Create(v));

        builder.Property(e => e.Saves)
            .IsRequired()
            .HasColumnType("integer")
            .HasColumnName(Constants.PlayerPitchingStatsByGames.Saves)
            .HasConversion(v => v.Value,
                v => NaturalNumber.Create(v));

        builder.Property(e => e.BlownSaves)
            .IsRequired()
            .HasColumnType("integer")
            .HasColumnName(Constants.PlayerPitchingStatsByGames.BlownSaves)
            .HasConversion(v => v.Value,
                v => NaturalNumber.Create(v));

        builder.Property(e => e.SaveOpportunities)
            .IsRequired()
            .HasColumnType("integer")
            .HasColumnName(Constants.PlayerPitchingStatsByGames.SaveOpportunities)
            .HasConversion(v => v.Value,
                v => NaturalNumber.Create(v));

        builder.Property(e => e.InningsPitched)
            .IsRequired()
            .HasColumnType("integer")
            .HasColumnName(Constants.PlayerPitchingStatsByGames.InningsPitched)
            .HasConversion(v => v.Value,
                v => InningsCount.Create(v));

        builder.Property(e => e.Hits)
            .IsRequired()
            .HasColumnType("integer")
            .HasColumnName(Constants.PlayerPitchingStatsByGames.Hits)
            .HasConversion(v => v.Value,
                v => NaturalNumber.Create(v));

        builder.Property(e => e.Doubles)
            .IsRequired()
            .HasColumnType("integer")
            .HasColumnName(Constants.PlayerPitchingStatsByGames.Doubles)
            .HasConversion(v => v.Value,
                v => NaturalNumber.Create(v));

        builder.Property(e => e.Triples)
            .IsRequired()
            .HasColumnType("integer")
            .HasColumnName(Constants.PlayerPitchingStatsByGames.Triples)
            .HasConversion(v => v.Value,
                v => NaturalNumber.Create(v));

        builder.Property(e => e.HomeRuns)
            .IsRequired()
            .HasColumnType("integer")
            .HasColumnName(Constants.PlayerPitchingStatsByGames.HomeRuns)
            .HasConversion(v => v.Value,
                v => NaturalNumber.Create(v));

        builder.Property(e => e.Runs)
            .IsRequired()
            .HasColumnType("integer")
            .HasColumnName(Constants.PlayerPitchingStatsByGames.Runs)
            .HasConversion(v => v.Value,
                v => NaturalNumber.Create(v));

        builder.Property(e => e.EarnedRuns)
            .IsRequired()
            .HasColumnType("integer")
            .HasColumnName(Constants.PlayerPitchingStatsByGames.EarnedRuns)
            .HasConversion(v => v.Value,
                v => NaturalNumber.Create(v));

        builder.Property(e => e.Strikeouts)
            .IsRequired()
            .HasColumnType("integer")
            .HasColumnName(Constants.PlayerPitchingStatsByGames.Strikeouts)
            .HasConversion(v => v.Value,
                v => NaturalNumber.Create(v));

        builder.Property(e => e.BaseOnBalls)
            .IsRequired()
            .HasColumnType("integer")
            .HasColumnName(Constants.PlayerPitchingStatsByGames.BaseOnBalls)
            .HasConversion(v => v.Value,
                v => NaturalNumber.Create(v));

        builder.Property(e => e.IntentionalWalks)
            .IsRequired()
            .HasColumnType("integer")
            .HasColumnName(Constants.PlayerPitchingStatsByGames.IntentionalWalks)
            .HasConversion(v => v.Value,
                v => NaturalNumber.Create(v));

        builder.Property(e => e.HitBatsmen)
            .IsRequired()
            .HasColumnType("integer")
            .HasColumnName(Constants.PlayerPitchingStatsByGames.HitBatsmen)
            .HasConversion(v => v.Value,
                v => NaturalNumber.Create(v));

        builder.Property(e => e.Outs)
            .IsRequired()
            .HasColumnType("integer")
            .HasColumnName(Constants.PlayerPitchingStatsByGames.Outs)
            .HasConversion(v => v.Value,
                v => NaturalNumber.Create(v));

        builder.Property(e => e.GroundOuts)
            .IsRequired()
            .HasColumnType("integer")
            .HasColumnName(Constants.PlayerPitchingStatsByGames.GroundOuts)
            .HasConversion(v => v.Value,
                v => NaturalNumber.Create(v));

        builder.Property(e => e.AirOuts)
            .IsRequired()
            .HasColumnType("integer")
            .HasColumnName(Constants.PlayerPitchingStatsByGames.AirOuts)
            .HasConversion(v => v.Value,
                v => NaturalNumber.Create(v));

        builder.Property(e => e.GroundIntoDoublePlays)
            .IsRequired()
            .HasColumnType("integer")
            .HasColumnName(Constants.PlayerPitchingStatsByGames.GroundIntoDoublePlays)
            .HasConversion(v => v.Value,
                v => NaturalNumber.Create(v));

        builder.Property(e => e.NumberOfPitches)
            .IsRequired()
            .HasColumnType("integer")
            .HasColumnName(Constants.PlayerPitchingStatsByGames.NumberOfPitches)
            .HasConversion(v => v.Value,
                v => NaturalNumber.Create(v));

        builder.Property(e => e.Strikes)
            .IsRequired()
            .HasColumnType("integer")
            .HasColumnName(Constants.PlayerPitchingStatsByGames.Strikes)
            .HasConversion(v => v.Value,
                v => NaturalNumber.Create(v));

        builder.Property(e => e.WildPitches)
            .IsRequired()
            .HasColumnType("integer")
            .HasColumnName(Constants.PlayerPitchingStatsByGames.WildPitches)
            .HasConversion(v => v.Value,
                v => NaturalNumber.Create(v));

        builder.Property(e => e.Balks)
            .IsRequired()
            .HasColumnType("integer")
            .HasColumnName(Constants.PlayerPitchingStatsByGames.Balks)
            .HasConversion(v => v.Value,
                v => NaturalNumber.Create(v));

        builder.Property(e => e.BattersFaced)
            .IsRequired()
            .HasColumnType("integer")
            .HasColumnName(Constants.PlayerPitchingStatsByGames.BattersFaced)
            .HasConversion(v => v.Value,
                v => NaturalNumber.Create(v));

        builder.Property(e => e.AtBats)
            .IsRequired()
            .HasColumnType("integer")
            .HasColumnName(Constants.PlayerPitchingStatsByGames.AtBats)
            .HasConversion(v => v.Value,
                v => NaturalNumber.Create(v));

        builder.Property(e => e.StolenBases)
            .IsRequired()
            .HasColumnType("integer")
            .HasColumnName(Constants.PlayerPitchingStatsByGames.StolenBases)
            .HasConversion(v => v.Value,
                v => NaturalNumber.Create(v));

        builder.Property(e => e.CaughtStealing)
            .IsRequired()
            .HasColumnType("integer")
            .HasColumnName(Constants.PlayerPitchingStatsByGames.CaughtStealing)
            .HasConversion(v => v.Value,
                v => NaturalNumber.Create(v));

        builder.Property(e => e.PickOffs)
            .IsRequired()
            .HasColumnType("integer")
            .HasColumnName(Constants.PlayerPitchingStatsByGames.PickOffs)
            .HasConversion(v => v.Value,
                v => NaturalNumber.Create(v));

        builder.Property(e => e.InheritedRunners)
            .IsRequired()
            .HasColumnType("integer")
            .HasColumnName(Constants.PlayerPitchingStatsByGames.InheritedRunners)
            .HasConversion(v => v.Value,
                v => NaturalNumber.Create(v));

        builder.Property(e => e.InheritedRunnersScored)
            .IsRequired()
            .HasColumnType("integer")
            .HasColumnName(Constants.PlayerPitchingStatsByGames.InheritedRunnersScored)
            .HasConversion(v => v.Value,
                v => NaturalNumber.Create(v));

        builder.Property(e => e.CatchersInterferences)
            .IsRequired()
            .HasColumnType("integer")
            .HasColumnName(Constants.PlayerPitchingStatsByGames.CatchersInterferences)
            .HasConversion(v => v.Value,
                v => NaturalNumber.Create(v));

        builder.Property(e => e.SacrificeBunts)
            .IsRequired()
            .HasColumnType("integer")
            .HasColumnName(Constants.PlayerPitchingStatsByGames.SacrificeBunts)
            .HasConversion(v => v.Value,
                v => NaturalNumber.Create(v));

        builder.Property(e => e.SacrificeFlies)
            .IsRequired()
            .HasColumnType("integer")
            .HasColumnName(Constants.PlayerPitchingStatsByGames.SacrificeFlies)
            .HasConversion(v => v.Value,
                v => NaturalNumber.Create(v));
    }
}