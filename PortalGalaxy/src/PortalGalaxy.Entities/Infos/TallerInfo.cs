namespace PortalGalaxy.Entities.Infos;

public class TallerInfo
{
    public int Id { get; set; }
    public string Nombre { get; set; } = null!;
    public string Categoria { get; set; } = null!;
    public string Instructor { get; set; } = null!;
    public DateOnly Fecha { get; set; } = default!;
    public string Situacion { get; set; } = null!;
}