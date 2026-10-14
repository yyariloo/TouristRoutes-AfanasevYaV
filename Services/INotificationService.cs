using TouristRoutes.Models;

namespace TouristRoutes.Services
{
    public interface INotificationService
    {
        void NotifySubscribers(TouristRoute route);
    }
}