﻿using com.brettnamba.MlbTheShowForecaster.Common.Application.Cqrs;
using com.brettnamba.MlbTheShowForecaster.Common.Domain.ValueObjects;
using com.brettnamba.MlbTheShowForecaster.GameCards.Application.Commands.UpdatePlayerCard;
using com.brettnamba.MlbTheShowForecaster.GameCards.Application.Dtos;
using com.brettnamba.MlbTheShowForecaster.GameCards.Application.Queries.GetPlayerCardByExternalId;
using com.brettnamba.MlbTheShowForecaster.GameCards.Application.Services;
using com.brettnamba.MlbTheShowForecaster.GameCards.Application.Services.Exceptions;
using com.brettnamba.MlbTheShowForecaster.GameCards.Domain.Cards.Entities;
using com.brettnamba.MlbTheShowForecaster.GameCards.Domain.Cards.ValueObjects;
using com.brettnamba.MlbTheShowForecaster.GameCards.Domain.Cards.ValueObjects.PlayerCards;
using com.brettnamba.MlbTheShowForecaster.GameCards.Domain.Tests.Cards.TestClasses;
using Microsoft.Extensions.Logging;
using Moq;

namespace com.brettnamba.MlbTheShowForecaster.GameCards.Application.Tests.Services;

public class PlayerRatingHistoryServiceTests
{
    [Fact]
    public async Task SyncHistory_NoMatchingPlayerCard_SkipsProcessingAndNoExceptionThrown()
    {
        /*
         * Arrange
         */
        var cToken = CancellationToken.None;
        // RatingChange has no corresponding PlayerCard
        PlayerCard? playerCard = null;
        var getPlayerCardQuery = new GetPlayerCardByExternalIdQuery(Year, CardExternalId);

        // Query sender
        var stubQuerySender = new Mock<IQuerySender>();
        stubQuerySender.Setup(x => x.Send(getPlayerCardQuery, cToken))
            .ReturnsAsync(playerCard);

        // Command sender
        var mockCommandSender = Mock.Of<ICommandSender>();

        // Roster update feed
        var stubRosterUpdateFeed = new Mock<IRosterUpdateFeed>();
        stubRosterUpdateFeed.Setup(x => x.GetNewRosterUpdates(Year, cToken))
            .ReturnsAsync(new AllRosterUpdates(new List<RosterUpdate>() { RosterUpdate1.Value }));

        // Card catalog
        var mockCardCatalog = Mock.Of<ICardCatalog>();

        // Logger
        var stubLogger = new Mock<ILogger<IPlayerRatingHistoryService>>();

        // Service
        var service = new PlayerRatingHistoryService(stubRosterUpdateFeed.Object, mockCardCatalog,
            stubQuerySender.Object, mockCommandSender, stubLogger.Object);

        /*
         * Act
         */
        var actual = await service.SyncHistory(Year, cToken);

        /*
         * Assert
         */
        // No cards were processed
        Assert.Empty(actual.UpdatedPlayerCards);
        // No exception was thrown, but it was logged
        stubLogger.Verify(x => x.Log(LogLevel.Warning,
                It.IsAny<EventId>(),
                It.Is<It.IsAnyType>((o, t) => o.ToString()!.Contains("there is no corresponding PlayerCard")),
                It.IsAny<NoPlayerCardFoundForRosterUpdateException>(),
                It.IsAny<Func<It.IsAnyType, Exception?, string>>()),
            Times.Once);
    }

