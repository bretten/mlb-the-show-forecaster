﻿using System.ComponentModel;
using com.brettnamba.MlbTheShowForecaster.Common.Domain.Enums;
using com.brettnamba.MlbTheShowForecaster.Common.Domain.ValueObjects;
using com.brettnamba.MlbTheShowForecaster.Common.Extensions;
using com.brettnamba.MlbTheShowForecaster.Performance.Domain.PlayerSeasons.ValueObjects;
using com.brettnamba.MlbTheShowForecaster.Performance.Domain.Statistics.ValueObjects.Shared;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace com.brettnamba.MlbTheShowForecaster.Performance.Infrastructure.PlayerSeasons.EntityFramework;

/// <summary>
/// Configures <see cref="PlayerFieldingStatsByGame"/> for EF
/// </summary>
public sealed class
    PlayerFieldingStatsByGameEntityTypeConfiguration : IEntityTypeConfiguration<PlayerFieldingStatsByGame>
{
    /// <summary>
    /// Configures <see cref="PlayerFieldingStatsByGame"/> for EF
    /// </summary>
    /// <param name="builder">The builder that configures the entity type</param>
    public void Configure(EntityTypeBuilder<PlayerFieldingStatsByGame> builder)
    {
        builder.ToTable(Constants.PlayerFieldingStatsByGames.TableName, Constants.Schema);

        builder.Property(e => e.PlayerMlbId)
            .IsRequired()
            .HasColumnName(Constants.PlayerFieldingStatsByGames.PlayerMlbId)
            .HasConversion(v => v.Value,
                v => MlbId.Create(v));

        builder.Property(e => e.SeasonYear)
            .IsRequired()
            .HasColumnName(Constants.PlayerFieldingStatsByGames.Season)
            .HasConversion(v => v.Value,
                v => SeasonYear.Create(v));

        builder.Property(e => e.Position)
            .IsRequired()
            .HasColumnType("varchar(4)")
            .HasColumnName(Constants.PlayerFieldingStatsByGames.Position)
            .HasConversion(v => v.GetDisplayName(),
                v => (Position)TypeDescriptor.GetConverter(typeof(Position)).ConvertFrom(v)!);

        builder.Property(e => e.GamesStarted)
            .IsRequired()
            .HasColumnType("integer")
            .HasColumnName(Constants.PlayerFieldingStatsByGames.GamesStarted)
            .HasConversion(v => v.Value,
                v => NaturalNumber.Create(v));

        builder.Property(e => e.InningsPlayed)
            .IsRequired()
            .HasColumnType("integer")
            .HasColumnName(Constants.PlayerFieldingStatsByGames.InningsPlayed)
            .HasConversion(v => v.Value,
                v => InningsCount.Create(v));

        builder.Property(e => e.Assists)
            .IsRequired()
            .HasColumnType("integer")
            .HasColumnName(Constants.PlayerFieldingStatsByGames.Assists)
            .HasConversion(v => v.Value,
                v => NaturalNumber.Create(v));

        builder.Property(e => e.PutOuts)
            .IsRequired()
            .HasColumnType("integer")
            .HasColumnName(Constants.PlayerFieldingStatsByGames.PutOuts)
            .HasConversion(v => v.Value,
                v => NaturalNumber.Create(v));

        builder.Property(e => e.Errors)
            .IsRequired()
            .HasColumnType("integer")
            .HasColumnName(Constants.PlayerFieldingStatsByGames.Errors)
            .HasConversion(v => v.Value,
                v => NaturalNumber.Create(v));

        builder.Property(e => e.ThrowingErrors)
            .IsRequired()
            .HasColumnType("integer")
            .HasColumnName(Constants.PlayerFieldingStatsByGames.ThrowingErrors)
            .HasConversion(v => v.Value,
                v => NaturalNumber.Create(v));

        builder.Property(e => e.DoublePlays)
            .IsRequired()
            .HasColumnType("integer")
            .HasColumnName(Constants.PlayerFieldingStatsByGames.DoublePlays)
            .HasConversion(v => v.Value,
                v => NaturalNumber.Create(v));

        builder.Property(e => e.TriplePlays)
            .IsRequired()
            .HasColumnType("integer")
            .HasColumnName(Constants.PlayerFieldingStatsByGames.TriplePlays)
            .HasConversion(v => v.Value,
                v => NaturalNumber.Create(v));
    }
}