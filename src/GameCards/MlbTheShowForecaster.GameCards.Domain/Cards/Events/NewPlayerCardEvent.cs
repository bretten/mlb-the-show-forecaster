using com.brettnamba.MlbTheShowForecaster.Common.Domain.Enums;
using com.brettnamba.MlbTheShowForecaster.Common.Domain.Events;
using com.brettnamba.MlbTheShowForecaster.Common.Domain.ValueObjects;
using com.brettnamba.MlbTheShowForecaster.GameCards.Domain.Cards.Entities;
using com.brettnamba.MlbTheShowForecaster.GameCards.Domain.Cards.ValueObjects;

namespace com.brettnamba.MlbTheShowForecaster.GameCards.Domain.Cards.Events;

/// <summary>
/// Raised when a new <see cref="PlayerCard"/> is created
/// </summary>
/// <param name="Year">The year of MLB The Show</param>
/// <param name="CardExternalId">The card ID from MLB The Show</param>
/// <param name="CardName">The card name from MLB The Show</param>
/// <param name="PrimaryPosition">Player's primary position</param>
/// <param name="OverallRating">The overall rating of the card</param>
/// <param name="TeamShortName">The player's team name abbreviated</param>
public sealed record NewPlayerCardEvent(
    SeasonYear Year,
    CardExternalId CardExternalId,
    CardName CardName,
    Position PrimaryPosition,
    OverallRating OverallRating,
    TeamShortName TeamShortName) : IDomainEvent;