namespace com.brettnamba.MlbTheShowForecaster.Common.Infrastructure.Configuration.Exceptions;

/// <summary>
/// Thrown when <see cref="ConfigurationExtensions.GetRequiredValue{T}"/> cannot find the specified key or value for
/// that key
/// </summary>
public sealed class ConfigurationNotSetException : Exception
{
    public ConfigurationNotSetException(string? message) : base(message)
    {
    }
}