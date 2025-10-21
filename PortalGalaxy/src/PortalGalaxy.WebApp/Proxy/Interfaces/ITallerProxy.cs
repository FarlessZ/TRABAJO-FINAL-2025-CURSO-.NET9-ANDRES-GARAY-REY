using PortalGalaxy.Common.Request;
using PortalGalaxy.Common.Response;

namespace PortalGalaxy.WebApp.Proxy.Interfaces;

public interface ITallerProxy : ICrudRestHelper<TallerDtoRequest, TallerDtoResponse>
{
    Task<PaginationResponse<TallerDtoResponse>> ListAsync(string? nombre, int? categoriaId, int? situacion, int pageNumber = 1, int pageSize = 5);

    Task<BaseResponse<ICollection<TallerSimpleDtoResponse>>> ListarAsync();

    Task<PaginationResponse<InscritosPorTallerDtoResponse>> ListAsync(BusquedaInscritosPorTallerRequest request);

    Task<BaseResponse<ICollection<TalleresPorMesDto>>> ListarPorMesAsync(int anio);

    Task<BaseResponse<ICollection<TalleresPorInstructorDto>>> ListarPorInstructorAsync(int anio);

    Task<Stream> ExportarPdf(BusquedaTallerRequest request);
}