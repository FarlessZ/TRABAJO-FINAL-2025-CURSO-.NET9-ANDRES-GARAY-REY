using System;

namespace PortalGalaxy.Common.Response;

public class LoginDtoResponse : BaseResponse
{
    public string NombreCompleto { get; set; } = null!;
    public string Token { get; set; } = null!;
    public List<string> Roles { get; set; } = new();
}
