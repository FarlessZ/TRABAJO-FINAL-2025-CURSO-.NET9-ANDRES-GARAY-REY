namespace PortalGalaxy.Entities.Infos;

public class InstructorInfo
{
    public int Id { get; set; }
    public string Nombres { get; set; } = null!;
    public string NroDocumento { get; set; } = null!;
    public string Categoria { get; set; } = null!;
    public int CategoriaId { get; set; }
}