using System;
using PortalGalaxy.Common.Request;
using PortalGalaxy.Common.Response;

namespace PortalGalaxy.WebApp.Proxy.Interfaces;

public interface IInscripcionProxy : ICrudRestHelper<InscripcionDtoRequest, InscripcionDtoResponse>
{
    Task<PaginationResponse<InscripcionDtoResponse>> ListAsync(BusquedaInscripcionRequest request);

    Task InscripcionMasivaAsync(InscripcionMasivaDtoRequest request);
}
