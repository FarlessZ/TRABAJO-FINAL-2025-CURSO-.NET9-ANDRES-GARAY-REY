namespace PortalGalaxy.Entities;

public class Alumno : EntityBase
{
    public string NombreCompleto { get; set; } = null!;

    public string NroDocumento { get; set; } = null!;

    public string Correo { get; set; } = null!;
    public string? Telefono { get; set; }
    public string Departamento { get; set; } = null!;
    public string Provincia { get; set; } = null!;
    public string Distrito { get; set; } = null!;
    public DateTime FechaInscripcion { get; set; }
    
    public virtual ICollection<Inscripcion> Inscripcions { get; set; } = new List<Inscripcion>();
}