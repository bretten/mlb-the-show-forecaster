using com.brettnamba.MlbTheShowForecaster.Common.Application.Cqrs;
using com.brettnamba.MlbTheShowForecaster.Common.Domain.SeedWork;
using com.brettnamba.MlbTheShowForecaster.PlayerStatus.Application.Dtos.Mapping;
using com.brettnamba.MlbTheShowForecaster.PlayerStatus.Domain;
using com.brettnamba.MlbTheShowForecaster.PlayerStatus.Domain.Players.Entities;
using com.brettnamba.MlbTheShowForecaster.PlayerStatus.Domain.Players.Repositories;

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
    /// The unit of work that defines all actions for creating a <see cref="Player"/>
    /// </summary>
    private readonly IUnitOfWork<IPlayerWork> _unitOfWork;

    /// <summary>
    /// Mapper that maps the player's status to a <see cref="Player"/>
    /// </summary>
    private readonly IPlayerMapper _playerMapper;

    /// <summary>
    /// The <see cref="Player"/> repository
    /// </summary>
    private readonly IPlayerRepository _playerRepository;

    /// <summary>
    /// Constructor
    /// </summary>
    /// <param name="unitOfWork">The unit of work that defines all actions for creating a <see cref="Player"/></param>
    /// <param name="playerMapper">Mapper that maps the player's status to a <see cref="Player"/></param>
    public CreatePlayerCommandHandler(IUnitOfWork<IPlayerWork> unitOfWork, IPlayerMapper playerMapper)
    {
        _unitOfWork = unitOfWork;
        _playerMapper = playerMapper;
        _playerRepository = unitOfWork.GetContributor<IPlayerRepository>();
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

        await _playerRepository.Add(player);

        await _unitOfWork.CommitAsync(cancellationToken);
    }
}