using PortalGalaxy.Common.Request;
using PortalGalaxy.Common.Response;
using PortalGalaxy.WebApp.Proxy.Interfaces;

namespace PortalGalaxy.WebApp.Proxy.Services;

public class InstructorProxy : CrudRestHelperBase<InstructorDtoRequest, InstructorDtoResponse>, IInstructorProxy
{
    public InstructorProxy(HttpClient httpClient) 
    : base("api/instructores", httpClient)
    {
    }

    public async Task<ICollection<InstructorDtoResponse>> ListAsync(string? filtro, string? nroDocumento, int? categoriaId)
    {
        var response = await SendAsync<BaseResponse<ICollection<InstructorDtoResponse>>>($"?filtro={filtro}&nroDocumento={nroDocumento}&categoriaId={categoriaId}");

        if (response is { Data: not null, Success: true })
        {
            return response.Data;
        }

        throw new InvalidOperationException($"No se pudo listar {response.ErrorMessage}");
    }
}
