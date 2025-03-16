using System.Collections.Immutable;
using com.brettnamba.MlbTheShowForecaster.Common.Application.Cqrs;
using com.brettnamba.MlbTheShowForecaster.Common.Domain.ValueObjects;
using com.brettnamba.MlbTheShowForecaster.GameCards.Application.Commands.CreatePlayerCard;
using com.brettnamba.MlbTheShowForecaster.GameCards.Application.Commands.UpdatePlayerCard;
using com.brettnamba.MlbTheShowForecaster.GameCards.Application.Commands.UpdatePlayerCardForecastMlbId;
using com.brettnamba.MlbTheShowForecaster.GameCards.Application.Dtos;
using com.brettnamba.MlbTheShowForecaster.GameCards.Application.Queries.GetPlayerCardByExternalId;
using com.brettnamba.MlbTheShowForecaster.GameCards.Application.Services;
using com.brettnamba.MlbTheShowForecaster.GameCards.Application.Services.Exceptions;
using com.brettnamba.MlbTheShowForecaster.GameCards.Domain.Cards.Entities;
using com.brettnamba.MlbTheShowForecaster.GameCards.Domain.Tests.Cards.TestClasses;
using Moq;

namespace com.brettnamba.MlbTheShowForecaster.GameCards.Application.Tests.Services;

public class PlayerCardTrackerTests
{
    [Fact]
    public async Task TrackPlayerCards_NoCards_ThrowsException()
    {
        // Arrange
        var cToken = CancellationToken.None;
        var seasonYear = SeasonYear.Create(2024);

        var stubCardCatalog = new Mock<ICardCatalog>();
        stubCardCatalog.Setup(x => x.GetActiveRosterMlbPlayerCards(seasonYear, cToken))
            .ReturnsAsync(ImmutableList<MlbPlayerCard>.Empty); // No player cards returned

        var mockQuerySender = Mock.Of<IQuerySender>();
        var mockCommandSender = Mock.Of<ICommandSender>();

        var tracker = new PlayerCardTracker(stubCardCatalog.Object, mockQuerySender, mockCommandSender);
        var action = () => tracker.TrackPlayerCards(seasonYear, cToken);

        // Act
        var actual = await Record.ExceptionAsync(action);

        // Assert
        Assert.NotNull(actual);
        Assert.IsType<PlayerCardTrackerFoundNoCardsException>(actual);
        Mock.Get(mockQuerySender).Verify(x => x.Send(It.IsAny<GetPlayerCardByExternalIdQuery>(), cToken), Times.Never);
        Mock.Get(mockCommandSender).Verify(x => x.Send(It.IsAny<CreatePlayerCardCommand>(), cToken), Times.Never);
    }

