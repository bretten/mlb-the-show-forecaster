namespace com.brettnamba.MlbTheShowForecaster.Common.Application.Jobs;

/// <summary>
/// Defines a service that manages <see cref="IJob"/>s
/// </summary>
public interface IJobManager
{
    /// <summary>
    /// Runs a <see cref="IJob"/> with the specified <see cref="IJobInput"/>
    /// </summary>
    /// <param name="input"><see cref="IJobInput"/></param>
    /// <param name="cancellationToken">A <see cref="CancellationToken" /> to observe while waiting for the task to complete</param>
    /// <typeparam name="T"><see cref="IJob"/></typeparam>
    /// <typeparam name="TOut"><see cref="IJobOutput"/></typeparam>
    /// <returns><see cref="IJobOutput"/></returns>
    Task<TOut> Run<T, TOut>(IJobInput input, CancellationToken cancellationToken = default)
        where T : IJob
        where TOut : IJobOutput;
}