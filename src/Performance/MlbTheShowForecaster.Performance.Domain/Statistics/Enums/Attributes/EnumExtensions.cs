using System.Reflection;

namespace com.brettnamba.MlbTheShowForecaster.Performance.Domain.Statistics.Enums.Attributes;

/// <summary>
/// Enum extensions
/// </summary>
public static class EnumExtensions
{
    private static readonly Type BattingStatEnumType = typeof(BattingStat);
    private static readonly Type PitchingStatEnumType = typeof(PitchingStat);
    private static readonly Type FieldingStatEnumType = typeof(FieldingStat);

    /// <summary>
    /// Returns true if the <see cref="BattingStat"/> is better when its value is lower
    /// </summary>
    /// <param name="stat"><see cref="BattingStat"/></param>
    /// <returns>True if the <see cref="BattingStat"/> is better when its value is lower</returns>
    public static bool IsLowerStatBetter(this BattingStat stat)
    {
        var attribute = GetLowerStatIsBetterAttribute(BattingStatEnumType, stat.ToString());
        return attribute?.IsLowerStatBetter ?? false;
    }

    /// <summary>
    /// Returns true if the <see cref="PitchingStat"/> is better when its value is lower
    /// </summary>
    /// <param name="stat"><see cref="PitchingStat"/></param>
    /// <returns>True if the <see cref="PitchingStat"/> is better when its value is lower</returns>
    public static bool IsLowerStatBetter(this PitchingStat stat)
    {
        var attribute = GetLowerStatIsBetterAttribute(PitchingStatEnumType, stat.ToString());
        return attribute?.IsLowerStatBetter ?? false;
    }

    /// <summary>
    /// Returns true if the <see cref="FieldingStat"/> is better when its value is lower
    /// </summary>
    /// <param name="stat"><see cref="FieldingStat"/></param>
    /// <returns>True if the <see cref="FieldingStat"/> is better when its value is lower</returns>
    public static bool IsLowerStatBetter(this FieldingStat stat)
    {
        var attribute = GetLowerStatIsBetterAttribute(FieldingStatEnumType, stat.ToString());
        return attribute?.IsLowerStatBetter ?? false;
    }

    /// <summary>
    /// Gets the <see cref="LowerStatIsBetterAttribute"/> for the specified enum type and value
    /// </summary>
    /// <param name="enumType">The enum type</param>
    /// <param name="enumMember">The enum value or member</param>
    /// <returns><see cref="LowerStatIsBetterAttribute"/></returns>
    private static LowerStatIsBetterAttribute? GetLowerStatIsBetterAttribute(Type enumType, string enumMember)
    {
        var lowerStatIsBetterAttribute = enumType
            .GetMember(enumMember)
            .First() // Enum member won't be null since it is strongly typed
            .GetCustomAttribute<LowerStatIsBetterAttribute>();

        return lowerStatIsBetterAttribute;
    }
}