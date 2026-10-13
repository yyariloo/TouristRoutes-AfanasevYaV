using TouristRoutes.Models;

namespace TouristRoutes.Services
{
    public class RoutePricingService : IRoutePricingService
    {
        public decimal CalculatePrice(TouristRoute route, int days) => route.RouteType switch
        {
            RouteType.Hiking => days * 1000m,
            RouteType.Ski => days * 1500m,
            RouteType.Water => days * 2000m,
            _ => days * 1000m
        };
    }
}