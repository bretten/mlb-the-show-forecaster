using com.brettnamba.MlbTheShowForecaster.PlayerStatus.Domain;
using com.brettnamba.MlbTheShowForecaster.PlayerStatus.Domain.Players.Entities;
using com.brettnamba.MlbTheShowForecaster.PlayerStatus.Domain.Teams.Services;
using Microsoft.EntityFrameworkCore;

namespace com.brettnamba.MlbTheShowForecaster.PlayerStatus.Infrastructure.Players.EntityFramework;

/// <summary>
/// DB context for <see cref="Player"/>
/// </summary>
public sealed class PlayersDbContext : DbContext, IPlayerWork
{
    /// <summary>
    /// Team provider, used to convert a team MLB ID to the corresponding Team value object
    /// </summary>
    private readonly ITeamProvider _teamProvider;

    /// <summary>
    /// Constructor
    /// </summary>
    /// <param name="options">DB Context options</param>
    /// <param name="teamProvider">Team provider, used to convert a team MLB ID to the corresponding Team value object</param>
    public PlayersDbContext(DbContextOptions<PlayersDbContext> options, ITeamProvider teamProvider) : base(options)
    {
        _teamProvider = teamProvider;
    }

    /// <summary>
    /// DB set for <see cref="Player"/>
    /// </summary>
    public DbSet<Player> Players { get; private init; } = null!;

    /// <summary>
    /// Model configuration for <see cref="Player"/>
    /// </summary>
    /// <param name="modelBuilder">Model builder</param>
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.HasDefaultSchema(Constants.Schema);
        modelBuilder.ApplyConfiguration(new PlayerEntityTypeConfiguration(_teamProvider));
    }
}