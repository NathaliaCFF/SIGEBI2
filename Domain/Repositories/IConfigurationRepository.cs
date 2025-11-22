using SIGEBI.Domain.Entities;
using SIGEBI.Shared.Base;

namespace SIGEBI.Domain.Repository
{
    public interface IConfigurationRepository : IBaseRepository<Configuration>
    {
        Task<int> ObtenerDuracionPrestamoDiasAsync();
        Task<bool> ActualizarDuracionPrestamoDiasAsync(int dias);
    }
}
