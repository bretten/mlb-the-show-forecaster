using System.Net;
using System.Text;
using Amazon.S3;
using Amazon.S3.Model;
using com.brettnamba.MlbTheShowForecaster.Common.Application.FileSystems.Exceptions;
using com.brettnamba.MlbTheShowForecaster.Common.Infrastructure.FileSystems.Amazon;
using Moq;

namespace com.brettnamba.MlbTheShowForecaster.Common.Infrastructure.Tests.FileSystems.Amazon;

public class AwsS3FileSystemTests
{
    [Fact]
    public async Task RetrieveFile_FileDoesNotExist_ThrowsException()
    {
        // Arrange
        const string bucket = "bucket";
        const string path = "path";
        var settings = new AwsS3FileSystem.Settings(bucket);

        var stubS3 = new Mock<IAmazonS3>();
        stubS3.Setup(x => x.GetObjectAsync(It.Is<GetObjectRequest>(y => y.BucketName == bucket && y.Key == path),
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(new GetObjectResponse()
            {
                HttpStatusCode = HttpStatusCode.NotFound
            });

        var fs = new AwsS3FileSystem(stubS3.Object, settings);

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
        const string bucket = "bucket";
        const string path = "path";
        var settings = new AwsS3FileSystem.Settings(bucket);

        var stubS3 = new Mock<IAmazonS3>();
        stubS3.Setup(x => x.GetObjectAsync(It.Is<GetObjectRequest>(y => y.BucketName == bucket && y.Key == path),
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(new GetObjectResponse()
            {
                ResponseStream = new MemoryStream(Encoding.UTF8.GetBytes("file content"))
            });

        var fs = new AwsS3FileSystem(stubS3.Object, settings);

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
        const string bucket = "bucket";
        const string destinationPath = "dest.txt";
        const bool overwrite = false;
        var settings = new AwsS3FileSystem.Settings(bucket);

        var stubS3 = new Mock<IAmazonS3>();
        stubS3.Setup(x =>
                x.GetObjectMetadataAsync(
                    It.Is<GetObjectMetadataRequest>(y => y.BucketName == bucket && y.Key == destinationPath),
                    It.IsAny<CancellationToken>()))
            .ReturnsAsync(new GetObjectMetadataResponse()
            {
                HttpStatusCode = HttpStatusCode.NotFound
            });

        var fs = new AwsS3FileSystem(stubS3.Object, settings);

        var action = async () => await fs.StoreFile(new MemoryStream(), destinationPath, overwrite);

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
        const string bucket = "bucket";
        const string destinationPath = "dest.txt";
        const bool overwrite = true;
        var settings = new AwsS3FileSystem.Settings(bucket);

        var stubS3 = new Mock<IAmazonS3>();
        stubS3.Setup(x =>
                x.GetObjectMetadataAsync(
                    It.Is<GetObjectMetadataRequest>(y => y.BucketName == bucket && y.Key == destinationPath),
                    It.IsAny<CancellationToken>()))
            .ReturnsAsync(new GetObjectMetadataResponse()
            {
                HttpStatusCode = HttpStatusCode.OK
            });

        var fs = new AwsS3FileSystem(stubS3.Object, settings);

        // Act
        await fs.StoreFile(stream, destinationPath, overwrite);

        // Assert
        stubS3.Verify(
            x => x.PutObjectAsync(
                It.Is<PutObjectRequest>(y =>
                    y.BucketName == bucket && y.Key == destinationPath && y.InputStream == stream),
                It.IsAny<CancellationToken>()), Times.Once);
    }
}