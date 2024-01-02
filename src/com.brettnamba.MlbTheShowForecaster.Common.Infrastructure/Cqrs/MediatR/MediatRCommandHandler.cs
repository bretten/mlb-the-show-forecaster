using com.brettnamba.MlbTheShowForecaster.Common.Application.Cqrs;
using MediatR;

namespace com.brettnamba.MlbTheShowForecaster.Common.Infrastructure.Cqrs.MediatR;

/// <summary>
/// MediatR implementation for <see cref="ICommandHandler{TCommand}"/>
///
/// <para>This command handler wraps MediatR. It handles a MediatR <see cref="IRequest"/> that holds
/// a <see cref="ICommand"/> by extracting the underlying command and delegating it to the actual handler
/// of this system: <see cref="ICommandHandler{TCommand}"/>. This prevents the inner layers from requiring
/// external dependencies such as MediatR.</para>
/// </summary>
/// <typeparam name="T"><see cref="ICommand"/></typeparam>
public sealed class MediatRCommandHandler<T> : IRequestHandler<MediatRCommand<T>> where T : ICommand
{
    /// <summary>
    /// The underlying <see cref="ICommandHandler{TCommand}"/> that has no external dependencies
    /// </summary>
    private readonly ICommandHandler<T> _handler;

    /// <summary>
    /// Constructor
    /// </summary>
    /// <param name="handler">The underlying <see cref="ICommandHandler{TCommand}"/> that has no external dependencies</param>
    public MediatRCommandHandler(ICommandHandler<T> handler)
    {
        _handler = handler;
    }

    /// <summary>
    /// Handles the <see cref="ICommand"/> which is wrapped in a <see cref="MediatRCommand{TCommand}"/>
    /// </summary>
    /// <param name="request">The <see cref="MediatRCommand{TCommand}"/> that contains the <see cref="ICommand"/></param>
    /// <param name="cancellationToken">A <see cref="CancellationToken" /> to observe while waiting for the task to complete</param>
    public async Task Handle(MediatRCommand<T> request, CancellationToken cancellationToken)
    {
        await _handler.Handle(request.Command, cancellationToken);
    }
}