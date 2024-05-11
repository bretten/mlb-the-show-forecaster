using System.ComponentModel;
using com.brettnamba.MlbTheShowForecaster.Common.Domain.Enums;
using com.brettnamba.MlbTheShowForecaster.Common.Domain.ValueObjects;
using com.brettnamba.MlbTheShowForecaster.Common.Extensions;
using com.brettnamba.MlbTheShowForecaster.PlayerStatus.Domain.Players.Entities;
using com.brettnamba.MlbTheShowForecaster.PlayerStatus.Domain.Players.ValueObjects;
using com.brettnamba.MlbTheShowForecaster.PlayerStatus.Domain.Teams.Services;
using com.brettnamba.MlbTheShowForecaster.PlayerStatus.Domain.Teams.ValueObjects;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace com.brettnamba.MlbTheShowForecaster.PlayerStatus.Infrastructure.Players.EntityFramework;

/// <summary>
/// Configures <see cref="Player"/> for EF Core
/// </summary>
public sealed class PlayerEntityTypeConfiguration : IEntityTypeConfiguration<Player>
{
    /// <summary>
    /// Converts team name to team abbreviation and vice-versa
    /// </summary>
    private readonly ITeamProvider _teamProvider;

    /// <summary>
    /// Constructor
    /// </summary>
    /// <param name="teamProvider">Converts team name to team abbreviation and vice-versa</param>
    public PlayerEntityTypeConfiguration(ITeamProvider teamProvider)
    {
        _teamProvider = teamProvider;
    }

    /// <summary>
    /// Configures <see cref="Player"/> for EF Core
    /// </summary>
    /// <param name="builder">The builder that configures the entity type</param>
    public void Configure(EntityTypeBuilder<Player> builder)
    {
        builder.ToTable(Constants.Players.TableName, Constants.Schema);

        builder.HasKey(e => e.Id)
            .HasName(Constants.Players.Keys.PrimaryKey);

        // Unique constraint for MLB ID
        builder.HasAlternateKey(e => e.MlbId)
            .HasName(Constants.Players.Keys.MlbIdKey);

        var columnOrder = 0;

        builder.Property(e => e.Id)
            .IsRequired()
            .HasColumnName(Constants.Players.Id)
            .HasColumnOrder(columnOrder++);

        builder.Property(e => e.MlbId)
            .IsRequired()
            .HasColumnName(Constants.Players.MlbId)
            .HasColumnOrder(columnOrder++)
            .HasConversion(v => v.Value,
                v => MlbId.Create(v));

        builder.Property(e => e.FirstName)
            .IsRequired()
            .HasColumnName(Constants.Players.FirstName)
            .HasColumnOrder(columnOrder++)
            .HasConversion(v => v.Value,
                v => PersonName.Create(v));

        builder.Property(e => e.LastName)
            .IsRequired()
            .HasColumnName(Constants.Players.LastName)
            .HasColumnOrder(columnOrder++)
            .HasConversion(v => v.Value,
                v => PersonName.Create(v));

        builder.Property(e => e.Birthdate)
            .IsRequired()
            .HasColumnType("date")
            .HasColumnName(Constants.Players.Birthdate)
            .HasColumnOrder(columnOrder++);

        builder.Property(e => e.Position)
            .IsRequired()
            .HasColumnType("varchar(4)")
            .HasColumnName(Constants.Players.Position)
            .HasColumnOrder(columnOrder++)
            .HasConversion(v => v.GetDisplayName(),
                v => (Position)TypeDescriptor.GetConverter(typeof(Position)).ConvertFrom(v)!);

        builder.Property(e => e.MlbDebutDate)
            .IsRequired()
            .HasColumnType("date")
            .HasColumnName(Constants.Players.MlbDebutDate)
            .HasColumnOrder(columnOrder++);

        builder.Property(e => e.BatSide)
            .IsRequired()
            .HasColumnType("varchar(4)")
            .HasColumnName(Constants.Players.BatSide)
            .HasColumnOrder(columnOrder++)
            .HasConversion(v => v.GetDisplayName(),
                v => (BatSide)TypeDescriptor.GetConverter(typeof(BatSide)).ConvertFrom(v)!);

        builder.Property(e => e.ThrowArm)
            .IsRequired()
            .HasColumnType("varchar(4)")
            .HasColumnName(Constants.Players.ThrowArm)
            .HasColumnOrder(columnOrder++)
            .HasConversion(v => v.GetDisplayName(),
                v => (ThrowArm)TypeDescriptor.GetConverter(typeof(ThrowArm)).ConvertFrom(v)!);

        builder.Property(e => e.Team)
            .IsRequired()
            .HasColumnType("varchar(4)")
            .HasColumnName(Constants.Players.Team)
            .HasColumnOrder(columnOrder++)
            .HasConversion(v => v != null ? v.Abbreviation.Value : null,
                v => v != null ? _teamProvider.GetBy(TeamAbbreviation.Create(v)) : null);

        builder.Property(e => e.Active)
            .IsRequired()
            .HasColumnName(Constants.Players.Active)
            .HasColumnOrder(columnOrder++);
    }
}