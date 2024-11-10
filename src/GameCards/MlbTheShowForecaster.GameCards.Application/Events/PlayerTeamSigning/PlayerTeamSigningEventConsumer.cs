using com.brettnamba.MlbTheShowForecaster.Common.Application.Cqrs;
using com.brettnamba.MlbTheShowForecaster.GameCards.Application.Services.Reports;
using com.brettnamba.MlbTheShowForecaster.GameCards.Domain.Forecasts.Entities;
using com.brettnamba.MlbTheShowForecaster.GameCards.Domain.Forecasts.ValueObjects;
using com.brettnamba.MlbTheShowForecaster.GameCards.Domain.Forecasts.ValueObjects.AdministrativeImpacts;

namespace com.brettnamba.MlbTheShowForecaster.GameCards.Application.Events.PlayerTeamSigning;

/// <summary>
/// Consumes a <see cref="PlayerTeamSigningEvent"/>
///
/// <para>Applies a <see cref="PlayerTeamSigningForecastImpact"/> to the specified <see cref="PlayerCardForecast"/></para>
/// </summary>
public sealed class PlayerTeamSigningEventConsumer : BaseForecastImpactEventConsumer<PlayerTeamSigningEvent>
{
    /// <inheritdoc />
    public PlayerTeamSigningEventConsumer(ICommandSender commandSender, ITrendReporter trendReporter,
        ForecastImpactDuration duration) : base(commandSender, trendReporter, duration)
    {
    }

    /// <inheritdoc />
    protected override ForecastImpact CreateImpact(IForecastImpactEvent ev)
    {
        return new PlayerTeamSigningForecastImpact(ev.Date, ev.Date.AddDays(Duration.PlayerTeamSigning));
    }
}