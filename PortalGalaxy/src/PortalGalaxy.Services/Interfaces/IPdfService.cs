using PortalGalaxy.Common.Request;
using PortalGalaxy.Common.Response;
using QuestPDF.Fluent;

namespace PortalGalaxy.Services.Interfaces;

public interface IPdfService
{
    Task<BaseResponse<Document>> Generar(BusquedaTallerRequest request);
}