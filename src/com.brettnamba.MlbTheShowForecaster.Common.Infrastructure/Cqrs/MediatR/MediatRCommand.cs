using com.brettnamba.MlbTheShowForecaster.Common.Application.Cqrs;
using MediatR;

namespace com.brettnamba.MlbTheShowForecaster.Common.Infrastructure.Cqrs.MediatR;

/// <summary>
/// Wraps <see cref="ICommand"/> in a MediatR request
/// </summary>
/// <param name="Command"><see cref="ICommand"/></param>
/// <typeparam name="TCommand"><see cref="ICommand"/></typeparam>
public readonly record struct MediatRCommand<TCommand>(TCommand Command) : IRequest where TCommand : ICommand;