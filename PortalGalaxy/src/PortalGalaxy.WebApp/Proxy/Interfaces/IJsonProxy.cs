using System;
using PortalGalaxy.Common.Response;

namespace PortalGalaxy.WebApp.Proxy.Interfaces;

public interface IJsonProxy
{
    Task<ICollection<DepartamentoModel>> ListDepartamentos();
    Task<ICollection<ProvinciaModel>> ListProvincias(string codigoDpto);
    Task<ICollection<DistritoModel>> ListDistritos(string codProvincia);

    Task<ICollection<SituacionModel>> ListSituaciones();
}
