using com.brettnamba.MlbTheShowForecaster.Common.Domain.SeedWork;
using com.brettnamba.MlbTheShowForecaster.Common.Domain.SeedWork.Exceptions;
using Microsoft.Extensions.DependencyInjection;

namespace com.brettnamba.MlbTheShowForecaster.Common.Infrastructure.UnitOfWork;

/// <summary>
/// Abstract <see cref="IUnitOfWork{T}"/> that creates a <see cref="IServiceScope"/> so that it may resolve services
/// that contribute to the <see cref="ScopedUnitOfWork{T}"/>
/// </summary>
/// <typeparam name="T">The type of work that is being committed</typeparam>
public abstract class ScopedUnitOfWork<T> : IUnitOfWork<T>, IDisposable, IAsyncDisposable where T : IUnitOfWorkType
{
    /// <summary>
    /// Service scope used to resolve services that contribute to this <see cref="ScopedUnitOfWork{T}"/>
    /// </summary>
    protected readonly IServiceScope Scope;

    /// <summary>
    /// Constructor
    /// </summary>
    /// <param name="serviceScopeFactory">Factory for the service scope used to resolve services that contribute to this <see cref="ScopedUnitOfWork{T}"/></param>
    protected ScopedUnitOfWork(IServiceScopeFactory serviceScopeFactory)
    {
        Scope = serviceScopeFactory.CreateScope();
    }

    /// <inheritdoc />
    public virtual TContributor GetContributor<TContributor>() where TContributor : notnull
    {
        return Scope.ServiceProvider.GetService<TContributor>() ??
               throw new UnitOfWorkContributorNotFoundException(
                   $"Contributor of type {nameof(TContributor)} not found");
    }

    /// <inheritdoc />
    public abstract Task CommitAsync(CancellationToken cancellationToken = default);

    /// <summary>
    /// Disposes of the service scope
    /// </summary>
    public virtual void Dispose()
    {
        Scope.Dispose();
    }

    /// <summary>
    /// Disposes of the service scope
    /// </summary>
    public virtual ValueTask DisposeAsync()
    {
        Scope.Dispose();
        return ValueTask.CompletedTask;
    }
}