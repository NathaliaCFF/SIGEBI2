using SIGEBI.Shared.Base;
using SIGEBI.Domain.Entities;
using System.Threading.Tasks;

namespace SIGEBI.Application.Interfaces
{
    public interface IConfiguracionService
    {
        Task<ServiceResult<Configuration>> ObtenerConfiguracionAsync();
        Task<ServiceResult> ActualizarDuracionPrestamoDiasAsync(int dias);
    }
}
