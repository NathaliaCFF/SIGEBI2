using SIGEBI.Domain.Entities;
using SIGEBI.Shared.Base;

namespace SIGEBI.Application.Interfaces
{
    public interface IConfiguracionService
    {
        Task<ServiceResult<Configuration>> ObtenerConfiguracionAsync();
        Task<ServiceResult> ActualizarDuracionPrestamoDiasAsync(int dias);
    }
}
