using BlazorBootstrap;
using Microsoft.JSInterop;
using PortalGalaxy.Common.Request;
using PortalGalaxy.Common.Response;
using PortalGalaxy.WebApp.Xls;

namespace PortalGalaxy.WebApp.Pages.Talleres
{
    public partial class ListInscritosPorTaller
    {

        private ICollection<CategoriaDtoResponse> Categorias { get; set; } = new List<CategoriaDtoResponse>();

        private ICollection<SituacionModel> Situaciones { get; set; } = new List<SituacionModel>();

        private ICollection<InscritosPorTallerDtoResponse>? Inscripciones { get; set; }

        public bool IsLoading { get; set; }

        public BusquedaInscritosPorTallerRequest BusquedaRequest { get; set; } = new() { PageNumber = 1, PageSize = 10 };

        public Grid<InscritosPorTallerDtoResponse> Grilla { get; set; } = null!;

        protected override async Task OnInitializedAsync()
        {
            Categorias = await CategoriaProxy.ListAsync();
            Situaciones = await JsonProxy.ListSituaciones();
        }

        private async Task OnRefresh()
        {
            await Grilla.RefreshDataAsync();   
        }

        private async Task OnSearch()
        {
            try
            {
                IsLoading = true;

                var response = await TallerProxy.ListAsync(BusquedaRequest);

                Inscripciones = response.Data;
            }
            catch (Exception ex)
            {
                ToastService.ShowError(ex.Message);
            }
            finally
            {
                IsLoading = false;
            }
        }

        private void OnLimpiar()
        {
            BusquedaRequest = new() { PageNumber = 1, PageSize = 10 };
        }

        private async Task OnExportarPdf()
        {
            if (Inscripciones is null) return;

            try
            {

                // TODO: Implementar exportacion a PDF
                await Task.CompletedTask;
            }
            catch (Exception ex)
            {
                ToastService.ShowError(ex.Message);
            }
        }

        private async Task OnExportarExcel()
        {
            if (Inscripciones is null) return;

            try
            {
                IsLoading = true;
                var plantilla = await HttpClient.GetStreamAsync("assets/xls/InscripcionTemplate.xlsx");

                var excel = new PlantillaXls();
                var bytes = excel.GenerarPlantilla(plantilla, Inscripciones);

                await JsRuntime.InvokeVoidAsync("descargarArchivo", "inscripciones.xlsx", bytes, "application/octet-stream");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                ToastService.ShowError(ex.Message);
            }
            finally
            {
                IsLoading = false;
            }
        }

        private async Task<GridDataProviderResult<InscritosPorTallerDtoResponse>> DataProvider(GridDataProviderRequest<InscritosPorTallerDtoResponse> request)
        {
            BusquedaRequest.PageNumber = request.PageNumber;
            BusquedaRequest.PageSize = request.PageSize;

            await OnSearch();
            if (Inscripciones is not null)
                request.ApplyTo(Inscripciones);

            return new GridDataProviderResult<InscritosPorTallerDtoResponse>
            {
                Data = Inscripciones,
                TotalCount = Inscripciones?.Count() ?? 0
            };
        }

        private void InstructorSeleccionado(InstructorDtoResponse item)
        {
            BusquedaRequest.Instructor = item.Nombres;
            BusquedaRequest.InstructorId = item.Id;
        }

    }
}