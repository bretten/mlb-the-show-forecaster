using com.brettnamba.MlbTheShowForecaster.Common.Application.Cqrs;

namespace com.brettnamba.MlbTheShowForecaster.Common.Infrastructure.Tests.Cqrs.TestClasses;

/// <summary>
/// Test query handler
/// </summary>
public sealed class TestQueryHandler : IQueryHandler<TestQuery, TestResponse>
{
    public Task<TestResponse?> Handle(TestQuery query, CancellationToken cancellationToken)
    {
        return Task.FromResult(new TestResponse())!;
    }
}