namespace PortalGalaxy.Entities;

public class Inscripcion : EntityBase
{
    public int AlumnoId { get; set; }

    public int TallerId { get; set; }

    public SituacionInscripcion Situacion { get; set; }

    public virtual Alumno Alumno { get; set; } = null!;

    public virtual Taller Taller { get; set; } = null!;

}