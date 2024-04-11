using com.brettnamba.MlbTheShowForecaster.GameCards.Domain.Cards.Entities;
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

        builder.HasOne<PlayerCard>()
            .WithMany("_historicalRatings")
            .HasForeignKey(Constants.PlayerCardHistoricalRatings.PlayerCardId)
            .HasConstraintName(Constants.PlayerCardHistoricalRatings.ForeignKeys.PlayerCardsConstraint);

        builder.HasKey([
            Constants.PlayerCardHistoricalRatings.PlayerCardId, nameof(PlayerCardHistoricalRating.StartDate),
            nameof(PlayerCardHistoricalRating.EndDate)
        ]).HasName(Constants.PlayerCardHistoricalRatings.PrimaryKeyName);

        var columnOrder = 0;

        builder.Property(Constants.PlayerCardHistoricalRatings.PlayerCardId)
            .HasColumnOrder(columnOrder++);

        builder.Property(e => e.StartDate)
            .IsRequired()
            .HasColumnType("date")
            .HasColumnName(Constants.PlayerCardHistoricalRatings.StartDate)
            .HasColumnOrder(columnOrder++);

        builder.Property(e => e.EndDate)
            .IsRequired()
            .HasColumnType("date")
            .HasColumnName(Constants.PlayerCardHistoricalRatings.EndDate)
            .HasColumnOrder(columnOrder++);

        builder.Property(e => e.OverallRating)
            .IsRequired()
            .HasColumnType("smallint")
            .HasColumnName(Constants.PlayerCards.OverallRating)
            .HasColumnOrder(columnOrder++)
            .HasConversion(v => v.Value,
                v => OverallRating.Create(v));

        builder.ComplexProperty(e => e.Attributes,
            b =>
            {
                b.Property(e => e.Stamina)
                    .IsRequired()
                    .HasColumnType("smallint")
                    .HasColumnName(Constants.PlayerCards.Stamina)
                    .HasColumnOrder(columnOrder++)
                    .HasConversion(v => v.Value,
                        v => AbilityAttribute.Create(v));

                b.Property(e => e.PitchingClutch)
                    .IsRequired()
                    .HasColumnType("smallint")
                    .HasColumnName(Constants.PlayerCards.PitchingClutch)
                    .HasColumnOrder(columnOrder++)
                    .HasConversion(v => v.Value,
                        v => AbilityAttribute.Create(v));

                b.Property(e => e.HitsPerNine)
                    .IsRequired()
                    .HasColumnType("smallint")
                    .HasColumnName(Constants.PlayerCards.HitsPerNine)
                    .HasColumnOrder(columnOrder++)
                    .HasConversion(v => v.Value,
                        v => AbilityAttribute.Create(v));

                b.Property(e => e.StrikeoutsPerNine)
                    .IsRequired()
                    .HasColumnType("smallint")
                    .HasColumnName(Constants.PlayerCards.StrikeoutsPerNine)
                    .HasColumnOrder(columnOrder++)
                    .HasConversion(v => v.Value,
                        v => AbilityAttribute.Create(v));

                b.Property(e => e.BaseOnBallsPerNine)
                    .IsRequired()
                    .HasColumnType("smallint")
                    .HasColumnName(Constants.PlayerCards.BaseOnBallsPerNine)
                    .HasColumnOrder(columnOrder++)
                    .HasConversion(v => v.Value,
                        v => AbilityAttribute.Create(v));

                b.Property(e => e.HomeRunsPerNine)
                    .IsRequired()
                    .HasColumnType("smallint")
                    .HasColumnName(Constants.PlayerCards.HomeRunsPerNine)
                    .HasColumnOrder(columnOrder++)
                    .HasConversion(v => v.Value,
                        v => AbilityAttribute.Create(v));

                b.Property(e => e.PitchVelocity)
                    .IsRequired()
                    .HasColumnType("smallint")
                    .HasColumnName(Constants.PlayerCards.PitchVelocity)
                    .HasColumnOrder(columnOrder++)
                    .HasConversion(v => v.Value,
                        v => AbilityAttribute.Create(v));

                b.Property(e => e.PitchControl)
                    .IsRequired()
                    .HasColumnType("smallint")
                    .HasColumnName(Constants.PlayerCards.PitchControl)
                    .HasColumnOrder(columnOrder++)
                    .HasConversion(v => v.Value,
                        v => AbilityAttribute.Create(v));

                b.Property(e => e.PitchMovement)
                    .IsRequired()
                    .HasColumnType("smallint")
                    .HasColumnName(Constants.PlayerCards.PitchMovement)
                    .HasColumnOrder(columnOrder++)
                    .HasConversion(v => v.Value,
                        v => AbilityAttribute.Create(v));

                b.Property(e => e.ContactLeft)
                    .IsRequired()
                    .HasColumnType("smallint")
                    .HasColumnName(Constants.PlayerCards.ContactLeft)
                    .HasColumnOrder(columnOrder++)
                    .HasConversion(v => v.Value,
                        v => AbilityAttribute.Create(v));

                b.Property(e => e.ContactRight)
                    .IsRequired()
                    .HasColumnType("smallint")
                    .HasColumnName(Constants.PlayerCards.ContactRight)
                    .HasColumnOrder(columnOrder++)
                    .HasConversion(v => v.Value,
                        v => AbilityAttribute.Create(v));

                b.Property(e => e.PowerLeft)
                    .IsRequired()
                    .HasColumnType("smallint")
                    .HasColumnName(Constants.PlayerCards.PowerLeft)
                    .HasColumnOrder(columnOrder++)
                    .HasConversion(v => v.Value,
                        v => AbilityAttribute.Create(v));

                b.Property(e => e.PowerRight)
                    .IsRequired()
                    .HasColumnType("smallint")
                    .HasColumnName(Constants.PlayerCards.PowerRight)
                    .HasColumnOrder(columnOrder++)
                    .HasConversion(v => v.Value,
                        v => AbilityAttribute.Create(v));

                b.Property(e => e.PlateVision)
                    .IsRequired()
                    .HasColumnType("smallint")
                    .HasColumnName(Constants.PlayerCards.PlateVision)
                    .HasColumnOrder(columnOrder++)
                    .HasConversion(v => v.Value,
                        v => AbilityAttribute.Create(v));

                b.Property(e => e.PlateDiscipline)
                    .IsRequired()
                    .HasColumnType("smallint")
                    .HasColumnName(Constants.PlayerCards.PlateDiscipline)
                    .HasColumnOrder(columnOrder++)
                    .HasConversion(v => v.Value,
                        v => AbilityAttribute.Create(v));

                b.Property(e => e.BattingClutch)
                    .IsRequired()
                    .HasColumnType("smallint")
                    .HasColumnName(Constants.PlayerCards.BattingClutch)
                    .HasColumnOrder(columnOrder++)
                    .HasConversion(v => v.Value,
                        v => AbilityAttribute.Create(v));

                b.Property(e => e.BuntingAbility)
                    .IsRequired()
                    .HasColumnType("smallint")
                    .HasColumnName(Constants.PlayerCards.BuntingAbility)
                    .HasColumnOrder(columnOrder++)
                    .HasConversion(v => v.Value,
                        v => AbilityAttribute.Create(v));

                b.Property(e => e.DragBuntingAbility)
                    .IsRequired()
                    .HasColumnType("smallint")
                    .HasColumnName(Constants.PlayerCards.DragBuntingAbility)
                    .HasColumnOrder(columnOrder++)
                    .HasConversion(v => v.Value,
                        v => AbilityAttribute.Create(v));

                b.Property(e => e.HittingDurability)
                    .IsRequired()
                    .HasColumnType("smallint")
                    .HasColumnName(Constants.PlayerCards.HittingDurability)
                    .HasColumnOrder(columnOrder++)
                    .HasConversion(v => v.Value,
                        v => AbilityAttribute.Create(v));

                b.Property(e => e.FieldingDurability)
                    .IsRequired()
                    .HasColumnType("smallint")
                    .HasColumnName(Constants.PlayerCards.FieldingDurability)
                    .HasColumnOrder(columnOrder++)
                    .HasConversion(v => v.Value,
                        v => AbilityAttribute.Create(v));

                b.Property(e => e.FieldingAbility)
                    .IsRequired()
                    .HasColumnType("smallint")
                    .HasColumnName(Constants.PlayerCards.FieldingAbility)
                    .HasColumnOrder(columnOrder++)
                    .HasConversion(v => v.Value,
                        v => AbilityAttribute.Create(v));

                b.Property(e => e.ArmStrength)
                    .IsRequired()
                    .HasColumnType("smallint")
                    .HasColumnName(Constants.PlayerCards.ArmStrength)
                    .HasColumnOrder(columnOrder++)
                    .HasConversion(v => v.Value,
                        v => AbilityAttribute.Create(v));

                b.Property(e => e.ArmAccuracy)
                    .IsRequired()
                    .HasColumnType("smallint")
                    .HasColumnName(Constants.PlayerCards.ArmAccuracy)
                    .HasColumnOrder(columnOrder++)
                    .HasConversion(v => v.Value,
                        v => AbilityAttribute.Create(v));

                b.Property(e => e.ReactionTime)
                    .IsRequired()
                    .HasColumnType("smallint")
                    .HasColumnName(Constants.PlayerCards.ReactionTime)
                    .HasColumnOrder(columnOrder++)
                    .HasConversion(v => v.Value,
                        v => AbilityAttribute.Create(v));

                b.Property(e => e.Blocking)
                    .IsRequired()
                    .HasColumnType("smallint")
                    .HasColumnName(Constants.PlayerCards.Blocking)
                    .HasColumnOrder(columnOrder++)
                    .HasConversion(v => v.Value,
                        v => AbilityAttribute.Create(v));

                b.Property(e => e.Speed)
                    .IsRequired()
                    .HasColumnType("smallint")
                    .HasColumnName(Constants.PlayerCards.Speed)
                    .HasColumnOrder(columnOrder++)
                    .HasConversion(v => v.Value,
                        v => AbilityAttribute.Create(v));

                b.Property(e => e.BaseRunningAbility)
                    .IsRequired()
                    .HasColumnType("smallint")
                    .HasColumnName(Constants.PlayerCards.BaseRunningAbility)
                    .HasColumnOrder(columnOrder++)
                    .HasConversion(v => v.Value,
                        v => AbilityAttribute.Create(v));

                b.Property(e => e.BaseRunningAggression)
                    .IsRequired()
                    .HasColumnType("smallint")
                    .HasColumnName(Constants.PlayerCards.BaseRunningAggression)
                    .HasColumnOrder(columnOrder++)
                    .HasConversion(v => v.Value,
                        v => AbilityAttribute.Create(v));
            });
    }
}