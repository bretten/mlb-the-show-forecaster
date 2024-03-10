using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace com.brettnamba.MlbTheShowForecaster.Performance.Infrastructure.PlayerSeasons.EntityFramework;

/// <summary>
/// Design-time configuration for the DB context. Only used when using tools like dotnet ef migrations
/// and never used during application run-time
/// </summary>
public sealed class PlayerSeasonsDesignTimeDbContextFactory : IDesignTimeDbContextFactory<PlayerSeasonsDbContext>
{
    /// <summary>
    /// Creates the DB context
    /// </summary>
    /// <param name="args">Command-line args</param>
    /// <returns>The DB context</returns>
    public PlayerSeasonsDbContext CreateDbContext(string[] args)
    {
        var optionsBuilder = new DbContextOptionsBuilder<PlayerSeasonsDbContext>();
        optionsBuilder.UseNpgsql();
        return new PlayerSeasonsDbContext(optionsBuilder.Options);
    }
}