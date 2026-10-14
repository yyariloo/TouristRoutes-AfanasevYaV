namespace TouristRoutes.Services
{
    public interface IMapService
    {
        Task DownloadAndCacheExternalMapAsync(int routeId, CancellationToken ct = default);
    }
}