using AutoMapper;
using PortalGalaxy.Common.Response;
using PortalGalaxy.Entities;

namespace PortalGalaxy.Services.Profiles;

public class CategoriaProfile : Profile
{
    public CategoriaProfile()
    {
        CreateMap<Categoria, CategoriaDtoResponse>();
    }
}