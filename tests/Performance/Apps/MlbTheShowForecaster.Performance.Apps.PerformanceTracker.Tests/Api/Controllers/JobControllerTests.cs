﻿using com.brettnamba.MlbTheShowForecaster.Common.Application.Jobs;
using com.brettnamba.MlbTheShowForecaster.Performance.Apps.PerformanceTracker.Api.Controllers;
using com.brettnamba.MlbTheShowForecaster.Performance.Apps.PerformanceTracker.Jobs;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Moq;

namespace com.brettnamba.MlbTheShowForecaster.Performance.Apps.PerformanceTracker.Tests.Api.Controllers;

public class JobControllerTests
{
    [Fact]
    public void Start_UnknownJob_ReturnsBadRequest()
    {
        // Arrange
        var cToken = CancellationToken.None;
        const int season = 2024;

        var jobManager = new Mock<IJobManager>();

        var controller = new JobController(jobManager.Object);

        // Act
        var actual = controller.Start("unknownJob", season, cToken);

        // Assert
        Assert.Equal(400, ((IStatusCodeActionResult)actual).StatusCode);
    }

    [Theory]
    [InlineData(nameof(PerformanceTrackerJob))]
    public void Start_JobAndSeason_RunsJobAndReturnsNoContent(string jobName)
    {
        // Arrange
        var cToken = CancellationToken.None;
        const int season = 2024;

        var jobManager = new Mock<IJobManager>();

        var controller = new JobController(jobManager.Object);

        // Act
        var actual = controller.Start(jobName, season, cToken);

        // Assert
        Assert.Equal(204, ((IStatusCodeActionResult)actual).StatusCode);
    }
}