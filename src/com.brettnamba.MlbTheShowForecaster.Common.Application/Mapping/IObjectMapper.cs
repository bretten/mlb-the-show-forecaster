namespace com.brettnamba.MlbTheShowForecaster.Common.Application.Mapping;

/// <summary>
/// Defines a mapper that can map an object to different type of object
/// </summary>
public interface IObjectMapper
{
    /// <summary>
    /// Maps an object of type TIn to type TOut
    /// </summary>
    /// <param name="source">The object to map</param>
    /// <typeparam name="TIn">The type of the object to map</typeparam>
    /// <typeparam name="TOut">The type of the resulting object</typeparam>
    /// <returns>The resulting object</returns>
    TOut Map<TIn, TOut>(TIn source);
}