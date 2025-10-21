using AutoMapper;
using PortalGalaxy.Common.Request;
using PortalGalaxy.Common.Response;
using PortalGalaxy.Entities;
using PortalGalaxy.Entities.Infos;

namespace PortalGalaxy.Services.Profiles;

public class InstructorProfile : Profile
{
    public InstructorProfile()
    {
        CreateMap<InstructorInfo, InstructorDtoResponse>();

        CreateMap<InstructorDtoRequest, Instructor>()
            .ReverseMap();

        CreateMap<InstructorInfo, InstructorDtoResponse>();
    }
}
