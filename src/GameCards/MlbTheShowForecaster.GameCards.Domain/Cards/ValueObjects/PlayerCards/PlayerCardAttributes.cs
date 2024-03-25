using com.brettnamba.MlbTheShowForecaster.Common.Domain.SeedWork;

namespace com.brettnamba.MlbTheShowForecaster.GameCards.Domain.Cards.ValueObjects.PlayerCards;

/// <summary>
/// Represents all the ability attributes on a player card
/// </summary>
public sealed class PlayerCardAttributes : ValueObject
{
    /// <summary>
    /// Pitcher's stamina
    /// </summary>
    public AbilityAttribute Stamina { get; }

    /// <summary>
    /// Pitcher's ability to pitch with runners in scoring position
    /// </summary>
    public AbilityAttribute PitchingClutch { get; }

    /// <summary>
    /// Pitcher's ability to prevent hits
    /// </summary>
    public AbilityAttribute HitsPerNine { get; }

    /// <summary>
    /// Pitcher's ability to cause a batter to swing and miss
    /// </summary>
    public AbilityAttribute StrikeoutsPerNine { get; }

    /// <summary>
    /// Pitcher's ability to prevent walks
    /// </summary>
    public AbilityAttribute BaseOnBallsPerNine { get; }

    /// <summary>
    /// Pitcher's ability to prevent home runs
    /// </summary>
    public AbilityAttribute HomeRunsPerNine { get; }

    /// <summary>
    /// The pitcher's velocity
    /// </summary>
    public AbilityAttribute PitchVelocity { get; }

    /// <summary>
    /// The pitcher's control
    /// </summary>
    public AbilityAttribute PitchControl { get; }

    /// <summary>
    /// The pitcher's ability to throw breaking pitches
    /// </summary>
    public AbilityAttribute PitchMovement { get; }

    /// <summary>
    /// The batter's ability to make contact with left handed pitchers
    /// </summary>
    public AbilityAttribute ContactLeft { get; }

    /// <summary>
    /// The batter's ability to make contact with right handed pitchers
    /// </summary>
    public AbilityAttribute ContactRight { get; }

    /// <summary>
    /// The batter's power against left handed pitchers
    /// </summary>
    public AbilityAttribute PowerLeft { get; }

    /// <summary>
    /// The batter's power against right handed pitchers
    /// </summary>
    public AbilityAttribute PowerRight { get; }

    /// <summary>
    /// The batter's ability to see the ball and prevent strikeouts
    /// </summary>
    public AbilityAttribute PlateVision { get; }

    /// <summary>
    /// The batter's ability to check a swing
    /// </summary>
    public AbilityAttribute PlateDiscipline { get; }

    /// <summary>
    /// The batter's ability to hit with runners in scoring position
    /// </summary>
    public AbilityAttribute BattingClutch { get; }

    /// <summary>
    /// The batter's bunting ability
    /// </summary>
    public AbilityAttribute BuntingAbility { get; }

    /// <summary>
    /// The batter's drag bunting ability
    /// </summary>
    public AbilityAttribute DragBuntingAbility { get; }

    /// <summary>
    /// The ability of a player to prevent injury when batting
    /// </summary>
    public AbilityAttribute HittingDurability { get; }

    /// <summary>
    /// The ability of a player to prevent injury when fielding
    /// </summary>
    public AbilityAttribute FieldingDurability { get; }

    /// <summary>
    /// The player's fielding ability
    /// </summary>
    public AbilityAttribute FieldingAbility { get; }

    /// <summary>
    /// The player's ability to throw the ball with velocity and distance
    /// </summary>
    public AbilityAttribute ArmStrength { get; }

    /// <summary>
    /// The player's ability to throw the ball accurately
    /// </summary>
    public AbilityAttribute ArmAccuracy { get; }

    /// <summary>
    /// The ability of a fielder to react when a batter makes contact with the ball
    /// </summary>
    public AbilityAttribute ReactionTime { get; }

    /// <summary>
    /// The ability of a catcher to block wild pitches
    /// </summary>
    public AbilityAttribute Blocking { get; }

    /// <summary>
    /// The speed of the player
    /// </summary>
    public AbilityAttribute Speed { get; }

    /// <summary>
    /// How well the player can run around the bases
    /// </summary>
    public AbilityAttribute BaseRunningAbility { get; }

    /// <summary>
    /// How likely it is the player can steal a base
    /// </summary>
    public AbilityAttribute BaseRunningAggression { get; }

