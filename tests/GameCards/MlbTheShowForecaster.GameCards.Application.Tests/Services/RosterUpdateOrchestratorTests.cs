using com.brettnamba.MlbTheShowForecaster.Common.Application.Cqrs;
using com.brettnamba.MlbTheShowForecaster.Common.Domain.ValueObjects;
using com.brettnamba.MlbTheShowForecaster.GameCards.Application.Commands.CreatePlayerCard;
using com.brettnamba.MlbTheShowForecaster.GameCards.Application.Commands.CreatePlayerCard.Exceptions;
using com.brettnamba.MlbTheShowForecaster.GameCards.Application.Commands.UpdatePlayerCard;
using com.brettnamba.MlbTheShowForecaster.GameCards.Application.Dtos;
using com.brettnamba.MlbTheShowForecaster.GameCards.Application.Queries.GetPlayerCardByExternalId;
using com.brettnamba.MlbTheShowForecaster.GameCards.Application.Services;
using com.brettnamba.MlbTheShowForecaster.GameCards.Application.Services.Exceptions;
using com.brettnamba.MlbTheShowForecaster.GameCards.Application.Tests.TestClasses;
using com.brettnamba.MlbTheShowForecaster.GameCards.Domain.Cards.Entities;
using com.brettnamba.MlbTheShowForecaster.GameCards.Domain.Cards.ValueObjects;
using Moq;

namespace com.brettnamba.MlbTheShowForecaster.GameCards.Application.Tests.Services;

public class RosterUpdateOrchestratorTests
{
    [Fact]
    public async Task SyncRosterUpdates_RatingChangePlayerCardNotFound_ThrowsException()
    {
        /*
         * Arrange
         */
        var cToken = CancellationToken.None;
        var seasonYear = SeasonYear.Create(2024);
        // RosterUpdate1 with RatingChange1
        PlayerCard? playerCard1 = null; // RatingChange1 has no corresponding PlayerCard
        var cardExternalId1 = Faker.FakeGuid1;
        var ratingChange1 = Dtos.TestClasses.Faker.FakePlayerRatingChange(cardExternalId: cardExternalId1);
        var getPlayerCardQuery1 = new GetPlayerCardByExternalIdQuery(ratingChange1.CardExternalId);

        var rosterUpdate1 = Dtos.TestClasses.Faker.FakeRosterUpdate(new DateOnly(2024, 4, 1),
            new List<PlayerRatingChange>() { ratingChange1 });

        // RosterUpdate2 with RatingChange2
        var cardExternalId2 = Faker.FakeCardExternalId(Faker.FakeGuid2);
        var ratingChange2 = Dtos.TestClasses.Faker.FakePlayerRatingChange(cardExternalId: cardExternalId2.Value);
        var getPlayerCardQuery2 = new GetPlayerCardByExternalIdQuery(ratingChange2.CardExternalId);

        var rosterUpdate2 = Dtos.TestClasses.Faker.FakeRosterUpdate(new DateOnly(2024, 4, 2),
            new List<PlayerRatingChange>() { ratingChange2 });

        // Query sender
        var stubQuerySender = new Mock<IQuerySender>();
        stubQuerySender.Setup(x => x.Send(getPlayerCardQuery1, cToken))
            .ReturnsAsync(playerCard1);

        // Command sender
        var mockCommandSender = Mock.Of<ICommandSender>();

        // Roster update feed
        var stubRosterUpdateFeed = new Mock<IRosterUpdateFeed>();
        stubRosterUpdateFeed.Setup(x => x.GetNewRosterUpdates(seasonYear, cToken))
            .ReturnsAsync(new AllRosterUpdates(new List<RosterUpdate>() { rosterUpdate1, rosterUpdate2 }));

        // Card catalog
        var mockCardCatalog = Mock.Of<ICardCatalog>();

        // Orchestrator
        var orchestrator = new RosterUpdateOrchestrator(stubRosterUpdateFeed.Object, mockCardCatalog,
            stubQuerySender.Object, mockCommandSender);

        // Action
        var action = () => orchestrator.SyncRosterUpdates(seasonYear, cToken);

        /*
         * Act
         */
        var actual = await Record.ExceptionAsync(action);

        /*
         * Assert
         */
        // Was the aggregate exception thrown?
        Assert.NotNull(actual);
        Assert.IsType<RosterUpdateOrchestratorInterruptedException>(actual);
        var actualException = actual as RosterUpdateOrchestratorInterruptedException;
        // Was an exception thrown because PlayerCard1 could not be found?
        Assert.Single(actualException!.InnerExceptions);
        Assert.IsType<NoPlayerCardFoundForRosterUpdateException>(actualException.InnerExceptions[0]);
        // There should have been nothing done for RosterUpdate2
        stubQuerySender.Verify(x => x.Send(getPlayerCardQuery2, cToken), Times.Never);
    }

