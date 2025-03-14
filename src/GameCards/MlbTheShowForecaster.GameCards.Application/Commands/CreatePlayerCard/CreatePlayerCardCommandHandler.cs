﻿using com.brettnamba.MlbTheShowForecaster.Common.Application.Cqrs;
using com.brettnamba.MlbTheShowForecaster.Common.Domain.SeedWork;
using com.brettnamba.MlbTheShowForecaster.GameCards.Application.Commands.CreatePlayerCard.Exceptions;
using com.brettnamba.MlbTheShowForecaster.GameCards.Application.Dtos.Mapping;
using com.brettnamba.MlbTheShowForecaster.GameCards.Domain;
using com.brettnamba.MlbTheShowForecaster.GameCards.Domain.Cards.Entities;
using com.brettnamba.MlbTheShowForecaster.GameCards.Domain.Cards.Repositories;

namespace com.brettnamba.MlbTheShowForecaster.GameCards.Application.Commands.CreatePlayerCard;

/// <summary>
/// Handles a <see cref="CreatePlayerCardCommand"/>
///
/// <para>Creates a new <see cref="PlayerCard"/> by adding it to the repository and wrapping the whole command as a
/// single unit of work</para>
/// </summary>
internal sealed class CreatePlayerCardCommandHandler : ICommandHandler<CreatePlayerCardCommand>
{
    /// <summary>
    /// The unit of work that encapsulates all actions for creating a <see cref="PlayerCard"/>
    /// </summary>
    private readonly IUnitOfWork<ICardWork> _unitOfWork;

    /// <summary>
    /// Maps the external card details to <see cref="PlayerCard"/>
    /// </summary>
    private readonly IPlayerCardMapper _playerCardMapper;

    /// <summary>
    /// The <see cref="PlayerCard"/> repository
    /// </summary>
    private readonly IPlayerCardRepository _playerCardRepository;

    /// <summary>
    /// Constructor
    /// </summary>
    /// <param name="unitOfWork">The unit of work that encapsulates all actions for creating a <see cref="PlayerCard"/></param>
    /// <param name="playerCardMapper">Maps the external card details to <see cref="PlayerCard"/></param>
    public CreatePlayerCardCommandHandler(IUnitOfWork<ICardWork> unitOfWork, IPlayerCardMapper playerCardMapper)
    {
        _unitOfWork = unitOfWork;
        _playerCardMapper = playerCardMapper;
        _playerCardRepository = unitOfWork.GetContributor<IPlayerCardRepository>();
    }

    /// <summary>
    /// Creates a <see cref="PlayerCard"/> and adds it to the domain
    /// </summary>
    /// <param name="command">The command</param>
    /// <param name="cancellationToken">A <see cref="CancellationToken" /> to observe while waiting for the task to complete</param>
    public async Task Handle(CreatePlayerCardCommand command, CancellationToken cancellationToken = default)
    {
        if (await _playerCardRepository.Exists(command.MlbPlayerCard.Year, command.MlbPlayerCard.ExternalUuid))
        {
            throw new PlayerCardAlreadyExistsException(
                $"{nameof(PlayerCard)} already exists for {command.MlbPlayerCard.ExternalUuid.Value}");
        }

        var playerCard = _playerCardMapper.Map(command.MlbPlayerCard);

        await _playerCardRepository.Add(playerCard);

        await _unitOfWork.CommitAsync(cancellationToken);
    }
}