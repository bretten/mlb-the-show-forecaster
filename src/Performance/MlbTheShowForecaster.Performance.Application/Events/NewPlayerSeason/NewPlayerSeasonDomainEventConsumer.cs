using com.brettnamba.MlbTheShowForecaster.Common.Application.Cqrs;
using com.brettnamba.MlbTheShowForecaster.Common.DateAndTime;
using com.brettnamba.MlbTheShowForecaster.Common.Domain.Events;
using com.brettnamba.MlbTheShowForecaster.Common.Domain.ValueObjects;
using com.brettnamba.MlbTheShowForecaster.Performance.Application.Commands.CreatePlayerStatsBySeason;
using com.brettnamba.MlbTheShowForecaster.Performance.Application.Services;
using com.brettnamba.MlbTheShowForecaster.Performance.Domain.PlayerSeasons.Entities;

namespace com.brettnamba.MlbTheShowForecaster.Performance.Application.Events.NewPlayerSeason;

/// <summary>
/// Consumes a <see cref="NewPlayerSeasonEvent"/>
///
/// <para>Creates a new <see cref="PlayerStatsBySeason"/> specified by the <see cref="NewPlayerSeasonEvent"/> with the
/// most recent stats</para>
/// </summary>
public sealed class NewPlayerSeasonDomainEventConsumer : IDomainEventConsumer<NewPlayerSeasonEvent>
{
    /// <summary>
    /// Live MLB player stats
    /// </summary>
    private readonly IPlayerStats _playerStats;

    /// <summary>
    /// Sends commands to mutate the system
    /// </summary>
    private readonly ICommandSender _commandSender;

    /// <summary>
    /// Gets the current season year
    /// </summary>
    private readonly ICalendar _calendar;

    /// <summary>
    /// Constructor
    /// </summary>
    /// <param name="playerStats">Live MLB player stats</param>
    /// <param name="commandSender">Sends commands to mutate the system</param>
    /// <param name="calendar">Gets the current season year</param>
    public NewPlayerSeasonDomainEventConsumer(IPlayerStats playerStats, ICommandSender commandSender,
        ICalendar calendar)
    {
        _playerStats = playerStats;
        _commandSender = commandSender;
        _calendar = calendar;
    }

    /// <summary>
    /// Handles a <see cref="NewPlayerSeasonEvent"/>
    /// </summary>
    /// <param name="e"><see cref="NewPlayerSeasonEvent"/></param>
    public async Task Handle(NewPlayerSeasonEvent e)
    {
        // Get the most recent season stats
        var seasonToDate =
            await _playerStats.GetPlayerSeason(e.PlayerMlbId, SeasonYear.Create((ushort)_calendar.Today().Year));

        // Create the player's season in the domain
        await _commandSender.Send(new CreatePlayerStatsBySeasonCommand(seasonToDate));
    }
}