    [Fact]
    public async Task SyncHistory_SingleRatingChange_ReturnsOneRatingForInitialStateAndOneForCurrentState()
    {
        /*
         * Arrange
         */
        var cToken = CancellationToken.None;
        // PlayerCard has no historical ratings
        var playerCard = Faker.FakePlayerCard(externalId: CardExternalId.Value);

        // Query for PlayerCard
        var getPlayerCardQuery = new GetPlayerCardByExternalIdQuery(Year, CardExternalId);

        // Query sender
        var stubQuerySender = new Mock<IQuerySender>();
        stubQuerySender.Setup(x => x.Send(getPlayerCardQuery, cToken))
            .ReturnsAsync(playerCard);

        // Command sender
        var mockCommandSender = new Mock<ICommandSender>();

        // Roster update feed
        var stubRosterUpdateFeed = new Mock<IRosterUpdateFeed>();
        stubRosterUpdateFeed.Setup(x => x.GetNewRosterUpdates(Year, cToken))
            .ReturnsAsync(new AllRosterUpdates(new List<RosterUpdate>() { RosterUpdate1.Value }));

        // Card catalog
        var stubCardCatalog = new Mock<ICardCatalog>();
        stubCardCatalog.Setup(x => x.GetMlbPlayerCard(Year, CardExternalId, cToken))
            .ReturnsAsync(CardStateAfterRosterUpdate1.Card);

        // Service
        var service = new PlayerRatingHistoryService(stubRosterUpdateFeed.Object, stubCardCatalog.Object,
            stubQuerySender.Object, mockCommandSender.Object, Mock.Of<ILogger<IPlayerRatingHistoryService>>());

        /*
         * Act
         */
        var actual = await service.SyncHistory(Year, cToken);
        service.Dispose();

        /*
         * Assert
         */
        // There should be two historical ratings
        var result = actual.UpdatedPlayerCards[playerCard];
        Assert.Equal(2, result.Count);
        // The 1st historical rating chronologically is the card's initial state: CardStateInitial
        var historical1 = result[0];
        Assert.Equal(PlayerCardHistoricalRatingType.Baseline, historical1.Type);
        Assert.Equal(RosterUpdate0.Date, historical1.StartDate);
        Assert.Null(historical1.EndDate);
        Assert.Equal(CardStateInitial.OverallRating, historical1.OverallRating);
        Assert.Equal(CardStateInitial.Card.GetAttributes(), historical1.Attributes);
        // The 2nd historical rating chronologically is from RosterUpdate1: CardStateAfterRosterUpdate1
        var historical2 = result[1];
        Assert.Equal(PlayerCardHistoricalRatingType.Baseline, historical2.Type);
        Assert.Equal(RosterUpdate1.Date, historical2.StartDate);
        Assert.Null(historical2.EndDate);
        Assert.Equal(CardStateAfterRosterUpdate1.OverallRating, historical2.OverallRating);
        Assert.Equal(CardStateAfterRosterUpdate1.Card.GetAttributes(), historical2.Attributes);
        // An update command should have been sent with the new historical ratings
        mockCommandSender.Verify(x => x.Send(
            It.Is<UpdatePlayerCardCommand>(c => c.PlayerCard == playerCard && ItIs(c.HistoricalRatings!, result)),
            cToken
        ), Times.Once);
        // The result should contain the updated PlayerCard
        Assert.Contains(playerCard, actual.UpdatedPlayerCards.Keys);
    }

