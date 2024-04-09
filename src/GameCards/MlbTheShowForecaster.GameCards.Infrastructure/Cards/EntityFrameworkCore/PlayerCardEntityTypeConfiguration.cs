using System.ComponentModel;
using com.brettnamba.MlbTheShowForecaster.Common.Domain.Enums;
using com.brettnamba.MlbTheShowForecaster.Common.Domain.ValueObjects;
using com.brettnamba.MlbTheShowForecaster.Common.Extensions;
using com.brettnamba.MlbTheShowForecaster.GameCards.Domain.Cards.Entities;
using com.brettnamba.MlbTheShowForecaster.GameCards.Domain.Cards.Enums;
using com.brettnamba.MlbTheShowForecaster.GameCards.Domain.Cards.ValueObjects;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace com.brettnamba.MlbTheShowForecaster.GameCards.Infrastructure.Cards.EntityFrameworkCore;

public sealed class PlayerCardEntityTypeConfiguration : IEntityTypeConfiguration<PlayerCard>
{
    public void Configure(EntityTypeBuilder<PlayerCard> builder)
    {
        builder.ToTable(Constants.PlayerCards.TableName, Constants.Schema);

        builder.Property(e => e.Id)
            .IsRequired()
            .HasColumnName(Constants.Cards.Id);

        builder.Property(e => e.Year)
            .IsRequired()
            .HasColumnName(Constants.Cards.Year)
            .HasConversion(v => v.Value,
                v => SeasonYear.Create(v));

        builder.Property(e => e.ExternalId)
            .IsRequired()
            .HasColumnName(Constants.Cards.ExternalId)
            .HasConversion(v => v.Value,
                v => CardExternalId.Create(v));

        builder.Property(e => e.Type)
            .IsRequired()
            .HasColumnType("varchar(12)")
            .HasColumnName(Constants.Cards.Type)
            .HasConversion(v => v.GetDisplayName(),
                v => (CardType)TypeDescriptor.GetConverter(typeof(CardType)).ConvertFrom(v)!);

        builder.Property(e => e.ImageLocation)
            .IsRequired()
            .HasColumnName(Constants.Cards.ImageLocation)
            .HasConversion(v => v.Value.OriginalString,
                v => CardImageLocation.Create(v));

        builder.Property(e => e.Name)
            .IsRequired()
            .HasColumnName(Constants.Cards.Name)
            .HasConversion(v => v.Value,
                v => CardName.Create(v));

        builder.Property(e => e.Rarity)
            .IsRequired()
            .HasColumnType("varchar(8)")
            .HasColumnName(Constants.Cards.Rarity)
            .HasConversion(v => v.GetDisplayName(),
                v => (Rarity)TypeDescriptor.GetConverter(typeof(Rarity)).ConvertFrom(v)!);

        builder.Property(e => e.Series)
            .IsRequired()
            .HasColumnType("varchar(8)")
            .HasColumnName(Constants.Cards.Series)
            .HasConversion(v => v.GetDisplayName(),
                v => (CardSeries)TypeDescriptor.GetConverter(typeof(CardSeries)).ConvertFrom(v)!);

        builder.Property(e => e.Position)
            .IsRequired()
            .HasColumnType("varchar(4)")
            .HasColumnName(Constants.PlayerCards.Position)
            .HasConversion(v => v.GetDisplayName(),
                v => (Position)TypeDescriptor.GetConverter(typeof(Position)).ConvertFrom(v)!);

        builder.Property(e => e.TeamShortName)
            .IsRequired()
            .HasColumnType("varchar(4)")
            .HasColumnName(Constants.PlayerCards.TeamShortName)
            .HasConversion(v => v.Value,
                v => TeamShortName.Create(v));

        builder.Property(e => e.OverallRating)
            .IsRequired()
            .HasColumnType("smallint")
            .HasColumnName(Constants.PlayerCards.OverallRating)
            .HasConversion(v => v.Value,
                v => OverallRating.Create(v));

        builder.Property(e => e.PlayerCardAttributes.Stamina)
            .IsRequired()
            .HasColumnType("smallint")
            .HasColumnName(Constants.PlayerCards.Stamina)
            .HasConversion(v => v.Value,
                v => AbilityAttribute.Create(v));

        builder.Property(e => e.PlayerCardAttributes.PitchingClutch)
            .IsRequired()
            .HasColumnType("smallint")
            .HasColumnName(Constants.PlayerCards.PitchingClutch)
            .HasConversion(v => v.Value,
                v => AbilityAttribute.Create(v));

        builder.Property(e => e.PlayerCardAttributes.HitsPerNine)
            .IsRequired()
            .HasColumnType("smallint")
            .HasColumnName(Constants.PlayerCards.HitsPerNine)
            .HasConversion(v => v.Value,
                v => AbilityAttribute.Create(v));

        builder.Property(e => e.PlayerCardAttributes.StrikeoutsPerNine)
            .IsRequired()
            .HasColumnType("smallint")
            .HasColumnName(Constants.PlayerCards.StrikeoutsPerNine)
            .HasConversion(v => v.Value,
                v => AbilityAttribute.Create(v));

        builder.Property(e => e.PlayerCardAttributes.BaseOnBallsPerNine)
            .IsRequired()
            .HasColumnType("smallint")
            .HasColumnName(Constants.PlayerCards.BaseOnBallsPerNine)
            .HasConversion(v => v.Value,
                v => AbilityAttribute.Create(v));

        builder.Property(e => e.PlayerCardAttributes.HomeRunsPerNine)
            .IsRequired()
            .HasColumnType("smallint")
            .HasColumnName(Constants.PlayerCards.HomeRunsPerNine)
            .HasConversion(v => v.Value,
                v => AbilityAttribute.Create(v));

        builder.Property(e => e.PlayerCardAttributes.PitchVelocity)
            .IsRequired()
            .HasColumnType("smallint")
            .HasColumnName(Constants.PlayerCards.PitchVelocity)
            .HasConversion(v => v.Value,
                v => AbilityAttribute.Create(v));

        builder.Property(e => e.PlayerCardAttributes.PitchControl)
            .IsRequired()
            .HasColumnType("smallint")
            .HasColumnName(Constants.PlayerCards.PitchControl)
            .HasConversion(v => v.Value,
                v => AbilityAttribute.Create(v));

        builder.Property(e => e.PlayerCardAttributes.PitchMovement)
            .IsRequired()
            .HasColumnType("smallint")
            .HasColumnName(Constants.PlayerCards.PitchMovement)
            .HasConversion(v => v.Value,
                v => AbilityAttribute.Create(v));

        builder.Property(e => e.PlayerCardAttributes.ContactLeft)
            .IsRequired()
            .HasColumnType("smallint")
            .HasColumnName(Constants.PlayerCards.ContactLeft)
            .HasConversion(v => v.Value,
                v => AbilityAttribute.Create(v));

        builder.Property(e => e.PlayerCardAttributes.ContactRight)
            .IsRequired()
            .HasColumnType("smallint")
            .HasColumnName(Constants.PlayerCards.ContactRight)
            .HasConversion(v => v.Value,
                v => AbilityAttribute.Create(v));

        builder.Property(e => e.PlayerCardAttributes.PowerLeft)
            .IsRequired()
            .HasColumnType("smallint")
            .HasColumnName(Constants.PlayerCards.PowerLeft)
            .HasConversion(v => v.Value,
                v => AbilityAttribute.Create(v));

        builder.Property(e => e.PlayerCardAttributes.PowerRight)
            .IsRequired()
            .HasColumnType("smallint")
            .HasColumnName(Constants.PlayerCards.PowerRight)
            .HasConversion(v => v.Value,
                v => AbilityAttribute.Create(v));

        builder.Property(e => e.PlayerCardAttributes.PlateVision)
            .IsRequired()
            .HasColumnType("smallint")
            .HasColumnName(Constants.PlayerCards.PlateVision)
            .HasConversion(v => v.Value,
                v => AbilityAttribute.Create(v));

        builder.Property(e => e.PlayerCardAttributes.PlateDiscipline)
            .IsRequired()
            .HasColumnType("smallint")
            .HasColumnName(Constants.PlayerCards.PlateDiscipline)
            .HasConversion(v => v.Value,
                v => AbilityAttribute.Create(v));

        builder.Property(e => e.PlayerCardAttributes.BattingClutch)
            .IsRequired()
            .HasColumnType("smallint")
            .HasColumnName(Constants.PlayerCards.BattingClutch)
            .HasConversion(v => v.Value,
                v => AbilityAttribute.Create(v));

        builder.Property(e => e.PlayerCardAttributes.BuntingAbility)
            .IsRequired()
            .HasColumnType("smallint")
            .HasColumnName(Constants.PlayerCards.BuntingAbility)
            .HasConversion(v => v.Value,
                v => AbilityAttribute.Create(v));

        builder.Property(e => e.PlayerCardAttributes.DragBuntingAbility)
            .IsRequired()
            .HasColumnType("smallint")
            .HasColumnName(Constants.PlayerCards.DragBuntingAbility)
            .HasConversion(v => v.Value,
                v => AbilityAttribute.Create(v));

        builder.Property(e => e.PlayerCardAttributes.HittingDurability)
            .IsRequired()
            .HasColumnType("smallint")
            .HasColumnName(Constants.PlayerCards.HittingDurability)
            .HasConversion(v => v.Value,
                v => AbilityAttribute.Create(v));

        builder.Property(e => e.PlayerCardAttributes.FieldingDurability)
            .IsRequired()
            .HasColumnType("smallint")
            .HasColumnName(Constants.PlayerCards.FieldingDurability)
            .HasConversion(v => v.Value,
                v => AbilityAttribute.Create(v));

        builder.Property(e => e.PlayerCardAttributes.FieldingAbility)
            .IsRequired()
            .HasColumnType("smallint")
            .HasColumnName(Constants.PlayerCards.FieldingAbility)
            .HasConversion(v => v.Value,
                v => AbilityAttribute.Create(v));

        builder.Property(e => e.PlayerCardAttributes.ArmStrength)
            .IsRequired()
            .HasColumnType("smallint")
            .HasColumnName(Constants.PlayerCards.ArmStrength)
            .HasConversion(v => v.Value,
                v => AbilityAttribute.Create(v));

        builder.Property(e => e.PlayerCardAttributes.ArmAccuracy)
            .IsRequired()
            .HasColumnType("smallint")
            .HasColumnName(Constants.PlayerCards.ArmAccuracy)
            .HasConversion(v => v.Value,
                v => AbilityAttribute.Create(v));

        builder.Property(e => e.PlayerCardAttributes.ReactionTime)
            .IsRequired()
            .HasColumnType("smallint")
            .HasColumnName(Constants.PlayerCards.ReactionTime)
            .HasConversion(v => v.Value,
                v => AbilityAttribute.Create(v));

        builder.Property(e => e.PlayerCardAttributes.Blocking)
            .IsRequired()
            .HasColumnType("smallint")
            .HasColumnName(Constants.PlayerCards.Blocking)
            .HasConversion(v => v.Value,
                v => AbilityAttribute.Create(v));

        builder.Property(e => e.PlayerCardAttributes.Speed)
            .IsRequired()
            .HasColumnType("smallint")
            .HasColumnName(Constants.PlayerCards.Speed)
            .HasConversion(v => v.Value,
                v => AbilityAttribute.Create(v));

        builder.Property(e => e.PlayerCardAttributes.BaseRunningAbility)
            .IsRequired()
            .HasColumnType("smallint")
            .HasColumnName(Constants.PlayerCards.BaseRunningAbility)
            .HasConversion(v => v.Value,
                v => AbilityAttribute.Create(v));

        builder.Property(e => e.PlayerCardAttributes.BaseRunningAggression)
            .IsRequired()
            .HasColumnType("smallint")
            .HasColumnName(Constants.PlayerCards.BaseRunningAggression)
            .HasConversion(v => v.Value,
                v => AbilityAttribute.Create(v));
    }
}