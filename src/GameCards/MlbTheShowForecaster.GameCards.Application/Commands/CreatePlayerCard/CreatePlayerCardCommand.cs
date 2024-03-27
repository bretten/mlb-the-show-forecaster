using com.brettnamba.MlbTheShowForecaster.Common.Application.Cqrs;
using com.brettnamba.MlbTheShowForecaster.GameCards.Application.Dtos;
using com.brettnamba.MlbTheShowForecaster.GameCards.Domain.Cards.Entities;

namespace com.brettnamba.MlbTheShowForecaster.GameCards.Application.Commands.CreatePlayerCard;

/// <summary>
/// Command that creates a <see cref="PlayerCard"/>
/// </summary>
/// <param name="MlbPlayerCard">Details about the player card to create</param>
internal readonly record struct CreatePlayerCardCommand(
    MlbPlayerCard MlbPlayerCard
) : ICommand;