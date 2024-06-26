﻿using com.brettnamba.MlbTheShowForecaster.Common.Application.Cqrs;
using com.brettnamba.MlbTheShowForecaster.Common.Domain.SeedWork;
using com.brettnamba.MlbTheShowForecaster.Performance.Application.Dtos;
using com.brettnamba.MlbTheShowForecaster.Performance.Application.Dtos.Mapping;
using com.brettnamba.MlbTheShowForecaster.Performance.Domain;
using com.brettnamba.MlbTheShowForecaster.Performance.Domain.PlayerSeasons.Entities;
using com.brettnamba.MlbTheShowForecaster.Performance.Domain.PlayerSeasons.Repositories;

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
    /// The <see cref="PlayerStatsBySeason"/> repository
    /// </summary>
    private readonly IPlayerStatsBySeasonRepository _playerStatsBySeasonRepository;

    /// <summary>
    /// Constructor
    /// </summary>
    /// <param name="unitOfWork">The unit of work that encapsulates all actions for creating a <see cref="PlayerStatsBySeason"/></param>
    /// <param name="playerSeasonMapper">Maps <see cref="PlayerSeason"/> to other objects</param>
    public CreatePlayerStatsBySeasonCommandHandler(IUnitOfWork<IPlayerSeasonWork> unitOfWork,
        IPlayerSeasonMapper playerSeasonMapper)
    {
        _unitOfWork = unitOfWork;
        _playerSeasonMapper = playerSeasonMapper;
        _playerStatsBySeasonRepository = unitOfWork.GetContributor<IPlayerStatsBySeasonRepository>();
    }

    /// <summary>
    /// Handles a <see cref="CreatePlayerStatsBySeasonCommand"/> command
    /// </summary>
    /// <param name="command">The command</param>
    /// <param name="cancellationToken">A <see cref="CancellationToken" /> to observe while waiting for the task to complete</param>
    public async Task Handle(CreatePlayerStatsBySeasonCommand command, CancellationToken cancellationToken = default)
    {
        var playerStatsBySeason = _playerSeasonMapper.Map(command.PlayerSeason);

        await _playerStatsBySeasonRepository.Add(playerStatsBySeason);

        await _unitOfWork.CommitAsync(cancellationToken);
    }
}