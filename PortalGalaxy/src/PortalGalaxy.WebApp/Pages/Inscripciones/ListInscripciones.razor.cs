using BlazorBootstrap;
using Microsoft.JSInterop;
using PortalGalaxy.Common.Request;
using PortalGalaxy.Common.Response;
using PortalGalaxy.WebApp.Xls;

namespace PortalGalaxy.WebApp.Pages.Inscripciones
{
    public partial class ListInscripciones
    {

        private ICollection<SituacionModel> Situaciones { get; set; } = new List<SituacionModel>();

        private ICollection<InscripcionDtoResponse>? Inscripciones { get; set; }

        public bool IsLoading { get; set; }

        public BusquedaInscripcionRequest BusquedaRequest { get; set; } = new() { PageNumber = 1, PageSize = 10 };

        private bool ExportaPdf { get; set; }

        private Grid<InscripcionDtoResponse> Grilla { get; set; } = null!;

        public int CurrentPage { get; set; } = 1;
        public int PageSize { get; set; } = 10;
        public int TotalCount { get; set; }


        protected override async Task OnInitializedAsync()
        {
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
                var response = await InscripcionProxy.ListAsync(BusquedaRequest);

                Inscripciones = response.Data;
            }
            catch (Exception ex)
            {
                ToastService.ShowError(ex.Message);
            }
        }

        private void OnLimpiar()
        {
            BusquedaRequest = new() { PageNumber = 1, PageSize = 5 };
        }

        private async Task OnExportarExcel()
        {
            if (Inscripciones is null) return;

            try
            {
                IsLoading = true;

                var plantilla = await HttpClient.GetStreamAsync("assets/xls/InscritosTemplate.xlsx");

                var excel = new PlantillaXls();
                var xlsStream = excel.GenerarPlantilla(plantilla, Inscripciones);

                await JsRuntime.InvokeVoidAsync("BlazorDownloadFile", "Inscritos.xlsx", xlsStream);
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

        private async Task OnExportarPdf()
        {
            if (Inscripciones is null) return;

            try
            {
                ExportaPdf = true;

                await using var module = await JsRuntime.InvokeAsync<IJSObjectReference>("import", "./js/HtmlToPdf.js");

                var contenidoTabla = await JsRuntime.InvokeAsync<string>("getTableHtml");
                // Generar y descargar el PDF
                await module.InvokeVoidAsync("generateAndDownloadPdf", $"<h1>Listado de Inscritos</h1><hr />{contenidoTabla}", "Inscritos.pdf");

                ExportaPdf = false;
            }
            catch (Exception ex)
            {
                ToastService.ShowError(ex.Message);
            }
        }

        public ICollection<AlumnoSimpleDtoResponse> ListaAlumnos { get; set; } = new List<AlumnoSimpleDtoResponse>();
        public ICollection<TallerSimpleDtoResponse> ListaTalleres { get; set; } = new List<TallerSimpleDtoResponse>();

        private async Task BuscarAlumnos(Tuple<string, string> tupla)
        {
            ListaAlumnos = await AlumnoProxy.ListarAsync(tupla.Item1, tupla.Item2);
        }

        private async Task CargarTalleres()
        {
            var response = await TallerProxy.ListarAsync();
            if (response is { Data: not null })
                ListaTalleres = response.Data;
        }

        private async Task InscribirMasivo(InscripcionMasivaDtoRequest request)
        {
            try
            {
                IsLoading = true;

                await InscripcionProxy.InscripcionMasivaAsync(request);

                ToastService.ShowSuccess("Inscripciones realizadas correctamente");

                await Grilla.RefreshDataAsync();
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

        private async Task<GridDataProviderResult<InscripcionDtoResponse>> OnReadData(GridDataProviderRequest<InscripcionDtoResponse> request)
        {
            BusquedaRequest.PageNumber = request.PageNumber;
            BusquedaRequest.PageSize = request.PageSize;

            await OnSearch();

            if (Inscripciones is not null)
                request.ApplyTo(Inscripciones);

            return await Task.FromResult(new GridDataProviderResult<InscripcionDtoResponse>
            {
                Data = Inscripciones ?? new List<InscripcionDtoResponse>(),
                TotalCount = Inscripciones?.Count ?? 0
            });
        }

    }
}