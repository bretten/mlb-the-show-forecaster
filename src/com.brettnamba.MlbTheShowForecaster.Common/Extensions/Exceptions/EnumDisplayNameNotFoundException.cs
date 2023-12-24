using System.ComponentModel.DataAnnotations;

namespace com.brettnamba.MlbTheShowForecaster.Common.Extensions.Exceptions;

/// <summary>
/// Thrown when <see cref="EnumExtensions"/> cannot find a <see cref="DisplayAttribute"/>
/// on an enum member
/// </summary>
public sealed class EnumDisplayNameNotFoundException : Exception
{
    public EnumDisplayNameNotFoundException(string? message) : base(message)
    {
    }
}