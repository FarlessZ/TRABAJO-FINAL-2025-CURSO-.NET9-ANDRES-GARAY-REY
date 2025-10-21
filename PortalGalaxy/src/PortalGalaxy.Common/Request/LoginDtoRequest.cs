using System.ComponentModel.DataAnnotations;

namespace PortalGalaxy.Common.Request;

public class LoginDtoRequest
{
    [Required]
    public string Usuario { get; set; } = null!;

    [Required]
    public string Password { get; set; } = null!;
}
