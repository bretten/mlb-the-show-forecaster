using System.ComponentModel.DataAnnotations;
using System.Reflection;
using com.brettnamba.MlbTheShowForecaster.Common.Extensions.Exceptions;

namespace com.brettnamba.MlbTheShowForecaster.Common.Extensions;

/// <summary>
/// Enum extensions
/// </summary>
public static class EnumExtensions
{
    /// <summary>
    /// Gets the name value of the <see cref="DisplayAttribute"/> for the specified enum member
    /// </summary>
    /// <param name="enumValue">The enum member</param>
    /// <returns>The <see cref="DisplayAttribute"/> name value</returns>
    /// <exception cref="EnumDisplayNameNotFoundException">Thrown if there is no <see cref="DisplayAttribute"/></exception>
    public static string? GetDisplayName(this Enum enumValue)
    {
        var displayAttribute = enumValue.GetType()
            .GetMember(enumValue.ToString())
            .First() // Enum member won't be null since it is strongly typed
            .GetCustomAttribute<DisplayAttribute>();
        if (displayAttribute == null)
        {
            throw new EnumDisplayNameNotFoundException($"Display attribute not found for Enum {enumValue}");
        }

        return displayAttribute.GetName();
    }
}