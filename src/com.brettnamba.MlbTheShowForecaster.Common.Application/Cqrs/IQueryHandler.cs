namespace com.brettnamba.MlbTheShowForecaster.Common.Application.Cqrs;

/// <summary>
/// Defines a query handler for <see cref="IQuery{TResponse}"/>
/// </summary>
/// <typeparam name="TQuery">The type of the query that implements <see cref="IQuery{TResponse}"/></typeparam>
/// <typeparam name="TResponse">The query response type</typeparam>
public interface IQueryHandler<in TQuery, TResponse> where TQuery : IQuery<TResponse>
{
    /// <summary>
    /// Handles the specified query
    /// </summary>
    /// <param name="query">The specified query</param>
    /// <param name="cancellationToken">A <see cref="CancellationToken" /> to observe while waiting for the task to complete</param>
    /// <returns>The response of the query</returns>
    Task<TResponse?> Handle(TQuery query, CancellationToken cancellationToken);
}