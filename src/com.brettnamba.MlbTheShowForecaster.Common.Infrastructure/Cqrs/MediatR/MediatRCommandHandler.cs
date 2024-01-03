using com.brettnamba.MlbTheShowForecaster.Common.Application.Cqrs;
using MediatR;

namespace com.brettnamba.MlbTheShowForecaster.Common.Infrastructure.Cqrs.MediatR;

/// <summary>
/// MediatR wrapper for <see cref="ICommandHandler{TCommand}"/>
///
/// <para>This MediatR handler is a wrapper for the actual underlying <see cref="ICommandHandler{TCommand}"/> and is
/// registered at setup as a handler of a command of type <see cref="ICommand"/>. When it receives the command wrapper
/// <see cref="MediatRCommand{TCommand}"/>, it delegates the wrapped <see cref="ICommand"/> to the corresponding,
/// underlying <see cref="ICommandHandler{TCommand}"/></para>
/// </summary>
/// <typeparam name="T"><see cref="ICommand"/></typeparam>
public sealed class MediatRCommandHandler<T> : IRequestHandler<MediatRCommand<T>> where T : ICommand
{
    /// <summary>
    /// The underlying <see cref="ICommandHandler{TCommand}"/>
    /// </summary>
    private readonly ICommandHandler<T> _handler;

    /// <summary>
    /// Constructor
    /// </summary>
    /// <param name="handler">The underlying <see cref="ICommandHandler{TCommand}"/></param>
    public MediatRCommandHandler(ICommandHandler<T> handler)
    {
        _handler = handler;
    }

    /// <summary>
    /// Handles and delegates the wrapped <see cref="ICommand"/> to the underlying <see cref="ICommandHandler{TCommand}"/>
    /// </summary>
    /// <param name="request">The <see cref="MediatRCommand{TCommand}"/> that contains the <see cref="ICommand"/></param>
    /// <param name="cancellationToken">A <see cref="CancellationToken" /> to observe while waiting for the task to complete</param>
    public async Task Handle(MediatRCommand<T> request, CancellationToken cancellationToken)
    {
        await _handler.Handle(request.Command, cancellationToken);
    }
}