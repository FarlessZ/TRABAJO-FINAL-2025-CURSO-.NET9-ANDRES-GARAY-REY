using PortalGalaxy.Common.Request;
using PortalGalaxy.Common.Response;

namespace PortalGalaxy.Services.Interfaces;

public interface ICategoriaService
{
    Task<PaginationResponse<CategoriaDtoResponse>> ListAsync();

    Task<BaseResponse<CategoriaDtoRequest>> FindByIdAsync(int id);

    Task<BaseResponse> CreateAsync(CategoriaDtoRequest request);

    Task<BaseResponse> UpdateAsync(int id, CategoriaDtoRequest request);

    Task<BaseResponse> DeleteAsync(int id);
}