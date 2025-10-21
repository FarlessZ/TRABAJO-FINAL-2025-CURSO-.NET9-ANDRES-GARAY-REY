using Microsoft.Extensions.Logging;
using PortalGalaxy.Common.Request;
using PortalGalaxy.Common.Response;
using PortalGalaxy.Services.Interfaces;
using PortalGalaxy.Services.Utils;
using QuestPDF.Fluent;
using QuestPDF.Infrastructure;

namespace PortalGalaxy.Services.Implementaciones;

public class PdfService : IPdfService
{
    private readonly ITallerService _tallerService;
    private readonly ILogger<PdfService> _logger;

    public PdfService(ITallerService tallerService, ILogger<PdfService> logger)
    {
        _tallerService = tallerService;
        _logger = logger;
    }

    public async Task<BaseResponse<Document>> Generar(BusquedaTallerRequest request)
    {
        var response = new BaseResponse<Document>();

        try
        {
            var data = await _tallerService.ListAsync(request);
            if (data is { Success: true, TotalPages: > 0, Data: not null })
            {
                QuestPDF.Settings.CheckIfAllTextGlyphsAreAvailable = false;

                var doc = Document.Create(document =>
                {
                    document.Page(page =>
                    {
                        page.MarginLeft(20);
                        page.MarginTop(20);
                        page.MarginRight(10);
                        page.Header().Row(row =>
                        {
                            row.ConstantItem(120).Height(80).AlignCenter().PaddingTop(20).Text("LISTADO DE TALLERES").FontSize (15);
                        });
                        page.Content()
    .PaddingVertical(15)
    .Column(col =>
    {
        // ENCABEZADO
        col.Item()
            .PaddingTop(10)
            .Border(1)
            .Background("#E7F1FA") // Azul suave
            .Row(row =>
            {
                var headerStyle = TextStyle.Default.SemiBold().FontSize(11);

                row.RelativeItem().AlignCenter().Text("ID").Style(headerStyle);
                row.RelativeItem().AlignCenter().Text("Nombre").Style(headerStyle);
                row.RelativeItem().AlignCenter().Text("Categoría").Style(headerStyle);
                row.RelativeItem().AlignCenter().Text("Instructor").Style(headerStyle);
                row.RelativeItem().AlignCenter().Text("Fecha").Style(headerStyle);
                row.RelativeItem().AlignCenter().Text("Situación").Style(headerStyle);
            });

        // CONTENIDO
        col.Item()
            .Border(1)
            .Row(row =>
            {
                row.RelativeItem().Column(c =>
                {
                    var rowStyle = TextStyle.Default.FontSize(10);

                    foreach (var taller in data.Data)
                    {
                        c.Item()
                            .BorderBottom(0.5f)
                            .PaddingVertical(4)
                            .Row(r =>
                            {
                                r.RelativeItem().AlignCenter().Text(taller.Id.ToString()).Style(rowStyle);
                                r.RelativeItem().Text(taller.Taller).Style(rowStyle);
                                r.RelativeItem().Text(taller.Categoria).Style(rowStyle);
                                r.RelativeItem().Text(taller.Instructor).Style(rowStyle);
                                r.RelativeItem().AlignCenter().Text(taller.Fecha).Style(rowStyle);
                                r.RelativeItem().AlignCenter().Text(taller.Situacion).Style(rowStyle);
                            });
                    }
                });
            });
    });

                    });
                });

                response.Data = doc;
                response.Success = true;
            }
        }
        catch (Exception ex)
        {
            response.ErrorMessage = "Error al generar el PDF";
            _logger.LogError(ex, "{ErrorMessage} {Message}", response.ErrorMessage, ex.Message);
        }

        return response;
    }
}