    [Fact]
    public async Task SyncHistory_MultipleRatingChanges_ReturnsOneRatingForEachChangeAndOneForCurrentState()
    {
        /*
         * Arrange
         */
        var cToken = CancellationToken.None;
        // PlayerCard has no historical ratings
        var playerCard = Faker.FakePlayerCard(externalId: CardExternalId.Value);

        // Query for PlayerCard
        var getPlayerCardQuery = new GetPlayerCardByExternalIdQuery(Year, CardExternalId);

        // Query sender
        var stubQuerySender = new Mock<IQuerySender>();
        stubQuerySender.Setup(x => x.Send(getPlayerCardQuery, cToken))
            .ReturnsAsync(playerCard);

        // Command sender
        var mockCommandSender = new Mock<ICommandSender>();

        // Roster update feed
        var stubRosterUpdateFeed = new Mock<IRosterUpdateFeed>();
        stubRosterUpdateFeed.Setup(x => x.GetNewRosterUpdates(Year, cToken))
            .ReturnsAsync(new AllRosterUpdates(new List<RosterUpdate>()
                { RosterUpdate1.Value, RosterUpdate2.Value, RosterUpdate3.Value }));

        // Card catalog
        var stubCardCatalog = new Mock<ICardCatalog>();
        stubCardCatalog.Setup(x => x.GetMlbPlayerCard(Year, CardExternalId, cToken))
            .ReturnsAsync(CardStateCurrent.Card);

        // Service
        var service = new PlayerRatingHistoryService(stubRosterUpdateFeed.Object, stubCardCatalog.Object,
            stubQuerySender.Object, mockCommandSender.Object, Mock.Of<ILogger<IPlayerRatingHistoryService>>());

        /*
         * Act
         */
        var actual = await service.SyncHistory(Year, cToken);

        /*
         * Assert
         */
        // There should be four historical ratings
        var result = actual.UpdatedPlayerCards[playerCard];
        Assert.Equal(4, result.Count);
        // The 1st historical rating chronologically is the card's initial state: CardStateInitial
        var historical1 = result[0];
        Assert.Equal(PlayerCardHistoricalRatingType.Baseline, historical1.Type);
        Assert.Equal(RosterUpdate0.Date, historical1.StartDate);
        Assert.Null(historical1.EndDate);
        Assert.Equal(CardStateInitial.OverallRating, historical1.OverallRating);
        Assert.Equal(CardStateInitial.Card.GetAttributes(), historical1.Attributes);
        // The 2nd historical rating chronologically is from RosterUpdate1: CardStateAfterRosterUpdate1
        var historical2 = result[1];
        Assert.Equal(PlayerCardHistoricalRatingType.Baseline, historical2.Type);
        Assert.Equal(RosterUpdate1.Date, historical2.StartDate);
        Assert.Null(historical2.EndDate);
        Assert.Equal(CardStateAfterRosterUpdate1.OverallRating, historical2.OverallRating);
        Assert.Equal(CardStateAfterRosterUpdate1.Card.GetAttributes(), historical2.Attributes);
        // The 3rd historical rating chronologically is from RosterUpdate2: CardStateAfterRosterUpdate2
        var historical3 = result[2];
        Assert.Equal(PlayerCardHistoricalRatingType.Baseline, historical3.Type);
        Assert.Equal(RosterUpdate2.Date, historical3.StartDate);
        Assert.Null(historical3.EndDate);
        Assert.Equal(CardStateAfterRosterUpdate2.OverallRating, historical3.OverallRating);
        Assert.Equal(CardStateAfterRosterUpdate2.Card.GetAttributes(), historical3.Attributes);
        // The 4th historical rating chronologically is from RosterUpdate3: CardStateCurrent
        var historical4 = result[3];
        Assert.Equal(PlayerCardHistoricalRatingType.Baseline, historical4.Type);
        Assert.Equal(RosterUpdate3.Date, historical4.StartDate);
        Assert.Null(historical4.EndDate);
        Assert.Equal(CardStateCurrent.OverallRating, historical4.OverallRating);
        Assert.Equal(CardStateCurrent.Card.GetAttributes(), historical4.Attributes);
        // An update command should have been sent with the new historical ratings
        mockCommandSender.Verify(x => x.Send(
            It.Is<UpdatePlayerCardCommand>(c => c.PlayerCard == playerCard && ItIs(c.HistoricalRatings!, result)),
            cToken
        ), Times.Once);
        // The result should contain the updated PlayerCard
        Assert.Contains(playerCard, actual.UpdatedPlayerCards.Keys);
    }

