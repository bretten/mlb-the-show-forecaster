using com.brettnamba.MlbTheShowForecaster.Performance.Apps.PerformanceTracker;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddControllers();

builder.Configuration.AddJsonFile("appsettings.json");
builder.Configuration.AddJsonFile($"appsettings.{builder.Environment.EnvironmentName}.json");

builder.Host.ConfigurePerformanceTracker(args);

var host = builder.Build();

if (host.Environment.IsDevelopment() || host.Environment.EnvironmentName == "Local")
{
    host.UseSwagger();
    host.UseSwaggerUI();
}

host.MapControllers()
    .WithOpenApi();

await host.RunAsync();

Console.ReadLine();