    [Fact]
    public async Task SyncRosterUpdates_PositionChangePlayerCardNotFound_ThrowsException()
    {
        /*
         * Arrange
         */
        var cToken = CancellationToken.None;
        var seasonYear = SeasonYear.Create(2024);
        // RosterUpdate1 with PositionChange1
        PlayerCard? playerCard1 = null; // RatingChange1 has no corresponding PlayerCard
        var cardExternalId1 = Faker.FakeGuid1;
        var positionChange1 = Dtos.TestClasses.Faker.FakePlayerPositionChange(cardExternalId: cardExternalId1);
        var getPlayerCardQuery1 = new GetPlayerCardByExternalIdQuery(positionChange1.CardExternalId);

        var rosterUpdate1 = Dtos.TestClasses.Faker.FakeRosterUpdate(new DateOnly(2024, 4, 1),
            positionChanges: new List<PlayerPositionChange>() { positionChange1 });

        // RosterUpdate2 with PositionChange2
        var cardExternalId2 = Faker.FakeGuid2;
        var positionChange2 = Dtos.TestClasses.Faker.FakePlayerPositionChange(cardExternalId: cardExternalId2);
        var getPlayerCardQuery2 = new GetPlayerCardByExternalIdQuery(positionChange2.CardExternalId);

        var rosterUpdate2 = Dtos.TestClasses.Faker.FakeRosterUpdate(new DateOnly(2024, 4, 2),
            positionChanges: new List<PlayerPositionChange>() { positionChange2 });

        // Query sender
        var stubQuerySender = new Mock<IQuerySender>();
        stubQuerySender.Setup(x => x.Send(getPlayerCardQuery1, cToken))
            .ReturnsAsync(playerCard1);

        // Command sender
        var mockCommandSender = Mock.Of<ICommandSender>();

        // Roster update feed
        var stubRosterUpdateFeed = new Mock<IRosterUpdateFeed>();
        stubRosterUpdateFeed.Setup(x => x.GetNewRosterUpdates(seasonYear, cToken))
            .ReturnsAsync(new AllRosterUpdates(new List<RosterUpdate>() { rosterUpdate1, rosterUpdate2 }));

        // Card catalog
        var mockCardCatalog = Mock.Of<ICardCatalog>();

        // Orchestrator
        var orchestrator = new RosterUpdateOrchestrator(stubRosterUpdateFeed.Object, mockCardCatalog,
            stubQuerySender.Object, mockCommandSender);

        // Action
        var action = () => orchestrator.SyncRosterUpdates(seasonYear, cToken);

        /*
         * Act
         */
        var actual = await Record.ExceptionAsync(action);

        /*
         * Assert
         */
        // Was the aggregate exception thrown?
        Assert.NotNull(actual);
        Assert.IsType<RosterUpdateOrchestratorInterruptedException>(actual);
        var actualException = actual as RosterUpdateOrchestratorInterruptedException;
        // Was an exception thrown because PlayerCard1 could not be found?
        Assert.Single(actualException!.InnerExceptions);
        Assert.IsType<NoPlayerCardFoundForRosterUpdateException>(actualException.InnerExceptions[0]);
        // There should have been nothing done for RosterUpdate2
        stubQuerySender.Verify(x => x.Send(getPlayerCardQuery2, cToken), Times.Never);
    }

