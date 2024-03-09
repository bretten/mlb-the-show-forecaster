using com.brettnamba.MlbTheShowForecaster.Performance.Domain.PlayerSeasons.Entities;
using com.brettnamba.MlbTheShowForecaster.Performance.Domain.PlayerSeasons.ValueObjects;
using Microsoft.EntityFrameworkCore;

namespace com.brettnamba.MlbTheShowForecaster.Performance.Infrastructure.PlayerSeasons.EntityFramework;

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
    public DbSet<PlayerStatsBySeason> PlayerStatsBySeasons { get; } = null!;

    /// <summary>
    /// DB set for <see cref="PlayerBattingStatsByGame"/>
    /// </summary>
    public DbSet<PlayerBattingStatsByGame> PlayerBattingStatsByGames { get; } = null!;

    /// <summary>
    /// DB set for <see cref="PlayerPitchingStatsByGame"/>
    /// </summary>
    public DbSet<PlayerPitchingStatsByGame> PlayerPitchingStatsByGames { get; } = null!;

    /// <summary>
    /// DB set for <see cref="PlayerFieldingStatsByGame"/>
    /// </summary>
    public DbSet<PlayerFieldingStatsByGame> PlayerFieldingStatsByGames { get; } = null!;

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