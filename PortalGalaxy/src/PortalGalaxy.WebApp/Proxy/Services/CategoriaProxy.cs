using PortalGalaxy.Common.Response;
using PortalGalaxy.WebApp.Proxy.Interfaces;

namespace PortalGalaxy.WebApp.Proxy.Services;

public class CategoriaProxy : RestBase, ICategoriaProxy
{
    public CategoriaProxy(HttpClient httpClient) 
        : base("api/categorias", httpClient)
    {
    }

    public async Task<ICollection<CategoriaDtoResponse>> ListAsync()
    {
        var data = await SendAsync<BaseResponse<ICollection<CategoriaDtoResponse>>>(string.Empty);
        if (data is { Success: true, Data: not null})
            return data.Data;

        throw new ApplicationException(data.ErrorMessage);
    }
}