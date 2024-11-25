using System.Diagnostics.CodeAnalysis;
using System.Text.Json;
using System.Text.Json.Nodes;
using com.brettnamba.MlbTheShowForecaster.Apps.Gateway.GatewayConfig;
using Ocelot.Authorization;
using Ocelot.Configuration.File;
using Ocelot.DependencyInjection;

namespace com.brettnamba.MlbTheShowForecaster.Apps.Gateway.Ocelot;

/// <summary>
/// Ocelot extensions
/// </summary>
[ExcludeFromCodeCoverage]
public static class OcelotExtensions
{
    /// <summary>
    /// Replaces the default <see cref="IClaimsAuthorizer"/> with <see cref="AwsCognitoClaimsAuthorizer"/>
    /// </summary>
    /// <param name="services"><see cref="IServiceCollection"/></param>
    /// <returns>Updated <see cref="IServiceCollection"/></returns>
    public static IServiceCollection AddAwsCognitoClaimsAuthorizer(this IServiceCollection services)
    {
        // Remove the default IClaimsAuthorizer
        var serviceDescriptor = services.First(x => x.ServiceType == typeof(IClaimsAuthorizer));
        services.Remove(serviceDescriptor);

        // Replace the original IClaimsAuthorizer with the AWS Cognito version
        var awsDescriptor = new ServiceDescriptor(serviceDescriptor.ImplementationType!,
            serviceDescriptor.ImplementationType!, serviceDescriptor.Lifetime);
        services.Add(awsDescriptor);

        services.AddTransient<IClaimsAuthorizer, AwsCognitoClaimsAuthorizer>();

        return services;
    }

    /// <summary>
    /// Replaces the scheme, host and port in the ocelot configuration
    /// </summary>
    /// <param name="builder"><see cref="WebApplicationBuilder"/></param>
    /// <param name="gatewayConfig">Contains replacement schemes, hosts and ports</param>
    /// <returns><see cref="WebApplicationBuilder"/></returns>
    /// <exception cref="ArgumentException">Thrown if no routes can be found</exception>
    public static WebApplicationBuilder ConfigureOcelot(this WebApplicationBuilder builder,
        GatewayConfiguration gatewayConfig)
    {
        var ocelotConfig = new FileConfiguration();
        var file = File.ReadAllText(Path.Combine(builder.Environment.ContentRootPath, "ocelot.json"));
        var json = JsonNode.Parse(file)!;

        // Update each route with the scheme, host and port
        var routes = (json["Routes"] as JsonArray)!;
        for (var i = 0; i < routes.Count; i++)
        {
            var route = (routes[i] as JsonObject)!;
            var target = route["Target"] ?? throw new ArgumentException($"No target found for route at {i}");
            var targetApp = gatewayConfig.GetByName(target.GetValue<string>());

            route["DownstreamScheme"] = targetApp.Scheme;
            route["DownstreamHostAndPorts"] = new JsonArray()
            {
                new JsonObject()
                {
                    { "Host", targetApp.Host },
                    { "Port", targetApp.Port },
                }
            };
        }

        // Set the routes
        ocelotConfig.Routes = JsonSerializer.Deserialize<List<FileRoute>>(routes.ToJsonString());

        // Global config
        ocelotConfig.GlobalConfiguration = new FileGlobalConfiguration()
        {
            BaseUrl = builder.Configuration["Urls"]
        };

        // Apply the config
        builder.Configuration.AddOcelot(ocelotConfig,
            Path.Combine(builder.Environment.ContentRootPath, "ocelot_override.json"));

        return builder;
    }
}