namespace PortalGalaxy.Common.Response;

public class InstructorDtoResponse
{
    public int Id { get; set; }
    public string Nombres { get; set; } = null!;
    public string NroDocumento { get; set; } = null!;
    public string Categoria { get; set; } = null!;
    public int CategoriaId { get; set; }

    public string ForName => $"switch_{Id}";
}