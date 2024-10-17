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
    /// <param name="logger">Logger</param>
    public SingleInstanceJobManager(IEnumerable<IJob> availableJobs, ILogger<SingleInstanceJobManager> logger)
    {
        _availableJobs = availableJobs.ToDictionary(job => job.GetType());
        _logger = logger;
    }

    /// <inheritdoc />
    public async Task<TOut> Run<T, TOut>(IJobInput input, CancellationToken cancellationToken = default)
        where T : IJob
        where TOut : IJobOutput
    {
        // Only one instance of a job type + input can run at a single time
        var jobExecution = new JobExecution(typeof(T), input);

        // The job is already running if the job and input exists in the active jobs
        if (ActiveJobs.TryGetValue(jobExecution, out var existingTcs))
        {
            _logger.LogInformation($"Job {typeof(T).Name} is already running");

            return (TOut)await existingTcs.Task;
        }

        // Indicate that the job is in progress by updating the active jobs
        var tcs = new TaskCompletionSource<IJobOutput>();
        if (!ActiveJobs.TryAdd(jobExecution, tcs))
        {
            // This handles the case where multiple threads try to add the same job at the same time
            return (TOut)await ActiveJobs[jobExecution].Task;
        }

        try
        {
            // await _realTimeBroadcastCallback(jobId, "Start");
            // await _realTimeBroadcastCallback(jobId, "In Progress");

            _logger.LogInformation($"Starting job {typeof(T).Name}");
            var job = _availableJobs[typeof(T)];
            var result = await job.Execute(input, cancellationToken);

            // When the job has finished, update the TaskCompletionSource result
            tcs.SetResult(result);

            // await _realTimeBroadcastCallback(jobId, "Finished");
            // await _realTimeBroadcastCallback(jobId, result);
            // await _realTimeBroadcastCallback(jobId, "Ready");
        }
        catch (Exception e)
        {
            _logger.LogError($"Job {typeof(T).Name} failed", e);
            //await _realTimeBroadcastCallback(jobId, "Error");
        }

        // The job is done and can be started again
        _logger.LogInformation($"Finished job {typeof(T).Name}");
        ActiveJobs.TryRemove(jobExecution, out _);

        return (TOut)await tcs.Task;
    }

    /// <summary>
    /// Represents an execution of a job
    /// </summary>
    private sealed record JobExecution(Type JobType, IJobInput JobInput);
}