using SIGEBI.Domain.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SIGEBI.Domain.Repository
{
    public interface IReporteRepository
    {
        Task<IEnumerable<Reporte>> LibrosMasPrestadosAsync();
    }
}
