using com.brettnamba.MlbTheShowForecaster.Common.Application.FileSystems;
using com.brettnamba.MlbTheShowForecaster.Common.Application.FileSystems.Exceptions;

namespace com.brettnamba.MlbTheShowForecaster.Common.Infrastructure.FileSystems;

/// <summary>
/// Represents a local file system. .NET abstracts Windows and Unix, so it is compatible with both
/// </summary>
public sealed class LocalFileSystem : IFileSystem
{
    /// <inheritdoc />
    public async Task<FileItem> RetrieveFile(string path)
    {
        if (!File.Exists(path))
        {
            throw new NoFileFoundException($"File not found at: {path}");
        }

        return new FileItem(path, await File.ReadAllBytesAsync(path));
    }

    /// <inheritdoc />
    public async Task StoreFile(Stream stream, string destinationPath, bool overwrite = false)
    {
        if (!overwrite && File.Exists(destinationPath))
        {
            throw new FileExistsAtDestinationException($"File already exists at {destinationPath}");
        }

        // Create the directories if they don't exist
        new FileInfo(destinationPath).Directory?.Create();

        await using var fileStream = new FileStream(destinationPath, FileMode.Create, FileAccess.Write);
        await stream.CopyToAsync(fileStream);
    }
}