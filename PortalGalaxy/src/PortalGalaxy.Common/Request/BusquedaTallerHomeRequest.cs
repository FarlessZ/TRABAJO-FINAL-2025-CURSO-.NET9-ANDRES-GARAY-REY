namespace PortalGalaxy.Common.Request;

public class BusquedaTallerHomeRequest : RequestBase
{
    public string? Nombre { get; set; }
    public int? InstructorId { get; set; }
    public DateOnly? FechaInicio { get; set; } 
    public DateOnly? FechaFin { get; set; } 
}