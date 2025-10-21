using AutoMapper;
using PortalGalaxy.Common.Request;
using PortalGalaxy.Common.Response;
using PortalGalaxy.Entities;
using PortalGalaxy.Entities.Infos;

namespace PortalGalaxy.Services.Profiles;

public class TallerProfile : Profile
{
    public TallerProfile()
    {
        CreateMap<TallerInfo, TallerDtoResponse>()
            .ForMember(dest => dest.Taller, opt => opt.MapFrom(src => src.Nombre))
            .ForMember(dest => dest.Fecha, opt => opt.MapFrom(src => src.Fecha.ToString("dd/MM/yyyy")));

        CreateMap<TallerDtoRequest, Taller>()
            .ReverseMap();

        CreateMap<InscritosPorTallerInfo, InscritosPorTallerDtoResponse>();

        CreateMap<TallerHomeInfo, TallerHomeDtoResponse>();

        CreateMap<TalleresPorMesInfo, TalleresPorMesDto>();

        CreateMap<TalleresPorInstructorInfo, TalleresPorInstructorDto>();
    }
}