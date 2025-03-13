using com.brettnamba.MlbTheShowForecaster.Common.Application.Pagination;
using com.brettnamba.MlbTheShowForecaster.Common.Domain.Enums;
using com.brettnamba.MlbTheShowForecaster.Common.Domain.ValueObjects;
using com.brettnamba.MlbTheShowForecaster.GameCards.Application.Dtos;
using com.brettnamba.MlbTheShowForecaster.GameCards.Application.Dtos.Reports;
using com.brettnamba.MlbTheShowForecaster.GameCards.Domain.Cards.Enums;
using com.brettnamba.MlbTheShowForecaster.GameCards.Domain.Cards.ValueObjects;

namespace com.brettnamba.MlbTheShowForecaster.GameCards.Application.Tests.Dtos.TestClasses;

/// <summary>
/// Creates fakes
/// </summary>
public static class Faker
{
    public static Guid FakeGuid1 = new("00000000-0000-0000-0000-000000000001");
    public static Guid FakeGuid2 = new("00000000-0000-0000-0000-000000000002");
    public static Guid FakeGuid3 = new("00000000-0000-0000-0000-000000000003");

    public static MlbPlayerCard FakeMlbPlayerCard(ushort year = 2024, Guid? cardExternalId = null,
        CardType type = CardType.MlbCard, string image = "img.png", string name = "name1",
        Rarity rarity = Rarity.Bronze, CardSeries series = CardSeries.Live, Position position = Position.RightField,
        string teamShortName = "DOT", int overallRating = 90, int stamina = 1, int pitchingClutch = 2,
        int hitsPerNine = 3, int strikeoutsPerNine = 4, int baseOnBallsPerNine = 5, int homeRunsPerNine = 6,
        int pitchVelocity = 7, int pitchControl = 8, int pitchMovement = 9, int contactLeft = 10, int contactRight = 1,
        int powerLeft = 2, int powerRight = 3, int plateVision = 4, int plateDiscipline = 5, int battingClutch = 6,
        int buntingAbility = 7, int dragBuntingAbility = 8, int hittingDurability = 9, int fieldingDurability = 10,
        int fieldingAbility = 1, int armStrength = 2, int armAccuracy = 3, int reactionTime = 4, int blocking = 5,
        int speed = 6, int baseRunningAbility = 7, int baseRunningAggression = 8, int scalar = 1,
        string? boostReason = null, DateTime? boostEndDate = null, int? temporaryOverallRating = null)
    {
        return new MlbPlayerCard(
            Year: SeasonYear.Create(year),
            ExternalUuid: Domain.Tests.Cards.TestClasses.Faker.FakeCardExternalId(cardExternalId),
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
            BaseRunningAggression: AbilityAttribute.Create(scalar * baseRunningAggression),
            BoostReason: boostReason,
            BoostEndDate: boostEndDate ?? new DateTime(2024, 8, 5),
            TemporaryOverallRating: temporaryOverallRating.HasValue
                ? OverallRating.Create(temporaryOverallRating.Value)
                : null
        );
    }

