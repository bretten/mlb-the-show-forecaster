namespace com.brettnamba.MlbTheShowForecaster.ExternalApis.MlbApi.Converters.Exceptions;

/// <summary>
/// Thrown when <see cref="StatJsonConverter"/> tries to parse an unknown stat group
/// </summary>
public sealed class UnknownStatGroupException : Exception
{
    public UnknownStatGroupException(string? message) : base(message)
    {
    }
}