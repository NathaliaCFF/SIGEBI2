using SIGEBI.Domain.Entities;
using SIGEBI.Shared.Base;

namespace SIGEBI.Domain.Repository
{
    public interface IPrestamoRepository : IBaseRepository<Prestamo>
    {
        Task<IEnumerable<Prestamo>> ObtenerPorUsuarioAsync(int usuarioId);
        Task<IEnumerable<Prestamo>> ObtenerPrestamosVencidosAsync();
    }
}
