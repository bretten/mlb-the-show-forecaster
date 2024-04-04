using com.brettnamba.MlbTheShowForecaster.Common.Domain.Enums;
using com.brettnamba.MlbTheShowForecaster.Common.Domain.ValueObjects;
using com.brettnamba.MlbTheShowForecaster.ExternalApis.MlbTheShowApi.Dtos.Items;
using com.brettnamba.MlbTheShowForecaster.GameCards.Application.Dtos;
using com.brettnamba.MlbTheShowForecaster.GameCards.Domain.Cards.Enums;
using com.brettnamba.MlbTheShowForecaster.GameCards.Domain.Cards.ValueObjects;
using com.brettnamba.MlbTheShowForecaster.GameCards.Infrastructure.Dtos.Mapping.Exceptions;

namespace com.brettnamba.MlbTheShowForecaster.GameCards.Infrastructure.Dtos.Mapping;

public sealed class MlbTheShowItemMapper : IMlbTheShowItemMapper
{
    public MlbPlayerCard Map(SeasonYear year, ItemDto item)
    {
        if (item is not MlbCardDto dto)
        {
            throw new UnexpectedTheShowItemException(
                $"Trying to map to {nameof(MlbPlayerCard)}, but could not map from {item.GetType().Name}");
        }

        return new MlbPlayerCard(
            Year: year,
            ExternalUuid: CardExternalId.Create(dto.Uuid),
            Type: CardType.MlbCard,
            ImageUrl: CardImageLocation.Create(dto.ImageUrl),
            Name: CardName.Create(dto.Name),
            Rarity: MapRarity(dto.Rarity),
            IsSellable: dto.IsSellable,
            Series: MapSeries(dto.Series),
            Position: Position.Catcher, // TODO
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

    public CardSeries MapSeries(string series)
    {
        return series switch
        {
            "Live" => CardSeries.Live,
            "Rookie" => CardSeries.Rookie,
            _ => throw new InvalidTheShowSeriesException($"Could not map series from MLB The Show: {series}")
        };
    }
}