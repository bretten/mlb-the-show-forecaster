using com.brettnamba.MlbTheShowForecaster.Common.Infrastructure.Cqrs.MediatR;
using com.brettnamba.MlbTheShowForecaster.Common.Infrastructure.Tests.Cqrs.TestClasses;
using MediatR;
using Moq;

namespace com.brettnamba.MlbTheShowForecaster.Common.Infrastructure.Tests.Cqrs.MediatR;

public class MediatRCommandSenderTests
{
    [Fact]
    public async Task Send_Command_MediatRSendsWrappedCommand()
    {
        // Arrange
        var command = new TestCommand();
        var mockMediatR = Mock.Of<IMediator>();
        var commandSender = new MediatRCommandSender(mockMediatR);

        var cToken = CancellationToken.None;

        // Act
        await commandSender.Send(command, cToken);

        // Assert
        Mock.Get(mockMediatR).Verify(x => x.Send(new MediatRCommand<TestCommand>(command), cToken));
    }
}