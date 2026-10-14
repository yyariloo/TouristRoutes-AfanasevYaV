namespace TouristRoutes.Services
{
    public class MapService : IMapService
    {
        private readonly HttpClient _httpClient;

        public MapService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task DownloadAndCacheExternalMapAsync(int routeId, CancellationToken ct = default)
        {
            using var response = await _httpClient.GetAsync($"https://maps.api/route/{routeId}", ct);
            response.EnsureSuccessStatusCode();

            await using var stream = await response.Content.ReadAsStreamAsync(ct);

            var cacheDirectory = "C:\\cache";
            Directory.CreateDirectory(cacheDirectory);
            var cachePath = Path.Combine(cacheDirectory, $"{routeId}.png");

            await using var fs = new FileStream(
                cachePath,
                FileMode.Create,
                FileAccess.Write,
                FileShare.None,
                bufferSize: 4096,
                useAsync: true);

            await stream.CopyToAsync(fs, ct);
        }
    }
}