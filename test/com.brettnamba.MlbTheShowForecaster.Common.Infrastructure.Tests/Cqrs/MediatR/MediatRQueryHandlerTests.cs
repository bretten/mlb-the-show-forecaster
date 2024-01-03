using com.brettnamba.MlbTheShowForecaster.Common.Application.Cqrs;
using com.brettnamba.MlbTheShowForecaster.Common.Infrastructure.Cqrs.MediatR;
using com.brettnamba.MlbTheShowForecaster.Common.Infrastructure.Tests.Cqrs.TestClasses;
using Moq;

namespace com.brettnamba.MlbTheShowForecaster.Common.Infrastructure.Tests.Cqrs.MediatR;

public class MediatRQueryHandlerTests
{
    [Fact]
    public async Task Handle_WrappedQuery_QueryDelegatedToUnderlyingHandler()
    {
        // Arrange
        var cToken = CancellationToken.None;
        var query = new TestQuery();
        var expected = new TestResponse();

        var mockUnderlyingHandler =
            Mock.Of<IQueryHandler<TestQuery, TestResponse>>(x => x.Handle(query, cToken) == Task.FromResult(expected));

        var queryWrapper = new MediatRQuery<TestQuery, TestResponse>(query);
        var handlerWrapper = new MediatRQueryHandler<TestQuery, TestResponse>(mockUnderlyingHandler);

        // Act
        var actual = await handlerWrapper.Handle(queryWrapper, cToken);

        // Assert
        Mock.Get(mockUnderlyingHandler).Verify(x => x.Handle(query, cToken), Times.Once);
        Assert.Equal(expected, actual);
    }
}