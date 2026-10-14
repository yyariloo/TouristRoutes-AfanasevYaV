namespace TouristRoutes.Models
{
    public enum RouteType
    {
        Hiking,
        Ski,
        Water,
        Other
    }
    public readonly record struct Coordinates(double Latitude, double Longitude, double Altitude);
    public class RouteFilterCriteria
    {
        public double MinLat { get; set; }
        public double MaxLat { get; set; }
        public double MinLng { get; set; }
        public double MaxLng { get; set; }
        public int MinDurationDays { get; set; }
        public int MaxDurationDays { get; set; }
        public double MinDifficulty { get; set; }
        public double MaxDifficulty { get; set; }
        public string? RegionName { get; set; }
        public string? Season { get; set; }
    }

    public class TouristRoute
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string GpxTrackData { get; set; }
        public RouteType? RouteType { get; set; }
        public List<Coordinates> Waypoints { get; set; } = new();
    }
}