using com.brettnamba.MlbTheShowForecaster.Common.Application.Jobs.Io;
using com.brettnamba.MlbTheShowForecaster.Common.Domain.ValueObjects;
using com.brettnamba.MlbTheShowForecaster.PlayerStatus.Application.Services;
using com.brettnamba.MlbTheShowForecaster.PlayerStatus.Application.Tests.Services.TestClasses;
using com.brettnamba.MlbTheShowForecaster.PlayerStatus.Apps.PlayerTracker.Jobs;
using Microsoft.Extensions.Logging;
using Moq;

namespace com.brettnamba.MlbTheShowForecaster.PlayerStatus.Apps.PlayerTracker.Tests.Jobs;

public class PlayerStatusTrackerJobTests
{
    [Fact]
    public async Task Execute_JobInput_RunsJob()
    {
        // Arrange
        var cToken = CancellationToken.None;
        var input = new SeasonJobInput(SeasonYear.Create(2024));
        var result = Faker.FakePlayerStatusTrackerResult(1, 2, 3);

        var stubPlayerStatusTracker = new Mock<IPlayerStatusTracker>();
        stubPlayerStatusTracker.Setup(x => x.TrackPlayers(input.Year, cToken))
            .ReturnsAsync(result);

        var mockLogger = Mock.Of<ILogger<PlayerStatusTrackerJob>>();

        var job = new PlayerStatusTrackerJob(stubPlayerStatusTracker.Object, mockLogger);

        // Act
        var actual = await job.Execute(input, cToken);

        // Arrange
        Assert.Equal(1, actual.TotalRosterEntries);
        Assert.Equal(2, actual.TotalNewPlayers);
        Assert.Equal(3, actual.TotalUpdatedPlayers);
    }
}