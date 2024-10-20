using com.brettnamba.MlbTheShowForecaster.Apps.Gateway.SignalR;
using Ocelot.DependencyInjection;
using Ocelot.Middleware;

var builder = WebApplication.CreateBuilder(args);

builder.Configuration.SetBasePath(builder.Environment.ContentRootPath);
builder.Configuration.AddJsonFile("appsettings.json", true, true)
    .AddJsonFile($"appsettings.{builder.Environment.EnvironmentName}.json", true, true)
    .AddJsonFile("ocelot.json")
    .AddEnvironmentVariables();

builder.Services.AddLogging();
builder.Services.AddControllers();
builder.Services.AddSignalR();

// SignalR multiplexer
builder.Services.AddSingleton<SignalRMultiplexer.Options>(sp =>
{
    var interval = TimeSpan.Parse(builder.Configuration["SignalRMultiplexer:Interval"] ??
                                  throw new ArgumentException("Missing SignalR interval"));
    var hubs = builder.Configuration.GetSection("SignalRMultiplexer:RelayedHubs").Get<HashSet<RelayedHub>>() ??
               throw new ArgumentException("Missing Hub relays");
    return new SignalRMultiplexer.Options(interval, hubs);
});
builder.Services.AddHostedService<SignalRMultiplexer>();

// Ocelot should be last
builder.Services.AddOcelot();

var app = builder.Build();

app.UseRouting();

// In order to use both Ocelot and ASP.NET controllers, you need UseEndpoints => MapControllers, not just MapControllers (https://github.com/Burgyn/MMLib.SwaggerForOcelot/issues/277#issuecomment-2004186620)
#pragma warning disable ASP0014
app.UseEndpoints(
    routeBuilder =>
    {
        routeBuilder.MapControllers();
        routeBuilder.MapHub<GatewayHub>("/job-hub");
    });
#pragma warning restore ASP0014

// Needs to be before UseOcelot (https://stackoverflow.com/a/63472914/1251396)
app.UseWebSockets();

// Ocelot should be last
await app.UseOcelot();

app.Run();