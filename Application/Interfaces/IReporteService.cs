using SIGEBI.Domain.Entities;
using SIGEBI.Shared.Base;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Application.Interfaces
{
    public interface IReporteService
    {
        Task<ServiceResult<IEnumerable<Reporte>>> ObtenerLibrosMasPrestadosAsync();
    }
}
