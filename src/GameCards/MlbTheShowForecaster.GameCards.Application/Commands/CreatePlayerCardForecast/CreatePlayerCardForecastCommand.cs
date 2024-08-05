using com.brettnamba.MlbTheShowForecaster.Common.Application.Cqrs;
using com.brettnamba.MlbTheShowForecaster.Common.Domain.Enums;
using com.brettnamba.MlbTheShowForecaster.Common.Domain.ValueObjects;
using com.brettnamba.MlbTheShowForecaster.GameCards.Domain.Cards.ValueObjects;
using com.brettnamba.MlbTheShowForecaster.GameCards.Domain.Forecasts.Entities;

namespace com.brettnamba.MlbTheShowForecaster.GameCards.Application.Commands.CreatePlayerCardForecast;

/// <summary>
/// Command that creates a <see cref="PlayerCardForecast"/>
/// </summary>
/// <param name="Year">The year of MLB The Show</param>
/// <param name="CardExternalId">The card ID from MLB The Show</param>
/// <param name="PrimaryPosition">Player's primary position</param>
/// <param name="OverallRating">The overall rating of the card</param>
/// <param name="MlbId">The MLB ID of the Player</param>
internal readonly record struct CreatePlayerCardForecastCommand(
    SeasonYear Year,
    CardExternalId CardExternalId,
    Position PrimaryPosition,
    OverallRating OverallRating,
    MlbId? MlbId) : ICommand;