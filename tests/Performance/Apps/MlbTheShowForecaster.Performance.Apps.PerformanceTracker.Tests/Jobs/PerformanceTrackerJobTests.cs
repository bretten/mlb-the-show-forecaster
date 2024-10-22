using com.brettnamba.MlbTheShowForecaster.Common.Domain.ValueObjects;
using com.brettnamba.MlbTheShowForecaster.Performance.Application.Services;
using com.brettnamba.MlbTheShowForecaster.Performance.Apps.PerformanceTracker.Jobs;
using com.brettnamba.MlbTheShowForecaster.Performance.Apps.PerformanceTracker.Jobs.Io;
using Microsoft.Extensions.Logging;
using Moq;

namespace com.brettnamba.MlbTheShowForecaster.Performance.Apps.PerformanceTracker.Tests.Jobs;

public class PerformanceTrackerJobTests
{
    [Fact]
    public async Task Execute_JobInput_RunsJob()
    {
        // Arrange
        var cToken = CancellationToken.None;
        var input = new SeasonJobInput(SeasonYear.Create(2024));
        var result = Application.Tests.Services.TestClasses.Faker.FakePerformanceTrackerResult(1, 2, 3);

        var stubPerformanceTracker = new Mock<IPerformanceTracker>();
        stubPerformanceTracker.Setup(x => x.TrackPlayerPerformance(input.Year, cToken))
            .ReturnsAsync(result);

        var mockLogger = Mock.Of<ILogger<PerformanceTrackerJob>>();

        var job = new PerformanceTrackerJob(stubPerformanceTracker.Object, mockLogger);

        // Act
        var actual = await job.Execute(input, cToken);

        // Arrange
        Assert.Equal(1, actual.TotalPlayerSeasons);
        Assert.Equal(2, actual.TotalNewPlayerSeasons);
        Assert.Equal(3, actual.TotalPlayerSeasonUpdates);
    }
}