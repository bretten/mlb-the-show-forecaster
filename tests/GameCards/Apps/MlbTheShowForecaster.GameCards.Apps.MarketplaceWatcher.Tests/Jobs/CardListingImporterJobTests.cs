using com.brettnamba.MlbTheShowForecaster.Common.Application.Jobs.Io;
using com.brettnamba.MlbTheShowForecaster.Common.Domain.ValueObjects;
using com.brettnamba.MlbTheShowForecaster.GameCards.Application.Services;
using com.brettnamba.MlbTheShowForecaster.GameCards.Application.Services.EventStores;
using com.brettnamba.MlbTheShowForecaster.GameCards.Apps.MarketplaceWatcher.Jobs;
using com.brettnamba.MlbTheShowForecaster.GameCards.Domain.Cards.Entities;
using com.brettnamba.MlbTheShowForecaster.GameCards.Domain.Cards.Repositories;
using com.brettnamba.MlbTheShowForecaster.GameCards.Domain.Tests.Cards.TestClasses;
using Microsoft.Extensions.Logging;
using Moq;

namespace com.brettnamba.MlbTheShowForecaster.GameCards.Apps.MarketplaceWatcher.Tests.Jobs;

public class CardListingImporterJobTests
{
    [Fact]
    public async Task Execute_JobInput_RunsJob()
    {
        // Arrange
        var cToken = CancellationToken.None;
        var input = new SeasonJobInput(SeasonYear.Create(2024));
        var card1 = Faker.FakePlayerCard(externalId: Faker.FakeGuid1);
        var card2 = Faker.FakePlayerCard(externalId: Faker.FakeGuid2);
        var playerCards = new List<PlayerCard>() { card1, card2 };

        var stubPlayerCardRepository = new Mock<IPlayerCardRepository>();
        stubPlayerCardRepository.Setup(x => x.GetAll(input.Year))
            .ReturnsAsync(playerCards);

        var stubCardMarketplace = new Mock<ICardMarketplace>();
        var listing1 = Application.Tests.Dtos.TestClasses.Faker.FakeCardListing(cardExternalId: Faker.FakeGuid1);
        var listing2 = Application.Tests.Dtos.TestClasses.Faker.FakeCardListing(cardExternalId: Faker.FakeGuid2);
        stubCardMarketplace.Setup(x => x.GetCardPrice(input.Year, card1.ExternalId, cToken))
            .ReturnsAsync(listing1);
        stubCardMarketplace.Setup(x => x.GetCardPrice(input.Year, card2.ExternalId, cToken))
            .ReturnsAsync(listing2);

        var mockListingEventStore = new Mock<IListingEventStore>();
        var mockLogger = Mock.Of<ILogger<CardListingImporterJob>>();

        var job = new CardListingImporterJob(stubPlayerCardRepository.Object, stubCardMarketplace.Object,
            mockListingEventStore.Object, mockLogger);

        // Act
        var actual = await job.Execute(input, cToken);

        // Arrange
        Assert.Equal(2, actual.TotalListings);
        mockListingEventStore.Verify(x => x.AppendNewPricesAndOrders(card1.Year, listing1), Times.Once);
        mockListingEventStore.Verify(x => x.AppendNewPricesAndOrders(card2.Year, listing2), Times.Once);
    }
}