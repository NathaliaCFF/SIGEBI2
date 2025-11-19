using System.Net.Http.Json;
using UI2.Models.Common;
using UI2.Models.Prestamos;

namespace UI2.Adapters
{
    public class PrestamoAdapter
    {
        private readonly HttpClient _http;

        public PrestamoAdapter(HttpClient http)
        {
            _http = http;
        }

        public async Task<AdapterResult<IList<PrestamoListItemModel>>> ObtenerPrestamosActivosAsync(int usuarioId)
        {
            var response = await _http.GetAsync($"prestamos/activos/{usuarioId}");

            if (!response.IsSuccessStatusCode)
                return AdapterResult<IList<PrestamoListItemModel>>.Fail(
                    "No se encontraron préstamos activos."
                );

            var data = await response.Content.ReadFromJsonAsync<IList<PrestamoListItemModel>>();

            return AdapterResult<IList<PrestamoListItemModel>>.Ok(
                data!,
                "Préstamos cargados correctamente."
            );
        }

        public async Task<AdapterResult<IList<PrestamoListItemModel>>> ObtenerPrestamosVencidosAsync()
        {
            var response = await _http.GetAsync("prestamos/vencidos");

            if (!response.IsSuccessStatusCode)
                return AdapterResult<IList<PrestamoListItemModel>>.Fail(
                    "No hay préstamos vencidos registrados."
                );

            var data = await response.Content.ReadFromJsonAsync<IList<PrestamoListItemModel>>();

            return AdapterResult<IList<PrestamoListItemModel>>.Ok(
                data!,
                "Préstamos vencidos cargados correctamente."
            );
        }
    }
}
