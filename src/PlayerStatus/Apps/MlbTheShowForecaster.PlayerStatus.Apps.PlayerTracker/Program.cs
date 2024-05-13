using com.brettnamba.MlbTheShowForecaster.PlayerStatus.Apps.PlayerTracker;

var host = HostCreator.Build(args);

await host.StartAsync();

Console.ReadLine();