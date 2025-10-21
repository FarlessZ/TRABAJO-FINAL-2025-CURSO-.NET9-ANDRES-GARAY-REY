namespace PortalGalaxy.Common.Request;

public class BusquedaTallerRequest : RequestBase
{
    public string? Nombre { get; set; }
    public int? Categoria { get; set; }
    public int? Situacion { get; set; }
}