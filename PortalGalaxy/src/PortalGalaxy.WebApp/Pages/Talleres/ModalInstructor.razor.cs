using BlazorBootstrap;
using Blazored.Toast.Services;
using CurrieTechnologies.Razor.SweetAlert2;
using Microsoft.AspNetCore.Components;
using PortalGalaxy.Common.Request;
using PortalGalaxy.Common.Response;
using PortalGalaxy.WebApp.Proxy.Interfaces;

namespace PortalGalaxy.WebApp.Pages.Talleres
{
    public partial class ModalInstructor(IInstructorProxy InstructorProxy, IToastService ToastService,
        SweetAlertService Swal)
    {

        [Parameter]
        public ICollection<CategoriaDtoResponse> Categorias { get; set; } = new List<CategoriaDtoResponse>();

        [Parameter]
        public EventCallback<InstructorDtoResponse> SeleccionInstructor { get; set; }

        private InstructorDtoRequest Model { get; set; } = new();

        private ICollection<InstructorDtoResponse>? Instructores { get; set; }

        public int? InstructorEditado { get; set; }

        public bool IsLoading { get; set; }

        public Grid<InstructorDtoResponse> Grilla { get; set; } = null!;

        private async Task Cargar()
        {
            try
            {
                Instructores = await InstructorProxy.ListAsync(Model.Nombres, Model.NroDocumento, Model.CategoriaSeleccionada);
            }
            catch (Exception ex)
            {
                ToastService.ShowError(ex.Message);
            }
        }

        private async Task Grabar()
        {
            try
            {
                IsLoading = true;
                if (Model.CategoriaSeleccionada is null)
                    throw new InvalidOperationException("La categoría es requerida");

                Model.CategoriaId = Model.CategoriaSeleccionada.Value;

                if (InstructorEditado.HasValue)
                {
                    await InstructorProxy.UpdateAsync(InstructorEditado.Value, Model);
                    InstructorEditado = null!;
                }
                else
                {
                    await InstructorProxy.CreateAsync<BaseResponse>(Model);
                }

                Model = new();
                await Grilla.RefreshDataAsync();
            }
            catch (Exception ex)
            {
                await Swal.FireAsync(new SweetAlertOptions("No se pudo crear el instructor")
                {
                    Text = ex.Message,
                    Icon = SweetAlertIcon.Error
                });
            }
            finally
            {
                IsLoading = false;
            }
        }

        private async Task OnEditar(InstructorDtoResponse item)
        {
            Model = await InstructorProxy.FindByIdAsync(item.Id);
            InstructorEditado = item.Id;
        }

        private async Task OnEliminar(InstructorDtoResponse item)
        {
            var result = await Swal.FireAsync(new SweetAlertOptions
            {
                Text = "¿Desea eliminar el registro de instructor?",
                Title = "Eliminar",
                Icon = SweetAlertIcon.Question,
                ShowCancelButton = true,
                ConfirmButtonText = "Sí",
                CancelButtonText = "No"
            });

            if (result.IsConfirmed)
            {
                await InstructorProxy.DeleteAsync(item.Id);
                Model = new();
                await Grilla.RefreshDataAsync();
            }
        }

        private void SeleccionaInstructor(InstructorDtoResponse item)
        {
            SeleccionInstructor.InvokeAsync(item);
        }

        private async Task<GridDataProviderResult<InstructorDtoResponse>> OnReadData        (GridDataProviderRequest<InstructorDtoResponse> request)
        {
            await Cargar();

            if (Instructores is not null)
                request.ApplyTo(Instructores);

            return await Task.FromResult(new GridDataProviderResult<InstructorDtoResponse>
            {
                Data = Instructores ?? new List<InstructorDtoResponse>(),
                TotalCount = Instructores?.Count ?? 0
            });
        }
    }

}