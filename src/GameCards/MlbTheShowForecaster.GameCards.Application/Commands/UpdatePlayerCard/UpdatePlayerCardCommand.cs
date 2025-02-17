﻿using com.brettnamba.MlbTheShowForecaster.Common.Application.Cqrs;
using com.brettnamba.MlbTheShowForecaster.GameCards.Application.Dtos;
using com.brettnamba.MlbTheShowForecaster.GameCards.Domain.Cards.Entities;
using com.brettnamba.MlbTheShowForecaster.GameCards.Domain.Cards.ValueObjects.PlayerCards;

namespace com.brettnamba.MlbTheShowForecaster.GameCards.Application.Commands.UpdatePlayerCard;

/// <summary>
/// Command that updates a <see cref="PlayerCard"/>
/// </summary>
/// <param name="PlayerCard">The <see cref="PlayerCard"/> to update</param>
/// <param name="ExternalPlayerCard">The current state of the player's card from the external source</param>
/// <param name="RatingChange">The new rating for the <see cref="PlayerCard"/></param>
/// <param name="PositionChange">The new position for the <see cref="PlayerCard"/></param>
/// <param name="HistoricalRatings">Old to new collection of <see cref="PlayerCardHistoricalRating"/> to apply to the <see cref="PlayerCard"/></param>
internal readonly record struct UpdatePlayerCardCommand(
    PlayerCard PlayerCard,
    MlbPlayerCard? ExternalPlayerCard = null,
    PlayerRatingChange? RatingChange = null,
    PlayerPositionChange? PositionChange = null,
    Stack<PlayerCardHistoricalRating>? HistoricalRatings = null
) : ICommand;