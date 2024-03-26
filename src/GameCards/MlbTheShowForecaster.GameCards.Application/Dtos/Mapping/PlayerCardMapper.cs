using com.brettnamba.MlbTheShowForecaster.GameCards.Domain.Cards.Entities;
using com.brettnamba.MlbTheShowForecaster.GameCards.Domain.Cards.ValueObjects.PlayerCards;

namespace com.brettnamba.MlbTheShowForecaster.GameCards.Application.Dtos.Mapping;

/// <summary>
/// Maps the Application-level player card to the Domain's <see cref="PlayerCard"/>
/// </summary>
public sealed class PlayerCardMapper : IPlayerCardMapper
{
    /// <summary>
    /// Maps <see cref="MlbPlayerCard"/> to <see cref="PlayerCard"/>
    /// </summary>
    /// <param name="card">The player card to map</param>
    /// <returns><see cref="PlayerCard"/></returns>
    public PlayerCard Map(MlbPlayerCard card)
    {
        return PlayerCard.Create(card.ExternalUuid,
            card.Type,
            card.ImageUrl,
            card.Name,
            card.Rarity,
            card.Series,
            card.TeamShortName,
            card.Overall,
            PlayerCardAttributes.Create(
                stamina: card.Stamina.Value,
                pitchingClutch: card.PitchingClutch.Value,
                hitsPerNine: card.HitsPerBf.Value,
                strikeoutsPerNine: card.KPerBf.Value,
                baseOnBallsPerNine: card.BbPerBf.Value,
                homeRunsPerNine: card.HrPerBf.Value,
                pitchVelocity: card.PitchVelocity.Value,
                pitchControl: card.PitchControl.Value,
                pitchMovement: card.PitchMovement.Value,
                contactLeft: card.ContactLeft.Value,
                contactRight: card.ContactRight.Value,
                powerLeft: card.PowerLeft.Value,
                powerRight: card.PowerRight.Value,
                plateVision: card.PlateVision.Value,
                plateDiscipline: card.PlateDiscipline.Value,
                battingClutch: card.BattingClutch.Value,
                buntingAbility: card.BuntingAbility.Value,
                dragBuntingAbility: card.DragBuntingAbility.Value,
                hittingDurability: card.HittingDurability.Value,
                fieldingDurability: card.FieldingDurability.Value,
                fieldingAbility: card.FieldingAbility.Value,
                armStrength: card.ArmStrength.Value,
                armAccuracy: card.ArmAccuracy.Value,
                reactionTime: card.ReactionTime.Value,
                blocking: card.Blocking.Value,
                speed: card.Speed.Value,
                baseRunningAbility: card.BaseRunningAbility.Value,
                baseRunningAggression: card.BaseRunningAggression.Value
            )
        );
    }
}