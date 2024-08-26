using Amazon.S3;
using com.brettnamba.MlbTheShowForecaster.Common.Application.FileSystems;
using com.brettnamba.MlbTheShowForecaster.Common.Infrastructure.Configuration;
using com.brettnamba.MlbTheShowForecaster.Common.Infrastructure.FileSystems.Amazon;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace com.brettnamba.MlbTheShowForecaster.Common.Infrastructure.FileSystems;

/// <summary>
/// Registers FileSystem module
/// </summary>
public static class Dependencies
{
    /// <summary>
    /// Configuration keys
    /// </summary>
    public static class ConfigKeys
    {
        /// <summary>
        /// File system type
        /// </summary>
        public const string Type = "FileSystem:Type";
    }

    /// <summary>
    /// Registers the FileSystem module
    /// </summary>
    /// <param name="services">The registered services</param>
    /// <param name="config">Configuration for file systems</param>
    public static void AddFileSystems(this IServiceCollection services, IConfiguration config)
    {
        var fileSystemType = config.GetRequiredValue<FileSystemType>(ConfigKeys.Type);
        if (fileSystemType == FileSystemType.AwsS3)
        {
            services.TryAddSingleton<IAmazonS3, AmazonS3Client>();
            services.TryAddSingleton<IFileSystem, AwsS3FileSystem>();
        }
        else
        {
            services.TryAddSingleton<IFileSystem, LocalFileSystem>();
        }
    }
}