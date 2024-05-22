using com.brettnamba.MlbTheShowForecaster.Common.Application.Cqrs;
using com.brettnamba.MlbTheShowForecaster.Common.Domain.ValueObjects;
using com.brettnamba.MlbTheShowForecaster.GameCards.Application.Commands.UpdatePlayerCard;
using com.brettnamba.MlbTheShowForecaster.GameCards.Application.Dtos;
using com.brettnamba.MlbTheShowForecaster.GameCards.Application.Queries.GetPlayerCardByExternalId;
using com.brettnamba.MlbTheShowForecaster.GameCards.Application.Services;
using com.brettnamba.MlbTheShowForecaster.GameCards.Application.Services.Exceptions;
using com.brettnamba.MlbTheShowForecaster.GameCards.Application.Tests.TestClasses;
using com.brettnamba.MlbTheShowForecaster.GameCards.Domain.Cards.Entities;
using com.brettnamba.MlbTheShowForecaster.GameCards.Domain.Cards.Enums;
using com.brettnamba.MlbTheShowForecaster.GameCards.Domain.Cards.ValueObjects;
using com.brettnamba.MlbTheShowForecaster.GameCards.Domain.Cards.ValueObjects.PlayerCards;
using Moq;

namespace com.brettnamba.MlbTheShowForecaster.GameCards.Application.Tests.Services;

public class PlayerRatingHistoryServiceTests
{
    [Fact]
    public async Task SyncHistory_NoMatchingPlayerCard_ThrowsException()
    {
        /*
         * Arrange
         */
        var cToken = CancellationToken.None;
        var seasonYear = SeasonYear.Create(2024);
        // RosterUpdate1 with RatingChange
        PlayerCard? playerCard = null; // RatingChange1 has no corresponding PlayerCard
        var cardExternalId = Faker.FakeGuid1;
        var ratingChange = Dtos.TestClasses.Faker.FakePlayerRatingChange(cardExternalId: cardExternalId);
        var getPlayerCardQuery = new GetPlayerCardByExternalIdQuery(ratingChange.CardExternalId);

        var rosterUpdate1 = Dtos.TestClasses.Faker.FakeRosterUpdate(new DateOnly(2024, 4, 1),
            new List<PlayerRatingChange>() { ratingChange });

        // Query sender
        var stubQuerySender = new Mock<IQuerySender>();
        stubQuerySender.Setup(x => x.Send(getPlayerCardQuery, cToken))
            .ReturnsAsync(playerCard);

        // Command sender
        var mockCommandSender = Mock.Of<ICommandSender>();

        // Roster update feed
        var stubRosterUpdateFeed = new Mock<IRosterUpdateFeed>();
        stubRosterUpdateFeed.Setup(x => x.GetNewRosterUpdates(seasonYear, cToken))
            .ReturnsAsync(new List<RosterUpdate>() { rosterUpdate1 });

        // Card catalog
        var mockCardCatalog = Mock.Of<ICardCatalog>();

        // Service
        var service = new PlayerRatingHistoryService(stubRosterUpdateFeed.Object, mockCardCatalog,
            stubQuerySender.Object, mockCommandSender);
        var action = async () => await service.SyncHistory(seasonYear, cToken);

        /*
         * Act
         */
        var actual = await Record.ExceptionAsync(action);

        /*
         * Assert
         */
        Assert.NotNull(actual);
        Assert.IsType<NoPlayerCardFoundForRosterUpdateException>(actual);
    }

