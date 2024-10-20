using com.brettnamba.MlbTheShowForecaster.Common.Domain.ValueObjects;
using com.brettnamba.MlbTheShowForecaster.GameCards.Application.Services;
using com.brettnamba.MlbTheShowForecaster.GameCards.Application.Services.Results;
using com.brettnamba.MlbTheShowForecaster.GameCards.Application.Tests.Services.TestClasses;
using com.brettnamba.MlbTheShowForecaster.GameCards.Apps.MarketplaceWatcher.Jobs;
using com.brettnamba.MlbTheShowForecaster.GameCards.Apps.MarketplaceWatcher.Jobs.Io;
using Microsoft.Extensions.Logging;
using Moq;

namespace com.brettnamba.MlbTheShowForecaster.GameCards.Apps.MarketplaceWatcher.Tests.Jobs;

public class RosterUpdaterJobTests
{
    [Fact]
    public async Task Execute_JobInput_RunsJob()
    {
        // Arrange
        var cToken = CancellationToken.None;
        var input = new SeasonJobInput(SeasonYear.Create(2024));
        var result = new List<RosterUpdateOrchestratorResult>() { Faker.FakeRosterUpdateOrchestratorResult() };
        var historyResult = Faker.FakePlayerRatingHistoryResult();

        var stubRosterUpdateOrchestrator = new Mock<IRosterUpdateOrchestrator>();
        stubRosterUpdateOrchestrator.Setup(x => x.SyncRosterUpdates(input.Year, cToken))
            .ReturnsAsync(result);

        var stubPlayerRatingHistoryService = new Mock<IPlayerRatingHistoryService>();
        stubPlayerRatingHistoryService.Setup(x => x.SyncHistory(input.Year, cToken))
            .ReturnsAsync(historyResult);

        var mockLogger = Mock.Of<ILogger<RosterUpdaterJob>>();

        var job = new RosterUpdaterJob(stubRosterUpdateOrchestrator.Object, stubPlayerRatingHistoryService.Object,
            mockLogger);

        // Act
        var actual = await job.Execute(input, cToken);

        // Arrange
        Assert.Equal(result, actual.Result);
    }
}