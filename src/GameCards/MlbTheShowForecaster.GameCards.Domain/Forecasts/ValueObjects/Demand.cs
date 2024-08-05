using com.brettnamba.MlbTheShowForecaster.Common.Domain.SeedWork;
using com.brettnamba.MlbTheShowForecaster.GameCards.Domain.Forecasts.Entities;

namespace com.brettnamba.MlbTheShowForecaster.GameCards.Domain.Forecasts.ValueObjects;

/// <summary>
/// Quantitative measure of how much the demand of a card is affected in a <see cref="PlayerCardForecast"/>
///
/// <see cref="Demand"/> is relative -- a negative value conceptually means a loss of demand relative
/// to its current demand. A zero demand means it is unaffected
/// </summary>
public sealed class Demand : ValueObject
{
    /// <summary>
    /// The underlying demand value as an integer
    /// </summary>
    public int Value { get; }

    /// <summary>
    /// Overload of sum
    /// </summary>
    public static Demand operator +(Demand l, Demand r) => Create(l.Value + r.Value);

    /// <summary>
    /// Overload of difference
    /// </summary>
    public static Demand operator -(Demand l, Demand r) => Create(l.Value - r.Value);

    /// <summary>
    /// Overload of greater than
    /// </summary>
    public static bool operator >(Demand l, Demand r) => l.Value > r.Value;

    /// <summary>
    /// Overload of less than
    /// </summary>
    public static bool operator <(Demand l, Demand r) => l.Value < r.Value;

    /// <summary>
    /// Constructor
    /// </summary>
    /// <param name="value">The underlying demand value as an integer</param>
    private Demand(int value)
    {
        Value = value;
    }

    /// <summary>
    /// Creates a high <see cref="Demand"/>
    /// </summary>
    /// <returns><see cref="Demand"/></returns>
    public static Demand High()
    {
        return new Demand(HighDemand);
    }

    /// <summary>
    /// Creates a low <see cref="Demand"/>
    /// </summary>
    /// <returns><see cref="Demand"/></returns>
    public static Demand Low()
    {
        return new Demand(LowDemand);
    }

    /// <summary>
    /// Creates a <see cref="Demand"/> that is stable or unchanged
    /// </summary>
    /// <returns><see cref="Demand"/></returns>
    public static Demand Stable()
    {
        return new Demand(StableDemand);
    }

    /// <summary>
    /// Creates a loss of <see cref="Demand"/>
    /// </summary>
    /// <returns><see cref="Demand"/></returns>
    public static Demand Loss()
    {
        return new Demand(LossOfDemand);
    }

    /// <summary>
    /// Creates a large loss of <see cref="Demand"/>
    /// </summary>
    /// <returns><see cref="Demand"/></returns>
    public static Demand BigLoss()
    {
        return new Demand(BigLossOfDemand);
    }

    /// <summary>
    /// Creates a <see cref="Demand"/>
    /// </summary>
    /// <param name="value">The demand amount</param>
    /// <returns><see cref="Demand"/></returns>
    public static Demand Create(int value)
    {
        return new Demand(value);
    }

    /// <summary>
    /// Represents high demand
    /// </summary>
    private const int HighDemand = 3;

    /// <summary>
    /// Represents low demand (but still desired)
    /// </summary>
    private const int LowDemand = 1;

    /// <summary>
    /// Represents when demand is stable or unaffected
    /// </summary>
    private const int StableDemand = 0;

    /// <summary>
    /// Represents a loss of demand
    /// </summary>
    private const int LossOfDemand = -1;

    /// <summary>
    /// Represents a large lass of demand
    /// </summary>
    private const int BigLossOfDemand = -3;
}