using com.brettnamba.MlbTheShowForecaster.Common.Application.Cqrs;
using com.brettnamba.MlbTheShowForecaster.Common.Domain.Events;
using com.brettnamba.MlbTheShowForecaster.GameCards.Application.Commands.CreatePlayerCardForecast;
using com.brettnamba.MlbTheShowForecaster.GameCards.Application.Services;
using com.brettnamba.MlbTheShowForecaster.GameCards.Domain.Cards.Entities;
using com.brettnamba.MlbTheShowForecaster.GameCards.Domain.Cards.Events;
using com.brettnamba.MlbTheShowForecaster.GameCards.Domain.Forecasts.Entities;

namespace com.brettnamba.MlbTheShowForecaster.GameCards.Application.Events.NewPlayerCard;

/// <summary>
/// Consumes a <see cref="NewPlayerCardEvent"/>
///
/// <para>Creates a <see cref="PlayerCardForecast"/> for the new <see cref="PlayerCard"/></para>
/// </summary>
public sealed class NewPlayerCardEventConsumer : IDomainEventConsumer<NewPlayerCardEvent>
{
    /// <summary>
    /// Sends commands to mutate the system
    /// </summary>
    private readonly ICommandSender _commandSender;

    /// <summary>
    /// Matches a player card with the corresponding player in the MLB
    /// </summary>
    private readonly IPlayerMatcher _playerMatcher;

    /// <summary>
    /// Constructor
    /// </summary>
    /// <param name="commandSender">Sends commands to mutate the system</param>
    /// <param name="playerMatcher">Matches a player card with the corresponding player in the MLB</param>
    public NewPlayerCardEventConsumer(ICommandSender commandSender, IPlayerMatcher playerMatcher)
    {
        _commandSender = commandSender;
        _playerMatcher = playerMatcher;
    }

    /// <summary>
    /// Handles a <see cref="NewPlayerCardEvent"/>
    /// </summary>
    /// <param name="e"><see cref="NewPlayerCardEvent"/></param>
    /// <param name="cancellationToken">A <see cref="CancellationToken" /> to observe while waiting for the task to complete</param>
    public async Task Handle(NewPlayerCardEvent e, CancellationToken cancellationToken = default)
    {
        var p = await _playerMatcher.GetPlayerByName(e.CardName, e.TeamShortName);

        await _commandSender.Send(
            new CreatePlayerCardForecastCommand(e.Year, e.CardExternalId, e.PrimaryPosition, e.OverallRating, p?.MlbId),
            cancellationToken);
    }
}