    [Fact]
    public async Task SyncRosterUpdates_PlayerAdditionExternalCardNotFound_ThrowsException()
    {
        /*
         * Arrange
         */
        var cToken = CancellationToken.None;
        var seasonYear = SeasonYear.Create(2024);
        // RosterUpdate1 with PlayerAddition1 (will have no corresponding MlbPlayerCard)
        var cardExternalId1 = Faker.FakeGuid1;
        var playerAddition1 = Dtos.TestClasses.Faker.FakePlayerAddition(cardExternalId: cardExternalId1);

        var rosterUpdate1 = Dtos.TestClasses.Faker.FakeRosterUpdate(new DateOnly(2024, 4, 1),
            newPlayers: new List<PlayerAddition>() { playerAddition1 });

        // RosterUpdate2 with PlayerAddition2
        var cardExternalId2 = Faker.FakeGuid2;
        var playerAddition2 = Dtos.TestClasses.Faker.FakePlayerAddition(cardExternalId: cardExternalId2);

        var rosterUpdate2 = Dtos.TestClasses.Faker.FakeRosterUpdate(new DateOnly(2024, 4, 2),
            newPlayers: new List<PlayerAddition>() { playerAddition2 });

        // Query sender
        var mockQuerySender = Mock.Of<IQuerySender>();

        // Command sender
        var mockCommandSender = Mock.Of<ICommandSender>();

        // Roster update feed
        var stubRosterUpdateFeed = new Mock<IRosterUpdateFeed>();
        stubRosterUpdateFeed.Setup(x => x.GetNewRosterUpdates(seasonYear, cToken))
            .ReturnsAsync(new AllRosterUpdates(new List<RosterUpdate>() { rosterUpdate1, rosterUpdate2 }));

        // Card catalog
        var stubCardCatalog = new Mock<ICardCatalog>();
        stubCardCatalog.Setup(x => x.GetMlbPlayerCard(seasonYear, playerAddition1.CardExternalId, cToken))
            .ThrowsAsync(new MlbPlayerCardNotFoundInCatalogException("Card not found"));

        // Orchestrator
        var orchestrator = new RosterUpdateOrchestrator(stubRosterUpdateFeed.Object, stubCardCatalog.Object,
            mockQuerySender, mockCommandSender);

        // Action
        var action = () => orchestrator.SyncRosterUpdates(seasonYear, cToken);

        /*
         * Act
         */
        var actual = await Record.ExceptionAsync(action);

        /*
         * Assert
         */
        // Was the aggregate exception thrown?
        Assert.NotNull(actual);
        Assert.IsType<RosterUpdateOrchestratorInterruptedException>(actual);
        var actualException = actual as RosterUpdateOrchestratorInterruptedException;
        // Was an exception thrown because an external PlayerCard1 could not be found?
        Assert.Single(actualException!.InnerExceptions);
        Assert.IsType<NoExternalPlayerCardFoundForRosterUpdateException>(actualException.InnerExceptions[0]);
        // There should have been nothing done for RosterUpdate2
        stubCardCatalog.Verify(x => x.GetMlbPlayerCard(seasonYear, playerAddition2.CardExternalId, cToken),
            Times.Never);
    }

