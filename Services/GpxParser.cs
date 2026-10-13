using System.Xml.Linq;
using TouristRoutes.Models;

namespace TouristRoutes.Services
{
    public class GpxParser : IGpxParser
    {
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
    }
}