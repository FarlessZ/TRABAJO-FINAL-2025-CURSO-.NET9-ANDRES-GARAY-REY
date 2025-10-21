using System;
using PortalGalaxy.Common.Request;
using PortalGalaxy.Common.Response;
using PortalGalaxy.WebApp.Proxy.Interfaces;

namespace PortalGalaxy.WebApp.Proxy.Services;

public class AlumnoProxy : CrudRestHelperBase<AlumnoDtoRequest, AlumnoDtoResponse>, IAlumnoProxy
{
    public AlumnoProxy(HttpClient httpClient)
        : base("api/Alumnos", httpClient)
    {
    }


    public async Task<ICollection<AlumnoSimpleDtoResponse>> ListarAsync(string? filtro, string? nroDocumento)
    {
        var response = await SendAsync<BaseResponse<ICollection<AlumnoSimpleDtoResponse>>>(
            $"simple?filtro={filtro}&nroDocumento={nroDocumento}");

        if (response is { Success: true, Data: not null })
        {
            return response.Data;
        }

        throw new InvalidOperationException(response.ErrorMessage);
    }
}
