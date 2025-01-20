using System.Globalization;
using System.Reflection;
using com.brettnamba.MlbTheShowForecaster.Common.Application.Jobs;
using com.brettnamba.MlbTheShowForecaster.Common.Application.Jobs.Io;
using com.brettnamba.MlbTheShowForecaster.Common.Application.RealTime;
using com.brettnamba.MlbTheShowForecaster.Common.Domain.ValueObjects;
using com.brettnamba.MlbTheShowForecaster.Common.Infrastructure.Configuration;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Logging;

namespace com.brettnamba.MlbTheShowForecaster.Common.Infrastructure.Jobs;

/// <summary>
/// Registers dependencies for <see cref="IJobManager"/>
/// </summary>
public static class Dependencies
{
    /// <summary>
    /// Registers the JobManager module
    /// </summary>
    /// <param name="services">The registered services</param>
    /// <param name="config">Configuration for the job manager</param>
    public static void AddJobManager(this IServiceCollection services, IConfiguration config)
    {
        services.TryAddSingleton<IJobManager>(sp =>
        {
            // Get the job schedules
            var jobSchedules = CreateJobSchedules(config);

            var scopeFactory = sp.GetRequiredService<IServiceScopeFactory>();
            var commService = sp.GetRequiredService<IRealTimeCommService>();
            var logger = sp.GetRequiredService<ILogger<ScopedSingleInstanceJobManager>>();
            return new ScopedSingleInstanceJobManager(scopeFactory, jobSchedules, commService, logger);
        });
    }

    /// <summary>
    /// Creates the job schedules
    /// </summary>
    private static List<JobSchedule> CreateJobSchedules(IConfiguration config)
    {
        // Get jobs from the assembly
        var assembly = Assembly.GetEntryAssembly()!;
        var jobTypes = assembly.GetTypes()
            .Where(t => typeof(IJob).IsAssignableFrom(t) && t.IsClass);
        var configuredSchedules = config.GetRequiredValue<string[]>("Jobs:Schedules")
            .Select(x => x.Split("-")) // [0] is job Type, [1] is interval
            .Select(x => new
            {
                // Job types from the current assembly
                Type = jobTypes.FirstOrDefault(y => y.Name == x[0]),
                Interval = ParseInterval(x[1])
            })
            .Where(x => x.Type != null)
            .ToList();

        // Schedule the jobs
        var runOnStartup = config.GetRequiredValue<bool>("Jobs:RunOnStartup");
        var seasons = config.GetRequiredValue<ushort[]>("Jobs:Seasons");
        var jobSchedules = new List<JobSchedule>();
        foreach (var season in seasons)
        {
            var input = new SeasonJobInput(SeasonYear.Create(season));
            foreach (var interval in configuredSchedules)
            {
                var job = new JobSchedule(JobType: interval.Type!, input, interval.Interval);
                job.LastRun = runOnStartup ? DateTime.MinValue : DateTime.UtcNow.Add(interval.Interval);
                jobSchedules.Add(job);
            }
        }

        return jobSchedules;
    }

    /// <summary>
    /// How often the JobManager should run
    /// </summary>
    /// <param name="config">Configuration for the job manager</param>
    /// <returns>The interval the JobManager runs on</returns>
    public static TimeSpan JobManagerInterval(IConfiguration config) =>
        ParseInterval(config.GetRequiredValue<string>("Jobs:ManagerInterval"));

    /// <summary>
    /// The JobManager work that runs the scheduled jobs
    /// </summary>
    public static readonly Func<IJobManager, IServiceProvider, CancellationToken, Task>
        JobManagerWork = async (jobManager, sp, ct) => { await jobManager.RunScheduled(ct); };

    private static TimeSpan ParseInterval(string interval)
    {
        return TimeSpan.ParseExact(interval, "g", CultureInfo.InvariantCulture);
    }
}