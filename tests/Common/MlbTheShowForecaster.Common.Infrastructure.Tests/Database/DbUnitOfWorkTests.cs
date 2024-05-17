using System.Data.Common;
using com.brettnamba.MlbTheShowForecaster.Common.Domain.SeedWork;
using com.brettnamba.MlbTheShowForecaster.Common.Domain.SeedWork.Exceptions;
using com.brettnamba.MlbTheShowForecaster.Common.Infrastructure.Database;
using Microsoft.Extensions.DependencyInjection;
using Moq;

namespace com.brettnamba.MlbTheShowForecaster.Common.Infrastructure.Tests.Database;

public class DbUnitOfWorkTests
{
    [Fact]
    public void GetContributor_UnknownTypeParam_ThrowsException()
    {
        // Arrange
        var (stubServiceScopeFactory, stubServiceScope) = MockScope();
        stubServiceScope.Setup(x => x.ServiceProvider.GetService(typeof(IAtomicDatabaseOperation)))
            .Returns(Mock.Of<IAtomicDatabaseOperation>());
        stubServiceScope.Setup(x => x.ServiceProvider.GetService(typeof(ISomeRepository)))
            .Returns(null!);

        var uow = new DbUnitOfWork<IDbUnitOfWork>(stubServiceScopeFactory.Object);
        var action = () => uow.GetContributor<ISomeRepository>();

        // Act
        var actual = Record.Exception(action);

        // Assert
        Assert.NotNull(actual);
        Assert.IsType<UnitOfWorkContributorNotFoundException>(actual);
    }

    [Fact]
    public void GetContributor_TypeParam_ReturnsContributorOfType()
    {
        // Arrange
        var mockRepository = Mock.Of<ISomeRepository>();

        var (stubServiceScopeFactory, stubServiceScope) = MockScope();
        stubServiceScope.Setup(x => x.ServiceProvider.GetService(typeof(IAtomicDatabaseOperation)))
            .Returns(Mock.Of<IAtomicDatabaseOperation>());
        stubServiceScope.Setup(x => x.ServiceProvider.GetService(typeof(ISomeRepository)))
            .Returns(mockRepository);

        var uow = new DbUnitOfWork<IDbUnitOfWork>(stubServiceScopeFactory.Object);

        // Act
        var actual = uow.GetContributor<ISomeRepository>();

        // Assert
        Assert.IsAssignableFrom<ISomeRepository>(actual);
    }

    [Fact]
    public async Task CommitAsync_NoParams_SaveChangesAsyncInvoked()
    {
        // Arrange
        var cToken = CancellationToken.None;

        var mockDbTransaction = Mock.Of<DbTransaction>();

        var stubAtomicDatabaseOperation = new Mock<IAtomicDatabaseOperation>();
        stubAtomicDatabaseOperation.Setup(x => x.GetCurrentActiveTransaction(cToken))
            .ReturnsAsync(mockDbTransaction);

        var (stubServiceScopeFactory, stubServiceScope) = MockScope();
        stubServiceScope.Setup(x => x.ServiceProvider.GetService(typeof(IAtomicDatabaseOperation)))
            .Returns(stubAtomicDatabaseOperation.Object);

        var uow = new DbUnitOfWork<IDbUnitOfWork>(stubServiceScopeFactory.Object);

        // Act
        await uow.CommitAsync(cToken);

        // Assert
        Mock.Get(mockDbTransaction).Verify(x => x.CommitAsync(cToken), Times.Once);
    }

    [Fact]
    public void Dispose_NoParams_TransactionDisposed()
    {
        // Arrange
        var stubAtomicDatabaseOperation = new Mock<IAtomicDatabaseOperation>();

        var (stubServiceScopeFactory, stubServiceScope) = MockScope();
        stubServiceScope.Setup(x => x.ServiceProvider.GetService(typeof(IAtomicDatabaseOperation)))
            .Returns(stubAtomicDatabaseOperation.Object);

        var uow = new DbUnitOfWork<IDbUnitOfWork>(stubServiceScopeFactory.Object);

        // Act
        uow.Dispose();

        // Assert
        stubAtomicDatabaseOperation.Verify(x => x.Dispose(), Times.Once);
        stubServiceScope.Verify(x => x.Dispose(), Times.Once);
    }

    [Fact]
    public async Task DisposeAsync_NoParams_TransactionDisposed()
    {
        // Arrange
        var stubAtomicDatabaseOperation = new Mock<IAtomicDatabaseOperation>();

        var (stubServiceScopeFactory, stubServiceScope) = MockScope();
        stubServiceScope.Setup(x => x.ServiceProvider.GetService(typeof(IAtomicDatabaseOperation)))
            .Returns(stubAtomicDatabaseOperation.Object);

        var uow = new DbUnitOfWork<IDbUnitOfWork>(stubServiceScopeFactory.Object);

        // Act
        await uow.DisposeAsync();

        // Assert
        stubAtomicDatabaseOperation.Verify(x => x.DisposeAsync(), Times.Once);
        stubServiceScope.Verify(x => x.Dispose(), Times.Once);
    }

    private interface IDbUnitOfWork : IUnitOfWorkType;

    public interface ISomeRepository;

    private (Mock<IServiceScopeFactory> stubServiceScopeFactory, Mock<IServiceScope> stubServiceScope) MockScope()
    {
        var mockServiceScope = new Mock<IServiceScope>();

        var stubServiceScopeFactory = new Mock<IServiceScopeFactory>();
        stubServiceScopeFactory.Setup(x => x.CreateScope())
            .Returns(mockServiceScope.Object);

        return (stubServiceScopeFactory, mockServiceScope);
    }
}