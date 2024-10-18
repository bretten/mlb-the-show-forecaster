using com.brettnamba.MlbTheShowForecaster.Common.Application.Jobs;
using com.brettnamba.MlbTheShowForecaster.Common.Application.Tests.Jobs.TestClasses;
using Microsoft.Extensions.Logging;
using Moq;

namespace com.brettnamba.MlbTheShowForecaster.Common.Application.Tests.Jobs;

public class SingleInstanceJobManagerTests
{
    [Fact]
    public async Task Run_FailedJob_ThrowsException()
    {
        // Arrange
        var cToken = CancellationToken.None;
        var jobInput = new TestJobInput("input1");
        var job = new TestExceptionJob();
        var jobSchedules = new List<JobSchedule>()
        {
            new JobSchedule(job.GetType(), jobInput, TimeSpan.FromMinutes(1))
        };

        var mockLogger = Mock.Of<ILogger<SingleInstanceJobManager>>();

        var jobManager = new SingleInstanceJobManager(new List<IJob>() { job }, jobSchedules, mockLogger);

        var action = async () => await jobManager.Run<TestExceptionJob, TestJobOutput>(jobInput, cToken);

        // Act
        var actual = await Record.ExceptionAsync(action);

        // Assert
        //Mock.Get(mockRealTimeCommService).Verify(x => x.Broadcast(jobName, "Error"), Times.Once);
        Mock.Get(mockLogger).Verify(x => x.Log(LogLevel.Error,
                It.IsAny<EventId>(),
                It.Is<It.IsAnyType>((o, t) => o.ToString()!.Equals("Job TestExceptionJob failed")),
                TestExceptionJob.JobException,
                It.IsAny<Func<It.IsAnyType, Exception?, string>>()
            ),
            Times.Once);
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

        var mockLogger = Mock.Of<ILogger<SingleInstanceJobManager>>();

        var jobManager = new SingleInstanceJobManager(new List<IJob>() { job }, jobSchedules, mockLogger);

        // Act
        _ = jobManager.Run<TestJob, TestJobOutput>(jobInput, cToken);
        _ = jobManager.Run<TestJob, TestJobOutput>(jobInput, cToken); // Invoke again to cover already active case
        var actual = await tcs.Task;

        // Assert
        Assert.Equal("Finished input1. Count: 1", actual.Output);
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

        var mockLogger = Mock.Of<ILogger<SingleInstanceJobManager>>();

        var jobManager = new SingleInstanceJobManager(new List<IJob>() { job }, jobSchedules, mockLogger);

        // Act
        _ = jobManager.RunScheduled(cToken); // Start the scheduled job
        await Task.Delay(1, cToken); // The scheduled job is still running at this point
        _ = jobManager.RunScheduled(cToken); // The job will not run again due to the first invocation
        await Task.Delay(jobDurationMs + 1, cToken); // Wait for the job to finish
        var actual = await job.GetTaskCompletionSource().Task;
        job.ResetTaskCompletionSource();

        await Task.Delay(jobIntervalMs + 1, cToken); // Wait for the schedule interval to pass
        _ = jobManager.RunScheduled(cToken); // Schedule the job again
        await Task.Delay(jobDurationMs + 1, cToken); // Wait for the job to finish
        var actual2 = await job.GetTaskCompletionSource().Task;

        // Assert
        Assert.Equal("Finished input1. Count: 1", actual.Output);
        Assert.Equal("Finished input1. Count: 2", actual2.Output);
    }
}