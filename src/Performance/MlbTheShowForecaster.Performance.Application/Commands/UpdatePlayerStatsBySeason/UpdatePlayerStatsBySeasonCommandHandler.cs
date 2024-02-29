using com.brettnamba.MlbTheShowForecaster.Common.Application.Cqrs;
using com.brettnamba.MlbTheShowForecaster.Common.Domain.SeedWork;
using com.brettnamba.MlbTheShowForecaster.Performance.Application.Dtos;
using com.brettnamba.MlbTheShowForecaster.Performance.Application.Dtos.Mapping;
using com.brettnamba.MlbTheShowForecaster.Performance.Domain.PlayerSeasons.Entities;
using com.brettnamba.MlbTheShowForecaster.Performance.Domain.PlayerSeasons.Repositories;

namespace com.brettnamba.MlbTheShowForecaster.Performance.Application.Commands.UpdatePlayerStatsBySeason;

/// <summary>
/// Handles a <see cref="UpdatePlayerStatsBySeasonCommand"/>
///
/// <para>Updates an existing <see cref="PlayerStatsBySeason"/> by logging any new stats for games the player has
/// participated in since the last update. All updates are bundled as a single unit of work</para>
/// </summary>
internal sealed class UpdatePlayerStatsBySeasonCommandHandler : ICommandHandler<UpdatePlayerStatsBySeasonCommand>
{
    /// <summary>
    /// Maps <see cref="PlayerSeason"/> to other objects
    /// </summary>
    private readonly IPlayerSeasonMapper _playerSeasonMapper;

    /// <summary>
    /// The <see cref="PlayerStatsBySeason"/> repository
    /// </summary>
    private readonly IPlayerStatsBySeasonRepository _playerStatsBySeasonRepository;

    /// <summary>
    /// The unit of work that encapsulates all actions for creating a <see cref="PlayerStatsBySeason"/>
    /// </summary>
    private readonly IUnitOfWork _unitOfWork;

    /// <summary>
    /// Constructor
    /// </summary>
    /// <param name="playerSeasonMapper">Maps <see cref="PlayerSeason"/> to other objects</param>
    /// <param name="playerStatsBySeasonRepository">The <see cref="PlayerStatsBySeason"/> repository</param>
    /// <param name="unitOfWork">The unit of work that encapsulates all actions for creating a <see cref="PlayerStatsBySeason"/></param>
    public UpdatePlayerStatsBySeasonCommandHandler(IPlayerSeasonMapper playerSeasonMapper,
        IPlayerStatsBySeasonRepository playerStatsBySeasonRepository, IUnitOfWork unitOfWork)
    {
        _playerSeasonMapper = playerSeasonMapper;
        _playerStatsBySeasonRepository = playerStatsBySeasonRepository;
        _unitOfWork = unitOfWork;
    }

    /// <summary>
    /// Handles a <see cref="UpdatePlayerStatsBySeasonCommand"/>
    /// </summary>
    /// <param name="command">The command</param>
    /// <param name="cancellationToken">A <see cref="CancellationToken" /> to observe while waiting for the task to complete</param>
    public async Task Handle(UpdatePlayerStatsBySeasonCommand command, CancellationToken cancellationToken = default)
    {
        // The player season stats that the system currently has stored
        var playerStatsBySeason = command.PlayerStatsBySeason;
        // The most up-to-date player season stats retrieved from an MLB source
        var playerSeason = command.PlayerSeason;

        // Log new game stats
        LogNewBattingGames(ref playerStatsBySeason, playerSeason.GameBattingStats);
        LogNewPitchingGames(ref playerStatsBySeason, playerSeason.GamePitchingStats);
        LogNewFieldingGames(ref playerStatsBySeason, playerSeason.GameFieldingStats);

        // Update
        await _playerStatsBySeasonRepository.Update(playerStatsBySeason);

        // Persist
        await _unitOfWork.CommitAsync(cancellationToken);
    }

    /// <summary>
    /// Logs new batting games
    /// </summary>
    /// <param name="playerStatsBySeason">The player season stats as it exists in the system currently</param>
    /// <param name="upToDateStats">The most up-to-date player season stats from the external MLB source</param>
    private void LogNewBattingGames(ref PlayerStatsBySeason playerStatsBySeason,
        IEnumerable<PlayerGameBattingStats> upToDateStats)
    {
        // Map the most up-to-date stats to domain entities
        var upToDateStatsByGames = _playerSeasonMapper.MapBattingGames(upToDateStats);

        // The stats stored internally which may not yet be up-to-date
        var previousStatsByGames = playerStatsBySeason.BattingStatsByGamesChronologically;

        // Get new stats that don't exist in the system
        var newStatsByGames = upToDateStatsByGames.Except(previousStatsByGames);

        // Log new games
        foreach (var statsByGame in newStatsByGames)
        {
            playerStatsBySeason.LogBattingGame(statsByGame);
        }
    }

    /// <summary>
    /// Logs new pitching games
    /// </summary>
    /// <param name="playerStatsBySeason">The player season stats as it exists in the system currently</param>
    /// <param name="upToDateStats">The most up-to-date player season stats from the external MLB source</param>
    private void LogNewPitchingGames(ref PlayerStatsBySeason playerStatsBySeason,
        IEnumerable<PlayerGamePitchingStats> upToDateStats)
    {
        // Map the most up-to-date stats to domain entities
        var upToDateStatsByGames = _playerSeasonMapper.MapPitchingGames(upToDateStats);

        // The stats stored internally which may not yet be up-to-date
        var previousStatsByGames = playerStatsBySeason.PitchingStatsByGamesChronologically;

        // Get new stats that don't exist in the system
        var newStatsByGames = upToDateStatsByGames.Except(previousStatsByGames);

        // Log new games
        foreach (var statsByGame in newStatsByGames)
        {
            playerStatsBySeason.LogPitchingGame(statsByGame);
        }
    }

    /// <summary>
    /// Logs new fielding games
    /// </summary>
    /// <param name="playerStatsBySeason">The player season stats as it exists in the system currently</param>
    /// <param name="upToDateStats">The most up-to-date player season stats from the external MLB source</param>
    private void LogNewFieldingGames(ref PlayerStatsBySeason playerStatsBySeason,
        IEnumerable<PlayerGameFieldingStats> upToDateStats)
    {
        // Map the most up-to-date stats to domain entities
        var upToDateStatsByGames = _playerSeasonMapper.MapFieldingGames(upToDateStats);

        // The stats stored internally which may not yet be up-to-date
        var previousStatsByGames = playerStatsBySeason.FieldingStatsByGamesChronologically;

        // Get new stats that don't exist in the system
        var newStatsByGames = upToDateStatsByGames.Except(previousStatsByGames);

        // Log new games
        foreach (var statsByGame in newStatsByGames)
        {
            playerStatsBySeason.LogFieldingGame(statsByGame);
        }
    }
}