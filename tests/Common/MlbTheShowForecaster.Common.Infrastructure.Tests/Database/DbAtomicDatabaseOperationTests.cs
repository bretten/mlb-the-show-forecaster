using System.Data.Common;
using com.brettnamba.MlbTheShowForecaster.Common.Infrastructure.Database;
using Moq;

namespace com.brettnamba.MlbTheShowForecaster.Common.Infrastructure.Tests.Database;

public class DbAtomicDatabaseOperationTests
{
    [Fact]
    public async Task GetCurrentActiveConnection_NoParams_ReturnsConnection()
    {
        // Arrange
        var cToken = CancellationToken.None;
        var mockConnection = new Mock<DbConnection>();
        
        var stubDbSource = new Mock<DbDataSource>();
        stubDbSource.Setup(x => x.OpenConnectionAsync(cToken))
            .ReturnsAsync(mockConnection.Object);

        var op = new DbAtomicDatabaseOperation(stubDbSource.Object);
        
        // Act
        var actual = await op.GetCurrentActiveConnection(cToken);
        
        // Assert
        Assert.Equal(mockConnection.Object, actual);
    }
}