    [Fact]
    public async Task SyncHistory_SingleRatingChange_AddsInitialStateToHistory()
    {
        /*
         * Arrange
         */
        var cToken = CancellationToken.None;
        // PlayerCard has no historical ratings
        var playerCard = Faker.FakePlayerCard(cardExternalId: CardExternalId.Value);

        // Query for PlayerCard
        var getPlayerCardQuery = new GetPlayerCardByExternalIdQuery(CardExternalId);

        // RosterUpdate1 with RatingChange1
        var rosterUpdate1 = Dtos.TestClasses.Faker.FakeRosterUpdate(RatingChange1.Date,
            new List<PlayerRatingChange>() { RatingChange1 });

        // Query sender
        var stubQuerySender = new Mock<IQuerySender>();
        stubQuerySender.Setup(x => x.Send(getPlayerCardQuery, cToken))
            .ReturnsAsync(playerCard);

        // Command sender
        var mockCommandSender = new Mock<ICommandSender>();

        // Roster update feed
        var stubRosterUpdateFeed = new Mock<IRosterUpdateFeed>();
        stubRosterUpdateFeed.Setup(x => x.GetNewRosterUpdates(Year, cToken))
            .ReturnsAsync(new List<RosterUpdate>() { rosterUpdate1 });

        // Card catalog
        var mockCardCatalog = new Mock<ICardCatalog>();
        mockCardCatalog.Setup(x => x.GetMlbPlayerCard(Year, CardExternalId, cToken))
            .ReturnsAsync(ExternalCardCurrentState);

        // Service
        var service = new PlayerRatingHistoryService(stubRosterUpdateFeed.Object, mockCardCatalog.Object,
            stubQuerySender.Object, mockCommandSender.Object);

        /*
         * Act
         */
        var actual = await service.SyncHistory(Year, cToken);
        service.Dispose();

        /*
         * Assert
         */
        // There should be a single historical rating
        Assert.Single(playerCard.HistoricalRatingsChronologically);
        var historical1 = playerCard.HistoricalRatingsChronologically[0];
        Assert.Equal(new DateOnly(2024, 1, 1), historical1.StartDate);
        Assert.Equal(new DateOnly(2024, 4, 21), historical1.EndDate);
        Assert.Equal(80, historical1.OverallRating.Value);
        Assert.Equal(AttributesBeforeRatingChange1IsApplied, historical1.Attributes);
        // An update command should have been sent with the most recent rating change
        var expectedCommand = new UpdatePlayerCardCommand(playerCard, ExternalCardCurrentState, RatingChange1, null);
        mockCommandSender.Verify(x => x.Send(expectedCommand, cToken), Times.Once);
        // The result should contain the updated PlayerCard
        Assert.Contains(playerCard, actual.UpdatedPlayerCards);
    }

