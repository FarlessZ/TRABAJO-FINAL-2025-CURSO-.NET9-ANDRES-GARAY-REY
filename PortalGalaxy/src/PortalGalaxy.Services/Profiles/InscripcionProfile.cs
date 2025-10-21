using System;
using AutoMapper;
using PortalGalaxy.Common.Request;
using PortalGalaxy.Common.Response;
using PortalGalaxy.Entities;
using PortalGalaxy.Entities.Infos;

namespace PortalGalaxy.Services.Profiles;

public class InscripcionProfile : Profile
{
    public InscripcionProfile()
    {
        CreateMap<Inscripcion, InscripcionDtoResponse>();
        CreateMap<InscripcionDtoRequest, Inscripcion>();

        CreateMap<InscripcionInfo, InscripcionDtoResponse>();
    }
}
