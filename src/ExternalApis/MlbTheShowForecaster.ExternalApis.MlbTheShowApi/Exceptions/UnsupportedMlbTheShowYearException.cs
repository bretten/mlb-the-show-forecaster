namespace com.brettnamba.MlbTheShowForecaster.ExternalApis.MlbTheShowApi.Exceptions;

/// <summary>
/// Thrown when an <see cref="IMlbTheShowApiFactory"/> cannot create a factory for the specified year
/// </summary>
public sealed class UnsupportedMlbTheShowYearException : Exception
{
    public UnsupportedMlbTheShowYearException(string? message) : base(message)
    {
    }
}