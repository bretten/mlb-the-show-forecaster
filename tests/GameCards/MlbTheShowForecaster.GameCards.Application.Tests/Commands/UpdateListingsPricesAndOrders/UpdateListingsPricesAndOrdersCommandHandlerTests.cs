using System.Collections.ObjectModel;
using com.brettnamba.MlbTheShowForecaster.Common.Domain.ValueObjects;
using com.brettnamba.MlbTheShowForecaster.GameCards.Application.Commands.UpdateListingsPricesAndOrders;
using com.brettnamba.MlbTheShowForecaster.GameCards.Application.Services.EventStores;
using com.brettnamba.MlbTheShowForecaster.GameCards.Domain.Cards.ValueObjects;
using com.brettnamba.MlbTheShowForecaster.GameCards.Domain.Marketplace.Entities;
using com.brettnamba.MlbTheShowForecaster.GameCards.Domain.Marketplace.Repositories;
using com.brettnamba.MlbTheShowForecaster.GameCards.Domain.Marketplace.ValueObjects;
using com.brettnamba.MlbTheShowForecaster.GameCards.Domain.Tests.Marketplace.TestClasses;
using Moq;

namespace com.brettnamba.MlbTheShowForecaster.GameCards.Application.Tests.Commands.UpdateListingsPricesAndOrders;

public class UpdateListingsPricesAndOrdersCommandHandlerTests
{
    [Fact]
    public async Task Handle_UpdateListingsPricesAndOrdersCommand_BatchInsertsPricesAndOrders()
    {
        // Arrange
        var cToken = CancellationToken.None;
        var year = SeasonYear.Create(2025);
        var listing1 = Faker.FakeListing(year.Value, Faker.FakeGuid1);
        var listing2 = Faker.FakeListing(year.Value, Faker.FakeGuid2);
        var command = new UpdateListingsPricesAndOrdersCommand(year, new Dictionary<CardExternalId, Listing>()
        {
            { listing1.CardExternalId, listing1 },
            { listing2.CardExternalId, listing2 },
        }, 100);

        var newPriceEvents = new NewPriceEvents("priceCheckpoint",
            new Dictionary<CardExternalId, ReadOnlyCollection<ListingHistoricalPrice>>()
            {
                {
                    listing1.CardExternalId,
                    new List<ListingHistoricalPrice>()
                    {
                        Faker.FakeListingHistoricalPrice(new DateOnly(2025, 3, 8))
                    }.AsReadOnly()
                },
                {
                    listing2.CardExternalId,
                    new List<ListingHistoricalPrice>()
                    {
                        Faker.FakeListingHistoricalPrice(new DateOnly(2025, 3, 8))
                    }.AsReadOnly()
                }
            });
        var newOrderEvents = new NewOrderEvents("orderCheckpoint",
            new Dictionary<CardExternalId, ReadOnlyCollection<ListingOrder>>()
            {
                {
                    listing1.CardExternalId,
                    new List<ListingOrder>()
                    {
                        Faker.FakeListingOrder()
                    }.AsReadOnly()
                },
                {
                    listing2.CardExternalId,
                    new List<ListingOrder>()
                    {
                        Faker.FakeListingOrder()
                    }.AsReadOnly()
                }
            });

        var stubEventStore = new Mock<IListingEventStore>();
        stubEventStore.Setup(x => x.PollNewPrices(year, command.BatchSize))
            .ReturnsAsync(newPriceEvents);
        stubEventStore.Setup(x => x.PollNewOrders(year, command.BatchSize))
            .ReturnsAsync(newOrderEvents);

        var mockListingRepository = Mock.Of<IListingRepository>();

        var handler = new UpdateListingsPricesAndOrdersCommandHandler(stubEventStore.Object, mockListingRepository);

        // Act
        await handler.Handle(command, cToken);

        // Assert
        Mock.Get(mockListingRepository).Verify(x => x.Add(command.Listings, newPriceEvents.Prices, cToken), Times.Once);
        stubEventStore.Verify(x => x.AcknowledgePrices(year, newPriceEvents.Checkpoint), Times.Once);

        Mock.Get(mockListingRepository).Verify(x => x.Add(command.Listings, newOrderEvents.Orders, cToken), Times.Once);
        stubEventStore.Verify(x => x.AcknowledgeOrders(year, newOrderEvents.Checkpoint), Times.Once);
    }
}