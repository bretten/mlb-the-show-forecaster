using com.brettnamba.MlbTheShowForecaster.Common.Domain.SeedWork.Exceptions;

namespace com.brettnamba.MlbTheShowForecaster.Common.Domain.SeedWork;

/// <summary>
/// Represents a set of mutations on the system that depend on each other and must be committed together.
/// </summary>
/// <typeparam name="T">The type of work, such as domain or bounded context, that is being committed</typeparam>
public interface IUnitOfWork<out T> where T : IUnitOfWorkType
{
    /// <summary>
    /// Gets a contributor to the unit of work of the specified type <see cref="TContributor"/>
    /// </summary>
    /// <typeparam name="TContributor">The type of contributor to get</typeparam>
    /// <returns>The contributor to the unit of work</returns>
    /// <exception cref="UnitOfWorkContributorNotFoundException">Thrown if the contributor could not be found</exception>
    TContributor GetContributor<TContributor>() where TContributor : notnull;

    /// <summary>
    /// Commits all of the mutations that depend on each other
    /// </summary>
    /// <param name="cancellationToken">A <see cref="CancellationToken" /> to observe while waiting for the task to complete.</param>
    /// <returns>The completed Task</returns>
    Task CommitAsync(CancellationToken cancellationToken = default);
}