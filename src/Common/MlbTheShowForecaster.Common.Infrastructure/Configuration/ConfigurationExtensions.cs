using com.brettnamba.MlbTheShowForecaster.Common.Infrastructure.Configuration.Exceptions;
using Microsoft.Extensions.Configuration;

namespace com.brettnamba.MlbTheShowForecaster.Common.Infrastructure.Configuration;

/// <summary>
/// Extensions for <see cref="IConfiguration"/>
/// </summary>
public static class ConfigurationExtensions
{
    /// <summary>
    /// Requires that the key and value be set when attempting to get the value, otherwise an exception of type
    /// <see cref="ConfigurationNotSetException"/> will be thrown
    /// </summary>
    /// <param name="configuration">The current <see cref="IConfiguration"/></param>
    /// <param name="key">The key of the value</param>
    /// <typeparam name="T">The type of the value</typeparam>
    /// <returns>The value</returns>
    /// <exception cref="ConfigurationNotSetException">Thrown if the key or value is not set</exception>
    public static T GetRequiredValue<T>(this IConfiguration configuration, string key)
    {
        var section = configuration.GetSection(key);
        if (!section.Exists())
        {
            throw new ConfigurationNotSetException($"ConfigurationSection does not exist for key {key}");
        }

        var value = section.Get<T>();
        if (value == null || (value is string s && string.IsNullOrWhiteSpace(s)))
        {
            throw new ConfigurationNotSetException($"Configuration value not set for key {key}");
        }

        return value;
    }

    /// <summary>
    /// Requires that the key and connection string be set, otherwise an exception of type
    /// <see cref="ConfigurationNotSetException"/> will be thrown
    /// </summary>
    /// <param name="configuration">The current <see cref="IConfiguration"/></param>
    /// <param name="key">The key for the "ConnectionStrings" section</param>
    /// <returns>Connection string</returns>
    /// <exception cref="ConfigurationNotSetException">Thrown if the key or connection string is not set</exception>
    public static string GetRequiredConnectionString(this IConfiguration configuration, string key)
    {
        var section = configuration.GetSection("ConnectionStrings");
        if (!section.Exists())
        {
            throw new ConfigurationNotSetException("ConnectionStrings section does not exist");
        }

        var value = configuration.GetConnectionString(key);
        if (string.IsNullOrWhiteSpace(value))
        {
            throw new ConfigurationNotSetException($"ConnectionString not set for key {key}");
        }

        return value;
    }
}