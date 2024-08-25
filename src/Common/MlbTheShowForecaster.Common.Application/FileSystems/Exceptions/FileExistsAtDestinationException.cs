namespace com.brettnamba.MlbTheShowForecaster.Common.Application.FileSystems.Exceptions;

/// <summary>
/// Thrown when <see cref="IFileSystem"/> tries to store a file at a path where a file already exists
/// </summary>
public sealed class FileExistsAtDestinationException : Exception
{
    public FileExistsAtDestinationException(string? message) : base(message)
    {
    }
}