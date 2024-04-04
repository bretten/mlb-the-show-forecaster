using com.brettnamba.MlbTheShowForecaster.Common.Application.Cqrs;
using com.brettnamba.MlbTheShowForecaster.GameCards.Application.Dtos;
using com.brettnamba.MlbTheShowForecaster.GameCards.Domain.Cards.Entities;

namespace com.brettnamba.MlbTheShowForecaster.GameCards.Application.Commands.UpdatePlayerCard;

/// <summary>
/// Command that updates a <see cref="PlayerCard"/>
/// </summary>
/// <param name="PlayerCard">The <see cref="PlayerCard"/> to update</param>
/// <param name="RatingChange">The new rating for the <see cref="PlayerCard"/></param>
/// <param name="PositionChange">The new position for the <see cref="PlayerCard"/></param>
internal readonly record struct UpdatePlayerCardCommand(
    PlayerCard PlayerCard,
    PlayerRatingChange? RatingChange,
    PlayerPositionChange? PositionChange
) : ICommand;