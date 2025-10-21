using PortalGalaxy.Common.Request;
using PortalGalaxy.Common.Response;

namespace PortalGalaxy.Services.Interfaces;

public interface ITallerService
{
    Task<PaginationResponse<TallerDtoResponse>> ListAsync(BusquedaTallerRequest request);
    Task<PaginationResponse<InscritosPorTallerDtoResponse>> ListAsync(BusquedaInscritosPorTallerRequest request);
    Task<BaseResponse<ICollection<TallerSimpleDtoResponse>>> ListSimpleAsync();
    Task<PaginationResponse<TallerHomeDtoResponse>> ListarTalleresHomeAsync(BusquedaTallerHomeRequest request);
    Task<BaseResponse> AddAsync(TallerDtoRequest request);
    Task<BaseResponse<TallerDtoRequest>> FindByIdAsync(int id);
    
    Task<BaseResponse<TallerHomeDtoResponse>> GetTallerHomeAsync(int id);

    Task<BaseResponse<ICollection<TalleresPorMesDto>>> ReporteTalleresPorMes(int anio);
    
    Task<BaseResponse<ICollection<TalleresPorInstructorDto>>> ReporteTalleresPorInstructor(int anio);
    
    Task<BaseResponse> UpdateAsync(int id, TallerDtoRequest request);
    Task<BaseResponse> DeleteAsync(int id);
}