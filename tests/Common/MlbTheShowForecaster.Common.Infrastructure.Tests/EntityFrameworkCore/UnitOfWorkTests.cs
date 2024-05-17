using com.brettnamba.MlbTheShowForecaster.Common.Domain.Events;
using com.brettnamba.MlbTheShowForecaster.Common.Domain.SeedWork.Exceptions;
using com.brettnamba.MlbTheShowForecaster.Common.Infrastructure.EntityFrameworkCore;
using com.brettnamba.MlbTheShowForecaster.Common.Infrastructure.Tests.EntityFrameworkCore.TestClasses;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Moq;

namespace com.brettnamba.MlbTheShowForecaster.Common.Infrastructure.Tests.EntityFrameworkCore;

public class UnitOfWorkTests
{
    [Fact]
    public void GetContributor_UnknownTypeParam_ThrowsException()
    {
        // Arrange
        var mockDbContext = Mock.Of<TestDbContext>();

        var (stubServiceScopeFactory, stubServiceScope) = MockScope();
        stubServiceScope.Setup(x => x.ServiceProvider.GetService(typeof(TestDbContext)))
            .Returns(mockDbContext);
        stubServiceScope.Setup(x => x.ServiceProvider.GetService(typeof(ISomeRepository)))
            .Returns(null!);

        var uow = new UnitOfWork<TestDbContext>(Mock.Of<IDomainEventDispatcher>(), stubServiceScopeFactory.Object);
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
        var mockDbContext = Mock.Of<TestDbContext>();
        var mockRepository = Mock.Of<ISomeRepository>();

        var (stubServiceScopeFactory, stubServiceScope) = MockScope();
        stubServiceScope.Setup(x => x.ServiceProvider.GetService(typeof(TestDbContext)))
            .Returns(mockDbContext);
        stubServiceScope.Setup(x => x.ServiceProvider.GetService(typeof(ISomeRepository)))
            .Returns(mockRepository);

        var uow = new UnitOfWork<TestDbContext>(Mock.Of<IDomainEventDispatcher>(), stubServiceScopeFactory.Object);

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

        var fakeEntity = TestEntity.Create(1, "A");
        var fakeDomainEvent = new TestEntityCreatedEvent();
        fakeEntity.AddDomainEvent(fakeDomainEvent);

        var dbContext = GetTestDbContext(nameof(CommitAsync_NoParams_SaveChangesAsyncInvoked));
        await dbContext.TestEntities.AddAsync(fakeEntity, cToken);

        var (stubServiceScopeFactory, stubServiceScope) = MockScope();
        stubServiceScope.Setup(x => x.ServiceProvider.GetService(typeof(TestDbContext)))
            .Returns(dbContext);

        var mockDomainEventDispatcher = Mock.Of<IDomainEventDispatcher>();
        var uow = new UnitOfWork<TestDbContext>(mockDomainEventDispatcher, stubServiceScopeFactory.Object);

        // Act
        await uow.CommitAsync(cToken);

        // Assert
        var assertDbContext = GetTestDbContext(nameof(CommitAsync_NoParams_SaveChangesAsyncInvoked));
        Assert.Equal(fakeEntity, assertDbContext.TestEntities.First());
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

        var (stubServiceScopeFactory, stubServiceScope) = MockScope();
        stubServiceScope.Setup(x => x.ServiceProvider.GetService(typeof(TestDbContext)))
            .Returns(mockDbContext);

        var mockDomainEventDispatcher = Mock.Of<IDomainEventDispatcher>();
        var uow = new UnitOfWork<TestDbContext>(mockDomainEventDispatcher, stubServiceScopeFactory.Object);

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

        var (stubServiceScopeFactory, stubServiceScope) = MockScope();
        stubServiceScope.Setup(x => x.ServiceProvider.GetService(typeof(TestDbContext)))
            .Returns(mockDbContext);

        var mockDomainEventDispatcher = Mock.Of<IDomainEventDispatcher>();
        var uow = new UnitOfWork<TestDbContext>(mockDomainEventDispatcher, stubServiceScopeFactory.Object);

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