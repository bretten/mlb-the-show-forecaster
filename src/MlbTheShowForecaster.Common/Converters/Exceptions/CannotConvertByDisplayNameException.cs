using System.ComponentModel.DataAnnotations;

namespace com.brettnamba.MlbTheShowForecaster.Common.Converters.Exceptions;

/// <summary>
/// Thrown when <see cref="EnumDisplayNameConverter"/> cannot find a string value that corresponds to an enum member's
/// <see cref="DisplayAttribute"/> name
/// </summary>
public sealed class CannotConvertByDisplayNameException : Exception
{
    public CannotConvertByDisplayNameException(string? message) : base(message)
    {
    }
}