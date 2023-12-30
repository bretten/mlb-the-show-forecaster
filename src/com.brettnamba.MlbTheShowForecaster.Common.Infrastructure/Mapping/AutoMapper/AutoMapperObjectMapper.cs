using AutoMapper;
using com.brettnamba.MlbTheShowForecaster.Common.Application.Mapping;

namespace com.brettnamba.MlbTheShowForecaster.Common.Infrastructure.Mapping.AutoMapper;

/// <summary>
/// AutoMapper implementation of <see cref="IObjectMapper"/>
/// </summary>
public sealed class AutoMapperObjectMapper : IObjectMapper
{
    /// <summary>
    /// AutoMapper
    /// </summary>
    private readonly IMapper _mapper;

    /// <summary>
    /// Constructor
    /// </summary>
    /// <param name="mapper">Automapper</param>
    public AutoMapperObjectMapper(IMapper mapper)
    {
        _mapper = mapper;
    }

    /// <summary>
    /// Maps object of type TIn to type TOut
    /// </summary>
    /// <param name="source">The object to map</param>
    /// <typeparam name="TIn">The type of the object to map</typeparam>
    /// <typeparam name="TOut">The type of the resulting object</typeparam>
    /// <returns>The mapped object</returns>
    public TOut Map<TIn, TOut>(TIn source)
    {
        return _mapper.Map<TIn, TOut>(source);
    }
}