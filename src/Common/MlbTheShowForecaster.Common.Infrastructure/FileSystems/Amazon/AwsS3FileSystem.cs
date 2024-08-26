using System.Net;
using Amazon.S3;
using Amazon.S3.Model;
using com.brettnamba.MlbTheShowForecaster.Common.Application.FileSystems;
using com.brettnamba.MlbTheShowForecaster.Common.Application.FileSystems.Exceptions;

namespace com.brettnamba.MlbTheShowForecaster.Common.Infrastructure.FileSystems.Amazon;

/// <summary>
/// Represents a S3 file system
/// </summary>
public sealed class AwsS3FileSystem : IFileSystem
{
    /// <summary>
    /// S3 client
    /// </summary>
    private readonly IAmazonS3 _s3;

    /// <summary>
    /// The bucket that represents this file system
    /// </summary>
    private readonly string _bucketName;

    /// <summary>
    /// Constructor
    /// </summary>
    /// <param name="s3">S3 client</param>
    /// <param name="bucketName">The bucket that represents this file system</param>
    public AwsS3FileSystem(IAmazonS3 s3, string bucketName)
    {
        _s3 = s3;
        _bucketName = bucketName;
    }

    /// <inheritdoc />
    public async Task<FileItem> RetrieveFile(string path)
    {
        var request = new GetObjectRequest()
        {
            BucketName = _bucketName,
            Key = path
        };

        var response = await _s3.GetObjectAsync(request);
        if (response == null || response.HttpStatusCode == HttpStatusCode.NotFound)
        {
            throw new NoFileFoundException($"No S3 object found at key: {path} in bucket {_bucketName}");
        }

        using var memoryStream = new MemoryStream();
        await response.ResponseStream.CopyToAsync(memoryStream);
        var bytes = memoryStream.ToArray();

        return new FileItem(path, bytes);
    }

    /// <inheritdoc />
    public async Task StoreFile(Stream stream, string destinationPath, bool overwrite = false)
    {
        if (!overwrite && !await ObjectExists(destinationPath))
        {
            throw new FileExistsAtDestinationException($"S3 object exists at key: {destinationPath}");
        }

        var request = new PutObjectRequest
        {
            BucketName = _bucketName,
            Key = destinationPath,
            InputStream = stream,
            ServerSideEncryptionMethod = ServerSideEncryptionMethod.AES256
        };

        await _s3.PutObjectAsync(request);
    }

    /// <summary>
    /// Checks if an object exists at the specified key
    /// </summary>
    /// <param name="key">The key to check</param>
    /// <returns>True if an object exists, otherwise false</returns>
    private async Task<bool> ObjectExists(string key)
    {
        var request = new GetObjectMetadataRequest()
        {
            BucketName = _bucketName,
            Key = key
        };
        var response = await _s3.GetObjectMetadataAsync(request);
        return response != null && response.HttpStatusCode != HttpStatusCode.NotFound;
    }
}