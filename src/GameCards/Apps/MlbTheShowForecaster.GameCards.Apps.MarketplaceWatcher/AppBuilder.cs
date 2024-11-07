using System.Diagnostics.CodeAnalysis;
using com.brettnamba.MlbTheShowForecaster.GameCards.Apps.MarketplaceWatcher.RealTime;
using com.brettnamba.MlbTheShowForecaster.GameCards.Infrastructure.Cards.EntityFrameworkCore;
using com.brettnamba.MlbTheShowForecaster.GameCards.Infrastructure.Forecasts.EntityFrameworkCore;
using com.brettnamba.MlbTheShowForecaster.GameCards.Infrastructure.Marketplace.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace com.brettnamba.MlbTheShowForecaster.GameCards.Apps.MarketplaceWatcher;

/// <summary>
/// Setup for the whole app
/// </summary>
[ExcludeFromCodeCoverage]
public static class AppBuilder
{
    /// <summary>
    /// Creates the app
    /// </summary>
    /// <param name="args">Application parameters</param>
    /// <returns>The app</returns>
    public static WebApplication CreateApp(string[] args)
    {
        var builder = CreateBuilder(args);

        var app = BuildApp(args, builder);

        return app;
    }

    /// <summary>
    /// Creates the builder
    /// </summary>
    /// <param name="args">Application parameters</param>
    /// <returns>App builder</returns>
    public static WebApplicationBuilder CreateBuilder(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen();

        builder.Services.AddControllers();
        builder.Services.AddSignalR();

        builder.Configuration.AddJsonFile("appsettings.json")
            .AddJsonFile($"appsettings.{builder.Environment.EnvironmentName}.json", true, true)
            .AddEnvironmentVariables();

        if (builder.Environment.IsDevelopment())
        {
            builder.Configuration.AddUserSecrets<Program>(true);
        }

        return builder;
    }

    /// <summary>
    /// Builds the app
    /// </summary>
    /// <param name="args">Application parameters</param>
    /// <param name="builder">The app builder</param>
    /// <returns>The app</returns>
    public static WebApplication BuildApp(string[] args, WebApplicationBuilder builder)
    {
        builder.Host.ConfigureMarketplaceWatcher(args);

        var app = builder.Build();

        if (app.Configuration.GetValue<bool>("RunMigrations"))
        {
            using var scope = app.Services.CreateScope();
            scope.ServiceProvider.GetRequiredService<CardsDbContext>().Database.Migrate();
            scope.ServiceProvider.GetRequiredService<MarketplaceDbContext>().Database.Migrate();
            scope.ServiceProvider.GetRequiredService<ForecastsDbContext>().Database.Migrate();
        }

        if (app.Environment.IsDevelopment() || app.Environment.EnvironmentName == "Local")
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }

        app.MapControllers()
            .WithOpenApi();
        app.MapHub<JobHub>("/job-hub");

        return app;
    }
}