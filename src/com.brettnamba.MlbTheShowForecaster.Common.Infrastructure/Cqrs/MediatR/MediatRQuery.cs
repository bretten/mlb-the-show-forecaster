using com.brettnamba.MlbTheShowForecaster.Common.Application.Cqrs;
using MediatR;

namespace com.brettnamba.MlbTheShowForecaster.Common.Infrastructure.Cqrs.MediatR;

/// <summary>
/// Wraps a <see cref="IQuery{TResponse}"/> in a MediatR request
/// </summary>
/// <param name="Query">The <see cref="IQuery{TResponse}"/> to wrap in the MediatR request</param>
/// <typeparam name="TQuery"><see cref="IQuery{TResponse}"/></typeparam>
/// <typeparam name="TResponse">The response type</typeparam>
public readonly record struct MediatRQuery<TQuery, TResponse>(TQuery Query) : IRequest<TResponse>
    where TQuery : IQuery<TResponse>;