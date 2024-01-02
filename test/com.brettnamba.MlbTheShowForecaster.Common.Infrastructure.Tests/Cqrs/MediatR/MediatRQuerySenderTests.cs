using com.brettnamba.MlbTheShowForecaster.Common.Infrastructure.Cqrs.MediatR;
using com.brettnamba.MlbTheShowForecaster.Common.Infrastructure.Tests.Cqrs.TestClasses;
using MediatR;
using Moq;

namespace com.brettnamba.MlbTheShowForecaster.Common.Infrastructure.Tests.Cqrs.MediatR;

public class MediatRQuerySenderTests
{
    [Fact]
    public async Task Send_Query_MediatRSendsWrappedQuery()
    {
        // Arrange
        var cToken = CancellationToken.None;
        var query = new TestQuery();
        var expected = new TestResponse();
        var wrappedQuery = new MediatRQuery<TestQuery, TestResponse>(query);
        var mockMediatR = Mock.Of<IMediator>(x => x.Send(wrappedQuery, cToken) == Task.FromResult(expected));
        var querySender = new MediatRQuerySender(mockMediatR);

        // Act
        var actual = await querySender.Send(query, cToken);

        // Assert
        Mock.Get(mockMediatR).Verify(x => x.Send(wrappedQuery, cToken), Times.Once);
        Assert.Equal(expected, actual);
    }
}