    [Fact]
    public async Task SyncRosterUpdates_PlayerAdditionExternalCardIdInvalidAndPlayerCardAlreadyExists_SkipsProcessing()
    {
        /*
         * Arrange
         */
        var cToken = CancellationToken.None;
        var seasonYear = SeasonYear.Create(2024);
        // RosterUpdate1 with PlayerAddition1
        var cardExternalId1 = Guid.Empty; // Invalid GUID, so it will skip processing for PlayerAddition1
        var playerAddition1 = Dtos.TestClasses.Faker.FakePlayerAddition(cardExternalId: cardExternalId1);

        var rosterUpdate1 = Dtos.TestClasses.Faker.FakeRosterUpdate(new DateOnly(2024, 4, 1),
            newPlayers: new List<PlayerAddition>() { playerAddition1 });

        // RosterUpdate2 with PlayerAddition2
        var cardExternalId2 = Faker.FakeGuid2;
        var playerAddition2 = Dtos.TestClasses.Faker.FakePlayerAddition(cardExternalId: cardExternalId2);
        var externalCard2 = Dtos.TestClasses.Faker.FakeMlbPlayerCard(cardExternalId: cardExternalId2);

        var rosterUpdate2 = Dtos.TestClasses.Faker.FakeRosterUpdate(new DateOnly(2024, 4, 2),
            newPlayers: new List<PlayerAddition>() { playerAddition2 });

        // Query sender
        var mockQuerySender = new Mock<IQuerySender>();

        // Command sender
        var stubCommandSender = new Mock<ICommandSender>();
        var createPlayerCard2Command = new CreatePlayerCardCommand(externalCard2);
        stubCommandSender.Setup(x => x.Send(createPlayerCard2Command, cToken))
            .ThrowsAsync(new PlayerCardAlreadyExistsException("PlayerCard already exists"));

        // Roster update feed
        var stubRosterUpdateFeed = new Mock<IRosterUpdateFeed>();
        stubRosterUpdateFeed.Setup(x => x.GetNewRosterUpdates(seasonYear, cToken))
            .ReturnsAsync(new AllRosterUpdates(new List<RosterUpdate>() { rosterUpdate1, rosterUpdate2 }));

        // Card catalog
        var stubCardCatalog = new Mock<ICardCatalog>();
        stubCardCatalog.Setup(x => x.GetMlbPlayerCard(seasonYear, playerAddition2.CardExternalId, cToken))
            .ReturnsAsync(externalCard2);

        // Orchestrator
        var orchestrator = new RosterUpdateOrchestrator(stubRosterUpdateFeed.Object, stubCardCatalog.Object,
            mockQuerySender.Object, stubCommandSender.Object);

        /*
         * Act
         */
        var actual = await orchestrator.SyncRosterUpdates(seasonYear, cToken);

        /*
         * Assert
         */
        // There were two roster updates, so there were two results
        Assert.NotNull(actual);
        var actualList = actual.ToList();
        Assert.Equal(2, actualList.Count);
        // RosterUpdate1 had a new player
        var rosterUpdate1Result = actualList.First(x => x.Date == rosterUpdate1.Date);
        Assert.Equal(0, rosterUpdate1Result.TotalRatingChanges);
        Assert.Equal(0, rosterUpdate1Result.TotalPositionChanges);
        Assert.Equal(1, rosterUpdate1Result.TotalNewPlayers);
        // RosterUpdate2 had a new player
        var rosterUpdate2Result = actualList.First(x => x.Date == rosterUpdate2.Date);
        Assert.Equal(0, rosterUpdate2Result.TotalRatingChanges);
        Assert.Equal(0, rosterUpdate2Result.TotalPositionChanges);
        Assert.Equal(1, rosterUpdate2Result.TotalNewPlayers);

        // No command should have been sent for PlayerAddition1 in RosterUpdate1
        var createPlayerCard1Command =
            new CreatePlayerCardCommand(Dtos.TestClasses.Faker.FakeMlbPlayerCard(cardExternalId: cardExternalId1));
        stubCommandSender.Verify(x => x.Send(createPlayerCard1Command, cToken), Times.Never);
        // ICardCatalog should not have been queried for PlayerAddition1 in RosterUpdate1, due to the invalid card external ID
        var externalId1 = CardExternalId.Create(Guid.Empty); // Empty = invalid
        stubCardCatalog.Verify(x => x.GetMlbPlayerCard(seasonYear, externalId1, cToken), Times.Never);
        // ICardCatalog should have been queried for PlayerAddition2 in RosterUpdate2
        stubCardCatalog.Verify(x => x.GetMlbPlayerCard(seasonYear, playerAddition2.CardExternalId, cToken), Times.Once);
        // A CreatePlayerCommand should have been sent for PlayerAddition2, but it threw an exception since it already exists
        stubCommandSender.Verify(x => x.Send(new CreatePlayerCardCommand(externalCard2), cToken), Times.Once);
    }

