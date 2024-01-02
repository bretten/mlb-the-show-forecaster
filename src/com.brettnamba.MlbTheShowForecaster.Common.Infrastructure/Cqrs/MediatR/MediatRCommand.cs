using com.brettnamba.MlbTheShowForecaster.Common.Application.Cqrs;
using MediatR;

namespace com.brettnamba.MlbTheShowForecaster.Common.Infrastructure.Cqrs.MediatR;

/// <summary>
/// Wraps <see cref="ICommand"/> in a MediatR request
/// </summary>
/// <typeparam name="TCommand"><see cref="ICommand"/></typeparam>
public readonly record struct MediatRCommand<TCommand> : IRequest where TCommand : ICommand
{
    /// <summary>
    /// Constructor
    /// </summary>
    /// <param name="command">The <see cref="ICommand"/> to wrap in a MediatR request</param>
    public MediatRCommand(TCommand command)
    {
        Command = command;
    }

    /// <summary>
    /// The underlying command
    /// </summary>
    public TCommand Command { get; }
}