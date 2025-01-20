using com.brettnamba.MlbTheShowForecaster.Common.Application.Jobs.Io;
using com.brettnamba.MlbTheShowForecaster.Common.Domain.ValueObjects;
using com.brettnamba.MlbTheShowForecaster.GameCards.Application.Services;
using com.brettnamba.MlbTheShowForecaster.GameCards.Application.Tests.Services.TestClasses;
using com.brettnamba.MlbTheShowForecaster.GameCards.Apps.MarketplaceWatcher.Jobs;
using Microsoft.Extensions.Logging;
using Moq;

namespace com.brettnamba.MlbTheShowForecaster.GameCards.Apps.MarketplaceWatcher.Tests.Jobs;

public class CardPriceTrackerJobTests
{
    [Fact]
    public async Task Execute_JobInput_RunsJob()
    {
        // Arrange
        var cToken = CancellationToken.None;
        var input = new SeasonJobInput(SeasonYear.Create(2024));
        var result = Faker.FakeCardPriceTrackerResult(1, 2, 3);

        var stubCardPriceTracker = new Mock<ICardPriceTracker>();
        stubCardPriceTracker.Setup(x => x.TrackCardPrices(input.Year, cToken))
            .ReturnsAsync(result);

        var mockLogger = Mock.Of<ILogger<CardPriceTrackerJob>>();

        var job = new CardPriceTrackerJob(stubCardPriceTracker.Object, mockLogger);

        // Act
        var actual = await job.Execute(input, cToken);

        // Arrange
        Assert.Equal(1, actual.TotalCards);
        Assert.Equal(2, actual.TotalNewListings);
        Assert.Equal(3, actual.TotalUpdatedListings);
    }
}