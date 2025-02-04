using com.brettnamba.MlbTheShowForecaster.GameCards.Domain.Cards.Entities;
using com.brettnamba.MlbTheShowForecaster.GameCards.Domain.Forecasts.Entities;

namespace com.brettnamba.MlbTheShowForecaster.GameCards.Application.Commands.UpdatePlayerCardForecastMlbId.Exceptions;

/// <summary>
/// Thrown when there is no <see cref="PlayerCardForecast"/> to update a MLB ID for
/// </summary>
public sealed class MissingForecastForMlbIdUpdateException : Exception
{
    public MissingForecastForMlbIdUpdateException(PlayerCard card) : base(
        $"No forecast for player card {card.ExternalId.Value}")
    {
    }
}