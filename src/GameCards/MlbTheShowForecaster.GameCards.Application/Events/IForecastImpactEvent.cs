using com.brettnamba.MlbTheShowForecaster.Common.Domain.Events;
using com.brettnamba.MlbTheShowForecaster.Common.Domain.ValueObjects;
using com.brettnamba.MlbTheShowForecaster.GameCards.Domain.Cards.ValueObjects;
using com.brettnamba.MlbTheShowForecaster.GameCards.Domain.Forecasts.Entities;
using com.brettnamba.MlbTheShowForecaster.GameCards.Domain.Forecasts.ValueObjects;

namespace com.brettnamba.MlbTheShowForecaster.GameCards.Application.Events;

/// <summary>
/// A consumable event where a <see cref="ForecastImpact"/> will influence a <see cref="PlayerCardForecast"/>
/// </summary>
public interface IForecastImpactEvent : IDomainEvent
{
    /// <summary>
    /// The year of MLB The Show
    /// </summary>
    public SeasonYear Year { get; }

    /// <summary>
    /// The card ID from MLB The Show
    /// </summary>
    public CardExternalId? CardExternalId { get; }

    /// <summary>
    /// The MLB ID of the Player
    /// </summary>
    public MlbId? MlbId { get; }
}