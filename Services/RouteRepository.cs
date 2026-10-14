using System.Threading;
using System.Threading.Tasks;
using TouristRoutes.Models;

namespace TouristRoutes.Services
{
    public class RouteRepository : IRouteRepository
    {
        public async Task<TouristRoute> GetRouteByIdAsync(int id, CancellationToken ct = default)
        {
            await Task.Delay(50, ct);
            return new TouristRoute { Id = id, Name = "Test" };
        }
    }
}