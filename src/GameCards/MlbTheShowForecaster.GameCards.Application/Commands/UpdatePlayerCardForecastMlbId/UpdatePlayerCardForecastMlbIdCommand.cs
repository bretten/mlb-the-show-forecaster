using com.brettnamba.MlbTheShowForecaster.Common.Application.Cqrs;
using com.brettnamba.MlbTheShowForecaster.GameCards.Domain.Cards.Entities;
using com.brettnamba.MlbTheShowForecaster.GameCards.Domain.Forecasts.Entities;

namespace com.brettnamba.MlbTheShowForecaster.GameCards.Application.Commands.UpdatePlayerCardForecastMlbId;

/// <summary>
/// Command that updates a <see cref="PlayerCardForecast"/>'s MLB ID
/// </summary>
/// <param name="Card">The corresponding <see cref="PlayerCard"/> of the <see cref="PlayerCardForecast"/></param>
internal readonly record struct UpdatePlayerCardForecastMlbIdCommand(PlayerCard Card) : ICommand;