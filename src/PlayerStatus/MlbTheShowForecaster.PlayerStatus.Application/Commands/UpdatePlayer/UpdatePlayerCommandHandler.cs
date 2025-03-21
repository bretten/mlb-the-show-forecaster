﻿using com.brettnamba.MlbTheShowForecaster.Common.Application.Cqrs;
using com.brettnamba.MlbTheShowForecaster.Common.Domain.SeedWork;
using com.brettnamba.MlbTheShowForecaster.Common.Domain.ValueObjects;
using com.brettnamba.MlbTheShowForecaster.PlayerStatus.Application.Commands.UpdatePlayer.Exceptions;
using com.brettnamba.MlbTheShowForecaster.PlayerStatus.Domain;
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
    /// The unit of work that bundles all status updates for a <see cref="Player"/>
    /// </summary>
    private readonly IUnitOfWork<IPlayerWork> _unitOfWork;

    /// <summary>
    /// The <see cref="Player"/> repository
    /// </summary>
    private readonly IPlayerRepository _playerRepository;

    /// <summary>
    /// Constructor
    /// </summary>
    /// <param name="unitOfWork">The unit of work that bundles all status updates for a <see cref="Player"/></param>
    public UpdatePlayerCommandHandler(IUnitOfWork<IPlayerWork> unitOfWork)
    {
        _unitOfWork = unitOfWork;
        _playerRepository = unitOfWork.GetContributor<IPlayerRepository>();
    }

    /// <summary>
    /// Handles the <see cref="UpdatePlayerCommand"/>
    /// </summary>
    /// <param name="command">The command</param>
    /// <param name="cancellationToken">A <see cref="CancellationToken" /> to observe while waiting for the task to complete</param>
    /// <returns>The completed task</returns>
    public async Task Handle(UpdatePlayerCommand command, CancellationToken cancellationToken = default)
    {
        var year = command.Year;
        var player = command.Player;
        var playerStatusChanges = command.PlayerStatusChanges;

        UpdateActiveStatus(ref player, year, playerStatusChanges, command.Date);
        UpdateTeamStatus(ref player, year, playerStatusChanges, command.Date);

        await _playerRepository.Update(player);

        await _unitOfWork.CommitAsync(cancellationToken);
    }

    /// <summary>
    /// Checks if a <see cref="Player"/>'s active status has changed and if so, mutates the Player
    /// </summary>
    /// <param name="player">The <see cref="Player"/></param>
    /// <param name="year">The year the player is being updated for</param>
    /// <param name="statusChanges">The status changes</param>
    /// <param name="date">The date</param>
    private void UpdateActiveStatus(ref Player player, SeasonYear year, PlayerStatusChanges statusChanges,
        DateOnly date)
    {
        if (statusChanges.Activated)
        {
            player.Activate(year, date);
        }
        else if (statusChanges.Inactivated)
        {
            player.Inactivate(year, date);
        }
    }

    /// <summary>
    /// Checks if a <see cref="Player"/>'s team has changed. If so, the <see cref="Player"/> will be mutated
    /// </summary>
    /// <param name="player">The <see cref="Player"/></param>
    /// <param name="year">The year the player is being updated for</param>
    /// <param name="statusChanges">The status changes</param>
    /// <param name="date">The date</param>
    /// <exception cref="MissingTeamContractSigningException">Thrown when there is no <see cref="Team"/> when signing a contract</exception>
    private void UpdateTeamStatus(ref Player player, SeasonYear year, PlayerStatusChanges statusChanges, DateOnly date)
    {
        if (statusChanges.EnteredFreeAgency)
        {
            player.EnterFreeAgency(year, date);
        }
        else if (statusChanges.SignedContractWithTeam)
        {
            player.SignContractWithTeam(year, statusChanges.NewTeam ??
                                              throw new MissingTeamContractSigningException(
                                                  $"No team specified when signing contract for {player.MlbId.Value} {player.FirstName} {player.LastName}"),
                date);
        }
    }
}