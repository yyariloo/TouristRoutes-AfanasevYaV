using Microsoft.AspNetCore.Mvc;
using TouristRoutes.Models;
using TouristRoutes.Services;

namespace TouristRoutes.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class RoutesController : ControllerBase
    {
        private readonly IRouteService _service;

        public RoutesController(IRouteService service)
        {
            _service = service;
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetRoute(int id)
        {
            var route = await _service.GetRouteByIdAsync(id);
            return Ok(route);
        }

        [HttpPost("upload-gpx")]
        public async Task<IActionResult> UploadGpx(int id, [FromBody] string gpxData)
        {
            await _service.SaveRouteGpxFileAsync(id, gpxData);
            _service.NotifySubscribers(new TouristRoute { Id = id });
            return Ok();
        }

        [HttpGet("filter")]
        public async Task<IActionResult> GetFilteredRoutes([FromQuery] RouteFilterCriteria filter, CancellationToken ct)
        {
            var routes = await FilterRoutesAsync(filter, ct);
            return Ok(routes);
        }

        private async Task<TouristRoute[]> FilterRoutesAsync(RouteFilterCriteria criteria, CancellationToken ct)
        {
            await Task.Delay(500, ct);
            return Array.Empty<TouristRoute>();
        }
    }
}