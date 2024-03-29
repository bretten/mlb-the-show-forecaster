﻿using com.brettnamba.MlbTheShowForecaster.Common.Domain.ValueObjects;
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

        builder.HasKey(e => new { e.PlayerMlbId, e.SeasonYear, e.GameDate, e.GameMlbId });

        builder.Property(e => e.PlayerMlbId)
            .IsRequired()
            .HasColumnName(Constants.PlayerBattingStatsByGames.PlayerMlbId)
            .HasConversion(v => v.Value,
                v => MlbId.Create(v));

        builder.Property(e => e.SeasonYear)
            .IsRequired()
            .HasColumnName(Constants.PlayerBattingStatsByGames.Season)
            .HasConversion(v => v.Value,
                v => SeasonYear.Create(v));

        builder.Property(e => e.GameDate)
            .IsRequired()
            .HasColumnType("date")
            .HasColumnName(Constants.PlayerBattingStatsByGames.GameDate);

        builder.Property(e => e.GameMlbId)
            .IsRequired()
            .HasColumnName(Constants.PlayerBattingStatsByGames.GameMlbId)
            .HasConversion(v => v.Value,
                v => MlbId.Create(v));

        builder.Property(e => e.TeamMlbId)
            .IsRequired()
            .HasColumnName(Constants.PlayerBattingStatsByGames.TeamMlbId)
            .HasConversion(v => v.Value,
                v => MlbId.Create(v));

        builder.Property(e => e.PlateAppearances)
            .IsRequired()
            .HasColumnType("integer")
            .HasColumnName(Constants.PlayerBattingStatsByGames.PlateAppearances)
            .HasConversion(v => v.Value,
                v => NaturalNumber.Create(v));

        builder.Property(e => e.AtBats)
            .IsRequired()
            .HasColumnType("integer")
            .HasColumnName(Constants.PlayerBattingStatsByGames.AtBats)
            .HasConversion(v => v.Value,
                v => NaturalNumber.Create(v));

        builder.Property(e => e.Runs)
            .IsRequired()
            .HasColumnType("integer")
            .HasColumnName(Constants.PlayerBattingStatsByGames.Runs)
            .HasConversion(v => v.Value,
                v => NaturalNumber.Create(v));

        builder.Property(e => e.Hits)
            .IsRequired()
            .HasColumnType("integer")
            .HasColumnName(Constants.PlayerBattingStatsByGames.Hits)
            .HasConversion(v => v.Value,
                v => NaturalNumber.Create(v));

        builder.Property(e => e.Doubles)
            .IsRequired()
            .HasColumnType("integer")
            .HasColumnName(Constants.PlayerBattingStatsByGames.Doubles)
            .HasConversion(v => v.Value,
                v => NaturalNumber.Create(v));

        builder.Property(e => e.Triples)
            .IsRequired()
            .HasColumnType("integer")
            .HasColumnName(Constants.PlayerBattingStatsByGames.Triples)
            .HasConversion(v => v.Value,
                v => NaturalNumber.Create(v));

        builder.Property(e => e.HomeRuns)
            .IsRequired()
            .HasColumnType("integer")
            .HasColumnName(Constants.PlayerBattingStatsByGames.HomeRuns)
            .HasConversion(v => v.Value,
                v => NaturalNumber.Create(v));

        builder.Property(e => e.RunsBattedIn)
            .IsRequired()
            .HasColumnType("integer")
            .HasColumnName(Constants.PlayerBattingStatsByGames.RunsBattedIn)
            .HasConversion(v => v.Value,
                v => NaturalNumber.Create(v));

        builder.Property(e => e.BaseOnBalls)
            .IsRequired()
            .HasColumnType("integer")
            .HasColumnName(Constants.PlayerBattingStatsByGames.BaseOnBalls)
            .HasConversion(v => v.Value,
                v => NaturalNumber.Create(v));

        builder.Property(e => e.IntentionalWalks)
            .IsRequired()
            .HasColumnType("integer")
            .HasColumnName(Constants.PlayerBattingStatsByGames.IntentionalWalks)
            .HasConversion(v => v.Value,
                v => NaturalNumber.Create(v));

        builder.Property(e => e.Strikeouts)
            .IsRequired()
            .HasColumnType("integer")
            .HasColumnName(Constants.PlayerBattingStatsByGames.Strikeouts)
            .HasConversion(v => v.Value,
                v => NaturalNumber.Create(v));

        builder.Property(e => e.StolenBases)
            .IsRequired()
            .HasColumnType("integer")
            .HasColumnName(Constants.PlayerBattingStatsByGames.StolenBases)
            .HasConversion(v => v.Value,
                v => NaturalNumber.Create(v));

        builder.Property(e => e.CaughtStealing)
            .IsRequired()
            .HasColumnType("integer")
            .HasColumnName(Constants.PlayerBattingStatsByGames.CaughtStealing)
            .HasConversion(v => v.Value,
                v => NaturalNumber.Create(v));

        builder.Property(e => e.HitByPitches)
            .IsRequired()
            .HasColumnType("integer")
            .HasColumnName(Constants.PlayerBattingStatsByGames.HitByPitches)
            .HasConversion(v => v.Value,
                v => NaturalNumber.Create(v));

        builder.Property(e => e.SacrificeBunts)
            .IsRequired()
            .HasColumnType("integer")
            .HasColumnName(Constants.PlayerBattingStatsByGames.SacrificeBunts)
            .HasConversion(v => v.Value,
                v => NaturalNumber.Create(v));

        builder.Property(e => e.SacrificeFlies)
            .IsRequired()
            .HasColumnType("integer")
            .HasColumnName(Constants.PlayerBattingStatsByGames.SacrificeFlies)
            .HasConversion(v => v.Value,
                v => NaturalNumber.Create(v));

        builder.Property(e => e.NumberOfPitchesSeen)
            .IsRequired()
            .HasColumnType("integer")
            .HasColumnName(Constants.PlayerBattingStatsByGames.NumberOfPitchesSeen)
            .HasConversion(v => v.Value,
                v => NaturalNumber.Create(v));

        builder.Property(e => e.LeftOnBase)
            .IsRequired()
            .HasColumnType("integer")
            .HasColumnName(Constants.PlayerBattingStatsByGames.LeftOnBase)
            .HasConversion(v => v.Value,
                v => NaturalNumber.Create(v));

        builder.Property(e => e.GroundOuts)
            .IsRequired()
            .HasColumnType("integer")
            .HasColumnName(Constants.PlayerBattingStatsByGames.GroundOuts)
            .HasConversion(v => v.Value,
                v => NaturalNumber.Create(v));

        builder.Property(e => e.GroundIntoDoublePlays)
            .IsRequired()
            .HasColumnType("integer")
            .HasColumnName(Constants.PlayerBattingStatsByGames.GroundIntoDoublePlays)
            .HasConversion(v => v.Value,
                v => NaturalNumber.Create(v));

        builder.Property(e => e.GroundIntoTriplePlays)
            .IsRequired()
            .HasColumnType("integer")
            .HasColumnName(Constants.PlayerBattingStatsByGames.GroundIntoTriplePlays)
            .HasConversion(v => v.Value,
                v => NaturalNumber.Create(v));

        builder.Property(e => e.AirOuts)
            .IsRequired()
            .HasColumnType("integer")
            .HasColumnName(Constants.PlayerBattingStatsByGames.AirOuts)
            .HasConversion(v => v.Value,
                v => NaturalNumber.Create(v));

        builder.Property(e => e.CatcherInterferences)
            .IsRequired()
            .HasColumnType("integer")
            .HasColumnName(Constants.PlayerBattingStatsByGames.CatcherInterferences)
            .HasConversion(v => v.Value,
                v => NaturalNumber.Create(v));
    }
}