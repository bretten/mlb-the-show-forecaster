using com.brettnamba.MlbTheShowForecaster.Common.Application.Cqrs;
using com.brettnamba.MlbTheShowForecaster.Common.DateAndTime;
using com.brettnamba.MlbTheShowForecaster.Common.Domain.SeedWork;
using com.brettnamba.MlbTheShowForecaster.Performance.Application.Dtos;
using com.brettnamba.MlbTheShowForecaster.Performance.Application.Dtos.Mapping;
using com.brettnamba.MlbTheShowForecaster.Performance.Domain;
using com.brettnamba.MlbTheShowForecaster.Performance.Domain.PlayerSeasons.Entities;
using com.brettnamba.MlbTheShowForecaster.Performance.Domain.PlayerSeasons.Repositories;
using com.brettnamba.MlbTheShowForecaster.Performance.Domain.PlayerSeasons.Services;

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
    /// Gets today's date
    /// </summary>
    private readonly ICalendar _calendar;

    /// <summary>
    /// Scorekeeper that logs new games for the season and assesses the player's performance to date
    /// </summary>
    private readonly IPlayerSeasonScorekeeper _playerSeasonScorekeeper;

    /// <summary>
    /// The <see cref="PlayerStatsBySeason"/> repository
    /// </summary>
    private readonly IPlayerStatsBySeasonRepository _playerStatsBySeasonRepository;

    /// <summary>
    /// The unit of work that encapsulates all actions for creating a <see cref="PlayerStatsBySeason"/>
    /// </summary>
    private readonly IUnitOfWork<IPlayerSeasonWork> _unitOfWork;

    /// <summary>
    /// Constructor
    /// </summary>
    /// <param name="playerSeasonMapper">Maps <see cref="PlayerSeason"/> to other objects</param>
    /// <param name="calendar">Gets today's date</param>
    /// <param name="playerSeasonScorekeeper">Scorekeeper that logs new games for the season and assesses the player's performance to date</param>
    /// <param name="playerStatsBySeasonRepository">The <see cref="PlayerStatsBySeason"/> repository</param>
    /// <param name="unitOfWork">The unit of work that encapsulates all actions for creating a <see cref="PlayerStatsBySeason"/></param>
    public UpdatePlayerStatsBySeasonCommandHandler(IPlayerSeasonMapper playerSeasonMapper, ICalendar calendar,
        IPlayerSeasonScorekeeper playerSeasonScorekeeper, IPlayerStatsBySeasonRepository playerStatsBySeasonRepository,
        IUnitOfWork<IPlayerSeasonWork> unitOfWork)
    {
        _playerSeasonMapper = playerSeasonMapper;
        _playerSeasonScorekeeper = playerSeasonScorekeeper;
        _calendar = calendar;
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

        // Map the stats by games to date to the domain
        var playerBattingStatsByGamesToDate = _playerSeasonMapper.MapBattingGames(playerSeason.GameBattingStats);
        var playerPitchingStatsByGamesToDate = _playerSeasonMapper.MapPitchingGames(playerSeason.GamePitchingStats);
        var playerFieldingStatsByGamesToDate = _playerSeasonMapper.MapFieldingGames(playerSeason.GameFieldingStats);

        // Score the player's season to date
        var updatedPlayerStatsBySeason = _playerSeasonScorekeeper.ScoreSeason(playerStatsBySeason, _calendar.Today(),
            playerBattingStatsByGamesToDate, playerPitchingStatsByGamesToDate, playerFieldingStatsByGamesToDate);

        // Update
        await _playerStatsBySeasonRepository.Update(updatedPlayerStatsBySeason);

        // Persist
        await _unitOfWork.CommitAsync(cancellationToken);
    }
}