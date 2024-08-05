using com.brettnamba.MlbTheShowForecaster.Common.Domain.ValueObjects;
using com.brettnamba.MlbTheShowForecaster.GameCards.Domain.Cards.ValueObjects;
using com.brettnamba.MlbTheShowForecaster.GameCards.Domain.Forecasts.Entities;

namespace com.brettnamba.MlbTheShowForecaster.GameCards.Application.Commands.UpdatePlayerCardForecastImpacts.Exceptions;

/// <summary>
/// Thrown when <see cref="UpdatePlayerCardForecastImpactsCommandHandler"/> cannot find the
/// specified <see cref="PlayerCardForecast"/>
/// </summary>
public sealed class PlayerCardForecastNotFoundException : Exception
{
    public PlayerCardForecastNotFoundException(CardExternalId? cardExternalId, MlbId? mlbId) : base(
        $"Could not find the {nameof(PlayerCardForecast)} for the {nameof(CardExternalId)} = {cardExternalId?.Value} or {nameof(MlbId)} = {mlbId?.Value}")
    {
    }
}