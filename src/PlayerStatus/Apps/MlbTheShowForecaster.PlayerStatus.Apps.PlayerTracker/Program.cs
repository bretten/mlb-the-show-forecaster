using System.Diagnostics;
using System.Reflection;
using com.brettnamba.MlbTheShowForecaster.PlayerStatus.Apps.PlayerTracker;

var app = AppBuilder.CreateApp(args);
Console.WriteLine($"Running {FileVersionInfo.GetVersionInfo(Assembly.GetEntryAssembly()!.Location).ProductVersion}");

await app.RunAsync();

Console.ReadLine();