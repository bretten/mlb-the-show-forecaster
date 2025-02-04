using com.brettnamba.MlbTheShowForecaster.Common.Application.Cqrs;
using com.brettnamba.MlbTheShowForecaster.Common.Domain.SeedWork;
using com.brettnamba.MlbTheShowForecaster.Common.Domain.ValueObjects;
using com.brettnamba.MlbTheShowForecaster.GameCards.Application.Commands.UpdatePlayerCardForecastMlbId.Exceptions;
using com.brettnamba.MlbTheShowForecaster.GameCards.Application.Services;
using com.brettnamba.MlbTheShowForecaster.GameCards.Domain;
using com.brettnamba.MlbTheShowForecaster.GameCards.Domain.Forecasts.Entities;
using com.brettnamba.MlbTheShowForecaster.GameCards.Domain.Forecasts.Repositories;

namespace com.brettnamba.MlbTheShowForecaster.GameCards.Application.Commands.UpdatePlayerCardForecastMlbId;

/// <summary>
/// Handles a <see cref="UpdatePlayerCardForecastMlbIdCommand"/>
///
/// <para>Updates a <see cref="PlayerCardForecast"/>'s <see cref="MlbId"/></para>
/// </summary>
internal sealed class
    UpdatePlayerCardForecastMlbIdCommandHandler : ICommandHandler<UpdatePlayerCardForecastMlbIdCommand>
{
    /// <summary>
    /// The unit of work that encapsulates all actions for updating a <see cref="PlayerCardForecast"/>
    /// </summary>
    private readonly IUnitOfWork<IForecastWork> _unitOfWork;

    /// <summary>
    /// Matches a player card with the corresponding player in the MLB
    /// </summary>
    private readonly IPlayerMatcher _playerMatcher;

    /// <summary>
    /// The <see cref="PlayerCardForecast"/> repository
    /// </summary>
    private readonly IForecastRepository _forecastRepository;

    /// <summary>
    /// Constructor
    /// </summary>
    /// <param name="unitOfWork">The unit of work that encapsulates all actions for updating a <see cref="PlayerCardForecast"/></param>
    /// <param name="playerMatcher">Matches a player card with the corresponding player in the MLB</param>
    public UpdatePlayerCardForecastMlbIdCommandHandler(IUnitOfWork<IForecastWork> unitOfWork,
        IPlayerMatcher playerMatcher)
    {
        _unitOfWork = unitOfWork;
        _playerMatcher = playerMatcher;
        _forecastRepository = _unitOfWork.GetContributor<IForecastRepository>();
    }

    /// <summary>
    /// Updates an existing <see cref="PlayerCardForecast"/>'s <see cref="MlbId"/>
    /// </summary>
    /// <param name="command">The command</param>
    /// <param name="cancellationToken">A <see cref="CancellationToken" /> to observe while waiting for the task to complete</param>
    public async Task Handle(UpdatePlayerCardForecastMlbIdCommand command,
        CancellationToken cancellationToken = default)
    {
        // Get the forecast
        var forecast = await _forecastRepository.GetBy(command.Card.Year, command.Card.ExternalId);
        if (forecast == null)
        {
            throw new MissingForecastForMlbIdUpdateException(command.Card);
        }

        // Get the MLB ID
        var p = await _playerMatcher.GetPlayerByName(command.Card.Name, command.Card.TeamShortName);
        if (p == null)
        {
            // A match doesn't exist, so can safely ignore
            return;
        }

        forecast.SetMlbId(p.Value.MlbId);

        await _forecastRepository.Update(forecast);

        await _unitOfWork.CommitAsync(cancellationToken);
    }
}