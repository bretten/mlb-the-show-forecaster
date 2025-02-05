using com.brettnamba.MlbTheShowForecaster.Common.Application.Jobs;
using com.brettnamba.MlbTheShowForecaster.Common.Application.RealTime;
using com.brettnamba.MlbTheShowForecaster.Common.Application.Tests.Jobs.TestClasses;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Moq;

namespace com.brettnamba.MlbTheShowForecaster.Common.Application.Tests.Jobs;

public class ScopedSingleInstanceJobManagerTests
{
    [Fact]
    public async Task Run_FailedJob_ThrowsException()
    {
        // Arrange
        var cToken = CancellationToken.None;
        var jobInput = new TestJobInput("input1");
        var job = new TestExceptionJob();
        var jobName = nameof(TestExceptionJob);
        var jobSchedules = new List<JobSchedule>()
        {
            new JobSchedule(job.GetType(), jobInput, TimeSpan.FromMinutes(1))
        };

        var (stubServiceScopeFactory, stubServiceScope) = MockScope(new List<IJob>() { job });
        var mockCommService = Mock.Of<IRealTimeCommService>();
        var mockLogger = Mock.Of<ILogger<ScopedSingleInstanceJobManager>>();

        var m = new ScopedSingleInstanceJobManager(stubServiceScopeFactory.Object, jobSchedules, mockCommService,
            mockLogger);

        var action = async () => await m.Run<TestExceptionJob, TestJobOutput>(jobInput, cToken);

        // Act
        var actual = await Record.ExceptionAsync(action);

        /*
         * Assert
         */
        // Make sure the error was broadcast
        var error = ScopedSingleInstanceJobManager.JobState.Error();
        Mock.Get(mockCommService).Verify(x => x.Broadcast(jobName, error, cToken), Times.Once);
        // Make sure the exception details were logged
        Mock.Get(mockLogger).Verify(x => x.Log(LogLevel.Error,
                It.IsAny<EventId>(),
                It.Is<It.IsAnyType>((o, t) => o.ToString()!.Equals("Job TestExceptionJob failed")),
                TestExceptionJob.JobException,
                It.IsAny<Func<It.IsAnyType, Exception?, string>>()
            ),
            Times.Once);
        // Make sure the logs indicated the job did not finish
        Mock.Get(mockLogger).Verify(x => x.Log(LogLevel.Information,
                It.IsAny<EventId>(),
                It.Is<It.IsAnyType>((o, t) => o.ToString()!.StartsWith("Failed job TestExceptionJob")),
                null,
                It.IsAny<Func<It.IsAnyType, Exception?, string>>()
            ),
            Times.Once);
        // Make sure the exception was thrown
        Assert.NotNull(actual);
        Assert.Equal(TestExceptionJob.JobException, actual);
    }

    [Fact]
    public async Task Run_JobInput_RunsAndTracksState()
    {
        // Arrange
        var cToken = CancellationToken.None;
        const int jobDurationMs = 100;
        var tcs = new TaskCompletionSource<TestJobOutput>();
        var jobInput = new TestJobInput("input1");
        var job = new TestJob(jobDurationMs, tcs);
        var jobSchedules = new List<JobSchedule>()
        {
            new JobSchedule(job.GetType(), jobInput, TimeSpan.FromMinutes(1))
        };

        var (stubServiceScopeFactory, stubServiceScope) = MockScope(new List<IJob>() { job });
        var mockCommService = Mock.Of<IRealTimeCommService>();
        var mockLogger = Mock.Of<ILogger<ScopedSingleInstanceJobManager>>();

        var m = new ScopedSingleInstanceJobManager(stubServiceScopeFactory.Object, jobSchedules, mockCommService,
            mockLogger);

        // Act
        _ = m.Run<TestJob, TestJobOutput>(jobInput, cToken);
        _ = m.Run<TestJob, TestJobOutput>(jobInput, cToken); // Invoke again to cover already active case
        var actual = await tcs.Task;
        await Task.Delay(jobDurationMs, cToken);

        // Assert
        Assert.Equal("Finished input1. Count: 1", actual.Output);
        Mock.Get(mockLogger).Verify(x => x.Log(LogLevel.Information,
                It.IsAny<EventId>(),
                It.Is<It.IsAnyType>((o, t) => o.ToString()!.StartsWith("Finished job TestJob")),
                null,
                It.IsAny<Func<It.IsAnyType, Exception?, string>>()
            ),
            Times.Once);
    }

    [Fact]
    public async Task RunScheduled_ManagerWithScheduledJobs_RunsJobsOnSchedule()
    {
        // Arrange
        var cToken = CancellationToken.None;
        const int jobDurationMs = 100;
        const int jobIntervalMs = 10;
        var tcs = new TaskCompletionSource<TestJobOutput>();
        var jobInput = new TestJobInput("input1");
        var job = new TestJob(jobDurationMs, tcs);
        var jobSchedules = new List<JobSchedule>()
        {
            new JobSchedule(job.GetType(), jobInput, TimeSpan.FromMilliseconds(jobIntervalMs))
        };

        var (stubServiceScopeFactory, stubServiceScope) = MockScope(new List<IJob>() { job });
        var mockCommService = Mock.Of<IRealTimeCommService>();
        var mockLogger = Mock.Of<ILogger<ScopedSingleInstanceJobManager>>();

        var m = new ScopedSingleInstanceJobManager(stubServiceScopeFactory.Object, jobSchedules, mockCommService,
            mockLogger);

        // Act
        _ = m.RunScheduled(cToken); // Start the scheduled job
        await Task.Delay(1, cToken); // The scheduled job is still running at this point
        _ = m.RunScheduled(cToken); // The job will not run again due to the first invocation
        await Task.Delay(jobDurationMs + 1, cToken); // Wait for the job to finish
        var actual = await job.GetTaskCompletionSource().Task;
        job.ResetTaskCompletionSource();

        await Task.Delay(jobIntervalMs + 1, cToken); // Wait for the schedule interval to pass
        _ = m.RunScheduled(cToken); // Schedule the job again
        await Task.Delay(jobDurationMs + 1, cToken); // Wait for the job to finish
        var actual2 = await job.GetTaskCompletionSource().Task;

        // Assert
        Assert.Equal("Finished input1. Count: 1", actual.Output);
        Assert.Equal("Finished input1. Count: 2", actual2.Output);
        Mock.Get(mockLogger).Verify(x => x.Log(LogLevel.Information,
                It.IsAny<EventId>(),
                It.Is<It.IsAnyType>((o, t) => o.ToString()!.StartsWith("Finished job TestJob")),
                null,
                It.IsAny<Func<It.IsAnyType, Exception?, string>>()
            ),
            Times.Exactly(2));
    }

    private static (Mock<IServiceScopeFactory> stubServiceScopeFactory, Mock<IServiceScope> stubServiceScope) MockScope(
        IEnumerable<IJob>? jobs = null)
    {
        var stubServiceScope = new Mock<IServiceScope>();

        var stubServiceScopeFactory = new Mock<IServiceScopeFactory>();
        stubServiceScopeFactory.Setup(x => x.CreateScope())
            .Returns(stubServiceScope.Object);

        if (jobs == null)
        {
            return (stubServiceScopeFactory, stubServiceScope);
        }

        foreach (var job in jobs)
        {
            stubServiceScope.Setup(x => x.ServiceProvider.GetService(job.GetType()))
                .Returns(job);
        }

        return (stubServiceScopeFactory, stubServiceScope);
    }
}