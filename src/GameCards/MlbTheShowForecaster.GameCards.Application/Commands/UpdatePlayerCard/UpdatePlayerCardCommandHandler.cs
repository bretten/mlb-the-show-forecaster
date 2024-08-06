using com.brettnamba.MlbTheShowForecaster.Common.Application.Cqrs;
using com.brettnamba.MlbTheShowForecaster.Common.DateAndTime;
using com.brettnamba.MlbTheShowForecaster.Common.Domain.SeedWork;
using com.brettnamba.MlbTheShowForecaster.GameCards.Application.Commands.UpdatePlayerCard.Exceptions;
using com.brettnamba.MlbTheShowForecaster.GameCards.Application.Dtos;
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
    /// Calendar to get the current date
    /// </summary>
    private readonly ICalendar _calendar;

    /// <summary>
    /// Constructor
    /// </summary>
    /// <param name="unitOfWork">The unit of work that encapsulates all actions for updating a <see cref="PlayerCard"/></param>
    /// <param name="calendar">Calendar to get the current date</param>
    public UpdatePlayerCardCommandHandler(IUnitOfWork<ICardWork> unitOfWork, ICalendar calendar)
    {
        _unitOfWork = unitOfWork;
        _calendar = calendar;
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

        // Copy the previous historical ratings BEFORE updating with the most recent rating
        foreach (var historicalRating in command.PlayerCard.HistoricalRatingsChronologically)
        {
            if (domainPlayerCard.IsRatingAppliedFor(historicalRating.StartDate)) continue;
            domainPlayerCard.AddHistoricalRating(historicalRating);
        }

        // The changes from the external source
        var externalPlayerCard = command.ExternalPlayerCard;
        var ratingChange = command.RatingChange;
        var positionChange = command.PositionChange;

        // If there was a rating change, apply it
        if (ratingChange != null && externalPlayerCard != null)
        {
            domainPlayerCard.ChangePlayerRating(ratingChange.Value.Date, ratingChange.Value.NewRating,
                externalPlayerCard.Value.GetAttributes());
        }

        // If there was a position change, apply it
        if (positionChange != null)
        {
            domainPlayerCard.ChangePosition(positionChange.Value.NewPosition);
        }

        // Add or remove a card boost
        if (externalPlayerCard != null && domainPlayerCard.IsBoosted != externalPlayerCard.Value.IsBoosted)
        {
            UpdateBoost(domainPlayerCard, externalPlayerCard.Value);
        }
        // Add or remove a temporary rating
        else if (externalPlayerCard != null &&
                 domainPlayerCard.HasTemporaryRating != externalPlayerCard.Value.HasTemporaryRating)
        {
            UpdateTemporaryRating(domainPlayerCard, externalPlayerCard.Value);
        }

        await _playerCardRepository.Update(domainPlayerCard);
        await _unitOfWork.CommitAsync(cancellationToken);
    }

    /// <summary>
    /// Updates the boost state of the domain's <see cref="PlayerCard"/>
    /// </summary>
    /// <param name="domainPlayerCard">The domain's <see cref="PlayerCard"/></param>
    /// <param name="externalPlayerCard">The player card data from the external source</param>
    private void UpdateBoost(PlayerCard domainPlayerCard, MlbPlayerCard externalPlayerCard)
    {
        if (externalPlayerCard.IsBoosted)
        {
            domainPlayerCard.Boost(_calendar.Today(), DateOnly.FromDateTime(externalPlayerCard.BoostEndDate!.Value),
                externalPlayerCard.BoostReason!, externalPlayerCard.GetAttributes());
        }
        else
        {
            domainPlayerCard.RemoveBoost(_calendar.Today(), externalPlayerCard.GetAttributes());
        }
    }

    /// <summary>
    /// Updates the temporary rating of the domain's <see cref="PlayerCard"/>
    /// </summary>
    /// <param name="domainPlayerCard">The domain's <see cref="PlayerCard"/></param>
    /// <param name="externalPlayerCard">The player card data from the external source</param>
    private void UpdateTemporaryRating(PlayerCard domainPlayerCard, MlbPlayerCard externalPlayerCard)
    {
        if (externalPlayerCard.HasTemporaryRating)
        {
            domainPlayerCard.SetTemporaryRating(_calendar.Today(), externalPlayerCard.TemporaryOverallRating!);
        }
        else
        {
            domainPlayerCard.RemoveTemporaryRating(_calendar.Today());
        }
    }
}