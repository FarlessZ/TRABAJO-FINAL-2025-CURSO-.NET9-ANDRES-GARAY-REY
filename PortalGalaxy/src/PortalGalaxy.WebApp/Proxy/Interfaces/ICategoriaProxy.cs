using PortalGalaxy.Common.Response;

namespace PortalGalaxy.WebApp.Proxy.Interfaces;

public interface ICategoriaProxy
{
    Task<ICollection<CategoriaDtoResponse>> ListAsync();
}