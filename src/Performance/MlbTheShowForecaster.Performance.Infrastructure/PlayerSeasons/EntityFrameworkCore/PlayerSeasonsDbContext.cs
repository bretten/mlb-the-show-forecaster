using com.brettnamba.MlbTheShowForecaster.Performance.Domain.PlayerSeasons.Entities;
using Microsoft.EntityFrameworkCore;

namespace com.brettnamba.MlbTheShowForecaster.Performance.Infrastructure.PlayerSeasons.EntityFrameworkCore;

/// <summary>
/// DB context for a player's MLB season stats
/// </summary>
public sealed class PlayerSeasonsDbContext : DbContext
{
    /// <summary>
    /// Constructor
    /// </summary>
    /// <param name="options">DB context options</param>
    public PlayerSeasonsDbContext(DbContextOptions<PlayerSeasonsDbContext> options) : base(options)
    {
    }

    /// <summary>
    /// DB set for <see cref="PlayerStatsBySeason"/>
    /// </summary>
    public DbSet<PlayerStatsBySeason> PlayerStatsBySeasons { get; private init; } = null!;

    /// <summary>
    /// Returns the <see cref="PlayerStatsBySeason"/> DB set with stats by games
    /// </summary>
    /// <returns><see cref="IQueryable"/> for <see cref="PlayerStatsBySeasons"/></returns>
    public IQueryable<PlayerStatsBySeason> PlayerStatsBySeasonsWithGames()
    {
        return PlayerStatsBySeasons
            .Include("_battingStatsByGames")
            .Include("_pitchingStatsByGames")
            .Include("_fieldingStatsByGames");
    }

    /// <summary>
    /// Configures the model for the player season stats domain
    /// </summary>
    /// <param name="modelBuilder">Model builder</param>
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.HasDefaultSchema(Constants.Schema);
        modelBuilder.ApplyConfiguration(new PlayerStatsBySeasonEntityTypeConfiguration());
        modelBuilder.ApplyConfiguration(new PlayerBattingStatsByGameEntityTypeConfiguration());
        modelBuilder.ApplyConfiguration(new PlayerPitchingStatsByGameEntityTypeConfiguration());
        modelBuilder.ApplyConfiguration(new PlayerFieldingStatsByGameEntityTypeConfiguration());
    }
}