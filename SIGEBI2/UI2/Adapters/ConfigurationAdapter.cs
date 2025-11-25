using System.Net.Http;
using UI2.Models.Configuration;
using UI2.Services;
using UI2.Models.Common;

namespace UI2.Adapters
{
    public class ConfigurationAdapter
    {
        private readonly ApiClient _apiClient;

        public ConfigurationAdapter(ApiClient apiClient)
        {
            _apiClient = apiClient;
        }

        public async Task<AdapterResult<List<ConfigurationItemModel>>> ObtenerListadoAsync()
        {
            var response = await _apiClient.SendAsync<List<ConfigurationItemModel>>(
                HttpMethod.Get,
                "/api/config/listado"
            );

            if (!response.Success || response.Data == null)
                return AdapterResult<List<ConfigurationItemModel>>.Fail("No se pudo obtener la configuración.");

            return AdapterResult<List<ConfigurationItemModel>>.Ok(response.Data, "Configuración cargada.");
        }

        public async Task<AdapterResult<bool>> ActualizarAsync(UpdateConfigurationModel model)
        {
            var response = await _apiClient.SendAsync<bool>(
                HttpMethod.Put,
                "/api/config/actualizar",
                model
            );

            if (!response.Success)
                return AdapterResult<bool>.Fail("No se pudo actualizar.");

            return AdapterResult<bool>.Ok(true, "Configuración actualizada correctamente.");
        }
    }
}
