namespace com.brettnamba.MlbTheShowForecaster.ExternalApis.MlbApi.Converters.Exceptions;

/// <summary>
/// Thrown when <see cref="StatJsonConverter"/> tries to serialize
/// </summary>
public sealed class JsonConverterWriteNotImplementedException : Exception
{
    public JsonConverterWriteNotImplementedException(string? message) : base(message)
    {
    }
}