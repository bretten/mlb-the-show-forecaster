using com.brettnamba.MlbTheShowForecaster.Common.Application.Cqrs;
using com.brettnamba.MlbTheShowForecaster.Common.Domain.SeedWork;
using com.brettnamba.MlbTheShowForecaster.Performance.Application.Dtos;
using com.brettnamba.MlbTheShowForecaster.Performance.Application.Dtos.Mapping;
using com.brettnamba.MlbTheShowForecaster.Performance.Domain;
using com.brettnamba.MlbTheShowForecaster.Performance.Domain.PlayerSeasons.Entities;
using com.brettnamba.MlbTheShowForecaster.Performance.Domain.PlayerSeasons.Repositories;
using com.brettnamba.MlbTheShowForecaster.Performance.Domain.PlayerSeasons.Services;

namespace com.brettnamba.MlbTheShowForecaster.Performance.Application.Commands.CreatePlayerStatsBySeason;

/// <summary>
/// Handles a <see cref="CreatePlayerStatsBySeasonCommand"/> command
///
/// <para>Creates a new <see cref="PlayerStatsBySeason"/> by adding it to the repository and wrapping the whole
/// command as a single unit of work</para>
/// </summary>
internal sealed class CreatePlayerStatsBySeasonCommandHandler : ICommandHandler<CreatePlayerStatsBySeasonCommand>
{
    /// <summary>
    /// The unit of work that encapsulates all actions for creating a <see cref="PlayerStatsBySeason"/>
    /// </summary>
    private readonly IUnitOfWork<IPlayerSeasonWork> _unitOfWork;

    /// <summary>
    /// Maps <see cref="PlayerSeason"/> to other objects
    /// </summary>
    private readonly IPlayerSeasonMapper _playerSeasonMapper;

    /// <summary>
    /// Scorekeeper that logs new games for the season and assesses the player's performance to date
    /// </summary>
    private readonly IPlayerSeasonScorekeeper _playerSeasonScorekeeper;

    /// <summary>
    /// The <see cref="PlayerStatsBySeason"/> repository
    /// </summary>
    private readonly IPlayerStatsBySeasonRepository _playerStatsBySeasonRepository;

    /// <summary>
    /// Constructor
    /// </summary>
    /// <param name="unitOfWork">The unit of work that encapsulates all actions for creating a <see cref="PlayerStatsBySeason"/></param>
    /// <param name="playerSeasonMapper">Maps <see cref="PlayerSeason"/> to other objects</param>
    /// <param name="playerSeasonScorekeeper">Scorekeeper that logs new games for the season and assesses the player's performance to date</param>
    public CreatePlayerStatsBySeasonCommandHandler(IUnitOfWork<IPlayerSeasonWork> unitOfWork,
        IPlayerSeasonMapper playerSeasonMapper, IPlayerSeasonScorekeeper playerSeasonScorekeeper)
    {
        _unitOfWork = unitOfWork;
        _playerSeasonMapper = playerSeasonMapper;
        _playerSeasonScorekeeper = playerSeasonScorekeeper;
        _playerStatsBySeasonRepository = unitOfWork.GetContributor<IPlayerStatsBySeasonRepository>();
    }

    /// <summary>
    /// Handles a <see cref="CreatePlayerStatsBySeasonCommand"/> command
    /// </summary>
    /// <param name="command">The command</param>
    /// <param name="cancellationToken">A <see cref="CancellationToken" /> to observe while waiting for the task to complete</param>
    public async Task Handle(CreatePlayerStatsBySeasonCommand command, CancellationToken cancellationToken = default)
    {
        // The most up-to-date player season stats retrieved from an MLB source
        var playerSeason = command.PlayerSeason;

        // Map the player season
        var playerStatsBySeason = _playerSeasonMapper.Map(playerSeason);

        // Map the stats by games to date to the domain
        var playerBattingStatsByGamesToDate = _playerSeasonMapper.MapBattingGames(playerSeason.GameBattingStats);
        var playerPitchingStatsByGamesToDate = _playerSeasonMapper.MapPitchingGames(playerSeason.GamePitchingStats);
        var playerFieldingStatsByGamesToDate = _playerSeasonMapper.MapFieldingGames(playerSeason.GameFieldingStats);

        // Score the player's season to date
        playerStatsBySeason = _playerSeasonScorekeeper.ScoreSeason(playerStatsBySeason, playerBattingStatsByGamesToDate,
            playerPitchingStatsByGamesToDate, playerFieldingStatsByGamesToDate);

        await _playerStatsBySeasonRepository.Add(playerStatsBySeason);

        await _unitOfWork.CommitAsync(cancellationToken);
    }
}