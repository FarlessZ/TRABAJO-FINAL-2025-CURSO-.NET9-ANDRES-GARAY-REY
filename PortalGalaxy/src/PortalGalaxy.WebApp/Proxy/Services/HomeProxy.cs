using System.Net.Http.Json;
using PortalGalaxy.Common.Request;
using PortalGalaxy.Common.Response;
using PortalGalaxy.WebApp.Proxy.Interfaces;

namespace PortalGalaxy.WebApp.Proxy.Services;

public class HomeProxy : RestBase, IHomeProxy
{
    public HomeProxy(HttpClient httpClient) 
        : base("api/Home", httpClient)
    {
    }

    public async Task<PaginationResponse<TallerHomeDtoResponse>> ListarTalleresHomeAsync(BusquedaTallerHomeRequest request)
    {
        var fechaInicio = request.FechaInicio?.ToString("yyyy-MM-dd");
        // Agregar un dia mas al final para que tome la fecha completa
        request.FechaFin = request.FechaFin?.AddDays(1);

        var fechaFin = request.FechaFin?.ToString("yyyy-MM-dd");
        var fechaFinPage = request.FechaFin?.AddDays(1).ToString("yyyy-MM-dd");

        var url = $"{BaseUrl}?Nombre={request.Nombre}&FechaInicio={fechaInicio}&FechaFin={fechaFinPage}&PageNumber={request.PageNumber}&PageSize={request.PageSize}";

        if (request.InstructorId.HasValue)
            url += $"&InstructorId={request.InstructorId.Value}";

        var response = await HttpClient.GetFromJsonAsync<PaginationResponse<TallerHomeDtoResponse>>(url);

        if (response is { Success: false })
            throw new ApplicationException(response.ErrorMessage);

        return response!;


    }

    public async Task<BaseResponse<TallerHomeDtoResponse>> GetTallerHomeAsync(int id)
    {
        var response = await HttpClient.GetFromJsonAsync<BaseResponse<TallerHomeDtoResponse>>($"{BaseUrl}/{id}");
        
        if (response is { Success: false })
            throw new ApplicationException(response.ErrorMessage);
        
        return response!;
    }
}