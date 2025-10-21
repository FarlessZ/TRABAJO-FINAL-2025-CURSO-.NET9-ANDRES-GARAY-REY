using System.Net;
using System.Net.Http.Json;
using PortalGalaxy.Common.Request;
using PortalGalaxy.Common.Response;
using PortalGalaxy.WebApp.Proxy.Interfaces;

namespace PortalGalaxy.WebApp.Proxy.Services;

public class TallerProxy : CrudRestHelperBase<TallerDtoRequest, TallerDtoResponse>, ITallerProxy
{
    public TallerProxy(HttpClient httpClient)
        : base("api/talleres", httpClient)
    {
    }

    public async Task<Stream> ExportarPdf(BusquedaTallerRequest request)
    {
        var response = await HttpClient.PostAsJsonAsync($"{BaseUrl}/pdf", request);
        response.EnsureSuccessStatusCode();
        var result = await response.Content.ReadAsStreamAsync();
        return result;
    }

    public async Task<BaseResponse<ICollection<TallerSimpleDtoResponse>>> ListarAsync()
    {
        var response = await SendAsync<BaseResponse<ICollection<TallerSimpleDtoResponse>>>("simple");

        if (response is { Success: true, Data: not null })
        {
            return response;
        }

        throw new InvalidOperationException(response.ErrorMessage);
    }

    public async Task<BaseResponse<ICollection<TalleresPorInstructorDto>>> ListarPorInstructorAsync(int anio)
    {
        var response = await SendAsync<BaseResponse<ICollection<TalleresPorInstructorDto>>>($"reportes/talleresporinstructor/{anio}");

        if (response is { Success: true, Data: not null })
        {
            return response;
        }

        throw new InvalidOperationException(response.ErrorMessage);
    }

    public async Task<BaseResponse<ICollection<TalleresPorMesDto>>> ListarPorMesAsync(int anio)
    {
        var response = await SendAsync<BaseResponse<ICollection<TalleresPorMesDto>>>($"reportes/tallerespormes/{anio}");
        if (response is { Success: true, Data: not null })
        {
            return response;
        }

        throw new InvalidOperationException(response.ErrorMessage);
    }

    public async Task<PaginationResponse<TallerDtoResponse>> ListAsync(string? nombre, int? categoriaId, int? situacion,
        int pageNumber = 1, int pageSize = 15)
    {
        var data = await SendAsync<PaginationResponse<TallerDtoResponse>>(
            $"?nombre={nombre}&categoria={categoriaId}&situacion={situacion}&pageNumber={pageNumber}&pageSize={pageSize}");

        return data;
    }

    public async Task<PaginationResponse<InscritosPorTallerDtoResponse>> ListAsync(BusquedaInscritosPorTallerRequest request)
    {
        var response = await SendAsync<PaginationResponse<InscritosPorTallerDtoResponse>>($"inscritos?instructorid={request.InstructorId}&taller={request.Taller}&situacion={request.Situacion}&fechaInicio={request.FechaInicio}&fechaFin={request.FechaFin}&pageNumber={request.PageNumber}&pageSize={request.PageSize}");

        if (response is { Success: true, Data: not null })
        {
            return response;
        }

        throw new InvalidOperationException(response.ErrorMessage);
    }
}