    [Fact]
    public async Task SyncRosterUpdates_MultipleUpdates_AllUpdatesApplied()
    {
        /*
         * Arrange
         */
        var cToken = CancellationToken.None;
        var seasonYear = SeasonYear.Create(2024);

        // RosterUpdate1 - PlayerCard1 is already up-to-date, so no command will be sent
        var rosterUpdate1Date = new DateOnly(2024, 4, 1);
        var cardExternalId1 = Faker.FakeGuid1;
        var playerCard1 = Faker.FakePlayerCard(cardExternalId: cardExternalId1);
        var playerCard1Rating =
            Faker.FakeBaselinePlayerCardHistoricalRating(rosterUpdate1Date, new DateOnly(2024, 4, 2));
        playerCard1.AddHistoricalRating(playerCard1Rating);
        var ratingChange1 =
            Dtos.TestClasses.Faker.FakePlayerRatingChange(cardExternalId: cardExternalId1, date: rosterUpdate1Date);

        var rosterUpdate1 = Dtos.TestClasses.Faker.FakeRosterUpdate(date: rosterUpdate1Date,
            ratingChanges: new List<PlayerRatingChange>() { ratingChange1 });

        // RosterUpdate2 - PlayerCard2 has both a rating change and a position change
        var rosterUpdate2Date = new DateOnly(2024, 4, 2);
        var cardExternalId2 = Faker.FakeGuid2;
        var playerCard2 = Faker.FakePlayerCard(cardExternalId: cardExternalId2);
        var ratingChange2 =
            Dtos.TestClasses.Faker.FakePlayerRatingChange(cardExternalId: cardExternalId2, date: rosterUpdate2Date);
        var positionChange2 = Dtos.TestClasses.Faker.FakePlayerPositionChange(cardExternalId: cardExternalId2);
        var externalCard2 = Dtos.TestClasses.Faker.FakeMlbPlayerCard(cardExternalId: cardExternalId2);

        // RosterUpdate2 - PlayerCard3 only has a position change
        var cardExternalId3 = Faker.FakeGuid3;
        var playerCard3 = Faker.FakePlayerCard(cardExternalId: cardExternalId3);
        var positionChange3 = Dtos.TestClasses.Faker.FakePlayerPositionChange(cardExternalId: cardExternalId3);

        var rosterUpdate2 = Dtos.TestClasses.Faker.FakeRosterUpdate(date: rosterUpdate2Date,
            ratingChanges: new List<PlayerRatingChange>() { ratingChange2 },
            positionChanges: new List<PlayerPositionChange>() { positionChange2, positionChange3 });

        // RosterUpdate3 - PlayerCard4 is a new addition to the domain
        var rosterUpdate3Date = new DateOnly(2024, 4, 3);
        var cardExternalId4 = Faker.FakeGuid4;
        var playerAddition4 = Dtos.TestClasses.Faker.FakePlayerAddition(cardExternalId4);
        var externalCard4 = Dtos.TestClasses.Faker.FakeMlbPlayerCard(cardExternalId: cardExternalId4);

        var rosterUpdate3 = Dtos.TestClasses.Faker.FakeRosterUpdate(date: rosterUpdate3Date,
            newPlayers: new List<PlayerAddition>() { playerAddition4 });

        // Query sender
        var stubQuerySender = new Mock<IQuerySender>();
        stubQuerySender.Setup(x => x.Send(new GetPlayerCardByExternalIdQuery(ratingChange1.CardExternalId), cToken))
            .ReturnsAsync(playerCard1);
        stubQuerySender.Setup(x => x.Send(new GetPlayerCardByExternalIdQuery(ratingChange2.CardExternalId), cToken))
            .ReturnsAsync(playerCard2);
        stubQuerySender.Setup(x => x.Send(new GetPlayerCardByExternalIdQuery(positionChange3.CardExternalId), cToken))
            .ReturnsAsync(playerCard3);

        // Command sender
        var mockCommandSender = Mock.Of<ICommandSender>();

        // Roster update feed
        var stubRosterUpdateFeed = new Mock<IRosterUpdateFeed>();
        stubRosterUpdateFeed.Setup(x => x.GetNewRosterUpdates(seasonYear, cToken))
            .ReturnsAsync(
                new AllRosterUpdates(new List<RosterUpdate>() { rosterUpdate1, rosterUpdate2, rosterUpdate3 }));
        var capturedCompleteRosterUpdateInvocations = new List<RosterUpdate>();
        stubRosterUpdateFeed.Setup(x =>
            x.CompleteRosterUpdate(Capture.In(capturedCompleteRosterUpdateInvocations), cToken));

        // Card catalog
        var stubCardCatalog = new Mock<ICardCatalog>();
        stubCardCatalog.Setup(x => x.GetMlbPlayerCard(seasonYear, externalCard2.ExternalUuid, cToken))
            .ReturnsAsync(externalCard2);
        stubCardCatalog.Setup(x => x.GetMlbPlayerCard(seasonYear, playerAddition4.CardExternalId, cToken))
            .ReturnsAsync(externalCard4);

        // Orchestrator
        var orchestrator = new RosterUpdateOrchestrator(stubRosterUpdateFeed.Object, stubCardCatalog.Object,
            stubQuerySender.Object, mockCommandSender);

        /*
         * Act
         */
        var actual = await orchestrator.SyncRosterUpdates(seasonYear, cToken);

        /*
         * Assert
         */
        // There were three roster updates, so there were three results
        Assert.NotNull(actual);
        var actualList = actual.ToList();
        Assert.Equal(3, actualList.Count);
        // RosterUpdate1 had a rating change
        var rosterUpdate1Result = actualList.First(x => x.Date == rosterUpdate1.Date);
        Assert.Equal(1, rosterUpdate1Result.TotalRatingChanges);
        Assert.Equal(0, rosterUpdate1Result.TotalPositionChanges);
        Assert.Equal(0, rosterUpdate1Result.TotalNewPlayers);
        // RosterUpdate2 had both a rating change and two position changes
        var rosterUpdate2Result = actualList.First(x => x.Date == rosterUpdate2.Date);
        Assert.Equal(1, rosterUpdate2Result.TotalRatingChanges);
        Assert.Equal(2, rosterUpdate2Result.TotalPositionChanges);
        Assert.Equal(0, rosterUpdate2Result.TotalNewPlayers);
        // RosterUpdate3 had a new player
        var rosterUpdate3Result = actualList.First(x => x.Date == rosterUpdate3.Date);
        Assert.Equal(0, rosterUpdate3Result.TotalRatingChanges);
        Assert.Equal(0, rosterUpdate3Result.TotalPositionChanges);
        Assert.Equal(1, rosterUpdate3Result.TotalNewPlayers);

        // PlayerCard1 - No command should have been sent
        Mock.Get(mockCommandSender)
            .Verify(x => x.Send(It.Is<UpdatePlayerCardCommand>(c => c.PlayerCard == playerCard1), cToken), Times.Never);
        // PlayerCard2 - A rating change and position change command should have been sent for PlayerCard2
        var playerCard2Command =
            new UpdatePlayerCardCommand(playerCard2, externalCard2, ratingChange2, positionChange2);
        Mock.Get(mockCommandSender).Verify(x => x.Send(playerCard2Command, cToken), Times.Once);
        // PlayerCard2 - The position change was updated with the rating change, so there should be no command with just the position change
        var playerCard2CommandNotSent = new UpdatePlayerCardCommand(playerCard2, null, null, positionChange2);
        Mock.Get(mockCommandSender).Verify(x => x.Send(playerCard2CommandNotSent, cToken), Times.Never);
        // PlayerCard3 - A position change command should have been sent
        var playerCard3Command = new UpdatePlayerCardCommand(playerCard3, null, null, positionChange3);
        Mock.Get(mockCommandSender).Verify(x => x.Send(playerCard3Command, cToken), Times.Once);
        // PlayerCard4 - A PlayerCard creation command should have been sent
        var playerCard4Command = new CreatePlayerCardCommand(externalCard4);
        Mock.Get(mockCommandSender).Verify(x => x.Send(playerCard4Command, cToken), Times.Once);
        // Was each roster update completed?
        stubRosterUpdateFeed.Verify(x => x.CompleteRosterUpdate(rosterUpdate1, cToken), Times.Once);
        stubRosterUpdateFeed.Verify(x => x.CompleteRosterUpdate(rosterUpdate2, cToken), Times.Once);
        stubRosterUpdateFeed.Verify(x => x.CompleteRosterUpdate(rosterUpdate3, cToken), Times.Once);
        // Was the order of the roster updates correct?
        Assert.Equal(new List<RosterUpdate>() { rosterUpdate1, rosterUpdate2, rosterUpdate3 },
            capturedCompleteRosterUpdateInvocations);
    }

    [Fact]
    public void Dispose_NoParams_DisposesOfDependencies()
    {
        // Arrange
        var mockRosterUpdateFeed = Mock.Of<IRosterUpdateFeed>();
        var mockCardCatalog = Mock.Of<ICardCatalog>();
        var mockQuerySender = Mock.Of<IQuerySender>();
        var mockCommandSender = Mock.Of<ICommandSender>();

        var orchestrator =
            new RosterUpdateOrchestrator(mockRosterUpdateFeed, mockCardCatalog, mockQuerySender, mockCommandSender);

        // Act
        orchestrator.Dispose();

        // Assert
        Mock.Get(mockRosterUpdateFeed).Verify(x => x.Dispose(), Times.Once);
        Mock.Get(mockCardCatalog).Verify(x => x.Dispose(), Times.Once);
    }
}