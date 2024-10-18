using com.brettnamba.MlbTheShowForecaster.Performance.Apps.PerformanceTracker;
using com.brettnamba.MlbTheShowForecaster.Performance.Apps.PerformanceTracker.RealTime;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddControllers();
builder.Services.AddSignalR();

builder.Configuration.AddJsonFile("appsettings.json");
builder.Configuration.AddJsonFile($"appsettings.{builder.Environment.EnvironmentName}.json");

builder.Host.ConfigurePerformanceTracker(args);

var app = builder.Build();

if (app.Environment.IsDevelopment() || app.Environment.EnvironmentName == "Local")
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.MapControllers()
    .WithOpenApi();
app.MapHub<JobHub>("/job-hub");

await app.RunAsync();

Console.ReadLine();