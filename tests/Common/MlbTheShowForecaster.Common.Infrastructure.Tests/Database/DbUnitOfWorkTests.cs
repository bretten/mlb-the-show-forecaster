﻿using System.Data.Common;
using com.brettnamba.MlbTheShowForecaster.Common.Domain.SeedWork;
using com.brettnamba.MlbTheShowForecaster.Common.Infrastructure.Database;
using Microsoft.Extensions.DependencyInjection;
using Moq;

namespace com.brettnamba.MlbTheShowForecaster.Common.Infrastructure.Tests.Database;

public class DbUnitOfWorkTests
{
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

    private (Mock<IServiceScopeFactory> stubServiceScopeFactory, Mock<IServiceScope> stubServiceScope) MockScope()
    {
        var mockServiceScope = new Mock<IServiceScope>();

        var stubServiceScopeFactory = new Mock<IServiceScopeFactory>();
        stubServiceScopeFactory.Setup(x => x.CreateScope())
            .Returns(mockServiceScope.Object);

        return (stubServiceScopeFactory, mockServiceScope);
    }
}