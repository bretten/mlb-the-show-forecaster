using com.brettnamba.MlbTheShowForecaster.Common.Domain.SeedWork;
using com.brettnamba.MlbTheShowForecaster.GameCards.Application.Commands.UpdateListing;
using com.brettnamba.MlbTheShowForecaster.GameCards.Application.Dtos;
using com.brettnamba.MlbTheShowForecaster.GameCards.Application.Tests.TestClasses;
using com.brettnamba.MlbTheShowForecaster.GameCards.Domain.Marketplace.Repositories;
using com.brettnamba.MlbTheShowForecaster.GameCards.Domain.Marketplace.ValueObjects;
using Moq;

namespace com.brettnamba.MlbTheShowForecaster.GameCards.Application.Tests.Commands.UpdateListing;

public class UpdateListingCommandHandlerTests
{
    [Fact]
    public async Task Handle_UpdateListingCommand_UpdatesListing()
    {
        // Arrange
        var externalId = Faker.FakeGuid1;
        var externalCardListing = Dtos.TestClasses.Faker.FakeCardListing(cardExternalId: externalId,
            bestBuyPrice: 100, bestSellPrice: 200, historicalPrices: new List<CardListingPrice>()
            {
                Dtos.TestClasses.Faker.FakeCardListingPrice(new DateOnly(2024, 4, 2), 10, 20),
                Dtos.TestClasses.Faker.FakeCardListingPrice(new DateOnly(2024, 4, 1), 1, 2)
            });
        var domainListing = Faker.FakeListing(cardExternalId: externalId, buyPrice: 1, sellPrice: 1);

        var stubPriceChangeThreshold = new Mock<IListingPriceSignificantChangeThreshold>();
        stubPriceChangeThreshold.Setup(x => x.BuyPricePercentageChangeThreshold)
            .Returns(50m);
        stubPriceChangeThreshold.Setup(x => x.SellPricePercentageChangeThreshold)
            .Returns(50m);

        var mockListingRepository = Mock.Of<IListingRepository>();
        var mockUnitOfWork = Mock.Of<IUnitOfWork>();

        var cToken = CancellationToken.None;
        var command = new UpdateListingCommand(domainListing, externalCardListing, stubPriceChangeThreshold.Object);
        var handler = new UpdateListingCommandHandler(mockListingRepository, mockUnitOfWork);

        // Act
        await handler.Handle(command, cToken);

        // Assert
        Mock.Get(mockListingRepository).Verify(x => x.Update(domainListing, cToken), Times.Once);
        Mock.Get(mockUnitOfWork).Verify(x => x.CommitAsync(cToken), Times.Once);

        Assert.Equal(new Guid("00000000-0000-0000-0000-000000000001"), domainListing.CardExternalId.Value);
        Assert.Equal(100, domainListing.BuyPrice.Value);
        Assert.Equal(200, domainListing.SellPrice.Value);
        Assert.Equal(2, domainListing.HistoricalPricesChronologically.Count);
        Assert.Equal(new DateOnly(2024, 4, 1), domainListing.HistoricalPricesChronologically[0].Date);
        Assert.Equal(1, domainListing.HistoricalPricesChronologically[0].BuyPrice.Value);
        Assert.Equal(2, domainListing.HistoricalPricesChronologically[0].SellPrice.Value);
        Assert.Equal(new DateOnly(2024, 4, 2), domainListing.HistoricalPricesChronologically[1].Date);
        Assert.Equal(10, domainListing.HistoricalPricesChronologically[1].BuyPrice.Value);
        Assert.Equal(20, domainListing.HistoricalPricesChronologically[1].SellPrice.Value);
    }
}