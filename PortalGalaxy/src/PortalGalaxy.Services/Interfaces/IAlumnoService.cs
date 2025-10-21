using PortalGalaxy.Common.Request;
using PortalGalaxy.Common.Response;

namespace PortalGalaxy.Services.Interfaces;

public interface IAlumnoService
{
    Task<BaseResponse<ICollection<AlumnoDtoResponse>>> ListAsync(string? filtro, string? nroDocumento);

    Task<BaseResponse<ICollection<AlumnoSimpleDtoResponse>>> ListSimpleAsync(string? filtro,
        string? nroDocumento);

    Task<BaseResponse<AlumnoDtoResponse>> FindByIdAsync(int id);

    Task<BaseResponse> AddAsync(AlumnoDtoRequest request);

    Task<BaseResponse> UpdateAsync(int id, AlumnoDtoRequest request);

    Task<BaseResponse> DeleteAsync(int id);

}