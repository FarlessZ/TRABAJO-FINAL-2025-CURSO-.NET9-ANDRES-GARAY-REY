using BlazorBootstrap;
using CurrieTechnologies.Razor.SweetAlert2;
using Microsoft.JSInterop;
using PortalGalaxy.Common.Response;
using PortalGalaxy.WebApp.Proxy.Interfaces;
using PortalGalaxy.WebApp.Xls;

namespace PortalGalaxy.WebApp.Pages.Talleres;

public partial class TalleresListPage
{
    private readonly IJsonProxy _jsonProxy;
    private readonly ICategoriaProxy _categoriaProxy;

    public TalleresListPage(IJsonProxy jsonProxy, ICategoriaProxy categoriaProxy)
    {
        _jsonProxy = jsonProxy;
        _categoriaProxy = categoriaProxy;
    }

    public ICollection<TallerDtoResponse>? Lista { get; set; }

    public ICollection<CategoriaDtoResponse> Categorias { get; set; } = new List<CategoriaDtoResponse>();
    public ICollection<SituacionModel> Situaciones { get; set; } = new List<SituacionModel>();

    public Grid<TallerDtoResponse> Grilla { get; set; } = null!;

    public bool IsLoading { get; set; }
    public string? Nombre { get; set; }
    public int? CategoriaId { get; set; }
    public int? Situacion { get; set; }

    public int CurrentPage { get; set; } = 1;
    public int PageSize { get; set; } = 15;
    public int TotalCount { get; set; }

    protected override async Task OnInitializedAsync()
    {
        Categorias = await _categoriaProxy.ListAsync();
        Situaciones = await _jsonProxy.ListSituaciones();
    }

    private async Task OnRefresh()
    {
        await Grilla.ResetPageNumber();
        await Grilla.RefreshDataAsync();
    }

    private void OnLimpiar()
    {
        Nombre = null;
        CategoriaId = null;
        Situacion = null;
    }

    private async Task OnSearch()
    {
        try
        {
            var response = await Proxy.ListAsync(Nombre, CategoriaId, Situacion, CurrentPage, PageSize);
            if (response.Success)
            {
                Lista = response.Data;
                TotalCount = response.TotalCount;
            }

            await OnRefresh();
        }
        catch (Exception ex)
        {
            ToastService.ShowError(ex.Message);
        }
    }

    private async Task<GridDataProviderResult<TallerDtoResponse>> OnReadData(GridDataProviderRequest<TallerDtoResponse> request)
    {
        CurrentPage = request.PageNumber;
        PageSize = request.PageSize;

        await OnSearch();

        IsLoading = false;

        return await Task.FromResult(new GridDataProviderResult<TallerDtoResponse>
        {
            Data = Lista ?? new List<TallerDtoResponse>(),
            TotalCount = TotalCount
        });
    }

    private async Task OnExportarPdf()
    {
        try
        {
            IsLoading = true;
            var request = new Common.Request.BusquedaTallerRequest
            {
                Nombre = Nombre,
                Categoria = CategoriaId,
                Situacion = Situacion,
                PageNumber = CurrentPage,
                PageSize = PageSize
            };
            var stream = await Proxy.ExportarPdf(request);

            await using var memory = new MemoryStream();
            await stream.CopyToAsync(memory);
            var byteArray = memory.ToArray();

            await JsRuntime.InvokeVoidAsync("descargarArchivo", "talleres.pdf", byteArray, "application/pdf");
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

    private async Task OnExportarExcel()
    {
        if (Lista is null) return;

        try
        {
            IsLoading = true;
            var plantilla = await HttpClient.GetStreamAsync("assets/xls/TallerTemplate.xlsx");

            var excel = new PlantillaXls();
            var bytes = excel.GenerarPlantilla(plantilla, Lista);

            await JsRuntime.InvokeVoidAsync("descargarArchivo", "talleres.xlsx", bytes, "application/octect-stream");
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

    private void OnNuevo()
    {
        NavigationManager.NavigateTo("taller/nuevo");
    }

    private void OnEditar(int id)
    {
        NavigationManager.NavigateTo($"/taller/editar/{id}");
    }

    private async Task OnEliminar(int id)
    {
        var confirm = await Swal.FireAsync(new SweetAlertOptions("¿Desea eliminar?")
        {
            ShowCancelButton = true,
            CancelButtonText = "No",
            ConfirmButtonText = "Sí, eliminar",
            Icon = SweetAlertIcon.Warning,
            Text = "Esta acción no se puede deshacer"
        });

        if (confirm.IsConfirmed)
        {
            await Proxy.DeleteAsync(id);
        }
    }
}