using com.brettnamba.MlbTheShowForecaster.GameCards.Domain.Cards.Entities;
using com.brettnamba.MlbTheShowForecaster.GameCards.Domain.Cards.ValueObjects.PlayerCards;
using Microsoft.EntityFrameworkCore;

namespace com.brettnamba.MlbTheShowForecaster.GameCards.Infrastructure.Cards.EntityFrameworkCore;

/// <summary>
/// DB context for <see cref="PlayerCard"/>
/// </summary>
public sealed class CardsDbContext : DbContext
{
    /// <summary>
    /// Constructor
    /// </summary>
    /// <param name="options">DB Context options</param>
    public CardsDbContext(DbContextOptions<CardsDbContext> options) : base(options)
    {
    }

    /// <summary>
    /// DB set for <see cref="PlayerCard"/>
    /// </summary>
    public DbSet<PlayerCard> PlayerCards { get; private init; } = null!;

    /// <summary>
    /// DB set for <see cref="PlayerCardHistoricalRating"/>
    /// </summary>
    public DbSet<PlayerCardHistoricalRating> PlayerCardHistoricalRatings { get; private init; } = null!;

    /// <summary>
    /// Returns the <see cref="PlayerCard"/> DB set with <see cref="PlayerCardHistoricalRatings"/>s included
    /// </summary>
    /// <returns><see cref="IQueryable"/> for <see cref="PlayerCard"/></returns>
    public IQueryable<PlayerCard> PlayerCardsWithHistoricalRatings()
    {
        return PlayerCards
            .Include("_historicalRatings");
    }

    /// <summary>
    /// Model configuration for <see cref="PlayerCard"/>
    /// </summary>
    /// <param name="modelBuilder">Model builder</param>
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.HasDefaultSchema(Constants.Schema);
        modelBuilder.ApplyConfiguration(new PlayerCardEntityTypeConfiguration());
        modelBuilder.ApplyConfiguration(new PlayerCardHistoricalRatingEntityTypeConfiguration());
    }
}