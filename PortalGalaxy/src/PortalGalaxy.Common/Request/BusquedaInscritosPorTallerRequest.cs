namespace PortalGalaxy.Common.Request;

public class BusquedaInscritosPorTallerRequest : RequestBase
{
    public string? Instructor { get; set; }
    public int? InstructorId { get; set; }
    public string? Taller { get; set; }
    public int? Situacion { get; set; }
    public DateOnly? FechaInicio { get; set; }
    public DateOnly? FechaFin { get; set; }

}