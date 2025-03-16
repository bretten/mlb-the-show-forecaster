using System.Collections.Immutable;
using com.brettnamba.MlbTheShowForecaster.Common.Application.Cqrs;
using com.brettnamba.MlbTheShowForecaster.Common.Domain.ValueObjects;
using com.brettnamba.MlbTheShowForecaster.GameCards.Application.Commands.CreateListing;
using com.brettnamba.MlbTheShowForecaster.GameCards.Application.Commands.UpdateListing;
using com.brettnamba.MlbTheShowForecaster.GameCards.Application.Commands.UpdateListingsPricesAndOrders;
using com.brettnamba.MlbTheShowForecaster.GameCards.Application.Queries.GetAllPlayerCards;
using com.brettnamba.MlbTheShowForecaster.GameCards.Application.Queries.GetListingByCardExternalId;
using com.brettnamba.MlbTheShowForecaster.GameCards.Application.Services;
using com.brettnamba.MlbTheShowForecaster.GameCards.Application.Services.EventStores;
using com.brettnamba.MlbTheShowForecaster.GameCards.Application.Services.Exceptions;
using com.brettnamba.MlbTheShowForecaster.GameCards.Domain.Cards.Entities;
using com.brettnamba.MlbTheShowForecaster.GameCards.Domain.Cards.ValueObjects;
using com.brettnamba.MlbTheShowForecaster.GameCards.Domain.Marketplace.Entities;
using com.brettnamba.MlbTheShowForecaster.GameCards.Domain.Marketplace.ValueObjects;
using com.brettnamba.MlbTheShowForecaster.GameCards.Domain.Tests.Cards.TestClasses;
using Moq;

namespace com.brettnamba.MlbTheShowForecaster.GameCards.Application.Tests.Services;

public class CardPriceTrackerTests
{
    [Fact]
    public async Task TrackCardPrices_NoCards_ThrowsException()
    {
        // Arrange
        var cToken = CancellationToken.None;
        var year = SeasonYear.Create(2024);

        var getAllPlayerCardsQuery = new GetAllPlayerCardsQuery(year);
        var getAllPlayerCardsQueryResult = ImmutableList<PlayerCard>.Empty;
        var stubQuerySender = new Mock<IQuerySender>();
        stubQuerySender.Setup(x => x.Send(getAllPlayerCardsQuery, cToken))
            .ReturnsAsync(getAllPlayerCardsQueryResult);

        var mockListingEventStore = Mock.Of<IListingEventStore>();
        var mockCommandSender = Mock.Of<ICommandSender>();
        var stubPriceChangeThreshold = Mock.Of<IListingPriceSignificantChangeThreshold>();

        var tracker = new CardPriceTracker(mockListingEventStore, stubQuerySender.Object, mockCommandSender,
            stubPriceChangeThreshold, 1000);
        var action = () => tracker.TrackCardPrices(year, cToken);

        // Act
        var actual = await Record.ExceptionAsync(action);

        // Assert
        Assert.NotNull(actual);
        Assert.IsType<CardPriceTrackerFoundNoCardsException>(actual);
        stubQuerySender.Verify(x => x.Send(It.IsAny<GetListingByCardExternalIdQuery>(), cToken), Times.Never);
        Mock.Get(mockListingEventStore).Verify(x => x.PeekListing(year, It.IsAny<CardExternalId>()), Times.Never);
        Mock.Get(mockCommandSender).Verify(x => x.Send(It.IsAny<CreateListingCommand>(), cToken), Times.Never);
        Mock.Get(mockCommandSender).Verify(x => x.Send(It.IsAny<UpdateListingCommand>(), cToken), Times.Never);
    }