    /// <summary>
    /// Constructor
    /// </summary>
    /// <param name="stamina">Pitcher's stamina</param>
    /// <param name="pitchingClutch">Pitcher's ability to pitch with runners in scoring position</param>
    /// <param name="hitsPerNine">Pitcher's ability to prevent hits</param>
    /// <param name="strikeoutsPerNine">Pitcher's ability to cause a batter to swing and miss</param>
    /// <param name="baseOnBallsPerNine">Pitcher's ability to prevent walks</param>
    /// <param name="homeRunsPerNine">Pitcher's ability to prevent home runs</param>
    /// <param name="pitchVelocity">The pitcher's velocity</param>
    /// <param name="pitchControl">The pitcher's control</param>
    /// <param name="pitchMovement">The pitcher's ability to throw breaking pitches</param>
    /// <param name="contactLeft">The batter's ability to make contact with left handed pitchers</param>
    /// <param name="contactRight">The batter's ability to make contact with right handed pitchers</param>
    /// <param name="powerLeft">The batter's power against left handed pitchers</param>
    /// <param name="powerRight">The batter's power against right handed pitchers</param>
    /// <param name="plateVision">The batter's ability to see the ball and prevent strikeouts</param>
    /// <param name="plateDiscipline">The batter's ability to check a swing</param>
    /// <param name="battingClutch">The batter's ability to hit with runners in scoring position</param>
    /// <param name="buntingAbility">The batter's bunting ability</param>
    /// <param name="dragBuntingAbility">The batter's drag bunting ability</param>
    /// <param name="hittingDurability">The ability of a player to prevent injury when batting</param>
    /// <param name="fieldingDurability">The ability of a player to prevent injury when fielding</param>
    /// <param name="fieldingAbility">The player's fielding ability</param>
    /// <param name="armStrength">The player's ability to throw the ball with velocity and distance</param>
    /// <param name="armAccuracy">The player's ability to throw the ball accurately</param>
    /// <param name="reactionTime">The ability of a fielder to react when a batter makes contact with the ball</param>
    /// <param name="blocking">The ability of a catcher to block wild pitches</param>
    /// <param name="speed">The speed of the player</param>
    /// <param name="baseRunningAbility">How well the player can run around the bases</param>
    /// <param name="baseRunningAggression">How likely it is the player can steal a base</param>
    private PlayerCardAttributes(AbilityAttribute stamina, AbilityAttribute pitchingClutch,
        AbilityAttribute hitsPerNine, AbilityAttribute strikeoutsPerNine, AbilityAttribute baseOnBallsPerNine,
        AbilityAttribute homeRunsPerNine, AbilityAttribute pitchVelocity, AbilityAttribute pitchControl,
        AbilityAttribute pitchMovement, AbilityAttribute contactLeft, AbilityAttribute contactRight,
        AbilityAttribute powerLeft, AbilityAttribute powerRight, AbilityAttribute plateVision,
        AbilityAttribute plateDiscipline, AbilityAttribute battingClutch, AbilityAttribute buntingAbility,
        AbilityAttribute dragBuntingAbility, AbilityAttribute hittingDurability, AbilityAttribute fieldingDurability,
        AbilityAttribute fieldingAbility, AbilityAttribute armStrength, AbilityAttribute armAccuracy,
        AbilityAttribute reactionTime, AbilityAttribute blocking, AbilityAttribute speed,
        AbilityAttribute baseRunningAbility, AbilityAttribute baseRunningAggression)
    {
        Stamina = stamina;
        PitchingClutch = pitchingClutch;
        HitsPerNine = hitsPerNine;
        StrikeoutsPerNine = strikeoutsPerNine;
        BaseOnBallsPerNine = baseOnBallsPerNine;
        HomeRunsPerNine = homeRunsPerNine;
        PitchVelocity = pitchVelocity;
        PitchControl = pitchControl;
        PitchMovement = pitchMovement;
        ContactLeft = contactLeft;
        ContactRight = contactRight;
        PowerLeft = powerLeft;
        PowerRight = powerRight;
        PlateVision = plateVision;
        PlateDiscipline = plateDiscipline;
        BattingClutch = battingClutch;
        BuntingAbility = buntingAbility;
        DragBuntingAbility = dragBuntingAbility;
        HittingDurability = hittingDurability;
        FieldingDurability = fieldingDurability;
        FieldingAbility = fieldingAbility;
        ArmStrength = armStrength;
        ArmAccuracy = armAccuracy;
        ReactionTime = reactionTime;
        Blocking = blocking;
        Speed = speed;
        BaseRunningAbility = baseRunningAbility;
        BaseRunningAggression = baseRunningAggression;
    }

