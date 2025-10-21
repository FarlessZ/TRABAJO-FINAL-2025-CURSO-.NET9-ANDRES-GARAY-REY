using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using PortalGalaxy.Common;
using PortalGalaxy.Common.Configuration;
using PortalGalaxy.Common.Request;
using PortalGalaxy.Common.Response;
using PortalGalaxy.DataAccess;
using PortalGalaxy.Entities;
using PortalGalaxy.Repositories.Interfaces;
using PortalGalaxy.Services.Interfaces;

namespace PortalGalaxy.Services.Implementaciones;

public class UserService : IUserService
{
    private readonly AppSettings _configuration;
    private readonly UserManager<GalaxyIdentityUser> _userManager;
    private readonly ILogger<UserService> _logger;
    private readonly IAlumnoRepository _alumnoRepository;

    public UserService(IOptions<AppSettings> configuration,
            UserManager<GalaxyIdentityUser> userManager,
            ILogger<UserService> logger,
            IAlumnoRepository alumnoRepository)
    {
        _configuration = configuration.Value;
        _userManager = userManager;
        _logger = logger;
        _alumnoRepository = alumnoRepository;
    }

    public async Task<LoginDtoResponse> LoginAsync(LoginDtoRequest request)
    {
        var response = new LoginDtoResponse();

        try
        {
            var identity = await _userManager.FindByNameAsync(request.Usuario);

            if (identity is null)
                throw new SecurityException("Usuario no existe");

            // Primero checamos que el usuario no este bloqueado
            if (await _userManager.IsLockedOutAsync(identity))
            {
                throw new SecurityException($"Demasiados intentos fallidos para el usuario {request.Usuario}");
            }

            // Validamos el usuario y clave
            if (!await _userManager.CheckPasswordAsync(identity, request.Password))
            {
                response.ErrorMessage = "Clave incorrecta";
                _logger.LogWarning($"Intento fallido de Login para el usuario {identity.UserName}");
                await _userManager.AccessFailedAsync(identity);

                return response;
            }

            var roles = await _userManager.GetRolesAsync(identity);
            var fechaExpiracion = DateTime.Now.AddHours(1);

            // Vamos a devolver los Claims
            var claims = new List<Claim>()
            {
                new Claim(ClaimTypes.Name, identity.NombreCompleto),
                new Claim(ClaimTypes.Email, identity.Email!),
                new Claim(ClaimTypes.Expiration, fechaExpiracion.ToString("yyyy-MM-dd HH:mm:ss")),
            };

            claims.AddRange(roles.Select(x => new Claim(ClaimTypes.Role, x)));

            response.Roles = roles.ToList();

            // Creamos el JWT
            var llaveSimetrica = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration.Jwt.SecretKey));
            var credenciales = new SigningCredentials(llaveSimetrica, SecurityAlgorithms.HmacSha256);

            var header = new JwtHeader(credenciales);

            var payload = new JwtPayload(
                _configuration.Jwt.Issuer,
                _configuration.Jwt.Audience,
                claims,
                DateTime.Now,
                fechaExpiracion
            );

            var jwtToken = new JwtSecurityToken(header, payload);

            response.Token = new JwtSecurityTokenHandler().WriteToken(jwtToken);
            response.NombreCompleto = identity.NombreCompleto;
            response.Success = true;

            _logger.LogInformation("Se creó el JWT de forma satisfactoria");
        }
        catch (SecurityException ex)
        {
            response.ErrorMessage = ex.Message;
            _logger.LogError(ex, "Error de seguridad {Message}", ex.Message);
        }
        catch (Exception ex)
        {
            response.ErrorMessage = "Error inesperado";
            _logger.LogError(ex, "Error al autenticar {Message}", ex.Message);
        }

        return response;
    }

    public async Task<BaseResponse> RegisterAsync(RegisterUserDto request)
    {
        var response = new BaseResponse();

        try
        {
            var identity = new GalaxyIdentityUser
            {
                NombreCompleto = request.NombreCompleto,
                UserName = request.Usuario,
                Email = request.Email,
                EmailConfirmed = true
            };

            var result = await _userManager.CreateAsync(identity, request.Password);
            if (result.Succeeded)
            {
                await _userManager.AddToRoleAsync(identity, Constantes.RolAlumno);

                var alumno = new Alumno
                {
                    NombreCompleto = request.NombreCompleto,
                    Correo = request.Email,
                    Telefono = request.Telefono,
                    NroDocumento = request.NroDocumento,
                    Departamento = request.CodigoDepartamento,
                    Provincia = request.CodigoProvincia,
                    Distrito = request.CodigoDistrito,
                    FechaInscripcion = DateTime.Today
                };

                await _alumnoRepository.AddAsync(alumno);

                // Enviar un email
                // await _emailService.SendEmailAsync(request.Email, "Registro exitoso",
                //     $@"<strong>Bienvenid@ al sistema</strong>
                //       Su cuenta <i>{request.Usuario}</i> se ha creado de forma exitosa
                //     ");
            }
            else
            {
                var sb = new StringBuilder();
                foreach (var identityError in result.Errors)
                {
                    sb.AppendFormat("{0} ", identityError.Description);
                }

                response.ErrorMessage = sb.ToString();
                sb.Clear();
            }

            response.Success = result.Succeeded;
        }
        catch (Exception ex)
        {
            response.ErrorMessage = "Error al registrar";
            _logger.LogWarning(ex, "{MensajeError} {Message}", response.ErrorMessage, ex.Message);
        }

        return response;
    }
}
