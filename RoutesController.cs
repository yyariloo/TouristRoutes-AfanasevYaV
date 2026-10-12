using Microsoft.AspNetCore.Mvc;
using System.Threading;
using System.Threading.Tasks;
using TouristRoutes.Models;
using TouristRoutes.Services;

namespace TouristRoutes.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class RoutesController : ControllerBase
    {
        private readonly RouteService _service = RouteService.Instance;

        [HttpGet("{id}")]
        public async Task<IActionResult> GetRoute(int id)
        {
            var route = await _service.GetRouteByIdAsync(id);
            return Ok(route);
        }

        [HttpPost("upload-gpx")]
        public async Task<IActionResult> UploadGpx(int id, [FromBody] string gpxData)
        {
            _service.SaveRouteGpxFile(id, gpxData);

            _service.NotifySubscribers(new TouristRoute { Id = id });

            return Ok();
        }

        [HttpGet("filter")]
        public IActionResult GetFilteredRoutes()
        {
            var filter = new RouteFilterCriteria
            {
                MinLat = 50.0,
                MaxLat = 60.0,
                RegionName = "Caucasus",
                Season = "Summer"
                // ... много полей
            };

            var routes = FilterRoutes(filter);
            return Ok(routes);
        }

        private TouristRoute[] FilterRoutes(RouteFilterCriteria criteria)
        {
            // Имитация фильтрации
            Thread.Sleep(500);
            return new TouristRoute[0];
        }
    }
}