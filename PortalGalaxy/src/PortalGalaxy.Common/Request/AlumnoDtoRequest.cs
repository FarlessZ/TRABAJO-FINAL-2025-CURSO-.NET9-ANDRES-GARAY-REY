namespace PortalGalaxy.Common.Request;

public class AlumnoDtoRequest
{
    public string NroDocumento { get; set; } = null!;
    public string NombreCompleto { get; set; } = null!;
    public string Correo { get; set; } = null!;
    public string? Telefono { get; set; }
    public string Departamento { get; set; } = null!;
    public string Provincia { get; set; } = null!;
    public string Distrito { get; set; } = null!;
}