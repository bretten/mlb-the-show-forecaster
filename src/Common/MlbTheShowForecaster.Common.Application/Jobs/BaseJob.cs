namespace com.brettnamba.MlbTheShowForecaster.Common.Application.Jobs;

/// <summary>
/// Base job
/// </summary>
/// <typeparam name="TIn"><see cref="IJob{TIn,TOut}"/></typeparam>
/// <typeparam name="TOut"><see cref="IJobOutput"/></typeparam>
public abstract class BaseJob<TIn, TOut> : IJob<TIn, TOut>
    where TIn : IJobInput
    where TOut : IJobOutput
{
    /// <summary>
    /// Executes the <see cref="IJob{TIn,TOut}"/> with the derived input and output types
    /// </summary>
    /// <param name="input"><see cref="TIn"/></param>
    /// <param name="cancellationToken">A <see cref="CancellationToken" /> to observe while waiting for the task to complete</param>
    /// <returns><see cref="TOut"/></returns>
    public abstract Task<TOut> Execute(TIn input, CancellationToken cancellationToken = default);

    /// <summary>
    /// Executes the <see cref="IJob"/> with the interfaces <see cref="IJobInput"/> and <see cref="IJobOutput"/>
    /// </summary>
    /// <param name="input"><see cref="IJobInput"/></param>
    /// <param name="cancellationToken">A <see cref="CancellationToken" /> to observe while waiting for the task to complete</param>
    /// <returns><see cref="IJobOutput"/></returns>
    public async Task<IJobOutput> Execute(IJobInput input, CancellationToken cancellationToken = default)
    {
        return await Execute((TIn)input, cancellationToken);
    }
}