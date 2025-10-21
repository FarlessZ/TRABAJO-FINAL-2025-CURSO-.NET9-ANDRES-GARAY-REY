namespace PortalGalaxy.Common.Response;

public class AlumnoSimpleDtoResponse
{
    public int Id { get; set; }
    public string NroDocumento { get; set; } = null!;
    public string NombreCompleto { get; set; } = null!;
    public string FechaRegistro { get; set; } = null!;
    public bool Seleccionado { get; set; }
}