    [Fact]
    public async Task SyncHistory_MultipleRatingChanges_AddsAllHistory()
    {
        /*
         * Arrange
         */
        var cToken = CancellationToken.None;
        // PlayerCard has no historical ratings
        var playerCard = Faker.FakePlayerCard(cardExternalId: CardExternalId.Value);

        // Query for PlayerCard
        var getPlayerCardQuery = new GetPlayerCardByExternalIdQuery(CardExternalId);

        // RosterUpdate1 with RatingChange1
        var rosterUpdate1 = Dtos.TestClasses.Faker.FakeRosterUpdate(RatingChange1.Date,
            new List<PlayerRatingChange>() { RatingChange1 });

        // RosterUpdate2 with RatingChange2
        var rosterUpdate2 = Dtos.TestClasses.Faker.FakeRosterUpdate(RatingChange2.Date,
            new List<PlayerRatingChange>() { RatingChange2 });

        // RosterUpdate3 with RatingChange3
        var rosterUpdate3 = Dtos.TestClasses.Faker.FakeRosterUpdate(RatingChange3.Date,
            new List<PlayerRatingChange>() { RatingChange3 });

        // Query sender
        var stubQuerySender = new Mock<IQuerySender>();
        stubQuerySender.Setup(x => x.Send(getPlayerCardQuery, cToken))
            .ReturnsAsync(playerCard);

        // Command sender
        var mockCommandSender = new Mock<ICommandSender>();

        // Roster update feed
        var stubRosterUpdateFeed = new Mock<IRosterUpdateFeed>();
        stubRosterUpdateFeed.Setup(x => x.GetNewRosterUpdates(Year, cToken))
            .ReturnsAsync(new List<RosterUpdate>() { rosterUpdate1, rosterUpdate2, rosterUpdate3 });

        // Card catalog
        var mockCardCatalog = new Mock<ICardCatalog>();
        mockCardCatalog.Setup(x => x.GetMlbPlayerCard(Year, CardExternalId, cToken))
            .ReturnsAsync(ExternalCardCurrentState);

        // Service
        var service = new PlayerRatingHistoryService(stubRosterUpdateFeed.Object, mockCardCatalog.Object,
            stubQuerySender.Object, mockCommandSender.Object);

        /*
         * Act
         */
        var actual = await service.SyncHistory(Year, cToken);

        /*
         * Assert
         */
        // There should be three historical ratings
        Assert.Equal(3, playerCard.HistoricalRatingsChronologically.Count);
        var historical1 = playerCard.HistoricalRatingsChronologically[0];
        Assert.Equal(new DateOnly(2024, 1, 1), historical1.StartDate);
        Assert.Equal(new DateOnly(2024, 4, 7), historical1.EndDate);
        Assert.Equal(70, historical1.OverallRating.Value);
        Assert.Equal(AttributesBeforeRatingChange1And2And3IsApplied, historical1.Attributes);
        var historical2 = playerCard.HistoricalRatingsChronologically[1];
        Assert.Equal(new DateOnly(2024, 4, 7), historical2.StartDate);
        Assert.Equal(new DateOnly(2024, 4, 14), historical2.EndDate);
        Assert.Equal(75, historical2.OverallRating.Value);
        Assert.Equal(AttributesBeforeRatingChange1And2IsApplied, historical2.Attributes);
        var historical3 = playerCard.HistoricalRatingsChronologically[2];
        Assert.Equal(new DateOnly(2024, 4, 14), historical3.StartDate);
        Assert.Equal(new DateOnly(2024, 4, 21), historical3.EndDate);
        Assert.Equal(80, historical3.OverallRating.Value);
        Assert.Equal(AttributesBeforeRatingChange1IsApplied, historical3.Attributes);
        // An update command should have been sent with the most recent rating change
        var expectedCommand = new UpdatePlayerCardCommand(playerCard, ExternalCardCurrentState, RatingChange1, null);
        mockCommandSender.Verify(x => x.Send(expectedCommand, cToken), Times.Once);
        // The result should contain the updated PlayerCard
        Assert.Contains(playerCard, actual.UpdatedPlayerCards);
    }

