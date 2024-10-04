using com.brettnamba.MlbTheShowForecaster.Common.Application.Cqrs;
using com.brettnamba.MlbTheShowForecaster.Common.DateAndTime;
using com.brettnamba.MlbTheShowForecaster.GameCards.Domain.Forecasts.Entities;
using com.brettnamba.MlbTheShowForecaster.GameCards.Domain.Forecasts.ValueObjects;
using com.brettnamba.MlbTheShowForecaster.GameCards.Domain.Forecasts.ValueObjects.AdministrativeImpacts;

namespace com.brettnamba.MlbTheShowForecaster.GameCards.Application.Events.PlayerDeactivation;

/// <summary>
/// Consumes a <see cref="PlayerDeactivationEvent"/>
///
/// <para>Applies a <see cref="PlayerDeactivationForecastImpact"/> to the specified <see cref="PlayerCardForecast"/></para>
/// </summary>
public sealed class PlayerDeactivationEventConsumer : BaseForecastImpactEventConsumer<PlayerDeactivationEvent>
{
    /// <inheritdoc />
    public PlayerDeactivationEventConsumer(ICommandSender commandSender, ICalendar calendar,
        ForecastImpactDuration duration) : base(commandSender, calendar, duration)
    {
    }

    /// <inheritdoc />
    protected override ForecastImpact CreateImpact(IForecastImpactEvent ev)
    {
        return new PlayerDeactivationForecastImpact(Calendar.Today(),
            Calendar.Today().AddDays(Duration.PlayerDeactivation));
    }
}