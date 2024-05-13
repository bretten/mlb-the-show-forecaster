using com.brettnamba.MlbTheShowForecaster.GameCards.Apps.MarketplaceWatcher;

var host = HostCreator.Build(args);

await host.StartAsync();

Console.ReadLine();