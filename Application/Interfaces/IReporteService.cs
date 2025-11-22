using SIGEBI.Domain.Entities;
using SIGEBI.Shared.Base;

namespace SIGEBI.Application.Interfaces
{
    public interface IReporteService
    {
        Task<ServiceResult<IEnumerable<Reporte>>> ObtenerLibrosMasPrestadosAsync();
    }
}
