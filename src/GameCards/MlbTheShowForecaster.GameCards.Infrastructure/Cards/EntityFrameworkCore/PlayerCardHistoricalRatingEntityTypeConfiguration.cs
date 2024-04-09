using com.brettnamba.MlbTheShowForecaster.GameCards.Domain.Cards.ValueObjects;
using com.brettnamba.MlbTheShowForecaster.GameCards.Domain.Cards.ValueObjects.PlayerCards;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace com.brettnamba.MlbTheShowForecaster.GameCards.Infrastructure.Cards.EntityFrameworkCore;

public sealed class
    PlayerCardHistoricalRatingEntityTypeConfiguration : IEntityTypeConfiguration<PlayerCardHistoricalRating>
{
    public void Configure(EntityTypeBuilder<PlayerCardHistoricalRating> builder)
    {
        builder.ToTable(Constants.PlayerCardHistoricalRatings.TableName, Constants.Schema);

        builder.Property(e => e.StartDate)
            .IsRequired()
            .HasColumnType("date")
            .HasColumnName(Constants.PlayerCardHistoricalRatings.StartDate);

        builder.Property(e => e.EndDate)
            .IsRequired()
            .HasColumnType("date")
            .HasColumnName(Constants.PlayerCardHistoricalRatings.EndDate);

        builder.Property(e => e.OverallRating)
            .IsRequired()
            .HasColumnType("smallint")
            .HasColumnName(Constants.PlayerCards.OverallRating)
            .HasConversion(v => v.Value,
                v => OverallRating.Create(v));

        builder.Property(e => e.Attributes.Stamina)
            .IsRequired()
            .HasColumnType("smallint")
            .HasColumnName(Constants.PlayerCards.Stamina)
            .HasConversion(v => v.Value,
                v => AbilityAttribute.Create(v));

        builder.Property(e => e.Attributes.PitchingClutch)
            .IsRequired()
            .HasColumnType("smallint")
            .HasColumnName(Constants.PlayerCards.PitchingClutch)
            .HasConversion(v => v.Value,
                v => AbilityAttribute.Create(v));

        builder.Property(e => e.Attributes.HitsPerNine)
            .IsRequired()
            .HasColumnType("smallint")
            .HasColumnName(Constants.PlayerCards.HitsPerNine)
            .HasConversion(v => v.Value,
                v => AbilityAttribute.Create(v));

        builder.Property(e => e.Attributes.StrikeoutsPerNine)
            .IsRequired()
            .HasColumnType("smallint")
            .HasColumnName(Constants.PlayerCards.StrikeoutsPerNine)
            .HasConversion(v => v.Value,
                v => AbilityAttribute.Create(v));

        builder.Property(e => e.Attributes.BaseOnBallsPerNine)
            .IsRequired()
            .HasColumnType("smallint")
            .HasColumnName(Constants.PlayerCards.BaseOnBallsPerNine)
            .HasConversion(v => v.Value,
                v => AbilityAttribute.Create(v));

        builder.Property(e => e.Attributes.HomeRunsPerNine)
            .IsRequired()
            .HasColumnType("smallint")
            .HasColumnName(Constants.PlayerCards.HomeRunsPerNine)
            .HasConversion(v => v.Value,
                v => AbilityAttribute.Create(v));

        builder.Property(e => e.Attributes.PitchVelocity)
            .IsRequired()
            .HasColumnType("smallint")
            .HasColumnName(Constants.PlayerCards.PitchVelocity)
            .HasConversion(v => v.Value,
                v => AbilityAttribute.Create(v));

        builder.Property(e => e.Attributes.PitchControl)
            .IsRequired()
            .HasColumnType("smallint")
            .HasColumnName(Constants.PlayerCards.PitchControl)
            .HasConversion(v => v.Value,
                v => AbilityAttribute.Create(v));

        builder.Property(e => e.Attributes.PitchMovement)
            .IsRequired()
            .HasColumnType("smallint")
            .HasColumnName(Constants.PlayerCards.PitchMovement)
            .HasConversion(v => v.Value,
                v => AbilityAttribute.Create(v));

        builder.Property(e => e.Attributes.ContactLeft)
            .IsRequired()
            .HasColumnType("smallint")
            .HasColumnName(Constants.PlayerCards.ContactLeft)
            .HasConversion(v => v.Value,
                v => AbilityAttribute.Create(v));

        builder.Property(e => e.Attributes.ContactRight)
            .IsRequired()
            .HasColumnType("smallint")
            .HasColumnName(Constants.PlayerCards.ContactRight)
            .HasConversion(v => v.Value,
                v => AbilityAttribute.Create(v));

        builder.Property(e => e.Attributes.PowerLeft)
            .IsRequired()
            .HasColumnType("smallint")
            .HasColumnName(Constants.PlayerCards.PowerLeft)
            .HasConversion(v => v.Value,
                v => AbilityAttribute.Create(v));

        builder.Property(e => e.Attributes.PowerRight)
            .IsRequired()
            .HasColumnType("smallint")
            .HasColumnName(Constants.PlayerCards.PowerRight)
            .HasConversion(v => v.Value,
                v => AbilityAttribute.Create(v));

        builder.Property(e => e.Attributes.PlateVision)
            .IsRequired()
            .HasColumnType("smallint")
            .HasColumnName(Constants.PlayerCards.PlateVision)
            .HasConversion(v => v.Value,
                v => AbilityAttribute.Create(v));

        builder.Property(e => e.Attributes.PlateDiscipline)
            .IsRequired()
            .HasColumnType("smallint")
            .HasColumnName(Constants.PlayerCards.PlateDiscipline)
            .HasConversion(v => v.Value,
                v => AbilityAttribute.Create(v));

        builder.Property(e => e.Attributes.BattingClutch)
            .IsRequired()
            .HasColumnType("smallint")
            .HasColumnName(Constants.PlayerCards.BattingClutch)
            .HasConversion(v => v.Value,
                v => AbilityAttribute.Create(v));

        builder.Property(e => e.Attributes.BuntingAbility)
            .IsRequired()
            .HasColumnType("smallint")
            .HasColumnName(Constants.PlayerCards.BuntingAbility)
            .HasConversion(v => v.Value,
                v => AbilityAttribute.Create(v));

        builder.Property(e => e.Attributes.DragBuntingAbility)
            .IsRequired()
            .HasColumnType("smallint")
            .HasColumnName(Constants.PlayerCards.DragBuntingAbility)
            .HasConversion(v => v.Value,
                v => AbilityAttribute.Create(v));

        builder.Property(e => e.Attributes.HittingDurability)
            .IsRequired()
            .HasColumnType("smallint")
            .HasColumnName(Constants.PlayerCards.HittingDurability)
            .HasConversion(v => v.Value,
                v => AbilityAttribute.Create(v));

        builder.Property(e => e.Attributes.FieldingDurability)
            .IsRequired()
            .HasColumnType("smallint")
            .HasColumnName(Constants.PlayerCards.FieldingDurability)
            .HasConversion(v => v.Value,
                v => AbilityAttribute.Create(v));

        builder.Property(e => e.Attributes.FieldingAbility)
            .IsRequired()
            .HasColumnType("smallint")
            .HasColumnName(Constants.PlayerCards.FieldingAbility)
            .HasConversion(v => v.Value,
                v => AbilityAttribute.Create(v));

        builder.Property(e => e.Attributes.ArmStrength)
            .IsRequired()
            .HasColumnType("smallint")
            .HasColumnName(Constants.PlayerCards.ArmStrength)
            .HasConversion(v => v.Value,
                v => AbilityAttribute.Create(v));

        builder.Property(e => e.Attributes.ArmAccuracy)
            .IsRequired()
            .HasColumnType("smallint")
            .HasColumnName(Constants.PlayerCards.ArmAccuracy)
            .HasConversion(v => v.Value,
                v => AbilityAttribute.Create(v));

        builder.Property(e => e.Attributes.ReactionTime)
            .IsRequired()
            .HasColumnType("smallint")
            .HasColumnName(Constants.PlayerCards.ReactionTime)
            .HasConversion(v => v.Value,
                v => AbilityAttribute.Create(v));

        builder.Property(e => e.Attributes.Blocking)
            .IsRequired()
            .HasColumnType("smallint")
            .HasColumnName(Constants.PlayerCards.Blocking)
            .HasConversion(v => v.Value,
                v => AbilityAttribute.Create(v));

        builder.Property(e => e.Attributes.Speed)
            .IsRequired()
            .HasColumnType("smallint")
            .HasColumnName(Constants.PlayerCards.Speed)
            .HasConversion(v => v.Value,
                v => AbilityAttribute.Create(v));

        builder.Property(e => e.Attributes.BaseRunningAbility)
            .IsRequired()
            .HasColumnType("smallint")
            .HasColumnName(Constants.PlayerCards.BaseRunningAbility)
            .HasConversion(v => v.Value,
                v => AbilityAttribute.Create(v));

        builder.Property(e => e.Attributes.BaseRunningAggression)
            .IsRequired()
            .HasColumnType("smallint")
            .HasColumnName(Constants.PlayerCards.BaseRunningAggression)
            .HasConversion(v => v.Value,
                v => AbilityAttribute.Create(v));
    }
}