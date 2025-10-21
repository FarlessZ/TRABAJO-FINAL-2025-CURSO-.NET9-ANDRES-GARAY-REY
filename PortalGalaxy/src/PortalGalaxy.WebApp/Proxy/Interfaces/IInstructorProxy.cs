using System;
using PortalGalaxy.Common.Request;
using PortalGalaxy.Common.Response;

namespace PortalGalaxy.WebApp.Proxy.Interfaces;

public interface IInstructorProxy : ICrudRestHelper<InstructorDtoRequest, InstructorDtoResponse>
{
    Task<ICollection<InstructorDtoResponse>> ListAsync(string? filtro, string? nroDocumento, int? categoriaId);
}
