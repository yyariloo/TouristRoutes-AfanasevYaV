using System.ComponentModel.DataAnnotations;

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
        [Range(-90, 90, ErrorMessage = "MinLat must be between -90 and 90")]
        public double MinLat { get; set; }

        [Range(-90, 90, ErrorMessage = "MaxLat must be between -90 and 90")]
        public double MaxLat { get; set; }

        [Range(-180, 180, ErrorMessage = "MinLng must be between -180 and 180")]
        public double MinLng { get; set; }

        [Range(-180, 180, ErrorMessage = "MaxLng must be between -180 and 180")]
        public double MaxLng { get; set; }

        [Range(0, 365, ErrorMessage = "MinDurationDays must be between 0 and 365")]
        public int MinDurationDays { get; set; }

        [Range(0, 365, ErrorMessage = "MaxDurationDays must be between 0 and 365")]
        public int MaxDurationDays { get; set; }

        [Range(0, 10, ErrorMessage = "MinDifficulty must be between 0 and 10")]
        public double MinDifficulty { get; set; }

        [Range(0, 10, ErrorMessage = "MaxDifficulty must be between 0 and 10")]
        public double MaxDifficulty { get; set; }

        [StringLength(100, ErrorMessage = "RegionName must not exceed 100 characters")]
        public string? RegionName { get; set; }

        [StringLength(50, ErrorMessage = "Season must not exceed 50 characters")]
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