    [Fact]
    public async Task SyncHistory_MultipleRatingChangesWithSomeAlreadyAdded_ReturnsOnlyRatingsThatHaveNotBeenApplied()
    {
        /*
         * Arrange
         */
        var cToken = CancellationToken.None;
        // PlayerCard
        var playerCard = Faker.FakePlayerCard(externalId: CardExternalId.Value);
        // Add the card's initial state the history
        playerCard.AddHistoricalRating(RosterUpdate0.AsHistoricalRating);
        // Add RosterUpdate1's changes to the history
        playerCard.AddHistoricalRating(RosterUpdate1.AsHistoricalRating);

        // Query for PlayerCard
        var getPlayerCardQuery = new GetPlayerCardByExternalIdQuery(Year, CardExternalId);

        // Query sender
        var stubQuerySender = new Mock<IQuerySender>();
        stubQuerySender.Setup(x => x.Send(getPlayerCardQuery, cToken))
            .ReturnsAsync(playerCard);

        // Command sender
        var mockCommandSender = new Mock<ICommandSender>();

        // Roster update feed
        var stubRosterUpdateFeed = new Mock<IRosterUpdateFeed>();
        stubRosterUpdateFeed.Setup(x => x.GetNewRosterUpdates(Year, cToken))
            .ReturnsAsync(new AllRosterUpdates(new List<RosterUpdate>()
                { RosterUpdate1.Value, RosterUpdate2.Value, RosterUpdate3.Value }));

        // Card catalog
        var stubCardCatalog = new Mock<ICardCatalog>();
        stubCardCatalog.Setup(x => x.GetMlbPlayerCard(Year, CardExternalId, cToken))
            .ReturnsAsync(CardStateCurrent.Card);

        // Service
        var service = new PlayerRatingHistoryService(stubRosterUpdateFeed.Object, stubCardCatalog.Object,
            stubQuerySender.Object, mockCommandSender.Object, Mock.Of<ILogger<IPlayerRatingHistoryService>>());

        /*
         * Act
         */
        var actual = await service.SyncHistory(Year, cToken);

        /*
         * Assert
         */
        // There should be two historical ratings, RosterUpdate0 and RosterUpdate1 are already applied
        var result = actual.UpdatedPlayerCards[playerCard];
        Assert.Equal(2, result.Count);
        // The 1st historical rating chronologically is from RosterUpdate2: CardStateAfterRosterUpdate2
        var historical1 = result[0];
        Assert.Equal(PlayerCardHistoricalRatingType.Baseline, historical1.Type);
        Assert.Equal(RosterUpdate2.Date, historical1.StartDate);
        Assert.Null(historical1.EndDate);
        Assert.Equal(CardStateAfterRosterUpdate2.OverallRating, historical1.OverallRating);
        Assert.Equal(CardStateAfterRosterUpdate2.Card.GetAttributes(), historical1.Attributes);
        // The 2nd historical rating chronologically is from RosterUpdate3: CardStateCurrent
        var historical2 = result[1];
        Assert.Equal(PlayerCardHistoricalRatingType.Baseline, historical2.Type);
        Assert.Equal(RosterUpdate3.Date, historical2.StartDate);
        Assert.Null(historical2.EndDate);
        Assert.Equal(CardStateCurrent.OverallRating, historical2.OverallRating);
        Assert.Equal(CardStateCurrent.Card.GetAttributes(), historical2.Attributes);
        // An update command should have been sent with the new historical ratings
        mockCommandSender.Verify(x => x.Send(
            It.Is<UpdatePlayerCardCommand>(c => c.PlayerCard == playerCard && ItIs(c.HistoricalRatings!, result)),
            cToken
        ), Times.Once);
        // The result should contain the updated PlayerCard
        Assert.Contains(playerCard, actual.UpdatedPlayerCards.Keys);
    }

    [Fact]
    public async Task SyncHistory_AllRatingChangesAlreadyAdded_SkipsUpdate()
    {
        /*
         * Arrange
         */
        var cToken = CancellationToken.None;
        // PlayerCard
        var playerCard = Faker.FakePlayerCard(externalId: CardExternalId.Value);
        // Add the card's initial state the history
        playerCard.AddHistoricalRating(RosterUpdate0.AsHistoricalRating);

        // Query for PlayerCard
        var getPlayerCardQuery = new GetPlayerCardByExternalIdQuery(Year, CardExternalId);

        // Query sender
        var stubQuerySender = new Mock<IQuerySender>();
        stubQuerySender.Setup(x => x.Send(getPlayerCardQuery, cToken))
            .ReturnsAsync(playerCard);

        // Command sender
        var mockCommandSender = new Mock<ICommandSender>();

        // Roster update feed
        var stubRosterUpdateFeed = new Mock<IRosterUpdateFeed>();
        stubRosterUpdateFeed.Setup(x => x.GetNewRosterUpdates(Year, cToken))
            .ReturnsAsync(new AllRosterUpdates(new List<RosterUpdate>() { RosterUpdate1.Value }));

        // Card catalog
        var stubCardCatalog = new Mock<ICardCatalog>();
        stubCardCatalog.Setup(x => x.GetMlbPlayerCard(Year, CardExternalId, cToken))
            .ReturnsAsync(CardStateAfterRosterUpdate1.Card);

        // Service
        var service = new PlayerRatingHistoryService(stubRosterUpdateFeed.Object, stubCardCatalog.Object,
            stubQuerySender.Object, mockCommandSender.Object, Mock.Of<ILogger<IPlayerRatingHistoryService>>());

        /*
         * Act
         */
        var actual = await service.SyncHistory(Year, cToken);

        /*
         * Assert
         */
        // The result should not contain any updated cards
        Assert.Empty(actual.UpdatedPlayerCards);
        // No update command should have been sent
        mockCommandSender.Verify(x => x.Send(It.IsAny<UpdatePlayerCardCommand>(), cToken), Times.Never);
    }

