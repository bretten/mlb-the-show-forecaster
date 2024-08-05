using com.brettnamba.MlbTheShowForecaster.Common.Application.Cqrs;
using com.brettnamba.MlbTheShowForecaster.Common.Domain.SeedWork;
using com.brettnamba.MlbTheShowForecaster.GameCards.Domain;
using com.brettnamba.MlbTheShowForecaster.GameCards.Domain.Forecasts.Entities;
using com.brettnamba.MlbTheShowForecaster.GameCards.Domain.Forecasts.Repositories;

namespace com.brettnamba.MlbTheShowForecaster.GameCards.Application.Commands.CreatePlayerCardForecast;

/// <summary>
/// Handles a <see cref="CreatePlayerCardForecastCommand"/>
///
/// <para>Creates a new <see cref="PlayerCardForecast"/> by adding it to the repository and wrapping the whole command as a
/// single unit of work</para>
/// </summary>
internal sealed class CreatePlayerCardForecastCommandHandler : ICommandHandler<CreatePlayerCardForecastCommand>
{
    /// <summary>
    /// The unit of work that encapsulates all actions for creating a <see cref="PlayerCardForecast"/>
    /// </summary>
    private readonly IUnitOfWork<IForecastWork> _unitOfWork;

    /// <summary>
    /// The <see cref="PlayerCardForecast"/> repository
    /// </summary>
    private readonly IForecastRepository _forecastRepository;

    /// <summary>
    /// Constructor
    /// </summary>
    /// <param name="unitOfWork">The unit of work that encapsulates all actions for creating a <see cref="PlayerCardForecast"/></param>
    public CreatePlayerCardForecastCommandHandler(IUnitOfWork<IForecastWork> unitOfWork)
    {
        _unitOfWork = unitOfWork;
        _forecastRepository = _unitOfWork.GetContributor<IForecastRepository>();
    }

    /// <summary>
    /// Creates a <see cref="PlayerCardForecast"/> and adds it to the domain
    /// </summary>
    /// <param name="command">The command</param>
    /// <param name="cancellationToken">A <see cref="CancellationToken" /> to observe while waiting for the task to complete</param>
    public async Task Handle(CreatePlayerCardForecastCommand command, CancellationToken cancellationToken = default)
    {
        var forecast = PlayerCardForecast.Create(command.Year, command.CardExternalId, command.MlbId,
            command.PrimaryPosition, command.OverallRating);

        await _forecastRepository.Add(forecast);

        await _unitOfWork.CommitAsync(cancellationToken);
    }
}