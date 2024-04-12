using com.brettnamba.MlbTheShowForecaster.Common.Application.Cqrs;
using com.brettnamba.MlbTheShowForecaster.Common.Domain.ValueObjects;
using com.brettnamba.MlbTheShowForecaster.GameCards.Application.Commands.CreatePlayerCard;
using com.brettnamba.MlbTheShowForecaster.GameCards.Application.Commands.UpdatePlayerCard;
using com.brettnamba.MlbTheShowForecaster.GameCards.Application.Dtos;
using com.brettnamba.MlbTheShowForecaster.GameCards.Application.Queries.GetPlayerCardByExternalId;
using com.brettnamba.MlbTheShowForecaster.GameCards.Application.Services;
using com.brettnamba.MlbTheShowForecaster.GameCards.Application.Services.Exceptions;
using com.brettnamba.MlbTheShowForecaster.GameCards.Application.Tests.Dtos.TestClasses;
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
        // RosterUpdate 1 with RatingChange1
        PlayerCard? playerCard1 = null; // RatingChange 1 has no corresponding PlayerCard
        var cardExternalId1 = TestClasses.Faker.FakeCardExternalId(TestClasses.Faker.FakeGuid1);
        var ratingChange1 = Faker.FakePlayerRatingChange(cardExternalId: cardExternalId1.Value);
        var getPlayerCardQuery1 = new GetPlayerCardByExternalIdQuery(ratingChange1.CardExternalId);

        var rosterUpdate1 = Faker.FakeRosterUpdate(new DateOnly(2024, 4, 1),
            new List<PlayerRatingChange>() { ratingChange1 });

        // RosterUpdate 2 with RatingChange2
        var cardExternalId2 = TestClasses.Faker.FakeCardExternalId(TestClasses.Faker.FakeGuid2);
        var ratingChange2 = Faker.FakePlayerRatingChange(cardExternalId: cardExternalId2.Value);
        var getPlayerCardQuery2 = new GetPlayerCardByExternalIdQuery(ratingChange2.CardExternalId);

        var rosterUpdate2 = Faker.FakeRosterUpdate(new DateOnly(2024, 4, 2),
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
            .ReturnsAsync(new List<RosterUpdate>() { rosterUpdate1, rosterUpdate2 });

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
        // Was an exception thrown because PlayerCard 1 could not be found?
        Assert.Single(actualException!.InnerExceptions);
        Assert.IsType<NoPlayerCardFoundForRosterUpdateException>(actualException.InnerExceptions[0]);
        // There should have been nothing done for RosterUpdate 2
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
        // RosterUpdate 1 with PositionChange1
        PlayerCard? playerCard1 = null; // RatingChange 1 has no corresponding PlayerCard
        var cardExternalId1 = TestClasses.Faker.FakeCardExternalId(TestClasses.Faker.FakeGuid1);
        var positionChange1 = Faker.FakePlayerPositionChange(cardExternalId: cardExternalId1.Value);
        var getPlayerCardQuery1 = new GetPlayerCardByExternalIdQuery(positionChange1.CardExternalId);

        var rosterUpdate1 = Faker.FakeRosterUpdate(new DateOnly(2024, 4, 1),
            positionChanges: new List<PlayerPositionChange>() { positionChange1 });

        // RosterUpdate 2 with PositionChange2
        var cardExternalId2 = TestClasses.Faker.FakeCardExternalId(TestClasses.Faker.FakeGuid2);
        var positionChange2 = Faker.FakePlayerPositionChange(cardExternalId: cardExternalId2.Value);
        var getPlayerCardQuery2 = new GetPlayerCardByExternalIdQuery(positionChange2.CardExternalId);

        var rosterUpdate2 = Faker.FakeRosterUpdate(new DateOnly(2024, 4, 2),
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
            .ReturnsAsync(new List<RosterUpdate>() { rosterUpdate1, rosterUpdate2 });

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
        // Was an exception thrown because PlayerCard 1 could not be found?
        Assert.Single(actualException!.InnerExceptions);
        Assert.IsType<NoPlayerCardFoundForRosterUpdateException>(actualException.InnerExceptions[0]);
        // There should have been nothing done for RosterUpdate 2
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
        // RosterUpdate 1 with PlayerAddition1
        PlayerCard? playerCard1 = null; // RatingChange 1 has no corresponding PlayerCard
        var cardExternalId1 = TestClasses.Faker.FakeCardExternalId(TestClasses.Faker.FakeGuid1);
        var playerAddition1 = Faker.FakePlayerAddition(cardExternalId: cardExternalId1.Value);
        var getPlayerCardQuery1 = new GetPlayerCardByExternalIdQuery(playerAddition1.CardExternalId);

        var rosterUpdate1 = Faker.FakeRosterUpdate(new DateOnly(2024, 4, 1),
            newPlayers: new List<PlayerAddition>() { playerAddition1 });

        // RosterUpdate 2 with PlayerAddition2
        var cardExternalId2 = TestClasses.Faker.FakeCardExternalId(TestClasses.Faker.FakeGuid2);
        var playerAddition2 = Faker.FakePlayerAddition(cardExternalId: cardExternalId2.Value);
        var getPlayerCardQuery2 = new GetPlayerCardByExternalIdQuery(playerAddition2.CardExternalId);

        var rosterUpdate2 = Faker.FakeRosterUpdate(new DateOnly(2024, 4, 2),
            newPlayers: new List<PlayerAddition>() { playerAddition2 });

        // Query sender
        var stubQuerySender = new Mock<IQuerySender>();
        stubQuerySender.Setup(x => x.Send(getPlayerCardQuery1, cToken))
            .ReturnsAsync(playerCard1);

        // Command sender
        var mockCommandSender = Mock.Of<ICommandSender>();

        // Roster update feed
        var stubRosterUpdateFeed = new Mock<IRosterUpdateFeed>();
        stubRosterUpdateFeed.Setup(x => x.GetNewRosterUpdates(seasonYear, cToken))
            .ReturnsAsync(new List<RosterUpdate>() { rosterUpdate1, rosterUpdate2 });

        // Card catalog
        var stubCardCatalog = new Mock<ICardCatalog>();
        stubCardCatalog.Setup(x => x.GetMlbPlayerCard(seasonYear, cardExternalId1, cToken))
            .ThrowsAsync(new MlbPlayerCardNotFoundInCatalogException("Card not found"));

        // Orchestrator
        var orchestrator = new RosterUpdateOrchestrator(stubRosterUpdateFeed.Object, stubCardCatalog.Object,
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
        // Was an exception thrown because an external PlayerCard 1 could not be found?
        Assert.Single(actualException!.InnerExceptions);
        Assert.IsType<NoExternalPlayerCardFoundForRosterUpdateException>(actualException.InnerExceptions[0]);
        // There should have been nothing done for RosterUpdate 2
        stubQuerySender.Verify(x => x.Send(getPlayerCardQuery2, cToken), Times.Never);
    }

    [Fact]
    public async Task SyncRosterUpdates_MultipleUpdates_AllUpdatesApplied()
    {
        /*
         * Arrange
         */
        var cToken = CancellationToken.None;
        var seasonYear = SeasonYear.Create(2024);

        // RosterUpdate 1 - PlayerCard 1 is already up-to-date, so no command will be sent
        var rosterUpdate1Date = new DateOnly(2024, 4, 1);
        var cardExternalId1 = TestClasses.Faker.FakeCardExternalId(TestClasses.Faker.FakeGuid1);
        var playerCard1 = TestClasses.Faker.FakePlayerCard(cardExternalId: cardExternalId1.Value);
        playerCard1.ChangePlayerRating(rosterUpdate1Date, TestClasses.Faker.FakeOverallRating(),
            TestClasses.Faker.FakePlayerCardAttributes());
        var ratingChange1 =
            Faker.FakePlayerRatingChange(cardExternalId: cardExternalId1.Value, date: rosterUpdate1Date);

        var rosterUpdate1 = Faker.FakeRosterUpdate(date: rosterUpdate1Date,
            ratingChanges: new List<PlayerRatingChange>() { ratingChange1 });

        // RosterUpdate 2 - PlayerCard 2 has both a rating change and a position change
        var rosterUpdate2Date = new DateOnly(2024, 4, 2);
        var cardExternalId2 = TestClasses.Faker.FakeCardExternalId(TestClasses.Faker.FakeGuid2);
        var playerCard2 = TestClasses.Faker.FakePlayerCard(cardExternalId: cardExternalId2.Value);
        var ratingChange2 =
            Faker.FakePlayerRatingChange(cardExternalId: cardExternalId2.Value, date: rosterUpdate2Date);
        var positionChange2 = Faker.FakePlayerPositionChange(cardExternalId: cardExternalId2.Value);

        // RosterUpdate 2 - PlayerCard 3 only has a position change
        var cardExternalId3 = TestClasses.Faker.FakeCardExternalId(TestClasses.Faker.FakeGuid3);
        var playerCard3 = TestClasses.Faker.FakePlayerCard(cardExternalId: cardExternalId3.Value);
        var positionChange3 = Faker.FakePlayerPositionChange(cardExternalId: cardExternalId3.Value);

        var rosterUpdate2 = Faker.FakeRosterUpdate(date: rosterUpdate2Date,
            ratingChanges: new List<PlayerRatingChange>() { ratingChange2 },
            positionChanges: new List<PlayerPositionChange>() { positionChange2, positionChange3 });

        // RosterUpdate 3 - PlayerCard 4 is a new addition to the domain
        var rosterUpdate3Date = new DateOnly(2024, 4, 3);
        var cardExternalId4 = TestClasses.Faker.FakeCardExternalId(TestClasses.Faker.FakeGuid4);
        var playerAddition4 = Faker.FakePlayerAddition(cardExternalId4.Value);
        var externalCard4 = Faker.FakeMlbPlayerCard(cardExternalId: cardExternalId4.Value);

        var rosterUpdate3 = Faker.FakeRosterUpdate(date: rosterUpdate3Date,
            newPlayers: new List<PlayerAddition>() { playerAddition4 });

        // Query sender
        var stubQuerySender = new Mock<IQuerySender>();
        stubQuerySender.Setup(x => x.Send(new GetPlayerCardByExternalIdQuery(cardExternalId1), cToken))
            .ReturnsAsync(playerCard1);
        stubQuerySender.Setup(x => x.Send(new GetPlayerCardByExternalIdQuery(cardExternalId2), cToken))
            .ReturnsAsync(playerCard2);
        stubQuerySender.Setup(x => x.Send(new GetPlayerCardByExternalIdQuery(cardExternalId3), cToken))
            .ReturnsAsync(playerCard3);

        // Command sender
        var mockCommandSender = Mock.Of<ICommandSender>();

        // Roster update feed
        var stubRosterUpdateFeed = new Mock<IRosterUpdateFeed>();
        stubRosterUpdateFeed.Setup(x => x.GetNewRosterUpdates(seasonYear, cToken))
            .ReturnsAsync(new List<RosterUpdate>() { rosterUpdate1, rosterUpdate2, rosterUpdate3 });
        var capturedCompleteRosterUpdateInvocations = new List<RosterUpdate>();
        stubRosterUpdateFeed.Setup(x =>
            x.CompleteRosterUpdate(Capture.In(capturedCompleteRosterUpdateInvocations), cToken));

        // Card catalog
        var stubCardCatalog = new Mock<ICardCatalog>();
        stubCardCatalog.Setup(x => x.GetMlbPlayerCard(seasonYear, cardExternalId4, cToken))
            .ReturnsAsync(externalCard4);

        // Orchestrator
        var orchestrator = new RosterUpdateOrchestrator(stubRosterUpdateFeed.Object, stubCardCatalog.Object,
            stubQuerySender.Object, mockCommandSender);

        /*
         * Act
         */
        await orchestrator.SyncRosterUpdates(seasonYear, cToken);

        /*
         * Assert
         */
        // PlayerCard 1 - No command should have been sent
        Mock.Get(mockCommandSender)
            .Verify(x => x.Send(It.Is<UpdatePlayerCardCommand>(c => c.PlayerCard == playerCard1), cToken), Times.Never);
        // PlayerCard 2 - A rating change and position change command should have been sent for PlayerCard 2
        var playerCard2Command = new UpdatePlayerCardCommand(playerCard2, ratingChange2, positionChange2);
        Mock.Get(mockCommandSender).Verify(x => x.Send(playerCard2Command, cToken), Times.Once);
        // PlayerCard 2 - The position change was updated with the rating change, so there should be no command with just the position change
        var playerCard2CommandNotSent = new UpdatePlayerCardCommand(playerCard2, null, positionChange2);
        Mock.Get(mockCommandSender).Verify(x => x.Send(playerCard2CommandNotSent, cToken), Times.Never);
        // PlayerCard 3 - A position change command should have been sent
        var playerCard3Command = new UpdatePlayerCardCommand(playerCard3, null, positionChange3);
        Mock.Get(mockCommandSender).Verify(x => x.Send(playerCard3Command, cToken), Times.Once);
        // PlayerCard 4 - A PlayerCard creation command should have been sent
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
}