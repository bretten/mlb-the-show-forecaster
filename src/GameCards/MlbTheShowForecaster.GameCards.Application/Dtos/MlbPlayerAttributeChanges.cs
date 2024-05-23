using com.brettnamba.MlbTheShowForecaster.GameCards.Domain.Cards.ValueObjects.PlayerCards;

namespace com.brettnamba.MlbTheShowForecaster.GameCards.Application.Dtos;

/// <summary>
/// Represents changes to attributes for a MLB player. An attribute can either go up (positive value) or
/// go down (negative value)
/// </summary>
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
public readonly record struct MlbPlayerAttributeChanges(
    int Stamina,
    int PitchingClutch,
    int HitsPerBf,
    int KPerBf,
    int BbPerBf,
    int HrPerBf,
    int PitchVelocity,
    int PitchControl,
    int PitchMovement,
    int ContactLeft,
    int ContactRight,
    int PowerLeft,
    int PowerRight,
    int PlateVision,
    int PlateDiscipline,
    int BattingClutch,
    int BuntingAbility,
    int DragBuntingAbility,
    int HittingDurability,
    int FieldingDurability,
    int FieldingAbility,
    int ArmStrength,
    int ArmAccuracy,
    int ReactionTime,
    int Blocking,
    int Speed,
    int BaseRunningAbility,
    int BaseRunningAggression
)
{
    /// <summary>
    /// Adds the attributes on this <see cref="MlbPlayerAttributeChanges"/> to the specified <see cref="PlayerCardAttributes"/>
    /// </summary>
    /// <param name="currentAttributes">The attributes to add to</param>
    /// <returns>The resulting, summed <see cref="PlayerCardAttributes"/></returns>
    public PlayerCardAttributes ApplyAttributes(PlayerCardAttributes currentAttributes)
    {
        return PlayerCardAttributes.Create(
            stamina: currentAttributes.Stamina.Value + Stamina,
            pitchingClutch: currentAttributes.PitchingClutch.Value + PitchingClutch,
            hitsPerNine: currentAttributes.HitsPerNine.Value + HitsPerBf,
            strikeoutsPerNine: currentAttributes.StrikeoutsPerNine.Value + KPerBf,
            baseOnBallsPerNine: currentAttributes.BaseOnBallsPerNine.Value + BbPerBf,
            homeRunsPerNine: currentAttributes.HomeRunsPerNine.Value + HrPerBf,
            pitchVelocity: currentAttributes.PitchVelocity.Value + PitchVelocity,
            pitchControl: currentAttributes.PitchControl.Value + PitchControl,
            pitchMovement: currentAttributes.PitchMovement.Value + PitchMovement,
            contactLeft: currentAttributes.ContactLeft.Value + ContactLeft,
            contactRight: currentAttributes.ContactRight.Value + ContactRight,
            powerLeft: currentAttributes.PowerLeft.Value + PowerLeft,
            powerRight: currentAttributes.PowerRight.Value + PowerRight,
            plateVision: currentAttributes.PlateVision.Value + PlateVision,
            plateDiscipline: currentAttributes.PlateDiscipline.Value + PlateDiscipline,
            battingClutch: currentAttributes.BattingClutch.Value + BattingClutch,
            buntingAbility: currentAttributes.BuntingAbility.Value + BuntingAbility,
            dragBuntingAbility: currentAttributes.DragBuntingAbility.Value + DragBuntingAbility,
            hittingDurability: currentAttributes.HittingDurability.Value + HittingDurability,
            fieldingDurability: currentAttributes.FieldingDurability.Value + FieldingDurability,
            fieldingAbility: currentAttributes.FieldingAbility.Value + FieldingAbility,
            armStrength: currentAttributes.ArmStrength.Value + ArmStrength,
            armAccuracy: currentAttributes.ArmAccuracy.Value + ArmAccuracy,
            reactionTime: currentAttributes.ReactionTime.Value + ReactionTime,
            blocking: currentAttributes.Blocking.Value + Blocking,
            speed: currentAttributes.Speed.Value + Speed,
            baseRunningAbility: currentAttributes.BaseRunningAbility.Value + BaseRunningAbility,
            baseRunningAggression: currentAttributes.BaseRunningAggression.Value + BaseRunningAggression
        );
    }

    /// <summary>
    /// Subtracts this <see cref="MlbPlayerAttributeChanges"/> from the specified <see cref="PlayerCardAttributes"/>
    /// </summary>
    /// <param name="currentAttributes">The <see cref="PlayerCardAttributes"/> to subtract from</param>
    /// <returns>A new <see cref="PlayerCardAttributes"/> with this <see cref="MlbPlayerAttributeChanges"/> subtracted from it</returns>
    public PlayerCardAttributes SubtractFrom(PlayerCardAttributes currentAttributes)
    {
        return PlayerCardAttributes.Create(
            stamina: currentAttributes.Stamina.Value - Stamina,
            pitchingClutch: currentAttributes.PitchingClutch.Value - PitchingClutch,
            hitsPerNine: currentAttributes.HitsPerNine.Value - HitsPerBf,
            strikeoutsPerNine: currentAttributes.StrikeoutsPerNine.Value - KPerBf,
            baseOnBallsPerNine: currentAttributes.BaseOnBallsPerNine.Value - BbPerBf,
            homeRunsPerNine: currentAttributes.HomeRunsPerNine.Value - HrPerBf,
            pitchVelocity: currentAttributes.PitchVelocity.Value - PitchVelocity,
            pitchControl: currentAttributes.PitchControl.Value - PitchControl,
            pitchMovement: currentAttributes.PitchMovement.Value - PitchMovement,
            contactLeft: currentAttributes.ContactLeft.Value - ContactLeft,
            contactRight: currentAttributes.ContactRight.Value - ContactRight,
            powerLeft: currentAttributes.PowerLeft.Value - PowerLeft,
            powerRight: currentAttributes.PowerRight.Value - PowerRight,
            plateVision: currentAttributes.PlateVision.Value - PlateVision,
            plateDiscipline: currentAttributes.PlateDiscipline.Value - PlateDiscipline,
            battingClutch: currentAttributes.BattingClutch.Value - BattingClutch,
            buntingAbility: currentAttributes.BuntingAbility.Value - BuntingAbility,
            dragBuntingAbility: currentAttributes.DragBuntingAbility.Value - DragBuntingAbility,
            hittingDurability: currentAttributes.HittingDurability.Value - HittingDurability,
            fieldingDurability: currentAttributes.FieldingDurability.Value - FieldingDurability,
            fieldingAbility: currentAttributes.FieldingAbility.Value - FieldingAbility,
            armStrength: currentAttributes.ArmStrength.Value - ArmStrength,
            armAccuracy: currentAttributes.ArmAccuracy.Value - ArmAccuracy,
            reactionTime: currentAttributes.ReactionTime.Value - ReactionTime,
            blocking: currentAttributes.Blocking.Value - Blocking,
            speed: currentAttributes.Speed.Value - Speed,
            baseRunningAbility: currentAttributes.BaseRunningAbility.Value - BaseRunningAbility,
            baseRunningAggression: currentAttributes.BaseRunningAggression.Value - BaseRunningAggression
        );
    }
};