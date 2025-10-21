using System;
using PortalGalaxy.Common.Request;
using PortalGalaxy.Common.Response;

namespace PortalGalaxy.WebApp.Proxy.Interfaces;

public interface IAlumnoProxy : ICrudRestHelper<AlumnoDtoRequest, AlumnoDtoResponse>
{
    Task<ICollection<AlumnoSimpleDtoResponse>> ListarAsync(string? filtro, string? nroDocumento);
}