    [Fact]
    public async Task SyncHistory_MultipleRatingChangesWithSomeAlreadyAdded_AddsOnlyNewHistory()
    {
        /*
         * Arrange
         */
        var cToken = CancellationToken.None;
        // PlayerCard has no historical ratings
        var playerCard = Faker.FakePlayerCard(cardExternalId: CardExternalId.Value);
        // Add RatingChange3 (the oldest) to the history
        playerCard.AddHistoricalRating(Faker.FakePlayerCardHistoricalRating(new DateOnly(2024, 1, 1),
            RatingChange3.Date, RatingChange3.OldRating.Value, AttributesBeforeRatingChange1And2And3IsApplied));
        playerCard.AddHistoricalRating(Faker.FakePlayerCardHistoricalRating(RatingChange3.Date,
            RatingChange2.Date, RatingChange2.OldRating.Value, AttributesBeforeRatingChange1And2IsApplied));

        // Query for PlayerCard
        var getPlayerCardQuery = new GetPlayerCardByExternalIdQuery(CardExternalId);

        // RosterUpdate1 with RatingChange1
        var rosterUpdate1 = Dtos.TestClasses.Faker.FakeRosterUpdate(RatingChange1.Date,
            new List<PlayerRatingChange>() { RatingChange1 });

        // RosterUpdate2 with RatingChange2
        var rosterUpdate2 = Dtos.TestClasses.Faker.FakeRosterUpdate(RatingChange2.Date,
            new List<PlayerRatingChange>() { RatingChange2 });

        // RosterUpdate3 with RatingChange3
        var rosterUpdate3 = Dtos.TestClasses.Faker.FakeRosterUpdate(RatingChange3.Date,
            new List<PlayerRatingChange>() { RatingChange3 });

        // Query sender
        var stubQuerySender = new Mock<IQuerySender>();
        stubQuerySender.Setup(x => x.Send(getPlayerCardQuery, cToken))
            .ReturnsAsync(playerCard);

        // Command sender
        var mockCommandSender = new Mock<ICommandSender>();

        // Roster update feed
        var stubRosterUpdateFeed = new Mock<IRosterUpdateFeed>();
        stubRosterUpdateFeed.Setup(x => x.GetNewRosterUpdates(Year, cToken))
            .ReturnsAsync(new List<RosterUpdate>() { rosterUpdate1, rosterUpdate2, rosterUpdate3 });

        // Card catalog
        var mockCardCatalog = new Mock<ICardCatalog>();
        mockCardCatalog.Setup(x => x.GetMlbPlayerCard(Year, CardExternalId, cToken))
            .ReturnsAsync(ExternalCardCurrentState);

        // Service
        var service = new PlayerRatingHistoryService(stubRosterUpdateFeed.Object, mockCardCatalog.Object,
            stubQuerySender.Object, mockCommandSender.Object);

        /*
         * Act
         */
        var actual = await service.SyncHistory(Year, cToken);

        /*
         * Assert
         */
        // There should be three historical ratings
        Assert.Equal(3, playerCard.HistoricalRatingsChronologically.Count);
        var historical1 = playerCard.HistoricalRatingsChronologically[0];
        Assert.Equal(new DateOnly(2024, 1, 1), historical1.StartDate);
        Assert.Equal(new DateOnly(2024, 4, 7), historical1.EndDate);
        Assert.Equal(70, historical1.OverallRating.Value);
        Assert.Equal(AttributesBeforeRatingChange1And2And3IsApplied, historical1.Attributes);
        var historical2 = playerCard.HistoricalRatingsChronologically[1];
        Assert.Equal(new DateOnly(2024, 4, 7), historical2.StartDate);
        Assert.Equal(new DateOnly(2024, 4, 14), historical2.EndDate);
        Assert.Equal(75, historical2.OverallRating.Value);
        Assert.Equal(AttributesBeforeRatingChange1And2IsApplied, historical2.Attributes);
        var historical3 = playerCard.HistoricalRatingsChronologically[2];
        Assert.Equal(new DateOnly(2024, 4, 14), historical3.StartDate);
        Assert.Equal(new DateOnly(2024, 4, 21), historical3.EndDate);
        Assert.Equal(80, historical3.OverallRating.Value);
        Assert.Equal(AttributesBeforeRatingChange1IsApplied, historical3.Attributes);
        // An update command should have been sent with the most recent rating change
        var expectedCommand = new UpdatePlayerCardCommand(playerCard, ExternalCardCurrentState, RatingChange1, null);
        mockCommandSender.Verify(x => x.Send(expectedCommand, cToken), Times.Once);
        // The result should contain the updated PlayerCard
        Assert.Contains(playerCard, actual.UpdatedPlayerCards);
    }

