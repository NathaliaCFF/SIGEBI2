using SIGEBI.Domain.Entities;
using SIGEBI.Shared.Base;

namespace SIGEBI.Application.Interfaces
{
    public interface IPrestamoService
    {
        Task<ServiceResult<Prestamo>> RegistrarPrestamoAsync(int usuarioId, List<int> librosIds);
        Task<ServiceResult> RegistrarDevolucionAsync(int prestamoId, List<int> librosIds);
        Task<ServiceResult<IEnumerable<Prestamo>>> ObtenerPrestamosActivosPorUsuarioAsync(int usuarioId);
        Task<ServiceResult<IEnumerable<Prestamo>>> ObtenerPrestamosVencidosAsync();
    }
}
