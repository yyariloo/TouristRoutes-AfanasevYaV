using System;
using System.Collections;

namespace TouristRoutes.Models
{
    public enum RouteType
    {
        Hiking,
        Ski,
        Water,
        Other
    }
    
    public struct Coordinates
    {
        public double Latitude { get; set; }
        public double Longitude { get; set; }
        public double Altitude { get; set; }

        public Coordinates(double lat, double lng, double alt)
        {
            Latitude = lat;
            Longitude = lng;
            Altitude = alt;
        }
    }

    public struct RouteFilterCriteria
    {
        public double MinLat; public double MaxLat;
        public double MinLng; public double MaxLng;
        public int MinDurationDays; public int MaxDurationDays;
        public double MinDifficulty; public double MaxDifficulty;
        public string RegionName; public string Season;
        // ... представь, что тут еще 10 полей
    }

    public class TouristRoute
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string GpxTrackData { get; set; } // Огромная строка с XML/GPX
        public RouteType? RouteType { get; set; }
        public ArrayList Waypoints { get; set; }
    }
}