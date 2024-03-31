using com.brettnamba.MlbTheShowForecaster.Common.Domain.Enums;
using com.brettnamba.MlbTheShowForecaster.Common.Domain.ValueObjects;
using com.brettnamba.MlbTheShowForecaster.GameCards.Application.Dtos;
using com.brettnamba.MlbTheShowForecaster.GameCards.Domain.Cards.Enums;
using com.brettnamba.MlbTheShowForecaster.GameCards.Domain.Cards.ValueObjects;

namespace com.brettnamba.MlbTheShowForecaster.GameCards.Application.Tests.Dtos.TestClasses;

/// <summary>
/// Creates fakes
/// </summary>
public static class Faker
{
    public static MlbPlayerCard FakeMlbPlayerCard(ushort year = 2024, string cardExternalId = "id1",
        CardType type = CardType.MlbCard, string image = "img.png", string name = "name1",
        Rarity rarity = Rarity.Bronze, CardSeries series = CardSeries.Live, Position position = Position.RightField,
        string teamShortName = "DOT", int overallRating = 90, int stamina = 1, int pitchingClutch = 2,
        int hitsPerNine = 3, int strikeoutsPerNine = 4, int baseOnBallsPerNine = 5, int homeRunsPerNine = 6,
        int pitchVelocity = 7, int pitchControl = 8, int pitchMovement = 9, int contactLeft = 10, int contactRight = 1,
        int powerLeft = 2, int powerRight = 3, int plateVision = 4, int plateDiscipline = 5, int battingClutch = 6,
        int buntingAbility = 7, int dragBuntingAbility = 8, int hittingDurability = 9, int fieldingDurability = 10,
        int fieldingAbility = 1, int armStrength = 2, int armAccuracy = 3, int reactionTime = 4, int blocking = 5,
        int speed = 6, int baseRunningAbility = 7, int baseRunningAggression = 8, int scalar = 1)
    {
        return new MlbPlayerCard(
            Year: SeasonYear.Create(year),
            ExternalUuid: CardExternalId.Create(cardExternalId),
            Type: type,
            ImageUrl: CardImageLocation.Create(image),
            Name: CardName.Create(name),
            Rarity: rarity,
            IsSellable: true,
            Series: series,
            Position: position,
            TeamShortName: TeamShortName.Create(teamShortName),
            Overall: OverallRating.Create(overallRating),
            Stamina: AbilityAttribute.Create(scalar * stamina),
            PitchingClutch: AbilityAttribute.Create(scalar * pitchingClutch),
            HitsPerBf: AbilityAttribute.Create(scalar * hitsPerNine),
            KPerBf: AbilityAttribute.Create(scalar * strikeoutsPerNine),
            BbPerBf: AbilityAttribute.Create(scalar * baseOnBallsPerNine),
            HrPerBf: AbilityAttribute.Create(scalar * homeRunsPerNine),
            PitchVelocity: AbilityAttribute.Create(scalar * pitchVelocity),
            PitchControl: AbilityAttribute.Create(scalar * pitchControl),
            PitchMovement: AbilityAttribute.Create(scalar * pitchMovement),
            ContactLeft: AbilityAttribute.Create(scalar * contactLeft),
            ContactRight: AbilityAttribute.Create(scalar * contactRight),
            PowerLeft: AbilityAttribute.Create(scalar * powerLeft),
            PowerRight: AbilityAttribute.Create(scalar * powerRight),
            PlateVision: AbilityAttribute.Create(scalar * plateVision),
            PlateDiscipline: AbilityAttribute.Create(scalar * plateDiscipline),
            BattingClutch: AbilityAttribute.Create(scalar * battingClutch),
            BuntingAbility: AbilityAttribute.Create(scalar * buntingAbility),
            DragBuntingAbility: AbilityAttribute.Create(scalar * dragBuntingAbility),
            HittingDurability: AbilityAttribute.Create(scalar * hittingDurability),
            FieldingDurability: AbilityAttribute.Create(scalar * fieldingDurability),
            FieldingAbility: AbilityAttribute.Create(scalar * fieldingAbility),
            ArmStrength: AbilityAttribute.Create(scalar * armStrength),
            ArmAccuracy: AbilityAttribute.Create(scalar * armAccuracy),
            ReactionTime: AbilityAttribute.Create(scalar * reactionTime),
            Blocking: AbilityAttribute.Create(scalar * blocking),
            Speed: AbilityAttribute.Create(scalar * speed),
            BaseRunningAbility: AbilityAttribute.Create(scalar * baseRunningAbility),
            BaseRunningAggression: AbilityAttribute.Create(scalar * baseRunningAggression)
        );
    }

    public static CardListing FakeCardListing(string listingName = "listingName1", int bestBuyPrice = 0,
        int bestSellPrice = 0, string cardExternalId = "card", IReadOnlyList<CardListingPrice>? historicalPrices = null)
    {
        return new CardListing(
            ListingName: listingName,
            BestBuyPrice: NaturalNumber.Create(bestBuyPrice),
            BestSellPrice: NaturalNumber.Create(bestSellPrice),
            CardExternalId: CardExternalId.Create(cardExternalId),
            HistoricalPrices: historicalPrices ?? new List<CardListingPrice>()
        );
    }

    public static CardListingPrice FakeCardListingPrice(DateOnly? date = null, int bestBuyPrice = 0,
        int bestSellPrice = 0)
    {
        return new CardListingPrice(
            Date: date ?? new DateOnly(2024, 4, 1),
            BestBuyPrice: NaturalNumber.Create(bestBuyPrice),
            BestSellPrice: NaturalNumber.Create(bestSellPrice)
        );
    }

    public static AttributeChange FakeAttributeChange(string attributeName = "attributeName1", int newValue = 10,
        int changeAmount = 1)
    {
        return new AttributeChange(
            AttributeName: attributeName,
            NewValue: NaturalNumber.Create(newValue),
            ChangeAmount: changeAmount
        );
    }

    public static PlayerRatingChange FakePlayerRatingChange(string cardExternalId = "id1", int newOverallRating = 90,
        Rarity newRarity = Rarity.Diamond, int oldOverallRating = 50, Rarity oldRarity = Rarity.Common,
        List<AttributeChange>? attributeChanges = null)
    {
        return new PlayerRatingChange(
            CardExternalId: CardExternalId.Create(cardExternalId),
            NewRating: OverallRating.Create(newOverallRating),
            NewRarity: newRarity,
            OldRating: OverallRating.Create(oldOverallRating),
            OldRarity: oldRarity,
            AttributeChanges: attributeChanges ?? new List<AttributeChange>()
        );
    }
}