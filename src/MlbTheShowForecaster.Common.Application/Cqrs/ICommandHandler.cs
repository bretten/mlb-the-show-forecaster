namespace com.brettnamba.MlbTheShowForecaster.Common.Application.Cqrs;

/// <summary>
/// Defines a handler for a <see cref="ICommand"/>
/// </summary>
/// <typeparam name="TCommand">The type of command</typeparam>
public interface ICommandHandler<in TCommand> where TCommand : ICommand
{
    /// <summary>
    /// Handles the specified command
    /// </summary>
    /// <param name="command">The command to handle</param>
    /// <param name="cancellationToken">A <see cref="CancellationToken" /> to observe while waiting for the task to complete</param>
    /// <returns>The completed task</returns>
    Task Handle(TCommand command, CancellationToken cancellationToken = default);
}