    [Fact]
    public async Task TrackPlayerCards_SeasonYear_CreatesNewPlayerCards()
    {
        /*
         * Arrange
         */
        var cToken = CancellationToken.None;
        var seasonYear = SeasonYear.Create(2024);
        // PlayerCard1 already exists in the domain, so no action will take place
        var cardExternalId1 = Faker.FakeCardExternalId(Faker.FakeGuid1);
        var domainPlayerCard1 = Faker.FakePlayerCard(externalId: cardExternalId1.Value);
        // ExternalCard1 from MLB The Show
        var externalCard1 = Dtos.TestClasses.Faker.FakeMlbPlayerCard(cardExternalId: cardExternalId1.Value);
        // Query to get PlayerCard1
        var query1 = new GetPlayerCardByExternalIdQuery(seasonYear, cardExternalId1);

        // PlayerCard2 does not exist in the domain, so it will be created
        var cardExternalId2 = Faker.FakeCardExternalId(Faker.FakeGuid2);
        PlayerCard? domainPlayerCard2 = null; // Card does not exist in the domain
        // ExternalCard2 from MLB The Show
        var externalCard2 = Dtos.TestClasses.Faker.FakeMlbPlayerCard(cardExternalId: cardExternalId2.Value);
        // Query to get PlayerCard2
        var query2 = new GetPlayerCardByExternalIdQuery(seasonYear, cardExternalId2);

        // PlayerCard3 already exists in the domain, but it has a different boost status
        var cardExternalId3 = Faker.FakeCardExternalId(Faker.FakeGuid3);
        var domainPlayerCard3 = Faker.FakePlayerCard(externalId: cardExternalId3.Value, isBoosted: false);
        // ExternalCard3 from MLB The Show
        var externalCard3 =
            Dtos.TestClasses.Faker.FakeMlbPlayerCard(cardExternalId: cardExternalId3.Value, boostReason: "Hit 5 HRs");
        // Query to get PlayerCard3
        var query3 = new GetPlayerCardByExternalIdQuery(seasonYear, cardExternalId3);

        // The card marketplace should return all external cards in MLB The Show
        var allExternalPlayerCards = new List<MlbPlayerCard>() { externalCard1, externalCard2, externalCard3 };
        var stubCardCatalog = new Mock<ICardCatalog>();
        stubCardCatalog.Setup(x => x.GetActiveRosterMlbPlayerCards(seasonYear, cToken))
            .ReturnsAsync(allExternalPlayerCards);

        // Stub the behavior of the query handler
        var stubQuerySender = new Mock<IQuerySender>();
        stubQuerySender.Setup(x => x.Send(query1, cToken)).ReturnsAsync(domainPlayerCard1);
        stubQuerySender.Setup(x => x.Send(query2, cToken)).ReturnsAsync(domainPlayerCard2);
        stubQuerySender.Setup(x => x.Send(query3, cToken)).ReturnsAsync(domainPlayerCard3);

        // No commands should be expected for PlayerCard1
        var mockCommandSender = Mock.Of<ICommandSender>();
        var notExpectedPlayerCard1Command = new CreatePlayerCardCommand(externalCard1);
        // The command sender should expect to send a create command for PlayerCard2
        var expectedPlayerCard2Command = new CreatePlayerCardCommand(externalCard2);
        // The command sender should expect an update command for PlayerCard3
        var expectedPlayerCard3Command = new UpdatePlayerCardCommand(domainPlayerCard3, externalCard3);
        // Expected MLB ID Forecast update commands
        var expectedForecast1Command = new UpdatePlayerCardForecastMlbIdCommand(domainPlayerCard1);
        var expectedForecast3Command = new UpdatePlayerCardForecastMlbIdCommand(domainPlayerCard3);

        // Tracker
        var tracker = new PlayerCardTracker(stubCardCatalog.Object, stubQuerySender.Object, mockCommandSender);

        /*
         * Act
         */
        var actual = await tracker.TrackPlayerCards(seasonYear, cToken);

        /*
         * Assert
         */
        // There were 3 player cards in the external card catalog
        Assert.Equal(3, actual.TotalCatalogCards);
        // PlayerCard2 was not in the domain yet, so it was created
        Assert.Equal(1, actual.TotalNewCatalogCards);
        // PlayerCard3 was already in the domain, but it needed updating
        Assert.Equal(1, actual.TotalUpdatedPlayerCards);
        // PlayerCard1 already existed in the domain, so no action was taken
        Assert.Equal(1, actual.TotalUnchangedPlayerCards);

        // Were all the player cards in MLB The Show retrieved?
        stubCardCatalog.Verify(x => x.GetActiveRosterMlbPlayerCards(seasonYear, cToken), Times.Once);

        // Was each card from MLB The Show checked to see if it existed in this domain?
        stubQuerySender.Verify(x => x.Send(query1, cToken), Times.Once);
        stubQuerySender.Verify(x => x.Send(query2, cToken), Times.Once);

        // A command should not have been sent for PlayerCard1
        Mock.Get(mockCommandSender).Verify(x => x.Send(notExpectedPlayerCard1Command, cToken), Times.Never);
        // Was a command sent to create a card for PlayerCard2 in this domain?
        Mock.Get(mockCommandSender).Verify(x => x.Send(expectedPlayerCard2Command, cToken), Times.Once);
        // Was a command sent to update PlayerCard3 in this domain?
        Mock.Get(mockCommandSender).Verify(x => x.Send(expectedPlayerCard3Command, cToken), Times.Once);
        // A command should have been sent to update the MLB ID for the forecasts of already existing player cards
        Mock.Get(mockCommandSender).Verify(x => x.Send(expectedForecast1Command, cToken), Times.Once);
        Mock.Get(mockCommandSender).Verify(x => x.Send(expectedForecast3Command, cToken), Times.Once);
    }
}