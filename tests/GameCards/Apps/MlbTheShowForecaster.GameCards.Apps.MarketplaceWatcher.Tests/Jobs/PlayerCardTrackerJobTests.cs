using com.brettnamba.MlbTheShowForecaster.Common.Application.Jobs.Io;
using com.brettnamba.MlbTheShowForecaster.Common.Domain.ValueObjects;
using com.brettnamba.MlbTheShowForecaster.GameCards.Application.Services;
using com.brettnamba.MlbTheShowForecaster.GameCards.Application.Tests.Services.TestClasses;
using com.brettnamba.MlbTheShowForecaster.GameCards.Apps.MarketplaceWatcher.Jobs;
using Microsoft.Extensions.Logging;
using Moq;

namespace com.brettnamba.MlbTheShowForecaster.GameCards.Apps.MarketplaceWatcher.Tests.Jobs;

public class PlayerCardTrackerJobTests
{
    [Fact]
    public async Task Execute_JobInput_RunsJob()
    {
        // Arrange
        var cToken = CancellationToken.None;
        var input = new SeasonJobInput(SeasonYear.Create(2024));
        var result = Faker.FakePlayerCardTrackerResult(1, 2, 3);

        var stubPlayerCardTracker = new Mock<IPlayerCardTracker>();
        stubPlayerCardTracker.Setup(x => x.TrackPlayerCards(input.Year, cToken))
            .ReturnsAsync(result);

        var mockLogger = Mock.Of<ILogger<PlayerCardTrackerJob>>();

        var job = new PlayerCardTrackerJob(stubPlayerCardTracker.Object, mockLogger);

        // Act
        var actual = await job.Execute(input, cToken);

        // Arrange
        Assert.Equal(1, actual.TotalCatalogCards);
        Assert.Equal(2, actual.TotalNewCatalogCards);
        Assert.Equal(3, actual.TotalUpdatedPlayerCards);
    }
}