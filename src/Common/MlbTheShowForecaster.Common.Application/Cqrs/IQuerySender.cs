namespace com.brettnamba.MlbTheShowForecaster.Common.Application.Cqrs;

/// <summary>
/// Defines a <see cref="IQuery{TResponse}"/> sender
/// </summary>
public interface IQuerySender
{
    /// <summary>
    /// Sends the specified query to the corresponding handler
    /// </summary>
    /// <param name="query">The query to send</param>
    /// <param name="cancellationToken">A <see cref="CancellationToken" /> to observe while waiting for the task to complete</param>
    /// <typeparam name="TResponse">The query response type</typeparam>
    /// <returns>The response of the query</returns>
    Task<TResponse?> Send<TResponse>(IQuery<TResponse> query, CancellationToken cancellationToken = default);
}