using PortalGalaxy.Common.Request;
using PortalGalaxy.Common.Response;
using PortalGalaxy.WebApp.Proxy.Interfaces;

namespace PortalGalaxy.WebApp.Proxy.Services;

public class InscripcionProxy : CrudRestHelperBase<InscripcionDtoRequest, InscripcionDtoResponse>, IInscripcionProxy
{
    public InscripcionProxy(HttpClient httpClient) 
    : base("api/inscripciones", httpClient)
    {
    }

    public async Task InscripcionMasivaAsync(InscripcionMasivaDtoRequest request)
    {
        var response = await SendAsync<InscripcionMasivaDtoRequest, BaseResponse>(request, HttpMethod.Post, "masiva");
        if (response is { Success: false })
        {
            throw new InvalidOperationException(response.ErrorMessage);
        }
    }

    public async Task<PaginationResponse<InscripcionDtoResponse>> ListAsync(BusquedaInscripcionRequest request)
    {
        var fechaInicio = request.FechaInicio?.ToString("yyyy-MM-dd") ?? string.Empty;
        request.FechaFin = request.FechaFin?.AddDays(1);
        var fechaFin = request.FechaFin?.ToString("yyyy-MM-dd") ?? string.Empty;

        var response = await SendAsync<PaginationResponse<InscripcionDtoResponse>>($"?inscrito={request.Inscrito}&taller={request.Taller}&situacion={request.Situacion}&fechaInicio={fechaInicio}&fechaFin={fechaFin}&pageNumber={request.PageNumber}&pageSize={request.PageSize}");

        if (response is { Success: false })
        {
            throw new InvalidOperationException(response.ErrorMessage);
        }

        return response;

    }
}
