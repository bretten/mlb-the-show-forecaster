using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using com.brettnamba.MlbTheShowForecaster.Common.Converters.Exceptions;

namespace com.brettnamba.MlbTheShowForecaster.Common.Converters;

/// <summary>
/// Converts an enum to and from the value of its <see cref="DisplayAttribute"/> name
/// </summary>
public class EnumDisplayNameConverter : EnumConverter
{
    /// <summary>
    /// Holds the mapping of a <see cref="DisplayAttribute"/> name value to the corresponding enum member
    /// </summary>
    private readonly Dictionary<string, Enum> _stringToEnumMap = new();

    /// <summary>
    /// Constructor
    /// </summary>
    /// <param name="type">The Enum type for the Converter</param>
    public EnumDisplayNameConverter(Type type) : base(type)
    {
        BuildMap();
    }

    /// <summary>
    /// Converts from the specified string to the corresponding enum value
    /// </summary>
    /// <param name="context">Type descriptor context</param>
    /// <param name="culture">Culture info</param>
    /// <param name="value">The value to convert</param>
    /// <returns>The corresponding enum value</returns>
    /// <exception cref="CannotConvertByDisplayNameException">Thrown if there is no enum member that corresponds to the string value</exception>
    /// <exception cref="InvalidEnumDisplayNameConvertFromArgumentException">Thrown if the value is not of type string</exception>
    public override object? ConvertFrom(ITypeDescriptorContext? context, CultureInfo? culture, object value)
    {
        if (value is string strValue)
        {
            if (_stringToEnumMap.TryGetValue(strValue, out var enumMember))
            {
                return enumMember;
            }

            throw new CannotConvertByDisplayNameException($"String {strValue} is not compatible with {EnumType.Name}");
        }

        throw new InvalidEnumDisplayNameConvertFromArgumentException(
            $"{GetType().Name} cannot convert type {value.GetType()} to enum by DisplayAttribute");
    }

    /// <summary>
    /// Builds a mapping of the enum member's <see cref="DisplayAttribute"/> name value to the enum member. This
    /// provides a quick look-up based on the <see cref="DisplayAttribute"/> name
    /// </summary>
    private void BuildMap()
    {
        foreach (Enum val in Enum.GetValues(EnumType))
        {
            // Get the FieldInfo so we can check its attributes
            var fi = EnumType.GetField(val.ToString());

            // Get the DisplayAttribute
            var displayAttribute = ((DisplayAttribute[])fi?.GetCustomAttributes(typeof(DisplayAttribute), false)!)
                .FirstOrDefault();
            if (displayAttribute?.Name == null)
            {
                continue;
            }

            _stringToEnumMap.Add(displayAttribute.Name, val);
        }
    }
}