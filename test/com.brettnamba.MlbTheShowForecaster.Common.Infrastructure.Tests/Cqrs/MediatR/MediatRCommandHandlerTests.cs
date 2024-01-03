using com.brettnamba.MlbTheShowForecaster.Common.Application.Cqrs;
using com.brettnamba.MlbTheShowForecaster.Common.Infrastructure.Cqrs.MediatR;
using com.brettnamba.MlbTheShowForecaster.Common.Infrastructure.Tests.Cqrs.TestClasses;
using Moq;

namespace com.brettnamba.MlbTheShowForecaster.Common.Infrastructure.Tests.Cqrs.MediatR;

public class MediatRCommandHandlerTests
{
    [Fact]
    public async Task Handle_WrappedCommand_CommandDelegatedToUnderlyingHandler()
    {
        // Arrange
        var underlyingCommand = new TestCommand();
        var mockUnderlyingHandler = Mock.Of<ICommandHandler<TestCommand>>();

        var commandWrapper = new MediatRCommand<TestCommand>(underlyingCommand);
        var handlerWrapper = new MediatRCommandHandler<TestCommand>(mockUnderlyingHandler);

        var cToken = CancellationToken.None;

        // Act
        await handlerWrapper.Handle(commandWrapper, cToken);

        // Assert
        Mock.Get(mockUnderlyingHandler).Verify(x => x.Handle(underlyingCommand, cToken), Times.Once);
    }
}