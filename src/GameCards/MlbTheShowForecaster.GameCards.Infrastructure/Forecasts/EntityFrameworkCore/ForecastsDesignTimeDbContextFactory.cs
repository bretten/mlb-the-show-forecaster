﻿using System.Diagnostics.CodeAnalysis;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace com.brettnamba.MlbTheShowForecaster.GameCards.Infrastructure.Forecasts.EntityFrameworkCore;

/// <summary>
/// Design-time configuration for the DB context. Only used when using tools like dotnet ef migrations
/// and never used during application run-time
/// </summary>
[ExcludeFromCodeCoverage]
public sealed class ForecastsDesignTimeDbContextFactory : IDesignTimeDbContextFactory<ForecastsDbContext>
{
    /// <summary>
    /// Creates the DB context
    /// </summary>
    /// <param name="args">Command-line args</param>
    /// <returns>The DB context</returns>
    public ForecastsDbContext CreateDbContext(string[] args)
    {
        if (args.Length < 1)
        {
            throw new ArgumentException("Please specify the connection string as the first argument");
        }

        var optionsBuilder = new DbContextOptionsBuilder<ForecastsDbContext>();
        optionsBuilder.UseNpgsql(args[0]);
        return new ForecastsDbContext(optionsBuilder.Options);
    }
}