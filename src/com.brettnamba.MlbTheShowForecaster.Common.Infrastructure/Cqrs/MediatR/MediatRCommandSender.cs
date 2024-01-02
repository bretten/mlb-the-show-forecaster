using com.brettnamba.MlbTheShowForecaster.Common.Application.Cqrs;
using MediatR;

namespace com.brettnamba.MlbTheShowForecaster.Common.Infrastructure.Cqrs.MediatR;

/// <summary>
/// MediatR implementation for <see cref="ICommandSender"/>
///
/// <para>This command sender wraps the <see cref="ICommand"/> in a generic MediatR <see cref="IRequest"/> of type
/// <see cref="MediatRCommand{TCommand}"/>. The MediatR request handler then does the delegating to the actual
/// command handler and prevents inner layers of the system from requiring external dependencies such as MediatR</para>
/// </summary>
public sealed class MediatRCommandSender : ICommandSender
{
    /// <summary>
    /// The underlying <see cref="IMediator"/> that sends the wrapped <see cref="ICommand"/>
    /// </summary>
    private readonly IMediator _mediator;

    /// <summary>
    /// Constructor
    /// </summary>
    /// <param name="mediator">The underlying <see cref="IMediator"/> that sends the wrapped <see cref="ICommand"/></param>
    public MediatRCommandSender(IMediator mediator)
    {
        _mediator = mediator;
    }

    /// <summary>
    /// Sends a <see cref="ICommand"/> by wrapping it in a MediatR <see cref="IRequest"/>
    /// </summary>
    /// <param name="command">The <see cref="ICommand"/></param>
    /// <param name="cancellationToken">A <see cref="CancellationToken" /> to observe while waiting for the task to complete</param>
    /// <typeparam name="TCommand"><see cref="ICommand"/></typeparam>
    public async Task Send<TCommand>(TCommand command, CancellationToken cancellationToken = default)
        where TCommand : ICommand
    {
        var request = new MediatRCommand<TCommand>(command);
        await _mediator.Send(request, cancellationToken);
    }
}