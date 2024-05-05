using com.brettnamba.MlbTheShowForecaster.Common.Application.Cqrs;
using com.brettnamba.MlbTheShowForecaster.Common.Domain.SeedWork;
using com.brettnamba.MlbTheShowForecaster.PlayerStatus.Application.Commands.UpdatePlayer.Exceptions;
using com.brettnamba.MlbTheShowForecaster.PlayerStatus.Domain.Players.Entities;
using com.brettnamba.MlbTheShowForecaster.PlayerStatus.Domain.Players.Repositories;
using com.brettnamba.MlbTheShowForecaster.PlayerStatus.Domain.Players.ValueObjects;
using com.brettnamba.MlbTheShowForecaster.PlayerStatus.Domain.Teams.ValueObjects;

namespace com.brettnamba.MlbTheShowForecaster.PlayerStatus.Application.Commands.UpdatePlayer;

/// <summary>
/// Handles a <see cref="UpdatePlayerCommand"/>
///
/// <para>Updates an existing <see cref="Player"/> by applying any status changes that the player has had
/// since the past update. All status updates are bundled as a single unit of work</para>
/// </summary>
internal sealed class UpdatePlayerCommandHandler : ICommandHandler<UpdatePlayerCommand>
{
    /// <summary>
    /// The <see cref="Player"/> repository
    /// </summary>
    private readonly IPlayerRepository _playerRepository;

    /// <summary>
    /// The unit of work that bundles all status updates for a <see cref="Player"/>
    /// </summary>
    private readonly IUnitOfWork<Player> _unitOfWork;

    /// <summary>
    /// Constructor
    /// </summary>
    /// <param name="playerRepository">The <see cref="Player"/> repository</param>
    /// <param name="unitOfWork">The unit of work that bundles all status updates for a <see cref="Player"/></param>
    public UpdatePlayerCommandHandler(IPlayerRepository playerRepository, IUnitOfWork<Player> unitOfWork)
    {
        _playerRepository = playerRepository;
        _unitOfWork = unitOfWork;
    }

    /// <summary>
    /// Handles the <see cref="UpdatePlayerCommand"/>
    /// </summary>
    /// <param name="command">The command</param>
    /// <param name="cancellationToken">A <see cref="CancellationToken" /> to observe while waiting for the task to complete</param>
    /// <returns>The completed task</returns>
    public async Task Handle(UpdatePlayerCommand command, CancellationToken cancellationToken = default)
    {
        var player = command.Player;
        var playerStatusChanges = command.PlayerStatusChanges;

        UpdateActiveStatus(ref player, playerStatusChanges);
        UpdateTeamStatus(ref player, playerStatusChanges);

        await _playerRepository.Update(player);

        await _unitOfWork.CommitAsync(cancellationToken);
    }

    /// <summary>
    /// Checks if a <see cref="Player"/>'s active status has changed and if so, mutates the Player
    /// </summary>
    /// <param name="player">The <see cref="Player"/></param>
    /// <param name="statusChanges">The status changes</param>
    private void UpdateActiveStatus(ref Player player, PlayerStatusChanges statusChanges)
    {
        if (statusChanges.Activated)
        {
            player.Activate();
        }
        else if (statusChanges.Inactivated)
        {
            player.Inactivate();
        }
    }

    /// <summary>
    /// Checks if a <see cref="Player"/>'s team has changed. If so, the <see cref="Player"/> will be mutated
    /// </summary>
    /// <param name="player">The <see cref="Player"/></param>
    /// <param name="statusChanges">The status changes</param>
    /// <exception cref="MissingTeamContractSigningException">Thrown when there is no <see cref="Team"/> when signing a contract</exception>
    private void UpdateTeamStatus(ref Player player, PlayerStatusChanges statusChanges)
    {
        if (statusChanges.EnteredFreeAgency)
        {
            player.EnterFreeAgency();
        }
        else if (statusChanges.SignedContractWithTeam)
        {
            player.SignContractWithTeam(statusChanges.NewTeam ??
                                        throw new MissingTeamContractSigningException(
                                            $"No team specified when signing contract for {player.MlbId.Value} {player.FirstName} {player.LastName}"));
        }
    }
}