using com.brettnamba.MlbTheShowForecaster.Common.Domain.Events;
using com.brettnamba.MlbTheShowForecaster.Common.Domain.SeedWork;
using com.brettnamba.MlbTheShowForecaster.GameCards.Application.Commands.UpdateListing;
using com.brettnamba.MlbTheShowForecaster.GameCards.Application.Commands.UpdateListing.Exceptions;
using com.brettnamba.MlbTheShowForecaster.GameCards.Application.Dtos;
using com.brettnamba.MlbTheShowForecaster.GameCards.Application.Tests.Dtos.TestClasses;
using com.brettnamba.MlbTheShowForecaster.GameCards.Domain;
using com.brettnamba.MlbTheShowForecaster.GameCards.Domain.Marketplace.Entities;
using com.brettnamba.MlbTheShowForecaster.GameCards.Domain.Marketplace.Repositories;
using com.brettnamba.MlbTheShowForecaster.GameCards.Domain.Marketplace.ValueObjects;
using Moq;

namespace com.brettnamba.MlbTheShowForecaster.GameCards.Application.Tests.Commands.UpdateListing;

public class UpdateListingCommandHandlerTests
{
    [Fact]
    public async Task Handle_MissingListing_ThrowsException()
    {
        // Arrange
        var cToken = CancellationToken.None;
        var externalCardListing = Faker.FakeCardListing();
        Listing? domainListing = null;

        var mockPriceChangeThreshold = Mock.Of<IListingPriceSignificantChangeThreshold>();
        var mockDomainEventDispatcher = Mock.Of<IDomainEventDispatcher>();

        var mockListingRepository = Mock.Of<IListingRepository>();

        var stubUnitOfWork = new Mock<IUnitOfWork<IMarketplaceWork>>();
        stubUnitOfWork.Setup(x => x.GetContributor<IListingRepository>())
            .Returns(mockListingRepository);

        var command = new UpdateListingCommand(domainListing!, externalCardListing, mockPriceChangeThreshold);
        var handler = new UpdateListingCommandHandler(stubUnitOfWork.Object, mockDomainEventDispatcher);

        var action = () => handler.Handle(command, cToken);

        // Act
        var actual = await Record.ExceptionAsync(action);

        // Assert
        Assert.NotNull(actual);
        Assert.IsType<ListingNotFoundException>(actual);
    }

    [Fact]
    public async Task Handle_UpdateListingCommand_UpdatesListing()
    {
        // Arrange
        var cToken = CancellationToken.None;
        var externalId = Domain.Tests.Marketplace.TestClasses.Faker.FakeGuid1;
        var externalCardListing = Faker.FakeCardListing(cardExternalId: externalId,
            bestBuyPrice: 100, bestSellPrice: 200, historicalPrices: new List<CardListingPrice>()
            {
                Faker.FakeCardListingPrice(new DateOnly(2024, 4, 2), 10, 20),
                Faker.FakeCardListingPrice(new DateOnly(2024, 4, 1), 1, 2)
            }, completedOrders: new List<CardListingOrder>()
            {
                Faker.FakeCompletedOrder(new DateTime(2024, 4, 3, 10, 20, 0), price: 10, sequenceNumber: 0),
                Faker.FakeCompletedOrder(new DateTime(2024, 4, 4, 10, 20, 0), price: 20, sequenceNumber: 1),
                Faker.FakeCompletedOrder(new DateTime(2024, 4, 4, 10, 20, 0), price: 20, sequenceNumber: 2),
            });
        var domainListing = Domain.Tests.Marketplace.TestClasses.Faker.FakeListing(
            cardExternalId: externalId, buyPrice: 1, sellPrice: 1);

        var stubPriceChangeThreshold = new Mock<IListingPriceSignificantChangeThreshold>();
        stubPriceChangeThreshold.Setup(x => x.BuyPricePercentageChangeThreshold)
            .Returns(50m);
        stubPriceChangeThreshold.Setup(x => x.SellPricePercentageChangeThreshold)
            .Returns(50m);

        var mockDomainEventDispatcher = Mock.Of<IDomainEventDispatcher>();
        var mockListingRepository = new Mock<IListingRepository>();

        var stubUnitOfWork = new Mock<IUnitOfWork<IMarketplaceWork>>();
        stubUnitOfWork.Setup(x => x.GetContributor<IListingRepository>())
            .Returns(mockListingRepository.Object);

        var command = new UpdateListingCommand(domainListing, externalCardListing, stubPriceChangeThreshold.Object);
        var handler = new UpdateListingCommandHandler(stubUnitOfWork.Object, mockDomainEventDispatcher);

        // Act
        await handler.Handle(command, cToken);

        // Assert
        mockListingRepository.Verify(x => x.Update(domainListing, cToken), Times.Once);
        Mock.Get(mockDomainEventDispatcher).Verify(x => x.Dispatch(domainListing.DomainEvents));

        Assert.Equal(new Guid("00000000-0000-0000-0000-000000000001"), domainListing.CardExternalId.Value);
        Assert.Equal(100, domainListing.BuyPrice.Value);
        Assert.Equal(200, domainListing.SellPrice.Value);
    }
}