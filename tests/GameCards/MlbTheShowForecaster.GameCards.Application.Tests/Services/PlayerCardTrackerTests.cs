using com.brettnamba.MlbTheShowForecaster.Common.Application.Cqrs;
using com.brettnamba.MlbTheShowForecaster.Common.Domain.ValueObjects;
using com.brettnamba.MlbTheShowForecaster.GameCards.Application.Commands.CreatePlayerCard;
using com.brettnamba.MlbTheShowForecaster.GameCards.Application.Dtos;
using com.brettnamba.MlbTheShowForecaster.GameCards.Application.Queries.GetPlayerCardByExternalId;
using com.brettnamba.MlbTheShowForecaster.GameCards.Application.Services;
using com.brettnamba.MlbTheShowForecaster.GameCards.Application.Tests.Dtos.TestClasses;
using com.brettnamba.MlbTheShowForecaster.GameCards.Domain.Cards.Entities;
using com.brettnamba.MlbTheShowForecaster.GameCards.Domain.Cards.ValueObjects;
using Moq;

namespace com.brettnamba.MlbTheShowForecaster.GameCards.Application.Tests.Services;

public class PlayerCardTrackerTests
{
    [Fact]
    public async Task TrackPlayerCards_SeasonYear_CreatesNewPlayerCards()
    {
        /*
         * Act
         */
        var cToken = CancellationToken.None;
        var seasonYear = SeasonYear.Create(2024);
        // Player card #1 already exists in the domain, so no action will take place
        var playerExternalId1 = CardExternalId.Create("id1");
        var externalCard1 = Faker.FakeMlbPlayerCard(externalId: playerExternalId1.Value); // Card from MLB The Show
        var domainPlayerCard1 = TestClasses.Faker.FakePlayerCard(externalId: playerExternalId1); // Card in this domain
        var query1 = new GetPlayerCardByExternalIdQuery(playerExternalId1);

        // Player card #2 does not exist in the domain, so it will be created
        var playerExternalId2 = CardExternalId.Create("id2");
        var externalCard2 = Faker.FakeMlbPlayerCard(externalId: playerExternalId2.Value); // Card from MLB The Show
        PlayerCard? domainPlayerCard2 = null; // Card does not exist in the domain
        var query2 = new GetPlayerCardByExternalIdQuery(playerExternalId2);

        // The card marketplace should return all external cards in MLB The Show
        var allExternalPlayerCards = new List<MlbPlayerCard>() { externalCard1, externalCard2 };
        var stubCardCatalog = new Mock<ICardCatalog>();
        stubCardCatalog.Setup(x => x.GetAllMlbPlayerCards(seasonYear, cToken))
            .ReturnsAsync(allExternalPlayerCards);

        // Stub the behavior of the query handler
        var stubQuerySender = new Mock<IQuerySender>();
        stubQuerySender.Setup(x => x.Send(query1, cToken)).ReturnsAsync(domainPlayerCard1);
        stubQuerySender.Setup(x => x.Send(query2, cToken)).ReturnsAsync(domainPlayerCard2);

        // The command sender should expect to send a create command for player card #2
        var mockCommandSender = Mock.Of<ICommandSender>();
        var expectedPlayerCard2Command = new CreatePlayerCardCommand(externalCard2);
        // The command sender should not expect any commands for player card #1
        var notExpectedPlayerCard1Command = new CreatePlayerCardCommand(externalCard1);

        // Tracker
        var tracker = new PlayerCardTracker(stubCardCatalog.Object, stubQuerySender.Object, mockCommandSender);

        /*
         * Act
         */
        await tracker.TrackPlayerCards(seasonYear, cToken);

        /*
         * Assert
         */
        // Were all the player cards in MLB The Show retrieved?
        stubCardCatalog.Verify(x => x.GetAllMlbPlayerCards(seasonYear, cToken), Times.Once);

        // Was each card from MLB The Show checked to see if it existed in this domain?
        stubQuerySender.Verify(x => x.Send(query1, cToken), Times.Once);
        stubQuerySender.Verify(x => x.Send(query2, cToken), Times.Once);

        // Was a command sent to create a card for player card 2 in this domain?
        Mock.Get(mockCommandSender).Verify(x => x.Send(expectedPlayerCard2Command, cToken), Times.Once);
        // A command should not have been sent for player card 1
        Mock.Get(mockCommandSender).Verify(x => x.Send(notExpectedPlayerCard1Command, cToken), Times.Never);
    }
}