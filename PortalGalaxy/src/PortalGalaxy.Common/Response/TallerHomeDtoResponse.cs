namespace PortalGalaxy.Common.Response;

public class TallerHomeDtoResponse
{
    public int Id { get; set; }
    public string Nombre { get; set; } = null!;
    public string FechaInicio { get; set; } = null!;
    public string HoraInicio { get; set; } = null!;
    public string? PortadaUrl { get; set; }
    public string? TemarioUrl { get; set; }
    public string? Descripcion { get; set; }
    public string Instructor { get; set; } = null!;
}