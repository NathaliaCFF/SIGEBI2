using SIGEBI.Domain.Entities;
using SIGEBI.Shared.Base;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SIGEBI.Domain.Repository
{
    public interface IDetallePrestamoRepository : IBaseRepository<DetallePrestamo>
    {
        Task<IEnumerable<DetallePrestamo>> ObtenerPorPrestamoAsync(int prestamoId);
        Task RegistrarDevolucionAsync(int detalleId);
    }
}
