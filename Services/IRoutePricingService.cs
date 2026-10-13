using TouristRoutes.Models;

namespace TouristRoutes
{
    public interface IRoutePricingService
    {
        decimal CalculatePrice(TouristRoute route, int days);
    }
}