using SIGEBI.Domain.Entities;

namespace SIGEBI.Domain.Repository
{
    public interface IReporteRepository
    {
        Task<IEnumerable<Reporte>> LibrosMasPrestadosAsync();
    }
}