    [Fact]
    public async Task SyncHistory_AllRatingChangesAlreadyAdded_SkipsUpdate()
    {
        /*
         * Arrange
         */
        var cToken = CancellationToken.None;
        // PlayerCard has no historical ratings
        var playerCard = Faker.FakePlayerCard(cardExternalId: CardExternalId.Value);
        // Add RatingChange3 (the oldest) to the history
        playerCard.AddHistoricalRating(Faker.FakePlayerCardHistoricalRating(new DateOnly(2024, 1, 1),
            RatingChange1.Date, RatingChange1.OldRating.Value, AttributesBeforeRatingChange1IsApplied));

        // Query for PlayerCard
        var getPlayerCardQuery = new GetPlayerCardByExternalIdQuery(CardExternalId);

        // RosterUpdate1 with RatingChange1
        var rosterUpdate1 = Dtos.TestClasses.Faker.FakeRosterUpdate(RatingChange1.Date,
            new List<PlayerRatingChange>() { RatingChange1 });

        // Query sender
        var stubQuerySender = new Mock<IQuerySender>();
        stubQuerySender.Setup(x => x.Send(getPlayerCardQuery, cToken))
            .ReturnsAsync(playerCard);

        // Command sender
        var mockCommandSender = new Mock<ICommandSender>();

        // Roster update feed
        var stubRosterUpdateFeed = new Mock<IRosterUpdateFeed>();
        stubRosterUpdateFeed.Setup(x => x.GetNewRosterUpdates(Year, cToken))
            .ReturnsAsync(new List<RosterUpdate>() { rosterUpdate1 });

        // Card catalog
        var mockCardCatalog = new Mock<ICardCatalog>();
        mockCardCatalog.Setup(x => x.GetMlbPlayerCard(Year, CardExternalId, cToken))
            .ReturnsAsync(ExternalCardCurrentState);

        // Service
        var service = new PlayerRatingHistoryService(stubRosterUpdateFeed.Object, mockCardCatalog.Object,
            stubQuerySender.Object, mockCommandSender.Object);

        /*
         * Act
         */
        var actual = await service.SyncHistory(Year, cToken);

        /*
         * Assert
         */
        // There should be a single historical rating
        Assert.Single(playerCard.HistoricalRatingsChronologically);
        var historical1 = playerCard.HistoricalRatingsChronologically[0];
        Assert.Equal(new DateOnly(2024, 1, 1), historical1.StartDate);
        Assert.Equal(new DateOnly(2024, 4, 21), historical1.EndDate);
        Assert.Equal(80, historical1.OverallRating.Value);
        Assert.Equal(AttributesBeforeRatingChange1IsApplied, historical1.Attributes);
        // No update command should have been sent
        var expectedCommand = new UpdatePlayerCardCommand(playerCard, ExternalCardCurrentState, RatingChange1, null);
        mockCommandSender.Verify(x => x.Send(expectedCommand, cToken), Times.Never);
        // The result should contain the updated PlayerCard
        Assert.DoesNotContain(playerCard, actual.UpdatedPlayerCards);
    }

    private static readonly SeasonYear Year = SeasonYear.Create(2024);
    private static readonly CardExternalId CardExternalId = CardExternalId.Create(Faker.FakeGuid1);

    /// <summary>
    /// The current overall rating from the external card catalog
    /// </summary>
    private static readonly OverallRating CurrentOverallRating = Faker.FakeOverallRating(99);

