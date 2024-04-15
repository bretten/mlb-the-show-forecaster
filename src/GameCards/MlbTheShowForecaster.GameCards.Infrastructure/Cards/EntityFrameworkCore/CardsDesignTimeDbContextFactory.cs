using System.Diagnostics.CodeAnalysis;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace com.brettnamba.MlbTheShowForecaster.GameCards.Infrastructure.Cards.EntityFrameworkCore;

/// <summary>
/// Design-time configuration for the DB context. Only used when using tools like dotnet ef migrations
/// and never used during application run-time
/// </summary>
[ExcludeFromCodeCoverage]
public sealed class CardsDesignTimeDbContextFactory : IDesignTimeDbContextFactory<CardsDbContext>
{
    /// <summary>
    /// Creates the DB context
    /// </summary>
    /// <param name="args">Command-line args</param>
    /// <returns>The DB context</returns>
    public CardsDbContext CreateDbContext(string[] args)
    {
        var optionsBuilder = new DbContextOptionsBuilder<CardsDbContext>();
        optionsBuilder.UseNpgsql(
            "Server=localhost;Port=54320;Database=mlbcards;Uid=postgres;Pwd=pass12;Search Path=game_cards");
        return new CardsDbContext(optionsBuilder.Options);
    }
}