using com.brettnamba.MlbTheShowForecaster.Common.Application.Cqrs;
using com.brettnamba.MlbTheShowForecaster.Common.Domain.SeedWork;
using com.brettnamba.MlbTheShowForecaster.PlayerStatus.Application.Dtos.Mapping;
using com.brettnamba.MlbTheShowForecaster.PlayerStatus.Domain;
using com.brettnamba.MlbTheShowForecaster.PlayerStatus.Domain.Players.Entities;
using com.brettnamba.MlbTheShowForecaster.PlayerStatus.Domain.Players.Repositories;
using com.brettnamba.MlbTheShowForecaster.PlayerStatus.Domain.Teams.Services;

namespace com.brettnamba.MlbTheShowForecaster.PlayerStatus.Application.Commands.CreatePlayer;

/// <summary>
/// Handles a <see cref="CreatePlayerCommand"/>
///
/// <para>Creates a new <see cref="Player"/> by adding it to the repository and wrapping the whole
/// command as a single unit of work</para>
/// </summary>
internal sealed class CreatePlayerCommandHandler : ICommandHandler<CreatePlayerCommand>
{
    /// <summary>
    /// The <see cref="Player"/> repository
    /// </summary>
    private readonly IPlayerRepository _playerRepository;

    /// <summary>
    /// The unit of work that defines all actions for creating a <see cref="Player"/>
    /// </summary>
    private readonly IUnitOfWork<IPlayerWork> _unitOfWork;

    /// <summary>
    /// Mapper that maps the player's status to a <see cref="Player"/>
    /// </summary>
    private readonly IPlayerMapper _playerMapper;

    /// <summary>
    /// Provides information on teams
    /// </summary>
    private readonly ITeamProvider _teamProvider;

    /// <summary>
    /// Constructor
    /// </summary>
    /// <param name="playerRepository">The <see cref="Player"/> repository</param>
    /// <param name="unitOfWork">The unit of work that defines all actions for creating a <see cref="Player"/></param>
    /// <param name="playerMapper">Mapper that maps the player's status to a <see cref="Player"/></param>
    /// <param name="teamProvider">Provides information on teams</param>
    public CreatePlayerCommandHandler(IPlayerRepository playerRepository, IUnitOfWork<IPlayerWork> unitOfWork,
        IPlayerMapper playerMapper, ITeamProvider teamProvider)
    {
        _playerRepository = playerRepository;
        _unitOfWork = unitOfWork;
        _playerMapper = playerMapper;
        _teamProvider = teamProvider;
    }

    /// <summary>
    /// Handles the <see cref="CreatePlayerCommand"/>
    /// </summary>
    /// <param name="command">The command</param>
    /// <param name="cancellationToken">A <see cref="CancellationToken" /> to observe while waiting for the task to complete</param>
    /// <returns>The completed task</returns>
    public async Task Handle(CreatePlayerCommand command, CancellationToken cancellationToken = default)
    {
        var player = _playerMapper.Map(command.RosterEntry);

        // The player is new, so they are being activated and signing with a team
        player.Activate();
        player.SignContractWithTeam(_teamProvider.GetBy(command.RosterEntry.CurrentTeamMlbId));

        await _playerRepository.Add(player);

        await _unitOfWork.CommitAsync(cancellationToken);
    }
}