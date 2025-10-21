using Microsoft.AspNetCore.Mvc;
using PortalGalaxy.Common.Request;
using PortalGalaxy.Services.Interfaces;

namespace PortalGalaxy.ApiRest.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly IUserService _service;
        private readonly ILogger<UsersController> _logger;

        public UsersController(IUserService service, ILogger<UsersController> logger)
        {
            _service = service;
            _logger = logger;
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginDtoRequest request)
        {
            var response = await _service.LoginAsync(request);

            _logger.LogInformation("Se inicio sesion desde {RequestID}", HttpContext.Connection.Id);

            return response.Success ? Ok(response) : Unauthorized(response);
        }

        // POST: api/Usuarios/Register
        [HttpPost]
        public async Task<IActionResult> Register(RegisterUserDto request)
        {
            var response = await _service.RegisterAsync(request);

            return Ok(response);
        }
    }
}
