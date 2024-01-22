using com.brettnamba.MlbTheShowForecaster.Common.Domain.Events;
using com.brettnamba.MlbTheShowForecaster.Common.Infrastructure.EntityFramework;
using com.brettnamba.MlbTheShowForecaster.Common.Infrastructure.Tests.EntityFramework.TestClasses;
using Microsoft.EntityFrameworkCore;
using Moq;

namespace com.brettnamba.MlbTheShowForecaster.Common.Infrastructure.Tests.EntityFramework;

public class UnitOfWorkTests
{
    [Fact]
    public async Task CommitAsync_NoParams_SaveChangesAsyncInvoked()
    {
        // Arrange
        var cToken = CancellationToken.None;

        var fakeEntity = TestEntity.Create(1, "A");
        var fakeDomainEvent = new TestEntityCreatedEvent();
        fakeEntity.AddDomainEvent(fakeDomainEvent);

        var dbContext = GetTestDbContext(nameof(CommitAsync_NoParams_SaveChangesAsyncInvoked));
        await dbContext.TestEntities.AddAsync(fakeEntity, cToken);

        var mockDomainEventDispatcher = Mock.Of<IDomainEventDispatcher>();
        var uow = new UnitOfWork<TestDbContext>(dbContext, mockDomainEventDispatcher);

        // Act
        await uow.CommitAsync(cToken);

        // Assert
        Assert.Equal(fakeEntity, dbContext.TestEntities.First());
        Mock.Get(mockDomainEventDispatcher)
            .Verify(x => x.Dispatch(It.Is<IList<IDomainEvent>>(i => i.Contains(fakeDomainEvent) && i.Count == 1)),
                Times.Once);
        Assert.Equal(0, fakeEntity.DomainEvents.Count);
    }

    [Fact]
    public void Dispose_NoParams_DbContextDisposed()
    {
        // Arrange
        var mockDbContext = Mock.Of<TestDbContext>();
        var mockDomainEventDispatcher = Mock.Of<IDomainEventDispatcher>();
        var uow = new UnitOfWork<TestDbContext>(mockDbContext, mockDomainEventDispatcher);

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
        var mockDomainEventDispatcher = Mock.Of<IDomainEventDispatcher>();
        var uow = new UnitOfWork<TestDbContext>(mockDbContext, mockDomainEventDispatcher);

        // Act
        await uow.DisposeAsync();

        // Assert
        Mock.Get(mockDbContext).Verify(x => x.DisposeAsync(), Times.Once);
    }

    /// <summary>
    /// <see cref="UnitOfWork{TDbContext}"/> only needs to test if SaveChanges was called and that domain events were
    /// sent. It does not require complex database queries, so an in-memory database is used.
    /// </summary>
    /// <param name="testDbName">The name of the test database</param>
    /// <returns>A DB context for testing</returns>
    private TestDbContext GetTestDbContext(string testDbName)
    {
        var dbContextOptions = new DbContextOptionsBuilder<TestDbContext>()
            .UseInMemoryDatabase(testDbName)
            .Options;
        return new TestDbContext(dbContextOptions);
    }
}