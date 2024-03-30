﻿using com.brettnamba.MlbTheShowForecaster.Common.Application.Cqrs;
using com.brettnamba.MlbTheShowForecaster.Common.Domain.ValueObjects;
using com.brettnamba.MlbTheShowForecaster.GameCards.Application.Commands.CreateListing;
using com.brettnamba.MlbTheShowForecaster.GameCards.Application.Commands.UpdateListing;
using com.brettnamba.MlbTheShowForecaster.GameCards.Application.Dtos;
using com.brettnamba.MlbTheShowForecaster.GameCards.Application.Queries.GetAllPlayerCards;
using com.brettnamba.MlbTheShowForecaster.GameCards.Application.Queries.GetListingByCardExternalId;
using com.brettnamba.MlbTheShowForecaster.GameCards.Application.Services;
using com.brettnamba.MlbTheShowForecaster.GameCards.Application.Tests.Dtos.TestClasses;
using com.brettnamba.MlbTheShowForecaster.GameCards.Domain.Cards.Entities;
using com.brettnamba.MlbTheShowForecaster.GameCards.Domain.Cards.ValueObjects;
using com.brettnamba.MlbTheShowForecaster.GameCards.Domain.Marketplace.Entities;
using com.brettnamba.MlbTheShowForecaster.GameCards.Domain.Marketplace.ValueObjects;
using Moq;

namespace com.brettnamba.MlbTheShowForecaster.GameCards.Application.Tests.Services;

public class CardPriceTrackerTests
{
    [Fact]
    public async Task TrackCardPrices_SeasonYear_CreatesAndUpdatesListingPrices()
    {
        /*
         * Arrange
         */
        var cToken = CancellationToken.None;
        var year = SeasonYear.Create(2024);
        var stubPriceChangeThreshold = Mock.Of<IListingPriceSignificantChangeThreshold>();
        // Listing 1 for PlayerCard 1 does not exist in the system and will be created
        var externalListingId1 = CardExternalId.Create("1");
        var externalListing1 = Faker.FakeCardListing(cardExternalId: externalListingId1.Value);
        var domainPlayerCard1 = TestClasses.Faker.FakePlayerCard(year: year.Value, externalId: externalListingId1.Value);
        // Listing 2 for PlayerCard 2 exists but the external listing has new data, so it will be updated
        var externalListingId2 = CardExternalId.Create("2");
        var externalListing2 = Faker.FakeCardListing(cardExternalId: externalListingId2.Value,
            historicalPrices: new List<CardListingPrice>() { Faker.FakeCardListingPrice() }
        );
        var domainListing2 = TestClasses.Faker.FakeListing(cardExternalId: externalListingId2.Value, 2, 20);
        var domainPlayerCard2 = TestClasses.Faker.FakePlayerCard(year: year.Value, externalId: externalListingId2.Value);
        // Listing 3 for PlayerCard 3 exists, but the external listing has no new data, so no action will be taken
        var externalListingId3 = CardExternalId.Create("3");
        var externalListing3 = Faker.FakeCardListing(cardExternalId: externalListingId3.Value);
        var domainListing3 = TestClasses.Faker.FakeListing(cardExternalId: externalListingId3.Value, 3, 30);
        var domainPlayerCard3 = TestClasses.Faker.FakePlayerCard(year: year.Value, externalId: externalListingId3.Value);

        // The query that returns all PlayerCards currently in the domain
        var getAllPlayerCardsQuery = new GetAllPlayerCardsQuery(year);
        var getAllPlayerCardsQueryResult = new List<PlayerCard>()
            { domainPlayerCard1, domainPlayerCard2, domainPlayerCard3 };

        // The card marketplace should return the current prices from the external source
        var stubCardMarketplace = new Mock<ICardMarketplace>();
        stubCardMarketplace.Setup(x => x.GetCardPrice(externalListingId1, cToken))
            .ReturnsAsync(externalListing1);
        stubCardMarketplace.Setup(x => x.GetCardPrice(externalListingId2, cToken))
            .ReturnsAsync(externalListing2);
        stubCardMarketplace.Setup(x => x.GetCardPrice(externalListingId3, cToken))
            .ReturnsAsync(externalListing3);

        // Stub the behavior of the query handler
        var stubQuerySender = new Mock<IQuerySender>();
        stubQuerySender.Setup(x => x.Send(getAllPlayerCardsQuery, cToken))
            .ReturnsAsync(getAllPlayerCardsQueryResult);
        var getListing1Query = new GetListingByCardExternalIdQuery(externalListingId1);
        stubQuerySender.Setup(x => x.Send(getListing1Query, cToken))
            .ReturnsAsync((Listing?)null);
        var getListing2Query = new GetListingByCardExternalIdQuery(externalListingId2);
        stubQuerySender.Setup(x => x.Send(getListing2Query, cToken))
            .ReturnsAsync(domainListing2);
        var getListing3Query = new GetListingByCardExternalIdQuery(externalListingId3);
        stubQuerySender.Setup(x => x.Send(getListing3Query, cToken))
            .ReturnsAsync(domainListing3);

        // The command sender should expect to send a create command for Listing 1
        var mockCommandSender = Mock.Of<ICommandSender>();
        var expectedListing1Command = new CreateListingCommand(externalListing1);
        // The command sender should expect to send an update command for Listing 2
        var expectedListing2Command =
            new UpdateListingCommand(domainListing2, externalListing2, stubPriceChangeThreshold);
        // The command sender should expect no update command for Listing 3
        var notExpectedListing3Command =
            new UpdateListingCommand(domainListing3, externalListing3, stubPriceChangeThreshold);

        // Tracker
        var tracker = new CardPriceTracker(stubCardMarketplace.Object, stubQuerySender.Object, mockCommandSender,
            stubPriceChangeThreshold);

        /*
         * Act
         */
        await tracker.TrackCardPrices(year, cToken);

        /*
         * Assert
         */
        // Were all the domain PlayerCards retrieved?
        stubQuerySender.Verify(x => x.Send(getAllPlayerCardsQuery, cToken), Times.Once);
        // Was each domain Listing retrieved?
        stubQuerySender.Verify(x => x.Send(getListing1Query, cToken), Times.Once);
        stubQuerySender.Verify(x => x.Send(getListing2Query, cToken), Times.Once);
        stubQuerySender.Verify(x => x.Send(getListing3Query, cToken), Times.Once);
        // Were the card prices retrieved from the external card marketplace?
        stubCardMarketplace.Verify(x => x.GetCardPrice(externalListingId1, cToken), Times.Once);
        stubCardMarketplace.Verify(x => x.GetCardPrice(externalListingId2, cToken), Times.Once);
        stubCardMarketplace.Verify(x => x.GetCardPrice(externalListingId3, cToken), Times.Once);
        // Was a create command sent for Listing 1?
        Mock.Get(mockCommandSender).Verify(x => x.Send(expectedListing1Command, cToken), Times.Once);
        // Was a update command sent for Listing 2?
        Mock.Get(mockCommandSender).Verify(x => x.Send(expectedListing2Command, cToken), Times.Once);
        // No command should have been sent for Listing 3
        Mock.Get(mockCommandSender).Verify(x => x.Send(notExpectedListing3Command, cToken), Times.Never);
    }
}