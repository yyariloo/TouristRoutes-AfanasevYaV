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
    public readonly record struct RouteFilterCriteria(
        double MinLat,
        double MaxLat,
        double MinLng,
        double MaxLng,
        int MinDurationDays,
        int MaxDurationDays,
        double MinDifficulty,
        double MaxDifficulty,
        string RegionName,
        string Season
    );

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