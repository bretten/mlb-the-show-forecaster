using com.brettnamba.MlbTheShowForecaster.Common.Application.Cqrs;
using MediatR;

namespace com.brettnamba.MlbTheShowForecaster.Common.Infrastructure.Cqrs.MediatR;

/// <summary>
/// MediatR implementation of <see cref="ICommandSender"/>
///
/// <para>This command sender uses MediatR to handle the actual delegating of the <see cref="ICommand"/> to the
/// appropriate handler</para>
/// </summary>
public sealed class MediatRCommandSender : ICommandSender
{
    /// <summary>
    /// The MediatR instance that does the delegating
    /// </summary>
    private readonly IMediator _mediator;

    /// <summary>
    /// Constructor
    /// </summary>
    /// <param name="mediator">The MediatR instance that does the delegating</param>
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