using System.Text;
using com.brettnamba.MlbTheShowForecaster.Common.Application.FileSystems.Exceptions;
using com.brettnamba.MlbTheShowForecaster.Common.Infrastructure.FileSystems;

namespace com.brettnamba.MlbTheShowForecaster.Common.Infrastructure.Tests.FileSystems;

public class LocalFileSystemTests
{
    private static readonly string SourceFile =
        $"FileSystems{Path.DirectorySeparatorChar}TestFiles{Path.DirectorySeparatorChar}sourceFile.txt";

    private static readonly string DestinationFile =
        $"FileSystems{Path.DirectorySeparatorChar}TestFiles{Path.DirectorySeparatorChar}destinationFile.txt";

    [Fact]
    public async Task RetrieveFile_FileDoesNotExist_ThrowsException()
    {
        // Arrange
        const string source = "source.txt";

        var fs = new LocalFileSystem();

        var action = async () => await fs.RetrieveFile(source);

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
        var source = SourceFile;

        var fs = new LocalFileSystem();

        // Act
        var actual = await fs.RetrieveFile(source);

        // Assert
        Assert.NotNull(actual);
        Assert.Equal(SourceFile, actual.Path);
        Assert.Equal("source", Encoding.UTF8.GetString(actual.Content));
    }

    [Fact]
    public async Task StoreFile_DestinationFileExists_ThrowsException()
    {
        // Arrange
        var source = SourceFile;
        var destination = DestinationFile;
        const bool overwrite = false;

        var fs = new LocalFileSystem();

        var action = async () => await fs.StoreFile(source, destination, overwrite);

        // Act
        var actual = await Record.ExceptionAsync(action);

        // Assert
        Assert.NotNull(actual);
        Assert.IsType<FileExistsAtDestinationException>(actual);
    }

    [Fact]
    public async Task StoreFile_SourceFileDoesNotExist_ThrowsException()
    {
        // Arrange
        const string source = "doesNotExist.txt";
        var destination = DestinationFile;
        const bool overwrite = true;

        var fs = new LocalFileSystem();

        var action = async () => await fs.StoreFile(source, destination, overwrite);

        // Act
        var actual = await Record.ExceptionAsync(action);

        // Assert
        Assert.NotNull(actual);
        Assert.IsType<NoFileFoundException>(actual);
    }

    [Fact]
    public async Task StoreFile_ValidSourceFile_OverwritesFile()
    {
        // Arrange
        var source = SourceFile;
        var destination = DestinationFile;
        const bool overwrite = true;

        var fs = new LocalFileSystem();

        // Act
        await fs.StoreFile(source, destination, overwrite);

        // Assert
        var file = new FileInfo(DestinationFile);
        Assert.True(file.Exists);
        Assert.Equal(await File.ReadAllTextAsync(source), await File.ReadAllTextAsync(destination));
    }
}