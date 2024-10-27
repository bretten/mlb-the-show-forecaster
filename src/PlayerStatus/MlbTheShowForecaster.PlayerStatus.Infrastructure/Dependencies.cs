using System.Reflection;
using System.Text.Json;
using System.Text.Json.Serialization;
using com.brettnamba.MlbTheShowForecaster.Common.Domain.SeedWork;
using com.brettnamba.MlbTheShowForecaster.Common.Infrastructure.Configuration;
using com.brettnamba.MlbTheShowForecaster.Common.Infrastructure.Cqrs.MediatR;
using com.brettnamba.MlbTheShowForecaster.Common.Infrastructure.EntityFrameworkCore;
using com.brettnamba.MlbTheShowForecaster.ExternalApis.MlbApi;
using com.brettnamba.MlbTheShowForecaster.ExternalApis.MlbApi.Fakes;
using com.brettnamba.MlbTheShowForecaster.PlayerStatus.Application.Dtos.Mapping;
using com.brettnamba.MlbTheShowForecaster.PlayerStatus.Application.Services;
using com.brettnamba.MlbTheShowForecaster.PlayerStatus.Domain;
using com.brettnamba.MlbTheShowForecaster.PlayerStatus.Domain.Players.Repositories;
using com.brettnamba.MlbTheShowForecaster.PlayerStatus.Domain.Players.Services;
using com.brettnamba.MlbTheShowForecaster.PlayerStatus.Domain.Teams.Services;
using com.brettnamba.MlbTheShowForecaster.PlayerStatus.Infrastructure.Mapping;
using com.brettnamba.MlbTheShowForecaster.PlayerStatus.Infrastructure.Players.EntityFrameworkCore;
using com.brettnamba.MlbTheShowForecaster.PlayerStatus.Infrastructure.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Refit;

namespace com.brettnamba.MlbTheShowForecaster.PlayerStatus.Infrastructure;

/// <summary>
/// Registers dependencies for PlayerStatus
/// </summary>
public static class Dependencies
{
    /// <summary>
    /// Configuration keys
    /// </summary>
    public static class ConfigKeys
    {
        /// <summary>
        /// Demo mode key
        /// </summary>
        public const string DemoMode = "DemoMode";

        /// <summary>
        /// Players connection string key
        /// </summary>
        public const string PlayersConnection = "Players";

        /// <summary>
        /// MLB API base address config key
        /// </summary>
        public const string MlbApiBaseAddress = "Api:Mlb:BaseAddress";
    }

    /// <summary>
    /// Registers PlayerStatus domain mapping
    /// </summary>
    /// <param name="services">The <see cref="IServiceCollection"/> to add services to</param>
    public static void AddPlayerStatusMapping(this IServiceCollection services)
    {
        services.TryAddSingleton<IPlayerMapper, PlayerMapper>();
    }

    /// <summary>
    /// Registers Player <see cref="TeamProvider"/>
    /// </summary>
    /// <param name="services">The <see cref="IServiceCollection"/> to add services to</param>
    public static void AddPlayerTeamProvider(this IServiceCollection services)
    {
        services.TryAddSingleton<ITeamProvider, TeamProvider>();
    }

    /// <summary>
    /// Registers <see cref="PlayerStatusTracker"/>
    /// </summary>
    /// <param name="services">The <see cref="IServiceCollection"/> to add services to</param>
    /// <param name="config">Configuration for MLB API</param>
    public static void AddPlayerStatusTracker(this IServiceCollection services, IConfiguration config)
    {
        services.AddMediatRCqrs(new List<Assembly>() { typeof(IPlayerStatusTracker).Assembly });

        services.AddMlbAPi(config);
        services.AddPlayerTeamProvider();
        services.TryAddSingleton<IMlbApiPlayerMapper, MlbApiPlayerMapper>();
        services.TryAddSingleton<IPlayerStatusChangeDetector, PlayerStatusChangeDetector>();
        services.TryAddTransient<IPlayerRoster, MlbApiPlayerRoster>();
        services.TryAddTransient<IPlayerStatusTracker, PlayerStatusTracker>();
    }

    /// <summary>
    /// Registers PlayerStatus EntityFrameworkCore dependencies
    /// </summary>
    /// <param name="services">The <see cref="IServiceCollection"/> to add services to</param>
    /// <param name="config">Configuration with connection strings</param>
    public static void AddPlayerStatusEntityFrameworkCoreRepositories(this IServiceCollection services,
        IConfiguration config)
    {
        services.AddPlayerTeamProvider();

        services.AddDbContext<PlayersDbContext>(optionsBuilder =>
        {
            optionsBuilder.UseNpgsql(config.GetRequiredConnectionString(ConfigKeys.PlayersConnection));
        });
        services.AddTransient<IPlayerRepository, EntityFrameworkCorePlayerRepository>();
        services.AddTransient<IUnitOfWork<IPlayerWork>, UnitOfWork<PlayersDbContext>>();
    }

    /// <summary>
    /// Registers <see cref="IPlayerSearchService"/>
    /// </summary>
    /// <param name="services">The <see cref="IServiceCollection"/> to add services to</param>
    /// <param name="config">Configuration with connection strings</param>
    public static void AddPlayerSearchService(this IServiceCollection services, IConfiguration config)
    {
        services.AddPlayerTeamProvider();
        services.AddPlayerStatusEntityFrameworkCoreRepositories(config);
        services.TryAddTransient<IPlayerSearchService, PlayerSearchService>();
    }

    /// <summary>
    /// Registers the MLB API
    /// </summary>
    /// <param name="services">The <see cref="IServiceCollection"/> to add services to</param>
    /// <param name="config">Configuration for MLB API</param>
    private static void AddMlbAPi(this IServiceCollection services, IConfiguration config)
    {
        if (config.GetRequiredValue<bool>(ConfigKeys.DemoMode))
        {
            services.AddSingleton<IMlbApi, FakeMlbApi>();
            return;
        }

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