using com.brettnamba.MlbTheShowForecaster.Common.Application.FileSystems.Exceptions;

namespace com.brettnamba.MlbTheShowForecaster.Common.Application.FileSystems;

/// <summary>
/// Defines a file system
/// </summary>
public interface IFileSystem
{
    /// <summary>
    /// Retrieves a file
    /// </summary>
    /// <param name="path">The path</param>
    /// <returns>The file</returns>
    Task<FileItem> RetrieveFile(string path);

    /// <summary>
    /// Stores the stream to the destination path
    /// </summary>
    /// <param name="stream">The stream to store</param>
    /// <param name="destinationPath">The destination path</param>
    /// <param name="overwrite">True to overwrite the existing file</param>
    /// <returns>The completed task</returns>
    /// <exception cref="FileExistsAtDestinationException">Thrown when a file already exists at the destination</exception>
    Task StoreFile(Stream stream, string destinationPath, bool overwrite = false);
}