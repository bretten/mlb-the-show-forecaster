using System.Collections.Concurrent;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics;
using com.brettnamba.MlbTheShowForecaster.Common.Application.RealTime;
using com.brettnamba.MlbTheShowForecaster.Common.Extensions;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace com.brettnamba.MlbTheShowForecaster.Common.Application.Jobs;

/// <summary>
/// <see cref="IJobManager"/> that allows only one combination of <see cref="IJob"/> and <see cref="IJobInput"/> to be
/// executing at any given time. Each job execution is in its own service scope
/// </summary>
public sealed class ScopedSingleInstanceJobManager : IJobManager
{
    /// <summary>
    /// <see cref="IServiceScopeFactory"/> that will resolve jobs within their own scope
    /// </summary>
    private readonly IServiceScopeFactory _serviceScopeFactory;

    /// <summary>
    /// Schedules for the available jobs
    /// </summary>
    private readonly IReadOnlyList<JobSchedule> _jobSchedules;

    /// <summary>
    /// Provides real time job status updates to clients
    /// </summary>
    private readonly IRealTimeCommService _commService;

    /// <summary>
    /// Logger
    /// </summary>
    private readonly ILogger<ScopedSingleInstanceJobManager> _logger;

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
    /// <param name="serviceScopeFactory"><see cref="IServiceScopeFactory"/> that will resolve jobs within their own scope</param>
    /// <param name="schedules">Schedules for the available jobs</param>
    /// <param name="commService">Provides real time job status updates to clients</param>
    /// <param name="logger">Logger</param>
    public ScopedSingleInstanceJobManager(IServiceScopeFactory serviceScopeFactory, IEnumerable<JobSchedule> schedules,
        IRealTimeCommService commService, ILogger<ScopedSingleInstanceJobManager> logger)
    {
        _serviceScopeFactory = serviceScopeFactory;
        _jobSchedules = schedules.ToList().AsReadOnly();
        _commService = commService;
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

        var stopwatch = new Stopwatch();
        try
        {
            stopwatch.Start();
            _logger.LogInformation($"Starting job {jobName} at {DateTime.Now}");
            await _commService.Broadcast(jobName, JobState.Start(), cancellationToken);

            // Resolve the job
            var scope = _serviceScopeFactory.CreateScope();
            var job = scope.ServiceProvider.GetRequiredService(jobType) as IJob;

            // Run the job
            var result = await job!.Execute(input, cancellationToken);
            await BroadcastProgress(jobExecution, cancellationToken);

            // When the job has finished, update the TaskCompletionSource result
            tcs.SetResult(result);
            UpdateLastRun(jobExecution);

            await _commService.Broadcast(jobName, JobState.Done(data: result), cancellationToken);
        }
        catch (Exception e)
        {
            _logger.LogError(e, $"Job {jobName} failed");
            await _commService.Broadcast(jobName, JobState.Error(), cancellationToken);
            tcs.SetException(e);
        }

        // The job is done and can be started again
        stopwatch.Stop();
        var logVerb = tcs.Task.IsFaulted ? "Failed" : "Finished";
        _logger.LogInformation($"{logVerb} job {jobName} at {DateTime.Now} ({stopwatch.Elapsed.TotalSeconds} s)");
        ActiveJobs.TryRemove(jobExecution, out _);

        return await tcs.Task;
    }

    /// <inheritdoc />
    public async Task RunScheduled(CancellationToken cancellationToken = default)
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

        foreach (var activeJob in ActiveJobs)
        {
            await BroadcastProgress(activeJob.Key, cancellationToken);
        }
    }

    /// <summary>
    /// Updates the last run of the scheduled job
    /// </summary>
    /// <param name="jobExecution">The scheduled job</param>
    private void UpdateLastRun(JobExecution jobExecution)
    {
        var jobSchedule = _jobSchedules.FirstOrDefault(schedule =>
            schedule.JobType == jobExecution.JobType && schedule.JobInput.Equals(jobExecution.JobInput));
        if (jobSchedule != null)
        {
            jobSchedule.LastRun = DateTime.UtcNow;
        }
    }

    /// <summary>
    /// Broadcasts the progress of the job
    /// </summary>
    /// <param name="jobExecution">The running job</param>
    /// <param name="cancellationToken">A <see cref="CancellationToken" /> to observe while waiting for the task to complete</param>
    private async Task BroadcastProgress(JobExecution jobExecution, CancellationToken cancellationToken)
    {
        _logger.LogDebug($"{jobExecution.JobType.Name} in progress...");
        await _commService.Broadcast(jobExecution.JobType.Name, JobState.InProgress(), cancellationToken);
    }

    /// <summary>
    /// Represents an execution of a job
    /// </summary>
    private sealed record JobExecution(Type JobType, IJobInput JobInput);

    /// <summary>
    /// Represents the state of a job
    /// </summary>
    public sealed record JobState
    {
        /// <summary>
        /// The state of the job
        /// </summary>
        private readonly StateType _state;

        /// <summary>
        /// State of the job as a string
        /// </summary>
        public string State => _state.ToString();

        /// <summary>
        /// Message about the state
        /// </summary>
        public string Message { get; }

        /// <summary>
        /// Data about the state
        /// </summary>
        public object? Data { get; }

        private JobState(StateType state, string message, object? data)
        {
            _state = state;
            Message = message;
            Data = data;
        }

        public static JobState Start(string? message = null, object? data = null)
        {
            return new JobState(StateType.Start, message ?? StateType.Start.GetDisplayName(), data);
        }

        public static JobState InProgress(string? message = null, object? data = null)
        {
            return new JobState(StateType.InProgress, message ?? StateType.InProgress.GetDisplayName(), data);
        }

        public static JobState Done(string? message = null, object? data = null)
        {
            return new JobState(StateType.Done, message ?? StateType.Done.GetDisplayName(), data);
        }

        public static JobState Error(string? message = null, object? data = null)
        {
            return new JobState(StateType.Error, message ?? StateType.Error.GetDisplayName(), data);
        }
    }

    /// <summary>
    /// All possible states
    /// </summary>
    private enum StateType
    {
        [Display(Name = "Starting...")] Start,

        [Display(Name = "In progress...")] InProgress,

        [Display(Name = "Finished!")] Done,

        [Display(Name = "The job encountered an error")]
        Error
    }

    /// <summary>
    /// Dispose
    /// </summary>
    public void Dispose()
    {
    }
}