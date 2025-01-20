using com.brettnamba.MlbTheShowForecaster.GameCards.Application.Dtos;
using com.brettnamba.MlbTheShowForecaster.GameCards.Domain.Tests.Cards.TestClasses;

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

    [Fact]
    public void SubtractFrom_PlayerCardAttributes_ReturnsDifferenceOfAttributes()
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
            stamina: 100,
            pitchingClutch: 100,
            hitsPerNine: 100,
            strikeoutsPerNine: 100,
            baseOnBallsPerNine: 100,
            homeRunsPerNine: 100,
            pitchVelocity: 100,
            pitchControl: 100,
            pitchMovement: 100,
            contactLeft: 100,
            contactRight: 100,
            powerLeft: 100,
            powerRight: 100,
            plateVision: 100,
            plateDiscipline: 100,
            battingClutch: 100,
            buntingAbility: 100,
            dragBuntingAbility: 100,
            hittingDurability: 100,
            fieldingDurability: 100,
            fieldingAbility: 100,
            armStrength: 100,
            armAccuracy: 100,
            reactionTime: 100,
            blocking: 100,
            speed: 100,
            baseRunningAbility: 100,
            baseRunningAggression: 100
        );

        // Act
        var actual = attributeChanges.SubtractFrom(currentAttributes);

        // Assert
        Assert.Equal(100 - 1, actual.Stamina.Value);
        Assert.Equal(100 - 2, actual.PitchingClutch.Value);
        Assert.Equal(100 - 3, actual.HitsPerNine.Value);
        Assert.Equal(100 - 4, actual.StrikeoutsPerNine.Value);
        Assert.Equal(100 - 5, actual.BaseOnBallsPerNine.Value);
        Assert.Equal(100 - 6, actual.HomeRunsPerNine.Value);
        Assert.Equal(100 - 7, actual.PitchVelocity.Value);
        Assert.Equal(100 - 8, actual.PitchControl.Value);
        Assert.Equal(100 - 9, actual.PitchMovement.Value);
        Assert.Equal(100 - 10, actual.ContactLeft.Value);
        Assert.Equal(100 - 11, actual.ContactRight.Value);
        Assert.Equal(100 - 12, actual.PowerLeft.Value);
        Assert.Equal(100 - 13, actual.PowerRight.Value);
        Assert.Equal(100 - 14, actual.PlateVision.Value);
        Assert.Equal(100 - 15, actual.PlateDiscipline.Value);
        Assert.Equal(100 - 16, actual.BattingClutch.Value);
        Assert.Equal(100 - 17, actual.BuntingAbility.Value);
        Assert.Equal(100 - 18, actual.DragBuntingAbility.Value);
        Assert.Equal(100 - 19, actual.HittingDurability.Value);
        Assert.Equal(100 - 20, actual.FieldingDurability.Value);
        Assert.Equal(100 - 21, actual.FieldingAbility.Value);
        Assert.Equal(100 - 22, actual.ArmStrength.Value);
        Assert.Equal(100 - 23, actual.ArmAccuracy.Value);
        Assert.Equal(100 - 24, actual.ReactionTime.Value);
        Assert.Equal(100 - 25, actual.Blocking.Value);
        Assert.Equal(100 - 26, actual.Speed.Value);
        Assert.Equal(100 - 27, actual.BaseRunningAbility.Value);
        Assert.Equal(100 - 28, actual.BaseRunningAggression.Value);
    }
}