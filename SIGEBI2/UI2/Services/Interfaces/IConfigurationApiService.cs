using UI2.Models.Common;
using UI2.Models.Configuration;

namespace UI2.Services.Interfaces
{
    public interface IConfigurationApiService
    {
        Task<AdapterResult<ConfigurationItemModel>> ObtenerConfiguracionAsync();
        Task<AdapterResult<bool>> ActualizarDiasAsync(int dias);
    }
}
