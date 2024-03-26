using com.brettnamba.MlbTheShowForecaster.Common.Domain.ValueObjects;

namespace com.brettnamba.MlbTheShowForecaster.GameCards.Application.Dtos;

/// <summary>
/// Represents an attribute change for a player
/// </summary>
/// <param name="AttributeName">The name of the attribute such as contact Vs left or power Vs Right</param>
/// <param name="NewValue">The new value of the attribute</param>
/// <param name="ChangeAmount">The amount the attribute changed by</param>
public readonly record struct AttributeChange(
    string AttributeName,
    NaturalNumber NewValue,
    int ChangeAmount
)
{
    /// <summary>
    /// True if the attribute increased, otherwise false
    /// </summary>
    public bool Increased => ChangeAmount > 0;
};