using TouristRoutes.Models;

namespace TouristRoutes.Services
{
    public interface IGpxParser
    {
        List<Coordinates> ParseGpxTrack(string gpxXml);
    }
}