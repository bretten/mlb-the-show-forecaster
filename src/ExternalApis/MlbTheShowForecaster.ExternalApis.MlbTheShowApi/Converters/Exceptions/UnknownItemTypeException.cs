namespace com.brettnamba.MlbTheShowForecaster.ExternalApis.MlbTheShowApi.Converters.Exceptions;

/// <summary>
/// Thrown when <see cref="ItemJsonConverter"/> cannot determine an Item's type
/// </summary>
public sealed class UnknownItemTypeException : Exception
{
    public UnknownItemTypeException(string? message) : base(message)
    {
    }
}