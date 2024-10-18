namespace com.brettnamba.MlbTheShowForecaster.Common.Application.Jobs;

/// <summary>
/// Represents a job that executes some <see cref="Task{TResult}"/>
/// </summary>
public interface IJob
{
    /// <summary>
    /// Executes a job with the specified input <see cref="IJobInput"/>
    /// </summary>
    /// <param name="input">The job input <see cref="IJobInput"/></param>
    /// <param name="cancellationToken">A <see cref="CancellationToken" /> to observe while waiting for the task to complete</param>
    /// <returns><see cref="IJobOutput"/></returns>
    Task<IJobOutput> Execute(IJobInput input, CancellationToken cancellationToken = default);
}

/// <summary>
/// Contravariant <see cref="IJob"/> - <see cref="TIn"/> can be more general. In other words, <see cref="IJob{TIn,TOut}"/>
/// can be treated as <see cref="IJob"/>
/// </summary>
/// <typeparam name="TIn"><see cref="IJobInput"/></typeparam>
/// <typeparam name="TOut"><see cref="IJobOutput"/></typeparam>
public interface IJob<in TIn, TOut> : IJob
    where TIn : IJobInput
    where TOut : IJobOutput
{
    /// <summary>
    /// Executes a job with the specified input <see cref="TIn"/>
    /// </summary>
    /// <param name="input">The job input <see cref="TIn"/></param>
    /// <param name="cancellationToken">A <see cref="CancellationToken" /> to observe while waiting for the task to complete</param>
    /// <returns><see cref="TOut"/></returns>
    Task<TOut> Execute(TIn input, CancellationToken cancellationToken = default);
}