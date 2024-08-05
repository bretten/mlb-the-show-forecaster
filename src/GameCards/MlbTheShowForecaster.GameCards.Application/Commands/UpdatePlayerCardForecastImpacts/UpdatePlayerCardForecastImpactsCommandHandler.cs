using com.brettnamba.MlbTheShowForecaster.Common.Application.Cqrs;
using com.brettnamba.MlbTheShowForecaster.Common.DateAndTime;
using com.brettnamba.MlbTheShowForecaster.Common.Domain.SeedWork;
using com.brettnamba.MlbTheShowForecaster.GameCards.Application.Commands.UpdatePlayerCardForecastImpacts.Exceptions;
using com.brettnamba.MlbTheShowForecaster.GameCards.Domain;
using com.brettnamba.MlbTheShowForecaster.GameCards.Domain.Forecasts.Entities;
using com.brettnamba.MlbTheShowForecaster.GameCards.Domain.Forecasts.Repositories;
using com.brettnamba.MlbTheShowForecaster.GameCards.Domain.Forecasts.ValueObjects;

namespace com.brettnamba.MlbTheShowForecaster.GameCards.Application.Commands.UpdatePlayerCardForecastImpacts;

/// <summary>
/// Handles a <see cref="UpdatePlayerCardForecastImpactsCommand"/>
///
/// <para>Updates a <see cref="PlayerCardForecast"/> by applying <see cref="ForecastImpact"/>s</para>
/// </summary>
internal sealed class
    UpdatePlayerCardForecastImpactsCommandHandler : ICommandHandler<UpdatePlayerCardForecastImpactsCommand>
{
    /// <summary>
    /// The unit of work that encapsulates all actions for updating a <see cref="PlayerCardForecast"/>
    /// </summary>
    private readonly IUnitOfWork<IForecastWork> _unitOfWork;

    /// <summary>
    /// Calendar to determine the date when reassessing the forecast's impacts
    /// </summary>
    private readonly ICalendar _calendar;

    /// <summary>
    /// The <see cref="PlayerCardForecast"/> repository
    /// </summary>
    private readonly IForecastRepository _forecastRepository;

    /// <summary>
    /// Constructor
    /// </summary>
    /// <param name="unitOfWork">The unit of work that encapsulates all actions for updating a <see cref="PlayerCardForecast"/></param>
    /// <param name="calendar">Calendar to determine the date when reassessing the forecast's impacts</param>
    public UpdatePlayerCardForecastImpactsCommandHandler(IUnitOfWork<IForecastWork> unitOfWork, ICalendar calendar)
    {
        _unitOfWork = unitOfWork;
        _calendar = calendar;
        _forecastRepository = _unitOfWork.GetContributor<IForecastRepository>();
    }

    /// <summary>
    /// Updates an existing <see cref="PlayerCardForecast"/>'s impacts
    /// </summary>
    /// <param name="command">The command</param>
    /// <param name="cancellationToken">A <see cref="CancellationToken" /> to observe while waiting for the task to complete</param>
    public async Task Handle(UpdatePlayerCardForecastImpactsCommand command,
        CancellationToken cancellationToken = default)
    {
        if (command.CardExternalId == null && command.MlbId == null)
        {
            throw new PlayerCardForecastIdentifierNotSpecifiedException();
        }

        // Get the forecast by whichever identifier was specified
        var forecast = command.CardExternalId != null
            ? await _forecastRepository.GetBy(command.Year, command.CardExternalId)
            : await _forecastRepository.GetBy(command.Year, command.MlbId!);

        if (forecast == null)
        {
            throw new PlayerCardForecastNotFoundException(command.CardExternalId, command.MlbId);
        }

        // Reassess the forecast for today using the new impacts
        var today = _calendar.Today();
        foreach (var impact in command.Impacts)
        {
            forecast.Reassess(impact, today);
        }

        await _forecastRepository.Update(forecast);

        await _unitOfWork.CommitAsync(cancellationToken);
    }
}