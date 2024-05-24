using com.brettnamba.MlbTheShowForecaster.ExternalApis.MlbTheShowApi.Dtos.Items;
using com.brettnamba.MlbTheShowForecaster.ExternalApis.MlbTheShowApi.Dtos.Listings;
using com.brettnamba.MlbTheShowForecaster.ExternalApis.MlbTheShowApi.Dtos.RosterUpdates;
using com.brettnamba.MlbTheShowForecaster.ExternalApis.MlbTheShowApi.Responses.RosterUpdates;

namespace com.brettnamba.MlbTheShowForecaster.GameCards.Infrastructure.Tests.Dtos.Mapping.TestClasses;

/// <summary>
/// Creates fakes
/// </summary>
public static class Faker
{
    public static Guid FakeGuid1 = new("00000000-0000-0000-0000-000000000001");
    public static Guid FakeGuid2 = new("00000000-0000-0000-0000-000000000002");
    public static Guid FakeGuid3 = new("00000000-0000-0000-0000-000000000003");

    public static UuidDto FakeUuidDto(Guid? guid = null)
    {
        return new UuidDto(guid ?? FakeGuid1);
    }

    public static MlbCardDto FakeMlbCardDto(Guid? uuid = null, string type = "mlb_card", string imageUrl = "img1.png",
        string name = "name1", string rarity = "Bronze", bool isSellable = false, string series = "Live",
        string teamShortName = "SEA", string displayPosition = "RF", int overall = 50, int stamina = 1,
        int pitchingClutch = 2, int hitsPerBf = 3, int kPerBf = 4, int bbPerBf = 5, int hrPerBf = 6,
        int pitchVelocity = 7, int pitchControl = 8, int pitchMovement = 9, int contactLeft = 10, int contactRight = 1,
        int powerLeft = 2, int powerRight = 3, int plateVision = 4, int plateDiscipline = 5, int battingClutch = 6,
        int buntingAbility = 7, int dragBuntingAbility = 8, int hittingDurability = 9, int fieldingDurability = 10,
        int fieldingAbility = 1, int armStrength = 2, int armAccuracy = 3, int reactionTime = 4, int blocking = 5,
        int speed = 6, int baseRunningAbility = 7, int baseRunningAggression = 8, int scalar = 1,
        bool hasAugment = false, string augmentText = "Hit 5 HRs", DateTime? augmentEndDate = null, int newRank = 60,
        bool hasRankChange = false)
    {
        return new MlbCardDto(
            Uuid: FakeUuidDto(uuid),
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
            BaseRunningAggression: scalar * baseRunningAggression,
            HasAugment: hasAugment,
            AugmentText: augmentText,
            AugmentEndDate: augmentEndDate ?? new DateTime(2024, 5, 23, 1, 2, 3),
            NewRank: newRank,
            HasRankChange: hasRankChange
        );
    }

    public static StadiumDto FakeStadiumDto(Guid? uuid = null, string type = "mlb_card", string imageUrl = "img1.png",
        string name = "name1", string rarity = "Bronze", bool isSellable = false, string teamShortName = "SEA",
        string capacity = "12300", string surface = "Grass", string elevation = "10000", int built = 2000)
    {
        return new StadiumDto(
            Uuid: FakeUuidDto(uuid),
            Type: type,
            ImageUrl: imageUrl,
            Name: name,
            Rarity: rarity,
            IsSellable: isSellable,
            TeamShortName: teamShortName,
            Capacity: capacity,
            Surface: surface,
            Elevation: elevation,
            Built: built
        );
    }

    public static ListingDto<ItemDto> FakeListingDto(string listingName = "name1", int bestSellPrice = 0,
        int bestBuyPrice = 0, ItemDto? itemDto = null, IReadOnlyList<ListingPriceDto>? priceHistory = null)
    {
        return new ListingDto<ItemDto>(listingName,
            BestSellPrice: bestSellPrice,
            BestBuyPrice: bestBuyPrice,
            Item: itemDto ?? FakeMlbCardDto(),
            PriceHistory: priceHistory ?? new List<ListingPriceDto>()
        );
    }

    public static PlayerAttributeChangeDto FakePlayerAttributeChangeDto(Guid? uuid = null, string name = "name1",
        string team = "team1", MlbCardDto? item = null, int currentRank = 70, string currentRarity = "Bronze",
        int oldRank = 50, string oldRarity = "Common", IEnumerable<AttributeChangeDto>? attributeChangeDtos = null)
    {
        return new PlayerAttributeChangeDto(
            FakeUuidDto(uuid),
            Name: name,
            Team: team,
            Item: item ?? FakeMlbCardDto(uuid),
            CurrentRank: currentRank,
            CurrentRarity: currentRarity,
            OldRank: oldRank,
            OldRarity: oldRarity,
            attributeChangeDtos ?? new List<AttributeChangeDto>()
        );
    }

    public static AttributeChangeDto FakeAttributeChangeDto(string name = "acc", string currentValue = "50",
        string direction = "positive", string delta = "+5", string color = "yellow")
    {
        return new AttributeChangeDto(
            Name: name,
            CurrentValue: currentValue,
            Direction: direction,
            Delta: delta,
            Color: color
        );
    }

    public static PlayerPositionChangeDto FakePlayerPositionChangeDto(Guid? uuid = null, string name = "name1",
        string team = "team1", MlbCardDto? item = null, string position = "1B")
    {
        return new PlayerPositionChangeDto(FakeUuidDto(uuid),
            Name: name,
            Team: team,
            Item: item ?? FakeMlbCardDto(),
            Position: position
        );
    }

    public static NewlyAddedPlayerDto FakeNewlyAddedPlayerDto(Guid? uuid = null, string name = "name1",
        string team = "team1", string position = "1B", int currentRank = 70, string currentRarity = "Bronze")
    {
        return new NewlyAddedPlayerDto(FakeUuidDto(uuid),
            Name: name,
            Team: team,
            Position: position,
            CurrentRank: currentRank,
            CurrentRarity: currentRarity
        );
    }

    public static GetRosterUpdateResponse FakeGetRosterUpdateResponse(
        IEnumerable<PlayerAttributeChangeDto>? playerAttributeChanges = null,
        IEnumerable<PlayerPositionChangeDto>? positionChanges = null,
        IEnumerable<NewlyAddedPlayerDto>? newlyAddedPlayers = null)
    {
        return new GetRosterUpdateResponse(playerAttributeChanges ?? new List<PlayerAttributeChangeDto>(),
            positionChanges ?? new List<PlayerPositionChangeDto>(),
            newlyAddedPlayers ?? new List<NewlyAddedPlayerDto>()
        );
    }
}