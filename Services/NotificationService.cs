using TouristRoutes.Models;

namespace TouristRoutes.Services
{
    public class NotificationService : INotificationService
    {
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