    [Fact]
    public async Task SyncHistory_BoostedPlayerCard_SkipsUpdate()
    {
        /*
         * Arrange
         */
        var cToken = CancellationToken.None;
        // PlayerCard
        var playerCard = Faker.FakePlayerCard(externalId: CardExternalId.Value);
        // Boost the card
        var boostDate = new DateOnly(2024, 5, 1);
        playerCard.Boost(boostDate, boostDate.AddDays(2), "Hit 5 HRs", Faker.FakePlayerCardAttributes(scalar: 2));

        // Query for PlayerCard
        var getPlayerCardQuery = new GetPlayerCardByExternalIdQuery(Year, CardExternalId);

        // Query sender
        var stubQuerySender = new Mock<IQuerySender>();
        stubQuerySender.Setup(x => x.Send(getPlayerCardQuery, cToken))
            .ReturnsAsync(playerCard);

        // Command sender
        var mockCommandSender = new Mock<ICommandSender>();

        // Roster update feed
        var stubRosterUpdateFeed = new Mock<IRosterUpdateFeed>();
        stubRosterUpdateFeed.Setup(x => x.GetNewRosterUpdates(Year, cToken))
            .ReturnsAsync(new AllRosterUpdates(new List<RosterUpdate>() { RosterUpdate1.Value }));

        // Card catalog
        var mockCardCatalog = Mock.Of<ICardCatalog>();

        // Service
        var service = new PlayerRatingHistoryService(stubRosterUpdateFeed.Object, mockCardCatalog,
            stubQuerySender.Object, mockCommandSender.Object, Mock.Of<ILogger<IPlayerRatingHistoryService>>());

        /*
         * Act
         */
        var actual = await service.SyncHistory(Year, cToken);

        /*
         * Assert
         */
        // The result should not contain any updated cards
        Assert.Empty(actual.UpdatedPlayerCards);
        // No update command should have been sent
        mockCommandSender.Verify(x => x.Send(It.IsAny<UpdatePlayerCardCommand>(), cToken), Times.Never);
    }

    private static readonly SeasonYear Year = SeasonYear.Create(2024);
    private static readonly CardExternalId CardExternalId = CardExternalId.Create(Faker.FakeGuid1);

    /// <summary>
    /// The initial state of the PlayerCard, before any roster updates
    /// </summary>
    private static class CardStateInitial
    {
        public static readonly OverallRating OverallRating = Faker.FakeOverallRating(70);

