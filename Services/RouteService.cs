using TouristRoutes.Models;

namespace TouristRoutes.Services
{
    public class RouteService : IRouteService
    {
        private readonly INotificationService _notificationService;

        public RouteService(INotificationService notificationService)
        {
            _notificationService = notificationService;
        }

        public async Task<TouristRoute> GetRouteByIdAsync(int id, CancellationToken ct = default)
        {
            await Task.Delay(50, ct);
            return new TouristRoute { Id = id, Name = "Test" };
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