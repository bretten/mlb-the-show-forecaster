using com.brettnamba.MlbTheShowForecaster.GameCards.Apps.MarketplaceWatcher;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;

var builder = Host.CreateDefaultBuilder(args)
    .ConfigureAppConfiguration((context, configurationBuilder) =>
    {
        configurationBuilder.AddJsonFile("appsettings.json");
        configurationBuilder.AddJsonFile($"appsettings.{context.HostingEnvironment.EnvironmentName}.json");
    })
    .ConfigureMarketplaceWatcher(args);

var host = builder.Build();

await host.StartAsync();

Console.ReadLine();