using AutoMapper;

namespace com.brettnamba.MlbTheShowForecaster.Common.Infrastructure.Tests.Mapping.AutoMapper.TestClasses;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<ObjectTypeA, ObjectTypeB>()
            .ForMember(dest => dest.IntegerValue, opt => opt.MapFrom(src => src.AnInt))
            .ForMember(dest => dest.StringValue, opt => opt.MapFrom(src => src.AString))
            .ConstructUsing(x => new ObjectTypeB(x.AnInt, x.AString));
    }
}