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

        builder.HasKey(e => e.Id)
            .HasName(Constants.PlayerCards.PrimaryKeyName);

        // Index for querying by game year and then the card's external ID
        builder.HasIndex(e => new { e.Year, e.ExternalId }, Constants.PlayerCards.Indexes.YearAndExternalId)
            .HasMethod("btree");

        var columnOrder = 0;

        builder.Property(e => e.Id)
            .IsRequired()
            .HasColumnType("uuid")
            .HasColumnName(Constants.Cards.Id)
            .HasColumnOrder(columnOrder++);

        builder.Property(e => e.Year)
            .IsRequired()
            .HasColumnType("smallint")
            .HasColumnName(Constants.Cards.Year)
            .HasColumnOrder(columnOrder++)
            .HasConversion(v => v.Value,
                v => SeasonYear.Create(v));

        builder.Property(e => e.ExternalId)
            .IsRequired()
            .HasColumnType("uuid")
            .HasColumnName(Constants.Cards.ExternalId)
            .HasColumnOrder(columnOrder++)
            .HasConversion(v => v.Value,
                v => CardExternalId.Create(v));

        builder.Property(e => e.Type)
            .IsRequired()
            .HasColumnType("varchar(12)")
            .HasColumnName(Constants.Cards.Type)
            .HasColumnOrder(columnOrder++)
            .HasConversion(v => v.GetDisplayName(),
                v => (CardType)TypeDescriptor.GetConverter(typeof(CardType)).ConvertFrom(v)!);

        builder.Property(e => e.ImageLocation)
            .IsRequired()
            .HasColumnName(Constants.Cards.ImageLocation)
            .HasColumnOrder(columnOrder++)
            .HasConversion(v => v.Value.OriginalString,
                v => CardImageLocation.Create(v));

        builder.Property(e => e.Name)
            .IsRequired()
            .HasColumnName(Constants.Cards.Name)
            .HasColumnOrder(columnOrder++)
            .HasConversion(v => v.Value,
                v => CardName.Create(v));

        builder.Property(e => e.Rarity)
            .IsRequired()
            .HasColumnType("varchar(8)")
            .HasColumnName(Constants.Cards.Rarity)
            .HasColumnOrder(columnOrder++)
            .HasConversion(v => v.GetDisplayName(),
                v => (Rarity)TypeDescriptor.GetConverter(typeof(Rarity)).ConvertFrom(v)!);

        builder.Property(e => e.Series)
            .IsRequired()
            .HasColumnType("varchar(8)")
            .HasColumnName(Constants.Cards.Series)
            .HasColumnOrder(columnOrder++)
            .HasConversion(v => v.GetDisplayName(),
                v => (CardSeries)TypeDescriptor.GetConverter(typeof(CardSeries)).ConvertFrom(v)!);

        builder.Property(e => e.Position)
            .IsRequired()
            .HasColumnType("varchar(4)")
            .HasColumnName(Constants.PlayerCards.Position)
            .HasColumnOrder(columnOrder++)
            .HasConversion(v => v.GetDisplayName(),
                v => (Position)TypeDescriptor.GetConverter(typeof(Position)).ConvertFrom(v)!);

        builder.Property(e => e.TeamShortName)
            .IsRequired()
            .HasColumnType("varchar(4)")
            .HasColumnName(Constants.PlayerCards.TeamShortName)
            .HasColumnOrder(columnOrder++)
            .HasConversion(v => v.Value,
                v => TeamShortName.Create(v));

        builder.Property(e => e.OverallRating)
            .IsRequired()
            .HasColumnType("smallint")
            .HasColumnName(Constants.PlayerCards.OverallRating)
            .HasColumnOrder(columnOrder++)
            .HasConversion(v => v.Value,
                v => OverallRating.Create(v));

        builder.ComplexProperty(e => e.PlayerCardAttributes,
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

        // Ignore these properties. They are not relationships/navigation properties, but just convenience methods for other members of the class
        builder.Ignore(x => x.HistoricalRatingsChronologically);

        // Relation is defined on PlayerCardHistoricalRating end
        // builder.HasMany<PlayerCardHistoricalRating>("_historicalRatings")
        //     .WithOne()
        //     .HasForeignKey(Constants.PlayerCardHistoricalRatings.PlayerCardId)
        //     .IsRequired();
    }
}