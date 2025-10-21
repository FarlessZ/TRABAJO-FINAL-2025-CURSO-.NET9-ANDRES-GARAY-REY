using Microsoft.AspNetCore.Mvc;
using PortalGalaxy.Common.Request;
using PortalGalaxy.Services.Interfaces;

namespace PortalGalaxy.ApiRest.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class HomeController : ControllerBase
    {
        private readonly ITallerService _tallerService;

        public HomeController(ITallerService tallerService)
        {
            _tallerService = tallerService;
        }

        [HttpGet]
        public async Task<IActionResult> Get([FromQuery] BusquedaTallerHomeRequest request)
        {
            if (request.PageNumber <= 0)
                request.PageNumber = 1;
            if (request.PageSize <= 0)
                request.PageSize = 5;

            var response = await _tallerService.ListarTalleresHomeAsync(request);
            return Ok(response);
        }

        [HttpGet("{id:int}")]
        public async Task<IActionResult> Get(int id)
        {
            var response = await _tallerService.GetTallerHomeAsync(id);
            return Ok(response);
        }
    }
}
