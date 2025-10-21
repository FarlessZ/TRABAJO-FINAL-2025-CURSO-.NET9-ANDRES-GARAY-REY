namespace PortalGalaxy.Entities;

public class Instructor : EntityBase
{
    public string Nombres { get; set; } = null!;

    public string NroDocumento { get; set; } = null!;

    public int CategoriaId { get; set; }
    public Categoria Categoria { get; set; } = null!;

    public virtual HashSet<Taller> Talleres { get; set; } = new();
}