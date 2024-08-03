using com.brettnamba.MlbTheShowForecaster.Common.Domain.SeedWork;
using com.brettnamba.MlbTheShowForecaster.GameCards.Domain.Forecasts.Entities;

namespace com.brettnamba.MlbTheShowForecaster.GameCards.Domain.Forecasts.ValueObjects;

/// <summary>
/// Should define some external event that has an effect on a <see cref="PlayerCardForecast"/>
/// </summary>
/// <param name="endDate">When this impact no longer has influence over the forecast</param>
public abstract class ForecastImpact(DateOnly endDate) : ValueObject
{
    /// <summary>
    /// When this impact no longer has influence over the forecast (inclusive -- still active on the end date)
    /// </summary>
    public DateOnly EndDate { get; } = endDate;

    /// <summary>
    /// Determines how strong the impact is
    /// </summary>
    protected abstract int ImpactCoefficient { get; }

    /// <summary>
    /// Determines if the impact adds or subtracts from the <see cref="Demand"/>
    /// </summary>
    protected abstract bool IsAdditive { get; }

    /// <summary>
    /// True if the forecast impact should have a negligible impact on the <see cref="Demand"/>
    /// </summary>
    protected virtual bool IsNegligible => false;

    /// <summary>
    /// Quantitative measure of how much the demand of a card is affected in a <see cref="PlayerCardForecast"/>
    /// </summary>
    public int Demand
    {
        get
        {
            if (IsNegligible)
            {
                return 0;
            }

            var baseDemand = IsAdditive ? ImpactConstants.BaseAdditiveDemand : ImpactConstants.BaseSubtractiveDemand;
            return ImpactCoefficient * baseDemand;
        }
    }

    /// <summary>
    /// Determines the demand on the specified date
    ///
    /// Demand can change over time based on various factors. Certain <see cref="ForecastImpact"/> will lose demand only
    /// on their <see cref="EndDate"/> while others might have some change before then
    /// </summary>
    /// <param name="date">The date to check the demand on</param>
    /// <returns><see cref="Demand"/></returns>
    public virtual int DemandOn(DateOnly date)
    {
        return date <= EndDate ? Demand : 0;
    }
}