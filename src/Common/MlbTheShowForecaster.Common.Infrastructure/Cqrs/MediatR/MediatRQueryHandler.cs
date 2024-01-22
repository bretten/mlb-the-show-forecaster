using com.brettnamba.MlbTheShowForecaster.Common.Application.Cqrs;
using MediatR;

namespace com.brettnamba.MlbTheShowForecaster.Common.Infrastructure.Cqrs.MediatR;

/// <summary>
/// MediatR wrapper for <see cref="IQueryHandler{TQuery,TResponse}"/>
///
/// <para>This MediatR handler is a wrapper for the actual underlying <see cref="IQueryHandler{TQuery,TResponse}"/>
/// and is registered at setup as a handler of a query of type <see cref="IQuery{TResponse}"/>. When it receives the query
/// wrapper <see cref="MediatRQuery{TQuery,TResponse}"/>, it delegates the wrapped <see cref="IQuery{TResponse}"/>
/// to the corresponding, underlying <see cref="IQueryHandler{TQuery,TResponse}"/></para>
/// </summary>
/// <typeparam name="TQuery"><see cref="IQuery{TResponse}"/></typeparam>
/// <typeparam name="TResponse">The response type</typeparam>
public sealed class MediatRQueryHandler<TQuery, TResponse> : IRequestHandler<MediatRQuery<TQuery, TResponse>, TResponse>
    where TQuery : IQuery<TResponse>
{
    /// <summary>
    /// The underlying <see cref="IQueryHandler{TQuery,TResponse}"/>
    /// </summary>
    private readonly IQueryHandler<TQuery, TResponse> _queryHandler;

    /// <summary>
    /// Constructor
    /// </summary>
    /// <param name="queryHandler">The underlying <see cref="IQueryHandler{TQuery,TResponse}"/></param>
    public MediatRQueryHandler(IQueryHandler<TQuery, TResponse> queryHandler)
    {
        _queryHandler = queryHandler;
    }

    /// <summary>
    /// Handles and delegates the wrapped <see cref="IQuery{TResponse}"/> to the underlying
    /// <see cref="IQueryHandler{TQuery,TResponse}"/>
    /// </summary>
    /// <param name="request">The <see cref="MediatRQuery{TQuery,TResponse}"/> that contains the <see cref="IQuery{TResponse}"/></param>
    /// <param name="cancellationToken">A <see cref="CancellationToken" /> to observe while waiting for the task to complete</param>
    /// <returns>The response of the query</returns>
    public async Task<TResponse> Handle(MediatRQuery<TQuery, TResponse> request, CancellationToken cancellationToken)
    {
        return (await _queryHandler.Handle(request.Query, cancellationToken))!;
    }
}