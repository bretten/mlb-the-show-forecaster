using System.Diagnostics.CodeAnalysis;
using com.brettnamba.MlbTheShowForecaster.PlayerStatus.Domain.Teams.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace com.brettnamba.MlbTheShowForecaster.PlayerStatus.Infrastructure.Players.EntityFrameworkCore;

/// <summary>
/// Design-time configuration for the DB context. Only used when using tools like dotnet ef migrations
/// and never used during application run-time
/// </summary>
[ExcludeFromCodeCoverage]
public sealed class PlayersDesignTimeDbContextFactory : IDesignTimeDbContextFactory<PlayersDbContext>
{
    /// <summary>
    /// Creates the DB context
    /// </summary>
    /// <param name="args">Command-line args</param>
    /// <returns>The DB context</returns>
    public PlayersDbContext CreateDbContext(string[] args)
    {
        if (args.Length < 1)
        {
            throw new ArgumentException("Please specify the connection string as the first argument");
        }

        var optionsBuilder = new DbContextOptionsBuilder<PlayersDbContext>();
        optionsBuilder.UseNpgsql(args[0],
            builder => { builder.MigrationsHistoryTable(Constants.MigrationsTable, Constants.Schema); });
        return new PlayersDbContext(optionsBuilder.Options, new TeamProvider());
    }
}