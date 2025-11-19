using System.Net.Http.Json;
using UI2.Models.Common;
using UI2.Models.Libros;

namespace UI2.Adapters
{
    public class LibroAdapter
    {
        private readonly HttpClient _http;

        public LibroAdapter(HttpClient http)
        {
            _http = http;
        }

        public async Task<AdapterResult<IList<LibroListItemModel>>> ListarLibrosAsync()
        {
            var response = await _http.GetAsync("libros");

            if (!response.IsSuccessStatusCode)
                return AdapterResult<IList<LibroListItemModel>>.Fail("No se pudieron obtener los libros.");

            var data = await response.Content.ReadFromJsonAsync<IList<LibroListItemModel>>();

            return AdapterResult<IList<LibroListItemModel>>.Ok(
                data!,
                "Libros cargados correctamente."
            );
        }

        public async Task<AdapterResult<IList<LibroListItemModel>>> BuscarLibrosAsync(string criterio)
        {
            var response = await _http.GetAsync($"libros/buscar?criterio={criterio}");

            if (!response.IsSuccessStatusCode)
                return AdapterResult<IList<LibroListItemModel>>.Fail("No se encontraron libros.");

            var data = await response.Content.ReadFromJsonAsync<IList<LibroListItemModel>>();

            return AdapterResult<IList<LibroListItemModel>>.Ok(
                data!,
                "Búsqueda completada."
            );
        }

        public async Task<AdapterResult<LibroListItemModel>> CrearLibroAsync(LibroCreateModel libro)
        {
            var response = await _http.PostAsJsonAsync("libros", libro);

            if (!response.IsSuccessStatusCode)
                return AdapterResult<LibroListItemModel>.Fail("No se pudo registrar el libro.");

            var data = await response.Content.ReadFromJsonAsync<LibroListItemModel>();

            return AdapterResult<LibroListItemModel>.Ok(
                data!,
                "Libro registrado correctamente."
            );
        }

        public async Task<AdapterResult<LibroListItemModel>> ActualizarLibroAsync(LibroUpdateModel libro)
        {
            var response = await _http.PutAsJsonAsync($"libros/{libro.Id}", libro);

            if (!response.IsSuccessStatusCode)
                return AdapterResult<LibroListItemModel>.Fail("No se pudo actualizar el libro.");

            var data = await response.Content.ReadFromJsonAsync<LibroListItemModel>();

            return AdapterResult<LibroListItemModel>.Ok(
                data!,
                "Libro actualizado correctamente."
            );
        }

    }
}
