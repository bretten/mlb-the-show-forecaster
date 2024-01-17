using System.ComponentModel;
using com.brettnamba.MlbTheShowForecaster.Common.Extensions;
using com.brettnamba.MlbTheShowForecaster.PlayerStatus.Domain.Common.ValueObjects;
using com.brettnamba.MlbTheShowForecaster.PlayerStatus.Domain.Players.Entities;
using com.brettnamba.MlbTheShowForecaster.PlayerStatus.Domain.Players.Enums;
using com.brettnamba.MlbTheShowForecaster.PlayerStatus.Domain.Players.ValueObjects;
using com.brettnamba.MlbTheShowForecaster.PlayerStatus.Domain.Teams.Services;
using com.brettnamba.MlbTheShowForecaster.PlayerStatus.Domain.Teams.ValueObjects;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace com.brettnamba.MlbTheShowForecaster.PlayerStatus.Infrastructure.Players.EntityFramework;

public sealed class PlayerEntityTypeConfiguration : IEntityTypeConfiguration<Player>
{
    private readonly ITeamProvider _teamProvider;

    public PlayerEntityTypeConfiguration(ITeamProvider teamProvider)
    {
        _teamProvider = teamProvider;
    }

    public void Configure(EntityTypeBuilder<Player> builder)
    {
        builder.ToTable(Constants.Players.TableName, Constants.Schema);

        builder.Property(e => e.Id)
            .IsRequired()
            .HasColumnName(Constants.Players.Id);

        builder.Property(e => e.MlbId)
            .IsRequired()
            .HasColumnName(Constants.Players.MlbId)
            .HasConversion(v => v.Value,
                v => MlbId.Create(v));

        builder.Property(e => e.FirstName)
            .IsRequired()
            .HasColumnName(Constants.Players.FirstName)
            .HasConversion(v => v.Value,
                v => PersonName.Create(v));

        builder.Property(e => e.LastName)
            .IsRequired()
            .HasColumnName(Constants.Players.LastName)
            .HasConversion(v => v.Value,
                v => PersonName.Create(v));

        builder.Property(e => e.Birthdate)
            .IsRequired()
            .HasColumnType("date")
            .HasColumnName(Constants.Players.Birthdate);

        builder.Property(e => e.Position)
            .IsRequired()
            .HasColumnType("varchar(4)")
            .HasColumnName(Constants.Players.Position)
            .HasConversion(v => v.GetDisplayName(),
                v => (Position)TypeDescriptor.GetConverter(typeof(Position)).ConvertFrom(v)!);

        builder.Property(e => e.MlbDebutDate)
            .IsRequired()
            .HasColumnType("date")
            .HasColumnName(Constants.Players.MlbDebutDate);

        builder.Property(e => e.BatSide)
            .IsRequired()
            .HasColumnType("varchar(4)")
            .HasColumnName(Constants.Players.BatSide)
            .HasConversion(v => v.GetDisplayName(),
                v => (BatSide)TypeDescriptor.GetConverter(typeof(BatSide)).ConvertFrom(v)!);

        builder.Property(e => e.ThrowArm)
            .IsRequired()
            .HasColumnType("varchar(4)")
            .HasColumnName(Constants.Players.ThrowArm)
            .HasConversion(v => v.GetDisplayName(),
                v => (ThrowArm)TypeDescriptor.GetConverter(typeof(ThrowArm)).ConvertFrom(v)!);

        builder.Property(e => e.Team)
            .IsRequired()
            .HasColumnType("varchar(4)")
            .HasColumnName(Constants.Players.Team)
            .HasConversion(v => v != null ? v.Abbreviation.Value : null,
                v => v != null ? _teamProvider.GetBy(TeamAbbreviation.Create(v)) : null);

        builder.Property(e => e.Active)
            .IsRequired()
            .HasColumnName(Constants.Players.Active);
    }
}