namespace com.brettnamba.MlbTheShowForecaster.Common.Application.Cqrs;

/// <summary>
/// Defines a sender for <see cref="ICommand"/>
/// </summary>
public interface ICommandSender
{
    /// <summary>
    /// Sends the specified command to the corresponding handler
    /// </summary>
    /// <param name="command">The command to send</param>
    /// <param name="cancellationToken">A <see cref="CancellationToken" /> to observe while waiting for the task to complete</param>
    /// <typeparam name="TCommand">The command type that implements <see cref="ICommand"/></typeparam>
    /// <returns>The completed task</returns>
    Task Send<TCommand>(TCommand command, CancellationToken cancellationToken = default) where TCommand : ICommand;
}