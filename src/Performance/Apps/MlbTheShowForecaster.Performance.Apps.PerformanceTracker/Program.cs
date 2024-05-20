using com.brettnamba.MlbTheShowForecaster.Performance.Apps.PerformanceTracker;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;

var builder = Host.CreateDefaultBuilder(args)
    .ConfigureAppConfiguration((context, configurationBuilder) =>
    {
        configurationBuilder.AddJsonFile("appsettings.json");
        configurationBuilder.AddJsonFile($"appsettings.{context.HostingEnvironment.EnvironmentName}.json");
    })
    .ConfigurePerformanceTracker(args);

var host = builder.Build();

await host.StartAsync();

Console.ReadLine();