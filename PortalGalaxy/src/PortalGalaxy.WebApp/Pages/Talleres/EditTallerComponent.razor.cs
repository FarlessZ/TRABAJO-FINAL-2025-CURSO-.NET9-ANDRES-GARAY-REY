using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.JSInterop;
using PortalGalaxy.Common.Request;
using PortalGalaxy.Common.Response;

namespace PortalGalaxy.WebApp.Pages.Talleres
{
    public partial class EditTallerComponent
    {

        [Parameter]
        [EditorRequired]
        public string Titulo { get; set; } = string.Empty;

        [Parameter] public TallerDtoRequest Request { get; set; } = new();

        [Parameter]
        [EditorRequired]
        public ICollection<CategoriaDtoResponse> Categorias { get; set; } = new List<CategoriaDtoResponse>();

        [Parameter]
        [EditorRequired]
        public ICollection<SituacionModel> Situaciones { get; set; } = new List<SituacionModel>();


        [Parameter]
        [EditorRequired]
        public EventCallback<TallerDtoRequest> OnGuardar { get; set; }

        [Parameter]
        public string NombreInstructor { get; set; } = null!;

        private InstructorDtoResponse? Seleccionado { get; set; }

        private const long MaxFileSize = 1024 * 1024 * 15; // 15 MB

        private void OnGrabar()
        {
            if (Seleccionado is not null)
                Request.InstructorId = Seleccionado.Id;

            OnGuardar.InvokeAsync(Request);
        }


        private async Task OnPortadaChanged(InputFileChangeEventArgs args)
        {
            try
            {
                var imagen = args.File;
                var buffer = new byte[imagen.Size];
                _ = await imagen.OpenReadStream(MaxFileSize).ReadAsync(buffer);

                Request.PortadaBase64 = Convert.ToBase64String(buffer);
                Request.PortadaFileName = imagen.Name;
                Request.PortadaUrl = null!;

                StateHasChanged();
            }
            catch (Exception ex)
            {
                await Swal.FireAsync("Error de Archivo", ex.Message);
            }
        }

        private async Task OnTemarioChanged(InputFileChangeEventArgs args)
        {
            try
            {
                var archivo = args.File;
                var buffer = new byte[archivo.Size];
                _ = await archivo.OpenReadStream(MaxFileSize).ReadAsync(buffer);

                Request.TemarioBase64 = Convert.ToBase64String(buffer);
                Request.TemarioFileName = archivo.Name;
                Request.TemarioUrl = null!;

                StateHasChanged();
            }
            catch (Exception ex)
            {
                await Swal.FireAsync("Error de Archivo", ex.Message);
            }
        }

        private void InstructorSeleccionado(InstructorDtoResponse item)
        {
            NombreInstructor = item.Nombres;
            Request.InstructorId = item.Id;
            Seleccionado = item;
        }

    }
}