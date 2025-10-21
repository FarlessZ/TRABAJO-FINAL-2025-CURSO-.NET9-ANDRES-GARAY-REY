using PortalGalaxy.Common.Request;
using PortalGalaxy.Common.Response;

namespace PortalGalaxy.Services.Interfaces;

public interface IInstructorService
{
    Task<BaseResponse<ICollection<InstructorDtoResponse>>> ListAsync(string? filtro, string? nroDocumento, int? categoriaId);

    Task<BaseResponse> AddAsync(InstructorDtoRequest request);
    Task<BaseResponse<InstructorDtoRequest>> FindByIdAsync(int id);
    Task<BaseResponse> UpdateAsync(int id, InstructorDtoRequest request);
    Task<BaseResponse> DeleteAsync(int id);
}