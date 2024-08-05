using com.brettnamba.MlbTheShowForecaster.Common.Application.Cqrs;
using com.brettnamba.MlbTheShowForecaster.Common.Domain.ValueObjects;
using com.brettnamba.MlbTheShowForecaster.GameCards.Domain.Cards.ValueObjects;
using com.brettnamba.MlbTheShowForecaster.GameCards.Domain.Forecasts.Entities;
using com.brettnamba.MlbTheShowForecaster.GameCards.Domain.Forecasts.ValueObjects;

namespace com.brettnamba.MlbTheShowForecaster.GameCards.Application.Commands.UpdatePlayerCardForecastImpacts;

/// <summary>
/// Command that updates a <see cref="PlayerCardForecast"/> with the specified <see cref="ForecastImpact"/>s
/// </summary>
/// <param name="Year">The year of MLB The Show</param>
/// <param name="CardExternalId">The card ID from MLB The Show</param>
/// <param name="MlbId">The MLB ID of the Player</param>
/// <param name="Impacts">The <see cref="ForecastImpact"/>s to apply to the <see cref="PlayerCardForecast"/></param>
internal readonly record struct UpdatePlayerCardForecastImpactsCommand(
    SeasonYear Year,
    CardExternalId? CardExternalId,
    MlbId? MlbId,
    params ForecastImpact[] Impacts) : ICommand;