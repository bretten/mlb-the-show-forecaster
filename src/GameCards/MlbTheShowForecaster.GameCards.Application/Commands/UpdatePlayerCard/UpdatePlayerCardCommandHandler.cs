using com.brettnamba.MlbTheShowForecaster.Common.Application.Cqrs;
using com.brettnamba.MlbTheShowForecaster.Common.Domain.SeedWork;
using com.brettnamba.MlbTheShowForecaster.GameCards.Application.Commands.UpdatePlayerCard.Exceptions;
using com.brettnamba.MlbTheShowForecaster.GameCards.Domain;
using com.brettnamba.MlbTheShowForecaster.GameCards.Domain.Cards.Entities;
using com.brettnamba.MlbTheShowForecaster.GameCards.Domain.Cards.Repositories;

namespace com.brettnamba.MlbTheShowForecaster.GameCards.Application.Commands.UpdatePlayerCard;

/// <summary>
/// Handles a <see cref="UpdatePlayerCardCommand"/>
///
/// <para>Updates a <see cref="PlayerCard"/> by applying a ratings change and/or a position change</para>
/// </summary>
internal sealed class UpdatePlayerCardCommandHandler : ICommandHandler<UpdatePlayerCardCommand>
{
    /// <summary>
    /// The unit of work that encapsulates all actions for updating a <see cref="PlayerCard"/>
    /// </summary>
    private readonly IUnitOfWork<ICardWork> _unitOfWork;

    /// <summary>
    /// The <see cref="PlayerCard"/> repository
    /// </summary>
    private readonly IPlayerCardRepository _playerCardRepository;

    /// <summary>
    /// Constructor
    /// </summary>
    /// <param name="unitOfWork">The unit of work that encapsulates all actions for updating a <see cref="PlayerCard"/></param>
    public UpdatePlayerCardCommandHandler(IUnitOfWork<ICardWork> unitOfWork)
    {
        _unitOfWork = unitOfWork;
        _playerCardRepository = unitOfWork.GetContributor<IPlayerCardRepository>();
    }

    /// <summary>
    /// Updates an existing <see cref="PlayerCard"/> in the domain by updating the card's rating and/or its position
    /// </summary>
    /// <param name="command">The command</param>
    /// <param name="cancellationToken">A <see cref="CancellationToken" /> to observe while waiting for the task to complete</param>
    public async Task Handle(UpdatePlayerCardCommand command, CancellationToken cancellationToken = default)
    {
        // The domain PlayerCard that is being updated
        var domainPlayerCard = await _playerCardRepository.GetByExternalId(command.PlayerCard.ExternalId);
        if (domainPlayerCard == null)
        {
            throw new PlayerCardNotFoundException(
                $"{nameof(PlayerCard)} not found for CardExternalId {command.PlayerCard.ExternalId.Value}");
        }

        // The changes from the external source
        var ratingChange = command.RatingChange;
        var positionChange = command.PositionChange;

        // If there was a rating change, apply it
        if (ratingChange != null)
        {
            domainPlayerCard.ChangePlayerRating(ratingChange.Value.Date, ratingChange.Value.NewRating,
                ratingChange.Value.AttributeChanges.ApplyAttributes(domainPlayerCard.PlayerCardAttributes));
        }

        // If there was a position change, apply it
        if (positionChange != null)
        {
            domainPlayerCard.ChangePosition(positionChange.Value.NewPosition);
        }

        await _playerCardRepository.Update(domainPlayerCard);
        await _unitOfWork.CommitAsync(cancellationToken);
    }
}