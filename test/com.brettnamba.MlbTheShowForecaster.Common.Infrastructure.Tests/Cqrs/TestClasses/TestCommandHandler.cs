using com.brettnamba.MlbTheShowForecaster.Common.Application.Cqrs;

namespace com.brettnamba.MlbTheShowForecaster.Common.Infrastructure.Tests.Cqrs.TestClasses;

/// <summary>
/// Test command handler
/// </summary>
public sealed class TestCommandHandler : ICommandHandler<TestCommand>
{
    public Task Handle(TestCommand command, CancellationToken cancellationToken = default)
    {
        return Task.CompletedTask;
    }
}