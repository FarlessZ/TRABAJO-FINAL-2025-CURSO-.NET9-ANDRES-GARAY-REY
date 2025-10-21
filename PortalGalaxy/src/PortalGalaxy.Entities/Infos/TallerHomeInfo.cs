namespace PortalGalaxy.Entities.Infos;

public class TallerHomeInfo
{
    public int Id { get; set; }
    public string Nombre { get; set; } = null!;
    public DateOnly FechaInicio { get; set; }
    public TimeOnly HoraInicio { get; set; }
    public string? PortadaUrl { get; set; }
    public string? TemarioUrl { get; set; }
    public string? Descripcion { get; set; }
    public string Instructor { get; set; } = null!;
}