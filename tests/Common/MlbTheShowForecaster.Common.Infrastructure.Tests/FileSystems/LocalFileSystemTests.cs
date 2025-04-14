using System.Text;
using com.brettnamba.MlbTheShowForecaster.Common.Application.FileSystems.Exceptions;
using com.brettnamba.MlbTheShowForecaster.Common.Infrastructure.FileSystems;

namespace com.brettnamba.MlbTheShowForecaster.Common.Infrastructure.Tests.FileSystems;

public class LocalFileSystemTests
{
    [Fact]
    public async Task RetrieveFile_FileDoesNotExist_ThrowsException()
    {
        // Arrange
        const string path = "path.txt";

        var fs = new LocalFileSystem(new LocalFileSystem.Settings(""));

        var action = async () => await fs.RetrieveFile(path);

        // Act
        var actual = await Record.ExceptionAsync(action);

        // Assert
        Assert.NotNull(actual);
        Assert.IsType<NoFileFoundException>(actual);
    }

    [Fact]
    public async Task RetrieveFile_FileExists_ReturnsFile()
    {
        // Arrange
        var path = Path.GetTempFileName();
        await File.WriteAllTextAsync(path, "file content");

        var fs = new LocalFileSystem(new LocalFileSystem.Settings(""));

        // Act
        var actual = await fs.RetrieveFile(path);

        // Assert
        Assert.NotNull(actual);
        Assert.Equal(path, actual.Path);
        Assert.Equal("file content", Encoding.UTF8.GetString(actual.Content));
    }

    [Fact]
    public async Task StoreFile_DestinationFileExists_ThrowsException()
    {
        // Arrange
        var stream = new MemoryStream(Encoding.UTF8.GetBytes("file content"));
        var destination = Path.GetTempFileName();
        const bool overwrite = false;

        var fs = new LocalFileSystem(new LocalFileSystem.Settings(""));

        var action = async () => await fs.StoreFile(stream, destination, overwrite);

        // Act
        var actual = await Record.ExceptionAsync(action);

        // Assert
        Assert.NotNull(actual);
        Assert.IsType<FileExistsAtDestinationException>(actual);
    }

    [Fact]
    public async Task StoreFile_Stream_OverwritesFile()
    {
        // Arrange
        var stream = new MemoryStream(Encoding.UTF8.GetBytes("file content"));
        var destination = Path.GetTempFileName();
        const bool overwrite = true;

        var fs = new LocalFileSystem(new LocalFileSystem.Settings(""));

        // Act
        await fs.StoreFile(stream, destination, overwrite);

        // Assert
        var file = new FileInfo(destination);
        Assert.True(file.Exists);
        Assert.Equal("file content", await File.ReadAllTextAsync(destination));
    }
}