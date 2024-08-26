namespace com.brettnamba.MlbTheShowForecaster.Common.Application.FileSystems;

/// <summary>
/// Represents a file
/// </summary>
public sealed class FileItem
{
    /// <summary>
    /// The path of the file
    /// </summary>
    public string Path { get; }

    /// <summary>
    /// The content of the file
    /// </summary>
    public byte[] Content { get; }

    /// <summary>
    /// Constructor
    /// </summary>
    /// <param name="path">The path of the file</param>
    /// <param name="content">The content of the file</param>
    public FileItem(string path, byte[] content)
    {
        Path = path;
        Content = content;
    }
}