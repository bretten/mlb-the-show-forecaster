using com.brettnamba.MlbTheShowForecaster.ExternalApis.MlbTheShowApi.Dtos.Items;

namespace com.brettnamba.MlbTheShowForecaster.GameCards.Infrastructure.Tests.Dtos.Mapping.TestClasses;

/// <summary>
/// Creates fakes
/// </summary>
public static class Faker
{
    public static MlbCardDto FakeMlbCardDto(string uuid = "id1", string type = "mlb_card", string imageUrl = "img1.png",
        string name = "name1", string rarity = "Bronze", bool isSellable = false, string series = "Live",
        string teamShortName = "SEA", string displayPosition = "RF", int overall = 50, int stamina = 1,
        int pitchingClutch = 2, int hitsPerBf = 3, int kPerBf = 4, int bbPerBf = 5, int hrPerBf = 6,
        int pitchVelocity = 7, int pitchControl = 8, int pitchMovement = 9, int contactLeft = 10, int contactRight = 1,
        int powerLeft = 2, int powerRight = 3, int plateVision = 4, int plateDiscipline = 5, int battingClutch = 6,
        int buntingAbility = 7, int dragBuntingAbility = 8, int hittingDurability = 9, int fieldingDurability = 10,
        int fieldingAbility = 1, int armStrength = 2, int armAccuracy = 3, int reactionTime = 4, int blocking = 5,
        int speed = 6, int baseRunningAbility = 7, int baseRunningAggression = 8, int scalar = 1)
    {
        return new MlbCardDto(
            Uuid: uuid,
            Type: type,
            ImageUrl: imageUrl,
            Name: name,
            Rarity: rarity,
            IsSellable: isSellable,
            Series: series,
            TeamShortName: teamShortName,
            DisplayPosition: displayPosition,
            Overall: overall,
            Stamina: scalar * stamina,
            PitchingClutch: scalar * pitchingClutch,
            HitsPerBf: scalar * hitsPerBf,
            KPerBf: scalar * kPerBf,
            BbPerBf: scalar * bbPerBf,
            HrPerBf: scalar * hrPerBf,
            PitchVelocity: scalar * pitchVelocity,
            PitchControl: scalar * pitchControl,
            PitchMovement: scalar * pitchMovement,
            ContactLeft: scalar * contactLeft,
            ContactRight: scalar * contactRight,
            PowerLeft: scalar * powerLeft,
            PowerRight: scalar * powerRight,
            PlateVision: scalar * plateVision,
            PlateDiscipline: scalar * plateDiscipline,
            BattingClutch: scalar * battingClutch,
            BuntingAbility: scalar * buntingAbility,
            DragBuntingAbility: scalar * dragBuntingAbility,
            HittingDurability: scalar * hittingDurability,
            FieldingDurability: scalar * fieldingDurability,
            FieldingAbility: scalar * fieldingAbility,
            ArmStrength: scalar * armStrength,
            ArmAccuracy: scalar * armAccuracy,
            ReactionTime: scalar * reactionTime,
            Blocking: scalar * blocking,
            Speed: scalar * speed,
            BaseRunningAbility: scalar * baseRunningAbility,
            BaseRunningAggression: scalar * baseRunningAggression
        );
    }
}