    public static CardListing FakeCardListing(ushort year = 2024, string name = "listingName1", int bestBuyPrice = 0,
        int bestSellPrice = 0, Guid? cardExternalId = null, IReadOnlyList<CardListingPrice>? historicalPrices = null,
        IReadOnlyList<CardListingOrder>? completedOrders = null)
    {
        return new CardListing(
            Year: SeasonYear.Create(year),
            ListingName: name,
            BestBuyPrice: NaturalNumber.Create(bestBuyPrice),
            BestSellPrice: NaturalNumber.Create(bestSellPrice),
            CardExternalId: Domain.Tests.Cards.TestClasses.Faker.FakeCardExternalId(cardExternalId),
            HistoricalPrices: historicalPrices ?? new List<CardListingPrice>(),
            RecentOrders: completedOrders ?? new List<CardListingOrder>()
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

    public static CardListingOrder FakeCompletedOrder(DateTime? date = null, int price = 0, int sequenceNumber = 0)
    {
        return new CardListingOrder(
            Date: date ?? new DateTime(2025, 1, 16, 10, 20, 0, DateTimeKind.Utc),
            Price: NaturalNumber.Create(price),
            SequenceNumber: NaturalNumber.Create(sequenceNumber)
        );
    }

    public static MlbPlayerAttributeChanges FakeMlbPlayerAttributeChanges(int stamina = 1, int pitchingClutch = 2,
        int hitsPerNine = 3, int strikeoutsPerNine = 4, int baseOnBallsPerNine = 5, int homeRunsPerNine = 6,
        int pitchVelocity = 7, int pitchControl = 8, int pitchMovement = 9, int contactLeft = 10, int contactRight = 1,
        int powerLeft = 2, int powerRight = 3, int plateVision = 4, int plateDiscipline = 5, int battingClutch = 6,
        int buntingAbility = 7, int dragBuntingAbility = 8, int hittingDurability = 9, int fieldingDurability = 10,
        int fieldingAbility = 1, int armStrength = 2, int armAccuracy = 3, int reactionTime = 4, int blocking = 5,
        int speed = 6, int baseRunningAbility = 7, int baseRunningAggression = 8, int scalar = 1)
    {
        return new MlbPlayerAttributeChanges(
            Stamina: scalar * stamina,
            PitchingClutch: scalar * pitchingClutch,
            HitsPerBf: scalar * hitsPerNine,
            KPerBf: scalar * strikeoutsPerNine,
            BbPerBf: scalar * baseOnBallsPerNine,
            HrPerBf: scalar * homeRunsPerNine,
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

    public static PlayerRatingChange FakePlayerRatingChange(DateOnly? date = null, Guid? cardExternalId = null,
        int newOverallRating = 90, Rarity newRarity = Rarity.Diamond, int oldOverallRating = 50,
        Rarity oldRarity = Rarity.Common, MlbPlayerAttributeChanges? attributeChanges = null)
    {
        return new PlayerRatingChange(
            Date: date ?? new DateOnly(2024, 4, 1),
            CardExternalId: Domain.Tests.Cards.TestClasses.Faker.FakeCardExternalId(cardExternalId),
            NewRating: OverallRating.Create(newOverallRating),
            NewRarity: newRarity,
            OldRating: OverallRating.Create(oldOverallRating),
            OldRarity: oldRarity,
            AttributeChanges: attributeChanges ?? default(MlbPlayerAttributeChanges)
        );
    }

    public static PlayerPositionChange FakePlayerPositionChange(DateOnly? date = null, Guid? cardExternalId = null,
        Position newPosition = Position.Shortstop)
    {
        return new PlayerPositionChange(
            Date: date ?? new DateOnly(2024, 4, 1),
            CardExternalId: Domain.Tests.Cards.TestClasses.Faker.FakeCardExternalId(cardExternalId),
            NewPosition: newPosition
        );
    }

    public static PlayerAddition FakePlayerAddition(DateOnly? date = null, Guid? cardExternalId = null,
        string name = "playerName1")
    {
        return new PlayerAddition(
            date: date ?? new DateOnly(2024, 4, 1),
            cardExternalId: Domain.Tests.Cards.TestClasses.Faker.FakeCardExternalId(cardExternalId),
            playerName: name
        );
    }

    public static RosterUpdate FakeRosterUpdate(DateOnly? date = null,
        IReadOnlyList<PlayerRatingChange>? ratingChanges = null,
        IReadOnlyList<PlayerPositionChange>? positionChanges = null, IReadOnlyList<PlayerAddition>? newPlayers = null)
    {
        return new RosterUpdate(
            Date: date ?? new DateOnly(2024, 4, 1),
            RatingChanges: ratingChanges ?? new List<PlayerRatingChange>(),
            PositionChanges: positionChanges ?? new List<PlayerPositionChange>(),
            NewPlayers: newPlayers ?? new List<PlayerAddition>()
        );
    }

    public static TrendReport FakeTrendReport(ushort year = 2024, Guid? externalId = null, int mlbId = 1,
        Position position = Position.RightField, int overallRating = 50, string cardName = "cardA",
        List<TrendMetricsByDate>? metricsByDate = null, List<TrendImpact>? impacts = null, int orders1H = 0,
        bool isBoosted = false, int orders24H = 0, int buyPrice = 0, decimal buyPriceChange24H = 0, int sellPrice = 0,
        decimal sellPriceChange24H = 0, decimal score = 0, decimal scoreChange2W = 0, int demand = 0)
    {
        return new TrendReport(
            Year: SeasonYear.Create(year),
            CardExternalId: Domain.Tests.Cards.TestClasses.Faker.FakeCardExternalId(externalId),
            MlbId: MlbId.Create(mlbId),
            PrimaryPosition: position,
            OverallRating: Domain.Tests.Cards.TestClasses.Faker.FakeOverallRating(overallRating),
            CardName: Domain.Tests.Cards.TestClasses.Faker.FakeCardName(cardName),
            MetricsByDate: metricsByDate ?? new List<TrendMetricsByDate>(),
            Impacts: impacts ?? new List<TrendImpact>(),
            IsBoosted: isBoosted,
            Orders1H: orders1H,
            Orders24H: orders24H,
            BuyPrice: buyPrice,
            BuyPriceChange24H: buyPriceChange24H,
            SellPrice: sellPrice,
            SellPriceChange24H: sellPriceChange24H,
            Score: score,
            ScoreChange2W: scoreChange2W,
            Demand: demand
        );
    }

    public static TrendMetricsByDate FakeTrendMetricsByDate(DateOnly? date = null,
        int buyPrice = 100,
        int sellPrice = 200,
        decimal? battingScore = 0.1m,
        bool significantBattingParticipation = false,
        decimal? pitchingScore = 0.2m,
        bool significantPitchingParticipation = false,
        decimal? fieldingScore = 0.3m,
        bool significantFieldingParticipation = false,
        decimal? battingAverage = 0.111m,
        decimal? onBasePercentage = 0.112m,
        decimal? slugging = 0.113m,
        decimal? earnedRunAverage = 0.114m,
        decimal? opponentsBattingAverage = 0.115m,
        decimal? strikeoutsPer9 = 0.116m,
        decimal? baseOnBallsPer9 = 0.117m,
        decimal? homeRunsPer9 = 0.118m,
        decimal? fieldingPercentage = 0.119m,
        int demand = 0,
        int orderCount = 0)
    {
        return new TrendMetricsByDate(date ?? new DateOnly(2024, 10, 5),
            BuyPrice: buyPrice,
            SellPrice: sellPrice,
            BattingScore: battingScore,
            SignificantBattingParticipation: significantBattingParticipation,
            PitchingScore: pitchingScore,
            SignificantPitchingParticipation: significantPitchingParticipation,
            FieldingScore: fieldingScore,
            SignificantFieldingParticipation: significantFieldingParticipation,
            BattingAverage: battingAverage,
            OnBasePercentage: onBasePercentage,
            Slugging: slugging,
            EarnedRunAverage: earnedRunAverage,
            OpponentsBattingAverage: opponentsBattingAverage,
            StrikeoutsPer9: strikeoutsPer9,
            BaseOnBallsPer9: baseOnBallsPer9,
            HomeRunsPer9: homeRunsPer9,
            FieldingPercentage: fieldingPercentage,
            Demand: demand,
            OrderCount: orderCount);
    }

    public static TrendImpact FakeTrendImpact(DateOnly? startDate = null, DateOnly? endDate = null,
        string? description = null, int demand = 0)
    {
        return new TrendImpact(Start: startDate ?? new DateOnly(2024, 10, 8),
            End: endDate ?? new DateOnly(2024, 10, 8),
            Description: description ?? "Trend impact description",
            Demand: demand);
    }

    public static PaginationResult<TrendReport> FakeTrendReportPaginationResult(int page = 1, int pageSize = 1,
        IEnumerable<TrendReport>? items = null, long totalItems = 0)
    {
        return PaginationResult<TrendReport>.Create(page, pageSize, totalItems, items ?? Array.Empty<TrendReport>());
    }

    public static Player FakePlayer(int mlbId = 1, string name = "dot spot", Position position = Position.CenterField,
        string team = "SEA")
    {
        return new Player(MlbId.Create(mlbId), name, position, TeamShortName.Create(team));
    }
}