using System.Xml.Linq;
using TouristRoutes.Models;

namespace TouristRoutes.Services
{
    public class RouteService : IRouteService
    {
        private readonly HttpClient _httpClient = new HttpClient();
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
        
        public List<Coordinates> ParseGpxTrack(string gpxXml)
        {
            if (string.IsNullOrWhiteSpace(gpxXml))
                return new List<Coordinates>();

            var doc = XDocument.Parse(gpxXml);
            
            return doc.Descendants()
                .Where(e => e.Name.LocalName == "trkpt")
                .Select(trkpt => new Coordinates(
                    (double)trkpt.Attribute("lat"),
                    (double)trkpt.Attribute("lon"),
                    0))
                .ToList();
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

        public void NotifySubscribers(TouristRoute route)
        {
            SendEmail(route);
            SendSms(route);
            LogToDatabase(route);
        }

        private void SendEmail(TouristRoute route) { /* SMTP логика */ }
        private void SendSms(TouristRoute route) { /* SMS логика */ }
        private void LogToDatabase(TouristRoute route) { /* DB логика */ }
    }
}