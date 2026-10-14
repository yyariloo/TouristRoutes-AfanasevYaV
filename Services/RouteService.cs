using TouristRoutes.Models;

namespace TouristRoutes.Services
{
    public class RouteService : IRouteService
    {
        private readonly IRouteRepository _routeRepository;
        private readonly INotificationService _notificationService;

        public RouteService(
            IRouteRepository routeRepository,
            INotificationService notificationService)
        {
            _routeRepository = routeRepository;
            _notificationService = notificationService;
        }

        public Task<TouristRoute> GetRouteByIdAsync(int id, CancellationToken ct = default)
        {
            return _routeRepository.GetRouteByIdAsync(id, ct);
        }

        public async Task SaveRouteGpxFileAsync(int routeId, string gpxContent, CancellationToken ct = default)
        {
            const string routesDirectory = "C:\\routes";
            Directory.CreateDirectory(routesDirectory);
            var path = Path.Combine(routesDirectory, $"{routeId}.gpx");
            await using var fs = new FileStream(path, FileMode.Create);
            await using var writer = new StreamWriter(fs);
            await writer.WriteAsync(gpxContent.AsMemory(), ct);
        }

        public void NotifySubscribers(TouristRoute route)
        {
            _notificationService.NotifySubscribers(route);
        }
    }
}