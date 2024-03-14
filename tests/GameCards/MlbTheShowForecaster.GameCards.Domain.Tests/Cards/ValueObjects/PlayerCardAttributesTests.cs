using com.brettnamba.MlbTheShowForecaster.GameCards.Domain.Cards.ValueObjects;

namespace com.brettnamba.MlbTheShowForecaster.GameCards.Domain.Tests.Cards.ValueObjects;

public class PlayerCardAttributesTests
{
    [Fact]
    public void Create_ValidValues_ReturnsPlayerCardAttributes()
    {
        // Arrange
        const int stamina = 1;
        const int pitchingClutch = 2;
        const int hitsPerNine = 3;
        const int strikeoutsPerNine = 4;
        const int baseOnBallsPerNine = 5;
        const int homeRunsPerNine = 6;
        const int pitchVelocity = 7;
        const int pitchControl = 8;
        const int pitchMovement = 9;
        const int contactLeft = 10;
        const int contactRight = 11;
        const int powerLeft = 12;
        const int powerRight = 13;
        const int plateVision = 14;
        const int plateDiscipline = 15;
        const int battingClutch = 16;
        const int buntingAbility = 17;
        const int dragBuntingAbility = 18;
        const int hittingDurability = 19;
        const int fieldingDurability = 20;
        const int fieldingAbility = 21;
        const int armStrength = 22;
        const int armAccuracy = 23;
        const int reactionTime = 24;
        const int blocking = 25;
        const int speed = 26;
        const int baseRunningAbility = 27;
        const int baseRunningAggression = 28;

        // Act
        var actual = PlayerCardAttributes.Create(stamina: stamina,
            pitchingClutch: pitchingClutch,
            hitsPerNine: hitsPerNine,
            strikeoutsPerNine: strikeoutsPerNine,
            baseOnBallsPerNine: baseOnBallsPerNine,
            homeRunsPerNine: homeRunsPerNine,
            pitchVelocity: pitchVelocity,
            pitchControl: pitchControl,
            pitchMovement: pitchMovement,
            contactLeft: contactLeft,
            contactRight: contactRight,
            powerLeft: powerLeft,
            powerRight: powerRight,
            plateVision: plateVision,
            plateDiscipline: plateDiscipline,
            battingClutch: battingClutch,
            buntingAbility: buntingAbility,
            dragBuntingAbility: dragBuntingAbility,
            hittingDurability: hittingDurability,
            fieldingDurability: fieldingDurability,
            fieldingAbility: fieldingAbility,
            armStrength: armStrength,
            armAccuracy: armAccuracy,
            reactionTime: reactionTime,
            blocking: blocking,
            speed: speed,
            baseRunningAbility: baseRunningAbility,
            baseRunningAggression: baseRunningAggression
        );

        // Assert
        Assert.Equal(1, actual.Stamina.Value);
        Assert.Equal(2, actual.PitchingClutch.Value);
        Assert.Equal(3, actual.HitsPerNine.Value);
        Assert.Equal(4, actual.StrikeoutsPerNine.Value);
        Assert.Equal(5, actual.BaseOnBallsPerNine.Value);
        Assert.Equal(6, actual.HomeRunsPerNine.Value);
        Assert.Equal(7, actual.PitchVelocity.Value);
        Assert.Equal(8, actual.PitchControl.Value);
        Assert.Equal(9, actual.PitchMovement.Value);
        Assert.Equal(10, actual.ContactLeft.Value);
        Assert.Equal(11, actual.ContactRight.Value);
        Assert.Equal(12, actual.PowerLeft.Value);
        Assert.Equal(13, actual.PowerRight.Value);
        Assert.Equal(14, actual.PlateVision.Value);
        Assert.Equal(15, actual.PlateDiscipline.Value);
        Assert.Equal(16, actual.BattingClutch.Value);
        Assert.Equal(17, actual.BuntingAbility.Value);
        Assert.Equal(18, actual.DragBuntingAbility.Value);
        Assert.Equal(19, actual.HittingDurability.Value);
        Assert.Equal(20, actual.FieldingDurability.Value);
        Assert.Equal(21, actual.FieldingAbility.Value);
        Assert.Equal(22, actual.ArmStrength.Value);
        Assert.Equal(23, actual.ArmAccuracy.Value);
        Assert.Equal(24, actual.ReactionTime.Value);
        Assert.Equal(25, actual.Blocking.Value);
        Assert.Equal(26, actual.Speed.Value);
        Assert.Equal(27, actual.BaseRunningAbility.Value);
        Assert.Equal(28, actual.BaseRunningAggression.Value);
    }
}