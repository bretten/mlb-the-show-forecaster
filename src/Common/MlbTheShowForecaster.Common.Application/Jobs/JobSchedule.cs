namespace com.brettnamba.MlbTheShowForecaster.Common.Application.Jobs;

/// <summary>
/// Represents a schedule for a job
/// </summary>
/// <param name="JobType">The job type</param>
/// <param name="JobInput">The job input <see cref="IJobInput"/></param>
/// <param name="Interval">The scheduled interval the job will run on</param>
public sealed record JobSchedule(Type JobType, IJobInput JobInput, TimeSpan Interval)
{
    /// <summary>
    /// The last run of the job
    /// </summary>
    public DateTime LastRun { get; set; }
};