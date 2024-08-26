namespace com.brettnamba.MlbTheShowForecaster.Common.Application.FileSystems.Exceptions;

/// <summary>
/// Thrown when <see cref="IFileSystem"/> cannot find a file
/// </summary>
public sealed class NoFileFoundException : Exception
{
    public NoFileFoundException(string? message) : base(message)
    {
    }
}