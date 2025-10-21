namespace PortalGalaxy.Entities.Infos;

public class InscritosPorTallerInfo
{
    public int Id { get; set; }
    public string Taller { get; set; } = null!;
    public string Categoria { get; set; } = null!;
    public string Instructor { get; set; } = null!;
    public string Fecha { get; set; } = null!;
    public string Situacion { get; set; } = null!;
    public int Cantidad { get; set; }
}