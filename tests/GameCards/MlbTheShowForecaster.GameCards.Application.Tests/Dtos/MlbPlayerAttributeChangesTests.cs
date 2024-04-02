using com.brettnamba.MlbTheShowForecaster.GameCards.Application.Dtos;
using com.brettnamba.MlbTheShowForecaster.GameCards.Application.Tests.TestClasses;

namespace com.brettnamba.MlbTheShowForecaster.GameCards.Application.Tests.Dtos;

public class MlbPlayerAttributeChangesTests
{
    [Fact]
    public void ApplyAttributes_CurrentAttributes_ReturnsSummedAttributes()
    {
        // Arrange
        var attributeChanges = new MlbPlayerAttributeChanges(
            Stamina: 1,
            PitchingClutch: 2,
            HitsPerBf: 3,
            KPerBf: 4,
            BbPerBf: 5,
            HrPerBf: 6,
            PitchVelocity: 7,
            PitchControl: 8,
            PitchMovement: 9,
            ContactLeft: 10,
            ContactRight: 11,
            PowerLeft: 12,
            PowerRight: 13,
            PlateVision: 14,
            PlateDiscipline: 15,
            BattingClutch: 16,
            BuntingAbility: 17,
            DragBuntingAbility: 18,
            HittingDurability: 19,
            FieldingDurability: 20,
            FieldingAbility: 21,
            ArmStrength: 22,
            ArmAccuracy: 23,
            ReactionTime: 24,
            Blocking: 25,
            Speed: 26,
            BaseRunningAbility: 27,
            BaseRunningAggression: 28
        );
        var currentAttributes = Faker.FakePlayerCardAttributes(
            stamina: 1,
            pitchingClutch: 1,
            hitsPerNine: 1,
            strikeoutsPerNine: 1,
            baseOnBallsPerNine: 1,
            homeRunsPerNine: 1,
            pitchVelocity: 1,
            pitchControl: 1,
            pitchMovement: 1,
            contactLeft: 1,
            contactRight: 1,
            powerLeft: 1,
            powerRight: 1,
            plateVision: 1,
            plateDiscipline: 1,
            battingClutch: 1,
            buntingAbility: 1,
            dragBuntingAbility: 1,
            hittingDurability: 1,
            fieldingDurability: 1,
            fieldingAbility: 1,
            armStrength: 1,
            armAccuracy: 1,
            reactionTime: 1,
            blocking: 1,
            speed: 1,
            baseRunningAbility: 1,
            baseRunningAggression: 1
        );

        // Act
        var actual = attributeChanges.ApplyAttributes(currentAttributes);

        // Assert
        Assert.Equal(2, actual.Stamina.Value);
        Assert.Equal(3, actual.PitchingClutch.Value);
        Assert.Equal(4, actual.HitsPerNine.Value);
        Assert.Equal(5, actual.StrikeoutsPerNine.Value);
        Assert.Equal(6, actual.BaseOnBallsPerNine.Value);
        Assert.Equal(7, actual.HomeRunsPerNine.Value);
        Assert.Equal(8, actual.PitchVelocity.Value);
        Assert.Equal(9, actual.PitchControl.Value);
        Assert.Equal(10, actual.PitchMovement.Value);
        Assert.Equal(11, actual.ContactLeft.Value);
        Assert.Equal(12, actual.ContactRight.Value);
        Assert.Equal(13, actual.PowerLeft.Value);
        Assert.Equal(14, actual.PowerRight.Value);
        Assert.Equal(15, actual.PlateVision.Value);
        Assert.Equal(16, actual.PlateDiscipline.Value);
        Assert.Equal(17, actual.BattingClutch.Value);
        Assert.Equal(18, actual.BuntingAbility.Value);
        Assert.Equal(19, actual.DragBuntingAbility.Value);
        Assert.Equal(20, actual.HittingDurability.Value);
        Assert.Equal(21, actual.FieldingDurability.Value);
        Assert.Equal(22, actual.FieldingAbility.Value);
        Assert.Equal(23, actual.ArmStrength.Value);
        Assert.Equal(24, actual.ArmAccuracy.Value);
        Assert.Equal(25, actual.ReactionTime.Value);
        Assert.Equal(26, actual.Blocking.Value);
        Assert.Equal(27, actual.Speed.Value);
        Assert.Equal(28, actual.BaseRunningAbility.Value);
        Assert.Equal(29, actual.BaseRunningAggression.Value);
    }
}