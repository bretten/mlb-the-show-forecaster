namespace com.brettnamba.MlbTheShowForecaster.Common.Converters.Exceptions;

/// <summary>
/// Thrown when <see cref="EnumDisplayNameConverter"/> is provided an unsupported parameter in ConvertFrom
/// </summary>
public class InvalidEnumDisplayNameConvertFromArgumentException : Exception
{
    public InvalidEnumDisplayNameConvertFromArgumentException(string? message) : base(message)
    {
    }
}