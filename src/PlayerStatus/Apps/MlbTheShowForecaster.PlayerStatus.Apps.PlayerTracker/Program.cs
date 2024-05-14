using com.brettnamba.MlbTheShowForecaster.PlayerStatus.Apps.PlayerTracker;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;

var builder = Host.CreateDefaultBuilder(args)
    .ConfigureAppConfiguration((context, configurationBuilder) =>
    {
        configurationBuilder.AddJsonFile("appsettings.json");
        configurationBuilder.AddJsonFile($"appsettings.{context.HostingEnvironment.EnvironmentName}.json");
    })
    .ConfigurePlayerTracker(args);

var host = builder.Build();

await host.StartAsync();

Console.ReadLine();