namespace PortalGalaxy.Common.Request;

public class TallerDtoRequest
{
    public string Nombre { get; set; } = null!;
    public string? Descripcion { get; set; }
    public int CategoriaId { get; set; }
    public int InstructorId { get; set; }
    public DateOnly FechaInicio { get; set; } = DateOnly.FromDateTime(DateTime.Today);
    public TimeOnly HoraInicio { get; set; } = TimeOnly.FromDateTime(DateTime.Now);
    public int Situacion { get; set; }
    public string? PortadaBase64 { get; set; }
    public string? PortadaFileName { get; set; }
    public string? TemarioBase64 { get; set; }
    public string? TemarioFileName { get; set; }
    public string? PortadaUrl { get; set; }
    public string? TemarioUrl { get; set; }
}