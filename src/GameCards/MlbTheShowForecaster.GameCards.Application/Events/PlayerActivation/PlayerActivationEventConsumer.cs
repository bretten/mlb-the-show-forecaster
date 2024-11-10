using com.brettnamba.MlbTheShowForecaster.Common.Application.Cqrs;
using com.brettnamba.MlbTheShowForecaster.GameCards.Application.Services.Reports;
using com.brettnamba.MlbTheShowForecaster.GameCards.Domain.Forecasts.Entities;
using com.brettnamba.MlbTheShowForecaster.GameCards.Domain.Forecasts.ValueObjects;
using com.brettnamba.MlbTheShowForecaster.GameCards.Domain.Forecasts.ValueObjects.AdministrativeImpacts;

namespace com.brettnamba.MlbTheShowForecaster.GameCards.Application.Events.PlayerActivation;

/// <summary>
/// Consumes a <see cref="PlayerActivationEvent"/>
///
/// <para>Applies a <see cref="PlayerActivationForecastImpact"/> to the specified <see cref="PlayerCardForecast"/></para>
/// </summary>
public sealed class PlayerActivationEventConsumer : BaseForecastImpactEventConsumer<PlayerActivationEvent>
{
    /// <inheritdoc />
    public PlayerActivationEventConsumer(ICommandSender commandSender, ITrendReporter trendReporter,
        ForecastImpactDuration duration) : base(commandSender, trendReporter, duration)
    {
    }

    /// <inheritdoc />
    protected override ForecastImpact CreateImpact(IForecastImpactEvent ev)
    {
        return new PlayerActivationForecastImpact(ev.Date, ev.Date.AddDays(Duration.PlayerActivation));
    }
}