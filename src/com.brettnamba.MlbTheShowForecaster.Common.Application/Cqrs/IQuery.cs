namespace com.brettnamba.MlbTheShowForecaster.Common.Application.Cqrs;

/// <summary>
/// Defines a read-only query that retrieves something from the system
/// </summary>
/// <typeparam name="TResponse">The query response type</typeparam>
public interface IQuery<out TResponse>
{
}