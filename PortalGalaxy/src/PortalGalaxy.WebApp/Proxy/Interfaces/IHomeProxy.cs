using PortalGalaxy.Common.Request;
using PortalGalaxy.Common.Response;

namespace PortalGalaxy.WebApp.Proxy.Interfaces;

public interface IHomeProxy
{
    Task<PaginationResponse<TallerHomeDtoResponse>> ListarTalleresHomeAsync(BusquedaTallerHomeRequest request);
    
    Task<BaseResponse<TallerHomeDtoResponse>> GetTallerHomeAsync(int id);
}