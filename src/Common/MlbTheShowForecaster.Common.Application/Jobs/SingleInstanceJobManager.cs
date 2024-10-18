using System.Collections.Concurrent;
using Microsoft.Extensions.Logging;

namespace com.brettnamba.MlbTheShowForecaster.Common.Application.Jobs;

/// <summary>
/// <see cref="IJobManager"/> that allows only one combination of <see cref="IJob"/> and <see cref="IJobInput"/> to be
/// executing at any given time
/// </summary>
public sealed class SingleInstanceJobManager : IJobManager
{
    /// <summary>
    /// The available jobs
    /// </summary>
    private readonly IReadOnlyDictionary<Type, IJob> _availableJobs;

    /// <summary>
    /// Schedules for the available jobs
    /// </summary>
    private readonly IReadOnlyList<JobSchedule> _jobSchedules;

    /// <summary>
    /// Logger
    /// </summary>
    private readonly ILogger<SingleInstanceJobManager> _logger;

    /// <summary>
    /// Status of the jobs, where the value is a <see cref="TaskCompletionSource{TResult}"/> that will track the result
    /// and state of the underlying task
    ///
    /// This class should be injected as a singleton and there can be multiple points of entry for triggering jobs,
    /// so have a thread safe way of tracking active jobs
    /// </summary>
    private static readonly ConcurrentDictionary<JobExecution, TaskCompletionSource<IJobOutput>> ActiveJobs = new();

    /// <summary>
    /// Constructor
    /// </summary>
    /// <param name="availableJobs">The available jobs</param>
    /// <param name="schedules">Schedules for the available jobs</param>
    /// <param name="logger">Logger</param>
    public SingleInstanceJobManager(IEnumerable<IJob> availableJobs, IEnumerable<JobSchedule> schedules,
        ILogger<SingleInstanceJobManager> logger)
    {
        _availableJobs = availableJobs.ToDictionary(job => job.GetType());
        _jobSchedules = schedules.ToList().AsReadOnly();
        _logger = logger;
    }

    /// <inheritdoc />
    public async Task<TOut> Run<T, TOut>(IJobInput input, CancellationToken cancellationToken = default)
        where T : IJob
        where TOut : IJobOutput
    {
        return (TOut)await Run(typeof(T), input, cancellationToken);
    }

    /// <inheritdoc cref="Run{T,TOut}"/>
    private async Task<IJobOutput> Run(Type jobType, IJobInput input, CancellationToken cancellationToken = default)
    {
        var jobName = jobType.Name;
        // Only one instance of a job type + input can run at a time
        var jobExecution = new JobExecution(jobType, input);

        // The job is already running if the job and input exists in the active jobs
        if (ActiveJobs.TryGetValue(jobExecution, out var existingTcs))
        {
            _logger.LogInformation($"Job {jobName} is already running");

            return await existingTcs.Task;
        }

        // Indicate that the job is in progress by updating the active jobs
        var tcs = new TaskCompletionSource<IJobOutput>();
        if (!ActiveJobs.TryAdd(jobExecution, tcs))
        {
            // This handles the case where multiple threads try to add the same job at the same time
            return await ActiveJobs[jobExecution].Task;
        }

        try
        {
            // await _commService.Broadcast(jobName, "Start");
            // await _commService.Broadcast(jobName, "In Progress");

            _logger.LogInformation($"Starting job {jobName}");
            var job = _availableJobs[jobType];
            var result = await job.Execute(input, cancellationToken);

            // When the job has finished, update the TaskCompletionSource result
            tcs.SetResult(result);
            UpdateLastRun(jobExecution);

            // await _commService.Broadcast(jobName, "Finished");
            // await _commService.Broadcast(jobName, result);
            // await _commService.Broadcast(jobName, "Ready");
        }
        catch (Exception e)
        {
            _logger.LogError(e, $"Job {jobName} failed");
            //await _commService.Broadcast(jobName, "Error");
            tcs.SetException(e);
        }

        // The job is done and can be started again
        _logger.LogInformation($"Finished job {jobName}");
        ActiveJobs.TryRemove(jobExecution, out _);

        return await tcs.Task;
    }

    /// <inheritdoc />
    public Task RunScheduled(CancellationToken cancellationToken = default)
    {
        foreach (var job in _jobSchedules)
        {
            // Only one instance of a job type + input can run at a time
            var jobExecution = new JobExecution(job.JobType, job.JobInput);

            var timeSinceLastRun = DateTime.UtcNow - job.LastRun;
            if (timeSinceLastRun < job.Interval || ActiveJobs.TryGetValue(jobExecution, out _))
            {
                continue;
            }

            // Run the task in the background by discarding the result since it will be tracked by the ActiveJobs collection
            _ = Task.Run(async () => await Run(job.JobType, job.JobInput, cancellationToken), cancellationToken);
        }

        return Task.CompletedTask;
    }

    /// <summary>
    /// Updates the last run of the scheduled job
    /// </summary>
    /// <param name="jobExecution">The scheduled job</param>
    private void UpdateLastRun(JobExecution jobExecution)
    {
        var jobSchedule = _jobSchedules.FirstOrDefault(schedule =>
            schedule.JobType == jobExecution.JobType && schedule.JobInput == jobExecution.JobInput);
        if (jobSchedule != null)
        {
            jobSchedule.LastRun = DateTime.UtcNow;
        }
    }

    /// <summary>
    /// Represents an execution of a job
    /// </summary>
    private sealed record JobExecution(Type JobType, IJobInput JobInput);
}