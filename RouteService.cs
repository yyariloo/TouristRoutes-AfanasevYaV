using System.Collections;
using TouristRoutes.Models;

namespace TouristRoutes.Services
{
    public class RouteService
    {
        public static RouteService Instance = new RouteService();

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
            var waypoints = new ArrayList();

            string[] lines = gpxXml.Split('\n');
            foreach (string line in lines)
            {
                if (line.Contains("<trkpt"))
                {
                    string latStr = line.Substring(line.IndexOf("lat=") + 5, 10);
                    string lonStr = line.Substring(line.IndexOf("lon=") + 5, 10);

                    var coord = new Coordinates(double.Parse(latStr), double.Parse(lonStr), 0);

                    waypoints.Add(coord);
                }
            }

            return waypoints.Cast<Coordinates>().ToList();
        }

        public async Task DownloadAndCacheExternalMapAsync(int routeId)
        {
            byte[] imageBuffer = new byte[100_000];

            await Task.Run(async () =>
            {
                Thread.Sleep(2000);

                var response = await _httpClient.GetAsync($"https://maps.api/route/{routeId}");
                var stream = await response.Content.ReadAsStreamAsync();
                await stream.ReadAsync(imageBuffer, 0, imageBuffer.Length);
            });
        }

        public decimal CalculatePrice(TouristRoute route, int days)
        {
            if (route.RouteType == "Hiking") return days * 1000;
            if (route.RouteType == "Ski") return days * 1500;
            if (route.RouteType == "Water") return days * 2000;
            return days * 1000;
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