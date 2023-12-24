namespace com.brettnamba.MlbTheShowForecaster.Common.Extensions.Exceptions;

/// <summary>
/// Thrown when <see cref="EnumExtensions"/>.GetDisplayName() cannot find a display name on the attribute
/// </summary>
public class UndefinedEnumDisplayNameException : Exception
{
    public UndefinedEnumDisplayNameException(string? message) : base(message)
    {
    }
}