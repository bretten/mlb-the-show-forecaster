using com.brettnamba.MlbTheShowForecaster.Common.Application.Cqrs;
using com.brettnamba.MlbTheShowForecaster.Common.DateAndTime;
using com.brettnamba.MlbTheShowForecaster.Common.Domain.Events;
using com.brettnamba.MlbTheShowForecaster.GameCards.Application.Commands.UpdatePlayerCardForecastImpacts;
using com.brettnamba.MlbTheShowForecaster.GameCards.Domain.Forecasts.Entities;
using com.brettnamba.MlbTheShowForecaster.GameCards.Domain.Forecasts.ValueObjects;

namespace com.brettnamba.MlbTheShowForecaster.GameCards.Application.Events;

/// <summary>
/// Consumes an event for a <see cref="ForecastImpact"/>
///
/// <para>Applies a <see cref="ForecastImpact"/> to the specified <see cref="PlayerCardForecast"/></para>
/// </summary>
public abstract class BaseForecastImpactEventConsumer<T> : IDomainEventConsumer<T> where T : IForecastImpactEvent
{
    /// <summary>
    /// Sends commands to mutate the system
    /// </summary>
    protected readonly ICommandSender CommandSender;

    /// <summary>
    /// Gets today's date
    /// </summary>
    protected readonly ICalendar Calendar;

    /// <summary>
    /// Determines how long the forecast impact will last
    /// </summary>
    protected readonly ForecastImpactDuration Duration;

    /// <summary>
    /// Constructor
    /// </summary>
    /// <param name="commandSender">Sends commands to mutate the system</param>
    /// <param name="calendar">Gets today's date</param>
    /// <param name="duration">Determines how long the forecast impact will last</param>
    protected BaseForecastImpactEventConsumer(ICommandSender commandSender, ICalendar calendar,
        ForecastImpactDuration duration)
    {
        CommandSender = commandSender;
        Calendar = calendar;
        Duration = duration;
    }

    /// <summary>
    /// Handles an event <see cref="T"/>
    /// </summary>
    /// <param name="e"><see cref="T"/></param>
    /// <param name="cancellationToken">A <see cref="CancellationToken" /> to observe while waiting for the task to complete</param>
    public async Task Handle(T e, CancellationToken cancellationToken = default)
    {
        await CommandSender.Send(
            new UpdatePlayerCardForecastImpactsCommand(e.Year, e.CardExternalId, e.MlbId, CreateImpact(e)),
            cancellationToken);
    }

    /// <summary>
    /// Creates the <see cref="ForecastImpact"/> corresponding to the <see cref="IForecastImpactEvent"/>
    /// </summary>
    /// <param name="ev"><see cref="IForecastImpactEvent"/></param>
    /// <returns><see cref="ForecastImpact"/></returns>
    protected abstract ForecastImpact CreateImpact(IForecastImpactEvent ev);
}