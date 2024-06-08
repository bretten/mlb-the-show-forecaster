using System.Reflection;
using System.Text.Json;
using System.Text.Json.Serialization;
using com.brettnamba.MlbTheShowForecaster.Common.Domain.SeedWork;
using com.brettnamba.MlbTheShowForecaster.Common.Infrastructure.Configuration;
using com.brettnamba.MlbTheShowForecaster.Common.Infrastructure.Cqrs.MediatR;
using com.brettnamba.MlbTheShowForecaster.Common.Infrastructure.EntityFrameworkCore;
using com.brettnamba.MlbTheShowForecaster.ExternalApis.MlbApi;
using com.brettnamba.MlbTheShowForecaster.Performance.Application.Dtos.Mapping;
using com.brettnamba.MlbTheShowForecaster.Performance.Application.Services;
using com.brettnamba.MlbTheShowForecaster.Performance.Domain;
using com.brettnamba.MlbTheShowForecaster.Performance.Domain.PerformanceAssessment.Services;
using com.brettnamba.MlbTheShowForecaster.Performance.Domain.PerformanceAssessment.Services.MinMaxNormalization;
using com.brettnamba.MlbTheShowForecaster.Performance.Domain.PerformanceAssessment.ValueObjects;
using com.brettnamba.MlbTheShowForecaster.Performance.Domain.PlayerSeasons.Repositories;
using com.brettnamba.MlbTheShowForecaster.Performance.Domain.PlayerSeasons.Services;
using com.brettnamba.MlbTheShowForecaster.Performance.Infrastructure.Dtos.Mapping;
using com.brettnamba.MlbTheShowForecaster.Performance.Infrastructure.PlayerSeasons.EntityFrameworkCore;
using com.brettnamba.MlbTheShowForecaster.Performance.Infrastructure.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Refit;

namespace com.brettnamba.MlbTheShowForecaster.Performance.Infrastructure;

/// <summary>
/// Registers dependencies for Performance
/// </summary>
public static class Dependencies
{
    /// <summary>
    /// Configuration keys
    /// </summary>
    public static class ConfigKeys
    {
        /// <summary>
        /// PlayerSeasons connection string key
        /// </summary>
        public const string PlayerSeasonsConnection = "PlayerSeasons";

        /// <summary>
        /// MLB API base address config key
        /// </summary>
        public const string MlbApiBaseAddress = "Api:Mlb:BaseAddress";

        /// <summary>
        /// <see cref="PerformanceScoreComparison"/> threshold config key
        /// </summary>
        public const string ScorePercentageChangeThreshold = "PerformanceAssessment:ScorePercentageChangeThreshold";

        /// <summary>
        /// Section key for min-max normalization batting criteria
        /// </summary>
        public const string MinMaxNormBattingCriteriaSection =
            "PerformanceAssessment:MinMaxNormalization:BattingCriteria";

        /// <summary>
        /// Section key for min-max normalization pitching criteria
        /// </summary>
        public const string MinMaxNormPitchingCriteriaSection =
            "PerformanceAssessment:MinMaxNormalization:PitchingCriteria";

        /// <summary>
        /// Section key for min-max normalization fielding criteria
        /// </summary>
        public const string MinMaxNormFieldingCriteriaSection =
            "PerformanceAssessment:MinMaxNormalization:FieldingCriteria";
    }

    /// <summary>
    /// Registers Performance assessment
    /// </summary>
    /// <param name="services">The <see cref="IServiceCollection"/> to add services to</param>
    /// <param name="config">Configuration for performance assessment</param>
    public static void AddPerformanceAssessment(this IServiceCollection services, IConfiguration config)
    {
        services.AddSingleton(new MinMaxNormalizationCriteria(
            config.GetRequiredValue<decimal>(ConfigKeys.ScorePercentageChangeThreshold),
            config.GetRequiredValue<MinMaxBattingStatCriteria[]>(ConfigKeys.MinMaxNormBattingCriteriaSection),
            config.GetRequiredValue<MinMaxPitchingStatCriteria[]>(ConfigKeys.MinMaxNormPitchingCriteriaSection),
            config.GetRequiredValue<MinMaxFieldingStatCriteria[]>(ConfigKeys.MinMaxNormFieldingCriteriaSection)
        ));
        services.AddSingleton<IPerformanceAssessor, MinMaxNormalizationPerformanceAssessor>();
    }

    /// <summary>
    /// Registers Performance domain mapping
    /// </summary>
    /// <param name="services">The <see cref="IServiceCollection"/> to add services to</param>
    public static void AddPerformanceMapping(this IServiceCollection services)
    {
        services.TryAddSingleton<IPlayerSeasonMapper, PlayerSeasonMapper>();
    }

    /// <summary>
    /// Registers Performance <see cref="PlayerSeasonScorekeeper"/>
    /// </summary>
    /// <param name="services">The <see cref="IServiceCollection"/> to add services to</param>
    public static void AddPerformancePlayerSeasonScorekeeper(this IServiceCollection services)
    {
        services.TryAddSingleton<IPlayerSeasonScorekeeper, PlayerSeasonScorekeeper>();
    }

    /// <summary>
    /// Registers <see cref="PerformanceTracker"/>
    /// </summary>
    /// <param name="services">The <see cref="IServiceCollection"/> to add services to</param>
    /// <param name="config">Configuration for MLB API</param>
    public static void AddPerformanceTracker(this IServiceCollection services, IConfiguration config)
    {
        services.AddMediatRCqrs(new List<Assembly>() { typeof(IPerformanceTracker).Assembly });

        services.AddMlbAPi(config);
        services.TryAddSingleton<IMlbApiPlayerStatsMapper, MlbApiPlayerStatsMapper>();
        services.TryAddTransient<IPlayerStats, MlbApiPlayerStats>();
        services.TryAddTransient<IPerformanceTracker, PerformanceTracker>();
    }

    /// <summary>
    /// Registers Performance EntityFrameworkCore dependencies
    /// </summary>
    /// <param name="services">The <see cref="IServiceCollection"/> to add services to</param>
    /// <param name="config">Configuration with connection strings</param>
    public static void AddPerformanceEntityFrameworkCoreRepositories(this IServiceCollection services,
        IConfiguration config)
    {
        services.AddDbContext<PlayerSeasonsDbContext>(optionsBuilder =>
        {
            optionsBuilder.UseNpgsql(config.GetRequiredConnectionString(ConfigKeys.PlayerSeasonsConnection));
        });
        services.AddTransient<IPlayerStatsBySeasonRepository, EntityFrameworkCorePlayerStatsBySeasonRepository>();
        services.AddTransient<IUnitOfWork<IPlayerSeasonWork>, UnitOfWork<PlayerSeasonsDbContext>>();
    }

    /// <summary>
    /// Registers the MLB API
    /// </summary>
    /// <param name="services">The <see cref="IServiceCollection"/> to add services to</param>
    /// <param name="config">Configuration for MLB API</param>
    private static void AddMlbAPi(this IServiceCollection services, IConfiguration config)
    {
        services.AddRefitClient<IMlbApi>(provider => new RefitSettings()
            {
                ContentSerializer = new SystemTextJsonContentSerializer(new JsonSerializerOptions()
                {
                    Converters = { new JsonStringEnumConverter() }
                })
            })
            .ConfigureHttpClient(client =>
            {
                var baseAddress = config.GetRequiredValue<string>(ConfigKeys.MlbApiBaseAddress);
                client.BaseAddress = new Uri(baseAddress);
            });
    }
}