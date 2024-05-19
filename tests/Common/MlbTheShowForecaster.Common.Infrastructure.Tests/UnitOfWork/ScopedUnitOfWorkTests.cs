using com.brettnamba.MlbTheShowForecaster.Common.Domain.SeedWork;
using com.brettnamba.MlbTheShowForecaster.Common.Domain.SeedWork.Exceptions;
using com.brettnamba.MlbTheShowForecaster.Common.Infrastructure.UnitOfWork;
using Microsoft.Extensions.DependencyInjection;
using Moq;

namespace com.brettnamba.MlbTheShowForecaster.Common.Infrastructure.Tests.UnitOfWork;

public class ScopedUnitOfWorkTests
{
    [Fact]
    public void GetContributor_UnknownTypeParam_ThrowsException()
    {
        // Arrange
        var (stubServiceScopeFactory, stubServiceScope) = MockScope();
        stubServiceScope.Setup(x => x.ServiceProvider.GetService(typeof(ISomeRepository)))
            .Returns(null!);

        var uow = new TestScopedUnitOfWork<ISomeWork>(stubServiceScopeFactory.Object);
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
        stubServiceScope.Setup(x => x.ServiceProvider.GetService(typeof(ISomeRepository)))
            .Returns(mockRepository);

        var uow = new TestScopedUnitOfWork<ISomeWork>(stubServiceScopeFactory.Object);

        // Act
        var actual = uow.GetContributor<ISomeRepository>();

        // Assert
        Assert.IsAssignableFrom<ISomeRepository>(actual);
    }

    private interface ISomeWork : IUnitOfWorkType;

    public interface ISomeRepository;

    private sealed class TestScopedUnitOfWork<T>(IServiceScopeFactory serviceScopeFactory)
        : ScopedUnitOfWork<T>(serviceScopeFactory) where T : IUnitOfWorkType
    {
        public override Task CommitAsync(CancellationToken cancellationToken = default)
        {
            return Task.CompletedTask;
        }
    }

    private (Mock<IServiceScopeFactory> stubServiceScopeFactory, Mock<IServiceScope> stubServiceScope) MockScope()
    {
        var mockServiceScope = new Mock<IServiceScope>();

        var stubServiceScopeFactory = new Mock<IServiceScopeFactory>();
        stubServiceScopeFactory.Setup(x => x.CreateScope())
            .Returns(mockServiceScope.Object);

        return (stubServiceScopeFactory, mockServiceScope);
    }
}