    /// <summary>
    /// The current attributes from the external card catalog
    /// </summary>
    private static readonly MlbPlayerCard ExternalCardCurrentState = Dtos.TestClasses.Faker.FakeMlbPlayerCard(
        Year.Value,
        CardExternalId.Value,
        rarity: Rarity.Diamond,
        overallRating: CurrentOverallRating.Value,
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

    /// <summary>
    /// The most recent rating change for the player card
    /// </summary>
    private static readonly PlayerRatingChange RatingChange1 = Dtos.TestClasses.Faker.FakePlayerRatingChange(
        new DateOnly(2024, 4, 21),
        CardExternalId.Value,
        newOverallRating: CurrentOverallRating.Value,
        newRarity: Rarity.Diamond,
        oldOverallRating: 80,
        oldRarity: Rarity.Gold,
        Dtos.TestClasses.Faker.FakeMlbPlayerAttributeChanges(
            stamina: 25,
            pitchingClutch: 25,
            hitsPerNine: 25,
            strikeoutsPerNine: 25,
            baseOnBallsPerNine: 25,
            homeRunsPerNine: 25,
            pitchVelocity: 25,
            pitchControl: 25,
            pitchMovement: 25,
            contactLeft: 25,
            contactRight: 25,
            powerLeft: 25,
            powerRight: 25,
            plateVision: 25,
            plateDiscipline: 25,
            battingClutch: 25,
            buntingAbility: 25,
            dragBuntingAbility: 25,
            hittingDurability: 25,
            fieldingDurability: 25,
            fieldingAbility: 25,
            armStrength: 25,
            armAccuracy: 25,
            reactionTime: 25,
            blocking: 25,
            speed: 25,
            baseRunningAbility: 25,
            baseRunningAggression: 25
        )
    );

    private static readonly PlayerRatingChange RatingChange2 = Dtos.TestClasses.Faker.FakePlayerRatingChange(
        new DateOnly(2024, 4, 14),
        CardExternalId.Value,
        newOverallRating: RatingChange1.OldRating.Value,
        newRarity: Rarity.Gold,
        oldOverallRating: 75,
        oldRarity: Rarity.Silver,
        Dtos.TestClasses.Faker.FakeMlbPlayerAttributeChanges(
            stamina: 1,
            pitchingClutch: 2,
            hitsPerNine: 3,
            strikeoutsPerNine: 4,
            baseOnBallsPerNine: 5,
            homeRunsPerNine: 6,
            pitchVelocity: 7,
            pitchControl: 8,
            pitchMovement: 9,
            contactLeft: 10,
            contactRight: 11,
            powerLeft: 12,
            powerRight: 13,
            plateVision: 14,
            plateDiscipline: 15,
            battingClutch: 16,
            buntingAbility: 17,
            dragBuntingAbility: 18,
            hittingDurability: 19,
            fieldingDurability: 20,
            fieldingAbility: 21,
            armStrength: 22,
            armAccuracy: 23,
            reactionTime: 24,
            blocking: 25,
            speed: 26,
            baseRunningAbility: 27,
            baseRunningAggression: 28
        )
    );

    private static readonly PlayerRatingChange RatingChange3 = Dtos.TestClasses.Faker.FakePlayerRatingChange(
        new DateOnly(2024, 4, 7),
        CardExternalId.Value,
        newOverallRating: RatingChange2.OldRating.Value,
        newRarity: Rarity.Silver,
        oldOverallRating: 70,
        oldRarity: Rarity.Bronze,
        Dtos.TestClasses.Faker.FakeMlbPlayerAttributeChanges(
            stamina: 2,
            pitchingClutch: 3,
            hitsPerNine: 4,
            strikeoutsPerNine: 5,
            baseOnBallsPerNine: 6,
            homeRunsPerNine: 7,
            pitchVelocity: 8,
            pitchControl: 9,
            pitchMovement: 10,
            contactLeft: 11,
            contactRight: 12,
            powerLeft: 13,
            powerRight: 14,
            plateVision: 15,
            plateDiscipline: 16,
            battingClutch: 17,
            buntingAbility: 18,
            dragBuntingAbility: 19,
            hittingDurability: 20,
            fieldingDurability: 21,
            fieldingAbility: 22,
            armStrength: 23,
            armAccuracy: 24,
            reactionTime: 25,
            blocking: 26,
            speed: 27,
            baseRunningAbility: 28,
            baseRunningAggression: 29
        )
    );

    private static readonly PlayerCardAttributes AttributesBeforeRatingChange1IsApplied =
        Faker.FakePlayerCardAttributes(
            stamina: 100 - 25,
            pitchingClutch: 100 - 25,
            hitsPerNine: 100 - 25,
            strikeoutsPerNine: 100 - 25,
            baseOnBallsPerNine: 100 - 25,
            homeRunsPerNine: 100 - 25,
            pitchVelocity: 100 - 25,
            pitchControl: 100 - 25,
            pitchMovement: 100 - 25,
            contactLeft: 100 - 25,
            contactRight: 100 - 25,
            powerLeft: 100 - 25,
            powerRight: 100 - 25,
            plateVision: 100 - 25,
            plateDiscipline: 100 - 25,
            battingClutch: 100 - 25,
            buntingAbility: 100 - 25,
            dragBuntingAbility: 100 - 25,
            hittingDurability: 100 - 25,
            fieldingDurability: 100 - 25,
            fieldingAbility: 100 - 25,
            armStrength: 100 - 25,
            armAccuracy: 100 - 25,
            reactionTime: 100 - 25,
            blocking: 100 - 25,
            speed: 100 - 25,
            baseRunningAbility: 100 - 25,
            baseRunningAggression: 100 - 25
        );

    private static readonly PlayerCardAttributes AttributesBeforeRatingChange1And2IsApplied =
        Faker.FakePlayerCardAttributes(
            stamina: 100 - 25 - 1,
            pitchingClutch: 100 - 25 - 2,
            hitsPerNine: 100 - 25 - 3,
            strikeoutsPerNine: 100 - 25 - 4,
            baseOnBallsPerNine: 100 - 25 - 5,
            homeRunsPerNine: 100 - 25 - 6,
            pitchVelocity: 100 - 25 - 7,
            pitchControl: 100 - 25 - 8,
            pitchMovement: 100 - 25 - 9,
            contactLeft: 100 - 25 - 10,
            contactRight: 100 - 25 - 11,
            powerLeft: 100 - 25 - 12,
            powerRight: 100 - 25 - 13,
            plateVision: 100 - 25 - 14,
            plateDiscipline: 100 - 25 - 15,
            battingClutch: 100 - 25 - 16,
            buntingAbility: 100 - 25 - 17,
            dragBuntingAbility: 100 - 25 - 18,
            hittingDurability: 100 - 25 - 19,
            fieldingDurability: 100 - 25 - 20,
            fieldingAbility: 100 - 25 - 21,
            armStrength: 100 - 25 - 22,
            armAccuracy: 100 - 25 - 23,
            reactionTime: 100 - 25 - 24,
            blocking: 100 - 25 - 25,
            speed: 100 - 25 - 26,
            baseRunningAbility: 100 - 25 - 27,
            baseRunningAggression: 100 - 25 - 28
        );

    private static readonly PlayerCardAttributes AttributesBeforeRatingChange1And2And3IsApplied =
        Faker.FakePlayerCardAttributes(
            stamina: 100 - 25 - 1 - 2,
            pitchingClutch: 100 - 25 - 2 - 3,
            hitsPerNine: 100 - 25 - 3 - 4,
            strikeoutsPerNine: 100 - 25 - 4 - 5,
            baseOnBallsPerNine: 100 - 25 - 5 - 6,
            homeRunsPerNine: 100 - 25 - 6 - 7,
            pitchVelocity: 100 - 25 - 7 - 8,
            pitchControl: 100 - 25 - 8 - 9,
            pitchMovement: 100 - 25 - 9 - 10,
            contactLeft: 100 - 25 - 10 - 11,
            contactRight: 100 - 25 - 11 - 12,
            powerLeft: 100 - 25 - 12 - 13,
            powerRight: 100 - 25 - 13 - 14,
            plateVision: 100 - 25 - 14 - 15,
            plateDiscipline: 100 - 25 - 15 - 16,
            battingClutch: 100 - 25 - 16 - 17,
            buntingAbility: 100 - 25 - 17 - 18,
            dragBuntingAbility: 100 - 25 - 18 - 19,
            hittingDurability: 100 - 25 - 19 - 20,
            fieldingDurability: 100 - 25 - 20 - 21,
            fieldingAbility: 100 - 25 - 21 - 22,
            armStrength: 100 - 25 - 22 - 23,
            armAccuracy: 100 - 25 - 23 - 24,
            reactionTime: 100 - 25 - 24 - 25,
            blocking: 100 - 25 - 25 - 26,
            speed: 100 - 25 - 26 - 27,
            baseRunningAbility: 100 - 25 - 27 - 28,
            baseRunningAggression: 100 - 25 - 28 - 29
        );
}