        public static readonly MlbPlayerCard Card = Dtos.TestClasses.Faker.FakeMlbPlayerCard(
            Year.Value,
            CardExternalId.Value,
            rarity: OverallRating.Rarity,
            overallRating: OverallRating.Value,
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

    /// <summary>
    /// The state of the PlayerCard after <see cref="RosterUpdate1"/>
    /// </summary>
    private static class CardStateAfterRosterUpdate1
    {
        public static readonly OverallRating OverallRating = Faker.FakeOverallRating(75);

        public static readonly MlbPlayerCard Card = Dtos.TestClasses.Faker.FakeMlbPlayerCard(
            Year.Value,
            CardExternalId.Value,
            rarity: OverallRating.Rarity,
            overallRating: OverallRating.Value,
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
    }

    /// <summary>
    /// The state of the PlayerCard after <see cref="RosterUpdate2"/>
    /// </summary>
    private static class CardStateAfterRosterUpdate2
    {
        public static readonly OverallRating OverallRating = Faker.FakeOverallRating(80);

        public static readonly MlbPlayerCard Card = Dtos.TestClasses.Faker.FakeMlbPlayerCard(
            Year.Value,
            CardExternalId.Value,
            rarity: OverallRating.Rarity,
            overallRating: OverallRating.Value,
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
    }

    /// <summary>
    /// The card state after the most recent roster update, <see cref="RosterUpdate3"/>
    /// </summary>
    private static class CardStateCurrent
    {
        public static readonly OverallRating OverallRating = Faker.FakeOverallRating(99);

        public static readonly MlbPlayerCard Card = Dtos.TestClasses.Faker.FakeMlbPlayerCard(
            Year.Value,
            CardExternalId.Value,
            rarity: OverallRating.Rarity,
            overallRating: OverallRating.Value,
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
    }

    /// <summary>
    /// Not actually a roster update, but the state of everything before <see cref="RosterUpdate1"/>
    /// </summary>
    private static class RosterUpdate0
    {
        public static readonly DateOnly Date = new(2024, 1, 1);

        public static readonly PlayerCardHistoricalRating AsHistoricalRating =
            Faker.FakeBaselinePlayerCardHistoricalRating(Date, RosterUpdate1.Date,
                CardStateInitial.OverallRating.Value, CardStateInitial.Card.GetAttributes());
    }

    /// <summary>
    /// The first roster update
    /// It changes the player card from <see cref="CardStateInitial"/> to <see cref="CardStateAfterRosterUpdate1"/>
    /// </summary>
    private static class RosterUpdate1
    {
        public static readonly DateOnly Date = new(2024, 4, 7);

        public static readonly PlayerCardHistoricalRating AsHistoricalRating =
            Faker.FakeBaselinePlayerCardHistoricalRating(Date, RosterUpdate2.Date,
                CardStateAfterRosterUpdate1.OverallRating.Value, CardStateAfterRosterUpdate1.Card.GetAttributes());

        public static readonly PlayerRatingChange RatingChange = Dtos.TestClasses.Faker.FakePlayerRatingChange(
            Date,
            CardExternalId.Value,
            newOverallRating: CardStateAfterRosterUpdate1.OverallRating.Value,
            newRarity: CardStateAfterRosterUpdate1.OverallRating.Rarity,
            oldOverallRating: CardStateInitial.OverallRating.Value,
            oldRarity: CardStateInitial.OverallRating.Rarity,
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

        public static readonly RosterUpdate Value = Dtos.TestClasses.Faker.FakeRosterUpdate(ratingChanges:
            new List<PlayerRatingChange>() { RatingChange });
    }

    /// <summary>
    /// The 2nd roster update
    /// It changes the player card from <see cref="CardStateAfterRosterUpdate1"/> to <see cref="CardStateAfterRosterUpdate2"/>
    /// </summary>
    private static class RosterUpdate2
    {
        public static readonly DateOnly Date = new(2024, 4, 14);

        public static readonly PlayerCardHistoricalRating AsHistoricalRating =
            Faker.FakeBaselinePlayerCardHistoricalRating(Date, RosterUpdate3.Date,
                CardStateAfterRosterUpdate2.OverallRating.Value, CardStateAfterRosterUpdate2.Card.GetAttributes());

        public static readonly PlayerRatingChange RatingChange = Dtos.TestClasses.Faker.FakePlayerRatingChange(
            Date,
            CardExternalId.Value,
            newOverallRating: CardStateAfterRosterUpdate2.OverallRating.Value,
            newRarity: CardStateAfterRosterUpdate2.OverallRating.Rarity,
            oldOverallRating: CardStateAfterRosterUpdate1.OverallRating.Value,
            oldRarity: CardStateAfterRosterUpdate1.OverallRating.Rarity,
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

        public static readonly RosterUpdate Value = Dtos.TestClasses.Faker.FakeRosterUpdate(ratingChanges:
            new List<PlayerRatingChange>() { RatingChange });
    }

    /// <summary>
    /// The most recent (3rd) roster update
    /// It changes the player card from <see cref="CardStateAfterRosterUpdate2"/> to <see cref="CardStateCurrent"/>
    /// </summary>
    private static class RosterUpdate3
    {
        public static readonly DateOnly Date = new(2024, 4, 21);

        public static readonly PlayerCardHistoricalRating AsHistoricalRating =
            Faker.FakeBaselinePlayerCardHistoricalRating(Date, DateOnly.MaxValue, CardStateCurrent.OverallRating.Value,
                CardStateInitial.Card.GetAttributes());

        public static readonly PlayerRatingChange RatingChange = Dtos.TestClasses.Faker.FakePlayerRatingChange(
            Date,
            CardExternalId.Value,
            newOverallRating: CardStateCurrent.OverallRating.Value,
            newRarity: CardStateCurrent.OverallRating.Rarity,
            oldOverallRating: CardStateAfterRosterUpdate2.OverallRating.Value,
            oldRarity: CardStateAfterRosterUpdate2.OverallRating.Rarity,
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

        public static readonly RosterUpdate Value = Dtos.TestClasses.Faker.FakeRosterUpdate(ratingChanges:
            new List<PlayerRatingChange>() { RatingChange });
    }

    private static bool ItIs(Stack<PlayerCardHistoricalRating> stack,
        IReadOnlyList<PlayerCardHistoricalRating> comparison)
    {
        for (int i = 0; i < stack.Count; i++)
        {
            if (!stack.ElementAt(i).Equals(comparison[i]))
            {
                return false;
            }
        }

        return stack.Count == comparison.Count;
    }
}