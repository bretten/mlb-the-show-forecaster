using System.Diagnostics.CodeAnalysis;
using com.brettnamba.MlbTheShowForecaster.PlayerStatus.Domain.Teams.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace com.brettnamba.MlbTheShowForecaster.PlayerStatus.Infrastructure.Players.EntityFramework;

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
        var optionsBuilder = new DbContextOptionsBuilder<PlayersDbContext>();
        optionsBuilder.UseNpgsql();
        return new PlayersDbContext(optionsBuilder.Options, new TeamProvider());
    }
}