using com.brettnamba.MlbTheShowForecaster.Common.Application.Cqrs;
using com.brettnamba.MlbTheShowForecaster.Common.DateAndTime;
using com.brettnamba.MlbTheShowForecaster.GameCards.Domain.Cards.Events;
using com.brettnamba.MlbTheShowForecaster.GameCards.Domain.Forecasts.Entities;
using com.brettnamba.MlbTheShowForecaster.GameCards.Domain.Forecasts.ValueObjects;

namespace com.brettnamba.MlbTheShowForecaster.GameCards.Application.Events.PlayerCardBoosted;

/// <summary>
/// Consumes a <see cref="PlayerCardBoostedEvent"/>
///
/// <para>Applies a <see cref="BoostForecastImpact"/> to the specified <see cref="PlayerCardForecast"/></para>
/// </summary>
public sealed class PlayerCardBoostEventConsumer : BaseForecastImpactEventConsumer<PlayerCardBoostEvent>
{
    /// <inheritdoc />
    public PlayerCardBoostEventConsumer(ICommandSender commandSender, ICalendar calendar,
        ForecastImpactDuration duration)
        : base(commandSender, calendar, duration)
    {
    }

    /// <inheritdoc />
    protected override ForecastImpact CreateImpact(IForecastImpactEvent ev)
    {
        var e = (PlayerCardBoostEvent)ev;
        return new BoostForecastImpact(e.BoostReason, Calendar.Today(), e.BoostEndDate);
    }
}