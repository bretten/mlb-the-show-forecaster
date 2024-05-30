using com.brettnamba.MlbTheShowForecaster.Common.DateAndTime;
using com.brettnamba.MlbTheShowForecaster.GameCards.Domain.Cards.Entities;
using com.brettnamba.MlbTheShowForecaster.GameCards.Domain.Cards.ValueObjects.PlayerCards;

namespace com.brettnamba.MlbTheShowForecaster.GameCards.Application.Dtos.Mapping;

/// <summary>
/// Maps the Application-level player card to the Domain's <see cref="PlayerCard"/>
/// </summary>
public sealed class PlayerCardMapper : IPlayerCardMapper
{
    /// <summary>
    /// Calendar to get the current date
    /// </summary>
    private readonly ICalendar _calendar;

    /// <summary>
    /// Constructor
    /// </summary>
    /// <param name="calendar">Calendar to get the current date</param>
    public PlayerCardMapper(ICalendar calendar)
    {
        _calendar = calendar;
    }

    /// <summary>
    /// Maps <see cref="MlbPlayerCard"/> to <see cref="PlayerCard"/>
    /// </summary>
    /// <param name="card">The player card to map</param>
    /// <returns><see cref="PlayerCard"/></returns>
    public PlayerCard Map(MlbPlayerCard card)
    {
        var mappedCard = MapCard(card);

        // A boost is also a temporary rating, so don't apply both
        if (card.IsBoosted)
        {
            mappedCard.Boost(_calendar.Today(), mappedCard.PlayerCardAttributes);
        }
        else if (card.HasTemporaryRating)
        {
            mappedCard.SetTemporaryRating(_calendar.Today(), card.TemporaryOverallRating!);
        }

        return mappedCard;
    }

    /// <summary>
    /// Only maps <see cref="MlbPlayerCard"/> to <see cref="PlayerCard"/>, but does not apply any temporary rating changes
    /// </summary>
    /// <param name="card">The player card to map</param>
    /// <returns><see cref="PlayerCard"/></returns>
    private PlayerCard MapCard(MlbPlayerCard card)
    {
        return PlayerCard.Create(card.Year,
            card.ExternalUuid,
            card.Type,
            card.ImageUrl,
            card.Name,
            card.Rarity,
            card.Series,
            card.Position,
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