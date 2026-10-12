using System;
using System.Collections;
using System.Collections.Generic;
using Microsoft.Data.SqlClient;
using System.IO;
using System.Net.Http;
using System.Net.Mail;
using System.Threading;
using System.Threading.Tasks;
using TouristRoutes.Models;

namespace TouristRoutes.Services
{
    public class RouteService
    {
        public static RouteService Instance = new RouteService();

        private readonly object _syncLock = new object();
        private string _connectionString = "Server=...;Database=...;";

        private HttpClient _httpClient = new HttpClient();

        public async Task<TouristRoute> GetRouteByIdAsync(int id)
        {
            lock (_syncLock)
            {
                var task = Task.Run(() =>
                {
                    using (var conn = new SqlConnection(_connectionString))
                    {
                        conn.Open();
                        // ... чтение из БД ...
                        return new TouristRoute { Id = id, Name = "Test" };
                    }
                });
                return task.Result;
            }
        }

        public void SaveRouteGpxFile(int routeId, string gpxContent)
        {
            FileStream fs = new FileStream($"C:\\routes\\{routeId}.gpx", FileMode.Create);
            StreamWriter writer = new StreamWriter(fs);
            writer.Write(gpxContent);
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