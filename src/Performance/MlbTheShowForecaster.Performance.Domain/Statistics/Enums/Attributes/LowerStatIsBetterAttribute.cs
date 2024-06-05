namespace com.brettnamba.MlbTheShowForecaster.Performance.Domain.Statistics.Enums.Attributes;

/// <summary>
/// Defines a stat that is better if its value is lower, such as the number of strikeouts for a batter or the ERA
/// of a pitcher
/// </summary>
/// <param name="isLowerStatBetter">True if a lower value for the stat means better performance</param>
public sealed class LowerStatIsBetterAttribute(bool isLowerStatBetter) : Attribute
{
    /// <summary>
    /// True if a lower value for the stat means better performance
    /// </summary>
    public bool IsLowerStatBetter { get; } = isLowerStatBetter;
}