    [Fact]
    public async Task TrackCardPrices_SeasonYear_CreatesAndUpdatesListingPrices()
    {
        /*
         * Arrange
         */
        var cToken = CancellationToken.None;
        var year = SeasonYear.Create(2024);
        var stubPriceChangeThreshold = Mock.Of<IListingPriceSignificantChangeThreshold>();
        // Listing1 for PlayerCard1 does not exist in the domain and will be created
        var cardExternalId1 = Faker.FakeCardExternalId(Faker.FakeGuid1);
        var externalListing1 = Dtos.TestClasses.Faker.FakeCardListing(year.Value, cardExternalId: cardExternalId1.Value,
            bestBuyPrice: 1, bestSellPrice: 10);
        var domainListing1 =
            Domain.Tests.Marketplace.TestClasses.Faker.FakeListing(year.Value, cardExternalId1.Value, 1, 10);
        var domainPlayerCard1 = Faker.FakePlayerCard(year: year.Value, externalId: cardExternalId1.Value);
        // Listing2 for PlayerCard2 exists but the external listing has new data, so it will be updated
        var cardExternalId2 = Faker.FakeCardExternalId(Faker.FakeGuid2);
        var externalListing2 = Dtos.TestClasses.Faker.FakeCardListing(year.Value, cardExternalId: cardExternalId2.Value,
            bestBuyPrice: 21, bestSellPrice: 210);
        var domainListing2 =
            Domain.Tests.Marketplace.TestClasses.Faker.FakeListing(year.Value, cardExternalId2.Value, 2, 20);
        var domainPlayerCard2 = Faker.FakePlayerCard(year: year.Value, externalId: cardExternalId2.Value);
        // Listing3 for PlayerCard3 exists, but the external listing has no new data, so no action will be taken
        var cardExternalId3 = Faker.FakeCardExternalId(Faker.FakeGuid3);
        var externalListing3 = Dtos.TestClasses.Faker.FakeCardListing(year.Value, cardExternalId: cardExternalId3.Value,
            bestBuyPrice: 3, bestSellPrice: 30);
        var domainListing3 =
            Domain.Tests.Marketplace.TestClasses.Faker.FakeListing(year.Value, cardExternalId3.Value, 3, 30);
        var domainPlayerCard3 = Faker.FakePlayerCard(year: year.Value, externalId: cardExternalId3.Value);

        // The query that returns all PlayerCards currently in the domain
        var getAllPlayerCardsQuery = new GetAllPlayerCardsQuery(year);
        var getAllPlayerCardsQueryResult = new List<PlayerCard>()
            { domainPlayerCard1, domainPlayerCard2, domainPlayerCard3 };

        // The card marketplace should return the current prices from the external source
        var stubListingEventStore = new Mock<IListingEventStore>();
        stubListingEventStore.Setup(x => x.PeekListing(year, cardExternalId1))
            .ReturnsAsync(externalListing1);
        stubListingEventStore.Setup(x => x.PeekListing(year, cardExternalId2))
            .ReturnsAsync(externalListing2);
        stubListingEventStore.Setup(x => x.PeekListing(year, cardExternalId3))
            .ReturnsAsync(externalListing3);

        // Stub the behavior of the query handler
        var stubQuerySender = new Mock<IQuerySender>();
        stubQuerySender.Setup(x => x.Send(getAllPlayerCardsQuery, cToken))
            .ReturnsAsync(getAllPlayerCardsQueryResult);
        var getListing1Query = new GetListingByCardExternalIdQuery(year, cardExternalId1, false);
        stubQuerySender.SetupSequence(x => x.Send(getListing1Query, cToken))
            .ReturnsAsync((Listing?)null)
            .ReturnsAsync(domainListing1);
        var getListing2Query = new GetListingByCardExternalIdQuery(year, cardExternalId2, false);
        stubQuerySender.Setup(x => x.Send(getListing2Query, cToken))
            .ReturnsAsync(domainListing2);
        var getListing3Query = new GetListingByCardExternalIdQuery(year, cardExternalId3, false);
        stubQuerySender.Setup(x => x.Send(getListing3Query, cToken))
            .ReturnsAsync(domainListing3);

        // The command sender should expect to send a create command for Listing1
        var mockCommandSender = Mock.Of<ICommandSender>();
        var expectedListing1Command = new CreateListingCommand(externalListing1);
        // The command sender should expect to send an update command for Listing2
        var expectedListing2Command =
            new UpdateListingCommand(domainListing2, externalListing2, stubPriceChangeThreshold);
        // No command should be expected for Listing3
        var notExpectedListing3CreateCommand = new CreateListingCommand(externalListing3);
        var notExpectedListing3UpdateCommand =
            new UpdateListingCommand(domainListing3, externalListing3, stubPriceChangeThreshold);

        // Tracker
        var tracker = new CardPriceTracker(stubListingEventStore.Object, stubQuerySender.Object, mockCommandSender,
            stubPriceChangeThreshold, 1000);

        /*
         * Act
         */
        var actual = await tracker.TrackCardPrices(year, cToken);

        /*
         * Assert
         */
        // There were 3 player cards in the domain
        Assert.Equal(3, actual.TotalCards);
        // PlayerCard1 had no listing, so it was created
        Assert.Equal(1, actual.TotalNewListings);
        // PlayerCard2 had an existing listing, so it was updated
        Assert.Equal(1, actual.TotalUpdatedListings);
        // PlayerCard 3 had an up-to-date listing, so nothing was changed
        Assert.Equal(1, actual.TotalUnchangedListings);

        // Were all the domain PlayerCards retrieved?
        stubQuerySender.Verify(x => x.Send(getAllPlayerCardsQuery, cToken), Times.Once);
        // Was each domain Listing retrieved? Listing1 will be requested twice: once to check if exists and again after adding
        stubQuerySender.Verify(x => x.Send(getListing1Query, cToken), Times.Exactly(2));
        stubQuerySender.Verify(x => x.Send(getListing2Query, cToken), Times.Once);
        stubQuerySender.Verify(x => x.Send(getListing3Query, cToken), Times.Once);
        // Were the card prices retrieved from the external card marketplace?
        stubListingEventStore.Verify(x => x.PeekListing(year, cardExternalId1), Times.Once);
        stubListingEventStore.Verify(x => x.PeekListing(year, cardExternalId2), Times.Once);
        stubListingEventStore.Verify(x => x.PeekListing(year, cardExternalId3), Times.Once);
        // Was a create command sent for Listing 1?
        Mock.Get(mockCommandSender).Verify(x => x.Send(expectedListing1Command, cToken), Times.Once);
        // Was a update command sent for Listing 2?
        Mock.Get(mockCommandSender).Verify(x => x.Send(expectedListing2Command, cToken), Times.Once);
        // No command should have been sent for Listing 3
        Mock.Get(mockCommandSender).Verify(x => x.Send(notExpectedListing3CreateCommand, cToken), Times.Never);
        Mock.Get(mockCommandSender).Verify(x => x.Send(notExpectedListing3UpdateCommand, cToken), Times.Never);

        // Was a command sent to batch update prices and orders?
        var expectedDictionary = new Dictionary<CardExternalId, Listing>()
        {
            { externalListing1.CardExternalId, domainListing1 },
            { externalListing2.CardExternalId, domainListing2 },
            { externalListing3.CardExternalId, domainListing3 },
        };
        Mock.Get(mockCommandSender).Verify(x => x.Send(It.Is<UpdateListingsPricesAndOrdersCommand>(y =>
                y.Year == year && ItIsListings(expectedDictionary, y.Listings) && y.BatchSize == 1000), cToken),
            Times.Once);
    }

    private static bool ItIsListings(Dictionary<CardExternalId, Listing> expected,
        Dictionary<CardExternalId, Listing> actual)
    {
        foreach (var pair in expected)
        {
            Assert.True(actual.ContainsKey(pair.Key));
            Assert.True(expected[pair.Key] == actual[pair.Key]);
        }

        return true;
    }
}