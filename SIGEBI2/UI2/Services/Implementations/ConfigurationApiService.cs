using UI2.Models.Common;
using UI2.Models.Configuration;
using UI2.Services.Interfaces;

namespace UI2.Services.Implementations
{
    public class ConfigurationApiService : IConfigurationApiService
    {
        private readonly ApiClient _apiClient;

        public ConfigurationApiService(ApiClient apiClient)
        {
            _apiClient = apiClient;
        }

        public async Task<AdapterResult<ConfigurationItemModel>> ObtenerConfiguracionAsync()
        {
            var response = await _apiClient.SendAsync<ConfigurationItemModel>(
                HttpMethod.Get,
                "/api/configuracion"
            );

            if (!response.Success || response.Data == null)
                return AdapterResult<ConfigurationItemModel>.Fail(response.Message ?? "No se pudo obtener la configuración.");

            return AdapterResult<ConfigurationItemModel>.Ok(response.Data, "Configuración cargada.");
        }

        public async Task<AdapterResult<bool>> ActualizarDiasAsync(int dias)
        {
            var response = await _apiClient.SendAsync<bool>(
                HttpMethod.Put,
                $"/api/configuracion/duracion/{dias}"
            );

            return response.Success
                ? AdapterResult<bool>.Ok(true, "Configuración actualizada correctamente.")
                : AdapterResult<bool>.Fail(response.Message ?? "No se pudo actualizar.");
        }
    }
}
