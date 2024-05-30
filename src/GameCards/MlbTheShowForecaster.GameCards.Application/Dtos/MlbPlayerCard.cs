using com.brettnamba.MlbTheShowForecaster.Common.Domain.Enums;
using com.brettnamba.MlbTheShowForecaster.Common.Domain.ValueObjects;
using com.brettnamba.MlbTheShowForecaster.GameCards.Domain.Cards.Enums;
using com.brettnamba.MlbTheShowForecaster.GameCards.Domain.Cards.ValueObjects;
using com.brettnamba.MlbTheShowForecaster.GameCards.Domain.Cards.ValueObjects.PlayerCards;

namespace com.brettnamba.MlbTheShowForecaster.GameCards.Application.Dtos;

/// <summary>
/// Represents a MLB Player Card from MLB The Show
/// </summary>
/// <param name="Year">The year of MLB The Show</param>
/// <param name="ExternalUuid">The unique ID</param>
/// <param name="Type">The type of item</param>
/// <param name="ImageUrl">The URL of the image associated with the item</param>
/// <param name="Name">The name of the item</param>
/// <param name="Rarity">The rarity of the item</param>
/// <param name="IsSellable">True if the item is sellable on the marketplace, otherwise false</param>
/// <param name="Series">The series the item is a part of</param>
/// <param name="Position">The player's primary position</param>
/// <param name="TeamShortName">The player's team name abbreviation</param>
/// <param name="Overall">The player's overall rating</param>
/// <param name="Stamina">Pitcher's stamina</param>
/// <param name="PitchingClutch">Pitcher's ability to pitch with runners in scoring position</param>
/// <param name="HitsPerBf">Pitcher's ability to prevent hits</param>
/// <param name="KPerBf">Pitcher's ability to cause a batter to swing and miss</param>
/// <param name="BbPerBf">Pitcher's ability to prevent walks</param>
/// <param name="HrPerBf">Pitcher's ability to prevent home runs</param>
/// <param name="PitchVelocity">The pitcher's velocity</param>
/// <param name="PitchControl">The pitcher's control</param>
/// <param name="PitchMovement">The pitcher's ability to throw breaking pitches</param>
/// <param name="ContactLeft">The batter's ability to make contact with left handed pitchers</param>
/// <param name="ContactRight">The batter's ability to make contact with right handed pitchers</param>
/// <param name="PowerLeft">The batter's power against left handed pitchers</param>
/// <param name="PowerRight">The batter's power against right handed pitchers</param>
/// <param name="PlateVision">The batter's ability to see the ball and prevent strikeouts</param>
/// <param name="PlateDiscipline">The batter's ability to check a swing</param>
/// <param name="BattingClutch">The batter's ability to hit with runners in scoring position</param>
/// <param name="BuntingAbility">The batter's bunting ability</param>
/// <param name="DragBuntingAbility">The batter's drag bunting ability</param>
/// <param name="HittingDurability">The ability of a player to prevent injury when batting</param>
/// <param name="FieldingDurability">The ability of a player to prevent injury when fielding</param>
/// <param name="FieldingAbility">The player's fielding ability</param>
/// <param name="ArmStrength">The player's ability to throw the ball with velocity and distance</param>
/// <param name="ArmAccuracy">The player's ability to throw the ball accurately</param>
/// <param name="ReactionTime">The ability of a fielder to react when a batter makes contact with the ball</param>
/// <param name="Blocking">The ability of a catcher to block wild pitches</param>
/// <param name="Speed">The speed of the player</param>
/// <param name="BaseRunningAbility">How well the player can run around the bases</param>
/// <param name="BaseRunningAggression">How likely it is the player can steal a base</param>
/// <param name="BoostReason">If the card is boosted, the reason why. Null if there is no boost</param>
/// <param name="TemporaryOverallRating">The temporary overall rating of the card</param>
public readonly record struct MlbPlayerCard(
    SeasonYear Year,
    CardExternalId ExternalUuid,
    CardType Type,
    CardImageLocation ImageUrl,
    CardName Name,
    Rarity Rarity,
    bool IsSellable,
    CardSeries Series,
    Position Position,
    TeamShortName TeamShortName,
    OverallRating Overall,
    AbilityAttribute Stamina,
    AbilityAttribute PitchingClutch,
    AbilityAttribute HitsPerBf,
    AbilityAttribute KPerBf,
    AbilityAttribute BbPerBf,
    AbilityAttribute HrPerBf,
    AbilityAttribute PitchVelocity,
    AbilityAttribute PitchControl,
    AbilityAttribute PitchMovement,
    AbilityAttribute ContactLeft,
    AbilityAttribute ContactRight,
    AbilityAttribute PowerLeft,
    AbilityAttribute PowerRight,
    AbilityAttribute PlateVision,
    AbilityAttribute PlateDiscipline,
    AbilityAttribute BattingClutch,
    AbilityAttribute BuntingAbility,
    AbilityAttribute DragBuntingAbility,
    AbilityAttribute HittingDurability,
    AbilityAttribute FieldingDurability,
    AbilityAttribute FieldingAbility,
    AbilityAttribute ArmStrength,
    AbilityAttribute ArmAccuracy,
    AbilityAttribute ReactionTime,
    AbilityAttribute Blocking,
    AbilityAttribute Speed,
    AbilityAttribute BaseRunningAbility,
    AbilityAttribute BaseRunningAggression,
    string? BoostReason,
    OverallRating? TemporaryOverallRating
)
{
    /// <summary>
    /// Determines the process priority of the card
    /// </summary>
    public int Priority => (int)Rarity;

    /// <summary>
    /// Returns true if this card is supported by the system, otherwise false
    /// </summary>
    public bool IsSupported => Series == CardSeries.Live;

    /// <summary>
    /// True if the card is boosted
    /// </summary>
    public bool IsBoosted => !string.IsNullOrEmpty(BoostReason);

    /// <summary>
    /// True if the card has a temporary rating
    /// </summary>
    public bool HasTemporaryRating => TemporaryOverallRating != null;

    /// <summary>
    /// Gets the attributes as <see cref="PlayerCardAttributes"/>
    /// </summary>
    /// <returns>The attributes as <see cref="PlayerCardAttributes"/></returns>
    public PlayerCardAttributes GetAttributes()
    {
        return PlayerCardAttributes.Create(
            stamina: Stamina.Value,
            pitchingClutch: PitchingClutch.Value,
            hitsPerNine: HitsPerBf.Value,
            strikeoutsPerNine: KPerBf.Value,
            baseOnBallsPerNine: BbPerBf.Value,
            homeRunsPerNine: HrPerBf.Value,
            pitchVelocity: PitchVelocity.Value,
            pitchControl: PitchControl.Value,
            pitchMovement: PitchMovement.Value,
            contactLeft: ContactLeft.Value,
            contactRight: ContactRight.Value,
            powerLeft: PowerLeft.Value,
            powerRight: PowerRight.Value,
            plateVision: PlateVision.Value,
            plateDiscipline: PlateDiscipline.Value,
            battingClutch: BattingClutch.Value,
            buntingAbility: BuntingAbility.Value,
            dragBuntingAbility: DragBuntingAbility.Value,
            hittingDurability: HittingDurability.Value,
            fieldingDurability: FieldingDurability.Value,
            fieldingAbility: FieldingAbility.Value,
            armStrength: ArmStrength.Value,
            armAccuracy: ArmAccuracy.Value,
            reactionTime: ReactionTime.Value,
            blocking: Blocking.Value,
            speed: Speed.Value,
            baseRunningAbility: BaseRunningAbility.Value,
            baseRunningAggression: BaseRunningAggression.Value
        );
    }
};