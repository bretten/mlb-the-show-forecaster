using com.brettnamba.MlbTheShowForecaster.Common.Application.Cqrs;
using MediatR;

namespace com.brettnamba.MlbTheShowForecaster.Common.Infrastructure.Cqrs.MediatR;

/// <summary>
/// MediatR implementation of <see cref="IQuerySender"/>
///
/// <para>This query sender uses MediatR to handle the actual delegating of the <see cref="IQuery{TResponse}"/> to
/// the appropriate handler</para>
/// </summary>
public sealed class MediatRQuerySender : IQuerySender
{
    /// <summary>
    /// The MediatR instance that does the delegating
    /// </summary>
    private readonly IMediator _mediator;

    /// <summary>
    /// Constructor
    /// </summary>
    /// <param name="mediator">The MediatR instance that does the delegating</param>
    public MediatRQuerySender(IMediator mediator)
    {
        _mediator = mediator;
    }

    /// <summary>
    /// Sends a <see cref="IQuery{TResponse}"/> by wrapping it in a MediatR <see cref="IRequest"/>
    /// </summary>
    /// <param name="query">The <see cref="IQuery{TResponse}"/></param>
    /// <param name="cancellationToken">A <see cref="CancellationToken" /> to observe while waiting for the task to complete</param>
    /// <typeparam name="TResponse">The response type</typeparam>
    /// <returns>The response</returns>
    public async Task<TResponse?> Send<TResponse>(IQuery<TResponse> query,
        CancellationToken cancellationToken = default)
    {
        var queryWrapperType = typeof(MediatRQuery<,>).MakeGenericType(query.GetType(), typeof(TResponse));
        var queryWrapper = Activator.CreateInstance(queryWrapperType, query) as IRequest<TResponse>;

        return await _mediator.Send(queryWrapper!, cancellationToken);
    }
}