    /// <summary>
    /// Creates a <see cref="PlayerCardAttributes"/>
    /// </summary>
    /// <param name="stamina">Pitcher's stamina</param>
    /// <param name="pitchingClutch">Pitcher's ability to pitch with runners in scoring position</param>
    /// <param name="hitsPerNine">Pitcher's ability to prevent hits</param>
    /// <param name="strikeoutsPerNine">Pitcher's ability to cause a batter to swing and miss</param>
    /// <param name="baseOnBallsPerNine">Pitcher's ability to prevent walks</param>
    /// <param name="homeRunsPerNine">Pitcher's ability to prevent home runs</param>
    /// <param name="pitchVelocity">The pitcher's velocity</param>
    /// <param name="pitchControl">The pitcher's control</param>
    /// <param name="pitchMovement">The pitcher's ability to throw breaking pitches</param>
    /// <param name="contactLeft">The batter's ability to make contact with left handed pitchers</param>
    /// <param name="contactRight">The batter's ability to make contact with right handed pitchers</param>
    /// <param name="powerLeft">The batter's power against left handed pitchers</param>
    /// <param name="powerRight">The batter's power against right handed pitchers</param>
    /// <param name="plateVision">The batter's ability to see the ball and prevent strikeouts</param>
    /// <param name="plateDiscipline">The batter's ability to check a swing</param>
    /// <param name="battingClutch">The batter's ability to hit with runners in scoring position</param>
    /// <param name="buntingAbility">The batter's bunting ability</param>
    /// <param name="dragBuntingAbility">The batter's drag bunting ability</param>
    /// <param name="hittingDurability">The ability of a player to prevent injury when batting</param>
    /// <param name="fieldingDurability">The ability of a player to prevent injury when fielding</param>
    /// <param name="fieldingAbility">The player's fielding ability</param>
    /// <param name="armStrength">The player's ability to throw the ball with velocity and distance</param>
    /// <param name="armAccuracy">The player's ability to throw the ball accurately</param>
    /// <param name="reactionTime">The ability of a fielder to react when a batter makes contact with the ball</param>
    /// <param name="blocking">The ability of a catcher to block wild pitches</param>
    /// <param name="speed">The speed of the player</param>
    /// <param name="baseRunningAbility">How well the player can run around the bases</param>
    /// <param name="baseRunningAggression">How likely it is the player can steal a base</param>
    /// <returns><see cref="PlayerCardAttributes"/></returns>
    public static PlayerCardAttributes Create(int stamina, int pitchingClutch, int hitsPerNine, int strikeoutsPerNine,
        int baseOnBallsPerNine, int homeRunsPerNine, int pitchVelocity, int pitchControl, int pitchMovement,
        int contactLeft, int contactRight, int powerLeft, int powerRight, int plateVision, int plateDiscipline,
        int battingClutch, int buntingAbility, int dragBuntingAbility, int hittingDurability, int fieldingDurability,
        int fieldingAbility, int armStrength, int armAccuracy, int reactionTime, int blocking, int speed,
        int baseRunningAbility, int baseRunningAggression)
    {
        return new PlayerCardAttributes(
            stamina: AbilityAttribute.Create(stamina),
            pitchingClutch: AbilityAttribute.Create(pitchingClutch),
            hitsPerNine: AbilityAttribute.Create(hitsPerNine),
            strikeoutsPerNine: AbilityAttribute.Create(strikeoutsPerNine),
            baseOnBallsPerNine: AbilityAttribute.Create(baseOnBallsPerNine),
            homeRunsPerNine: AbilityAttribute.Create(homeRunsPerNine),
            pitchVelocity: AbilityAttribute.Create(pitchVelocity),
            pitchControl: AbilityAttribute.Create(pitchControl),
            pitchMovement: AbilityAttribute.Create(pitchMovement),
            contactLeft: AbilityAttribute.Create(contactLeft),
            contactRight: AbilityAttribute.Create(contactRight),
            powerLeft: AbilityAttribute.Create(powerLeft),
            powerRight: AbilityAttribute.Create(powerRight),
            plateVision: AbilityAttribute.Create(plateVision),
            plateDiscipline: AbilityAttribute.Create(plateDiscipline),
            battingClutch: AbilityAttribute.Create(battingClutch),
            buntingAbility: AbilityAttribute.Create(buntingAbility),
            dragBuntingAbility: AbilityAttribute.Create(dragBuntingAbility),
            hittingDurability: AbilityAttribute.Create(hittingDurability),
            fieldingDurability: AbilityAttribute.Create(fieldingDurability),
            fieldingAbility: AbilityAttribute.Create(fieldingAbility),
            armStrength: AbilityAttribute.Create(armStrength),
            armAccuracy: AbilityAttribute.Create(armAccuracy),
            reactionTime: AbilityAttribute.Create(reactionTime),
            blocking: AbilityAttribute.Create(blocking),
            speed: AbilityAttribute.Create(speed),
            baseRunningAbility: AbilityAttribute.Create(baseRunningAbility),
            baseRunningAggression: AbilityAttribute.Create(baseRunningAggression)
        );
    }
}