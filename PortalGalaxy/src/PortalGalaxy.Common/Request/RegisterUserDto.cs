using System;
using System.ComponentModel.DataAnnotations;

namespace PortalGalaxy.Common.Request;

public class RegisterUserDto
{
    [Required]
    public string Usuario { get; set; } = null!;

    [Required]  
    public string NombreCompleto { get; set; } = null!;

    [EmailAddress]
    public string Email { get; set; } = null!;

    [Required]
    public string Telefono { get; set; } = null!;

    [Required]
    public string NroDocumento { get; set; } = null!;

    [Required]
    public string Password { get; set; } = null!;

    [Compare(nameof(Password))]
    public string ConfirmPassword { get; set; } = null!;
    
    public string CodigoDepartamento { get; set; } = string.Empty;
    public string CodigoProvincia { get; set; } = string.Empty;
    public string CodigoDistrito { get; set; } = string.Empty;
}
