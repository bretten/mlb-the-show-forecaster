using com.brettnamba.MlbTheShowForecaster.GameCards.Domain.Forecasts.Entities;

namespace com.brettnamba.MlbTheShowForecaster.GameCards.Application.Commands.UpdatePlayerCardForecastImpacts.Exceptions;

/// <summary>
/// Thrown when <see cref="UpdatePlayerCardForecastImpactsCommandHandler"/> is not provided a
/// <see cref="PlayerCardForecast"/> identifer to indicate which forecast is being updated
/// </summary>
public sealed class PlayerCardForecastIdentifierNotSpecifiedException : Exception
{
    private const string DefaultMessage =
        $"Could not apply forecast impacts to a {nameof(PlayerCardForecast)} because there was no identifer";

    public PlayerCardForecastIdentifierNotSpecifiedException() : base(DefaultMessage)
    {
    }
}