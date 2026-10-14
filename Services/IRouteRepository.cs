using System.Threading;
using System.Threading.Tasks;
using TouristRoutes.Models;

namespace TouristRoutes.Services
{
    public interface IRouteRepository
    {
        Task<TouristRoute> GetRouteByIdAsync(int id, CancellationToken ct = default);
    }
}