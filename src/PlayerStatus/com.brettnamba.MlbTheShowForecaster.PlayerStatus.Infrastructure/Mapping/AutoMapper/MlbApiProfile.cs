using System.ComponentModel;
using AutoMapper;
using com.brettnamba.MlbTheShowForecaster.ExternalApis.MlbApi.Dtos;
using com.brettnamba.MlbTheShowForecaster.PlayerStatus.Application.Dtos;
using com.brettnamba.MlbTheShowForecaster.PlayerStatus.Domain.Common.ValueObjects;
using com.brettnamba.MlbTheShowForecaster.PlayerStatus.Domain.Players.Enums;
using com.brettnamba.MlbTheShowForecaster.PlayerStatus.Domain.Players.ValueObjects;
using Position = com.brettnamba.MlbTheShowForecaster.ExternalApis.MlbApi.Dtos.Position;

namespace com.brettnamba.MlbTheShowForecaster.PlayerStatus.Infrastructure.Mapping.AutoMapper;

/// <summary>
/// AutoMapper profile for mapping external DTOs to application-relevant objects
/// </summary>
public sealed class MlbApiProfile : Profile
{
    /// <summary>
    /// Constructor that sets up the maps
    /// </summary>
    public MlbApiProfile()
    {
        CreateMap<Player, RosterEntry>()
            .ForMember(dest => dest.MlbId, opt => opt.MapFrom(src => MlbId.Create(src.Id)))
            .ForMember(dest => dest.FirstName, opt => opt.MapFrom(src => PersonName.Create(src.FirstName)))
            .ForMember(dest => dest.LastName, opt => opt.MapFrom(src => PersonName.Create(src.LastName)))
            .ForMember(dest => dest.Birthdate, opt => opt.MapFrom(src => src.Birthdate))
            .ForMember(dest => dest.Position,
                opt => opt.MapFrom(src =>
                    TypeDescriptor.GetConverter(typeof(Position)).ConvertFrom(src.Position.Abbreviation)))
            .ForMember(dest => dest.MlbDebutDate, opt => opt.MapFrom(src => src.MlbDebutDate))
            .ForMember(dest => dest.BatSide,
                opt => opt.MapFrom(src => TypeDescriptor.GetConverter(typeof(BatSide)).ConvertFrom(src.BatSide.Code)))
            .ForMember(dest => dest.ThrowArm,
                opt => opt.MapFrom(src => TypeDescriptor.GetConverter(typeof(ThrowArm)).ConvertFrom(src.ThrowArm.Code)))
            .ForMember(dest => dest.CurrentTeamMlbId, opt => opt.MapFrom(src => MlbId.Create(src.CurrentTeam.Id)))
            .ForMember(dest => dest.Active, opt => opt.MapFrom(src => src.Active));
    }
}