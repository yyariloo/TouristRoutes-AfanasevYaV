using TouristRoutes.Models;

namespace TouristRoutes.Services
{
    public interface IRouteService
    {
        Task<TouristRoute> GetRouteByIdAsync(int id, CancellationToken ct = default);

        Task SaveRouteGpxFileAsync(int routeId, string gpxContent, CancellationToken ct = default);
        
        Task DownloadAndCacheExternalMapAsync(int routeId, CancellationToken ct = default);

        void NotifySubscribers(TouristRoute route);
    }
}