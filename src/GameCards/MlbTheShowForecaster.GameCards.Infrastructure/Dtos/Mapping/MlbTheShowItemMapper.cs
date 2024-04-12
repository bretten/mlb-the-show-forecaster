using System.ComponentModel;
using com.brettnamba.MlbTheShowForecaster.Common.Domain.Enums;
using com.brettnamba.MlbTheShowForecaster.Common.Domain.ValueObjects;
using com.brettnamba.MlbTheShowForecaster.ExternalApis.MlbTheShowApi.Dtos.Items;
using com.brettnamba.MlbTheShowForecaster.GameCards.Application.Dtos;
using com.brettnamba.MlbTheShowForecaster.GameCards.Domain.Cards.Enums;
using com.brettnamba.MlbTheShowForecaster.GameCards.Domain.Cards.ValueObjects;
using com.brettnamba.MlbTheShowForecaster.GameCards.Infrastructure.Dtos.Mapping.Exceptions;

namespace com.brettnamba.MlbTheShowForecaster.GameCards.Infrastructure.Dtos.Mapping;

/// <summary>
/// Mapper that maps <see cref="ItemDto"/>s from MLB The Show to application-level DTOs
/// </summary>
public sealed class MlbTheShowItemMapper : IMlbTheShowItemMapper
{
    /// <summary>
    /// Maps a MLB The Show <see cref="ItemDto"/> to an application <see cref="MlbPlayerCard"/>
    /// </summary>
    /// <param name="year">The year of the player card</param>
    /// <param name="item">The <see cref="ItemDto"/> to map</param>
    /// <returns><see cref="MlbPlayerCard"/></returns>
    /// <exception cref="UnexpectedTheShowItemException">Thrown when the <see cref="ItemDto"/> is not of a type that can be mapped to <see cref="MlbPlayerCard"/></exception>
    public MlbPlayerCard Map(SeasonYear year, ItemDto item)
    {
        if (item is not MlbCardDto dto)
        {
            throw new UnexpectedTheShowItemException(
                $"Trying to map item to {nameof(MlbPlayerCard)}, but could not map from {item.GetType().Name}");
        }

        return new MlbPlayerCard(
            Year: year,
            ExternalUuid: CardExternalId.Create(dto.Uuid.ValueAsString),
            Type: CardType.MlbCard,
            ImageUrl: CardImageLocation.Create(dto.ImageUrl),
            Name: CardName.Create(dto.Name),
            Rarity: MapRarity(dto.Rarity),
            IsSellable: dto.IsSellable,
            Series: MapCardSeries(dto.Series),
            Position: MapPosition(dto.DisplayPosition),
            TeamShortName: TeamShortName.Create(dto.TeamShortName),
            Overall: OverallRating.Create(dto.Overall),
            Stamina: AbilityAttribute.Create(dto.Stamina),
            PitchingClutch: AbilityAttribute.Create(dto.PitchingClutch),
            HitsPerBf: AbilityAttribute.Create(dto.HitsPerBf),
            KPerBf: AbilityAttribute.Create(dto.KPerBf),
            BbPerBf: AbilityAttribute.Create(dto.BbPerBf),
            HrPerBf: AbilityAttribute.Create(dto.HrPerBf),
            PitchVelocity: AbilityAttribute.Create(dto.PitchVelocity),
            PitchControl: AbilityAttribute.Create(dto.PitchControl),
            PitchMovement: AbilityAttribute.Create(dto.PitchMovement),
            ContactLeft: AbilityAttribute.Create(dto.ContactLeft),
            ContactRight: AbilityAttribute.Create(dto.ContactRight),
            PowerLeft: AbilityAttribute.Create(dto.PowerLeft),
            PowerRight: AbilityAttribute.Create(dto.PowerRight),
            PlateVision: AbilityAttribute.Create(dto.PlateVision),
            PlateDiscipline: AbilityAttribute.Create(dto.PlateDiscipline),
            BattingClutch: AbilityAttribute.Create(dto.BattingClutch),
            BuntingAbility: AbilityAttribute.Create(dto.BuntingAbility),
            DragBuntingAbility: AbilityAttribute.Create(dto.DragBuntingAbility),
            HittingDurability: AbilityAttribute.Create(dto.HittingDurability),
            FieldingDurability: AbilityAttribute.Create(dto.FieldingDurability),
            FieldingAbility: AbilityAttribute.Create(dto.FieldingAbility),
            ArmStrength: AbilityAttribute.Create(dto.ArmStrength),
            ArmAccuracy: AbilityAttribute.Create(dto.ArmAccuracy),
            ReactionTime: AbilityAttribute.Create(dto.ReactionTime),
            Blocking: AbilityAttribute.Create(dto.Blocking),
            Speed: AbilityAttribute.Create(dto.Speed),
            BaseRunningAbility: AbilityAttribute.Create(dto.BaseRunningAbility),
            BaseRunningAggression: AbilityAttribute.Create(dto.BaseRunningAggression)
        );
    }

    /// <summary>
    /// Maps a rarity value from MLB The Show to <see cref="Rarity"/>
    /// </summary>
    /// <param name="rarity">The rarity string value from MLB The Show</param>
    /// <returns><see cref="Rarity"/></returns>
    /// <exception cref="InvalidTheShowRarityException">Thrown if the rarity string value is invalid</exception>
    public Rarity MapRarity(string rarity)
    {
        return rarity switch
        {
            "Diamond" => Rarity.Diamond,
            "Gold" => Rarity.Gold,
            "Silver" => Rarity.Silver,
            "Bronze" => Rarity.Bronze,
            "Common" => Rarity.Common,
            _ => throw new InvalidTheShowRarityException($"Could not map rarity from MLB The Show: {rarity}")
        };
    }

    /// <summary>
    /// Maps a card series value from MLB The Show to <see cref="CardSeries"/>
    /// </summary>
    /// <param name="cardSeries">The card series string value from MLB The Show</param>
    /// <returns><see cref="CardSeries"/></returns>
    /// <exception cref="InvalidTheShowCardSeriesException">Thrown if the card series string value is invalid</exception>
    public CardSeries MapCardSeries(string cardSeries)
    {
        return cardSeries switch
        {
            "Live" => CardSeries.Live,
            "Rookie" => CardSeries.Rookie,
            _ => throw new InvalidTheShowCardSeriesException(
                $"Could not map card series from MLB The Show: {cardSeries}")
        };
    }

    /// <summary>
    /// Maps a position value from MLB The Show to <see cref="Position"/>
    /// </summary>
    /// <param name="position">The position value from MLB The Show</param>
    /// <returns><see cref="Position"/></returns>
    /// <exception cref="InvalidTheShowPositionException">Thrown if the position string value is invalid</exception>
    public Position MapPosition(string position)
    {
        switch (position)
        {
            case "SP":
            case "RP":
            case "LF":
            case "RF":
            case "CF":
            case "1B":
            case "3B":
            case "SS":
            case "2B":
            case "CP":
            case "C":
            case "DH":
                return (Position)TypeDescriptor.GetConverter(typeof(Position)).ConvertFrom(position)!;
            default:
                throw new InvalidTheShowPositionException($"Could not map position from MLB The Show: {position}");
        }
    }
}