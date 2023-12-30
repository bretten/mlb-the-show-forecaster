using com.brettnamba.MlbTheShowForecaster.Common.Infrastructure.EntityFramework;
using com.brettnamba.MlbTheShowForecaster.Common.Infrastructure.Tests.EntityFramework.TestClasses;
using Moq;

namespace com.brettnamba.MlbTheShowForecaster.Common.Infrastructure.Tests.EntityFramework;

public class UnitOfWorkTests
{
    [Fact]
    public async Task CommitAsync_NoParams_SaveChangesAsyncInvoked()
    {
        // Arrange
        var cToken = CancellationToken.None;
        var mockDbContext = Mock.Of<TestDbContext>(x => x.SaveChangesAsync(cToken) == Task.FromResult(1));
        var uow = new UnitOfWork<TestDbContext>(mockDbContext);

        // Act
        await uow.CommitAsync(cToken);

        // Assert
        Mock.Get(mockDbContext).Verify(x => x.SaveChangesAsync(cToken), Times.Once);
    }

    [Fact]
    public void Dispose_NoParams_DbContextDisposed()
    {
        // Arrange
        var mockDbContext = Mock.Of<TestDbContext>();
        var uow = new UnitOfWork<TestDbContext>(mockDbContext);

        // Act
        uow.Dispose();

        // Assert
        Mock.Get(mockDbContext).Verify(x => x.Dispose(), Times.Once);
    }

    [Fact]
    public async Task DisposeAsync_NoParams_DbContextDisposed()
    {
        // Arrange
        var mockDbContext = Mock.Of<TestDbContext>();
        var uow = new UnitOfWork<TestDbContext>(mockDbContext);

        // Act
        await uow.DisposeAsync();

        // Assert
        Mock.Get(mockDbContext).Verify(x => x.DisposeAsync(), Times.Once);
    }
}