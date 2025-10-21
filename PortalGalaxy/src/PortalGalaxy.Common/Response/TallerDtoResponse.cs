using System;

namespace PortalGalaxy.Common.Response;

public class TallerDtoResponse
{
    public int Id { get; set; }
    public string Taller { get; set; } = null!;
    public string Categoria { get; set; } = null!;
    public string Instructor { get; set; } = null!;
    public string Fecha { get; set; } = null!;
    public string Situacion { get; set; } = null!;
}
