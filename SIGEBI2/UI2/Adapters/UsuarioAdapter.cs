using System.Net.Http.Json;
using UI2.Models.Common;
using UI2.Models.Usuarios;

namespace UI2.Adapters
{
    public class UsuarioAdapter
    {
        private readonly HttpClient _http;

        public UsuarioAdapter(HttpClient http)
        {
            _http = http;
        }

        public async Task<AdapterResult<IList<UsuarioListItemModel>>> ObtenerUsuariosAsync(int usuarioActualId)
        {
            var response = await _http.GetAsync($"usuarios?actualId={usuarioActualId}");

            if (!response.IsSuccessStatusCode)
                return AdapterResult<IList<UsuarioListItemModel>>
                    .Fail("No se pudieron cargar los usuarios.");

            var data = await response.Content.ReadFromJsonAsync<IList<UsuarioListItemModel>>();

            return AdapterResult<IList<UsuarioListItemModel>>
                .Ok(data!, "Usuarios cargados correctamente.");
        }

        public async Task<AdapterResult<UsuarioListItemModel>> CrearUsuarioAsync(UsuarioFormModel model)
        {
            var response = await _http.PostAsJsonAsync("usuarios", model);

            if (!response.IsSuccessStatusCode)
                return AdapterResult<UsuarioListItemModel>.Fail("No se pudo registrar el usuario.");

            var data = await response.Content.ReadFromJsonAsync<UsuarioListItemModel>();

            return AdapterResult<UsuarioListItemModel>
                .Ok(data!, "Usuario registrado correctamente.");
        }

        public async Task<AdapterResult<UsuarioListItemModel>> ActualizarUsuarioAsync(UsuarioFormModel model)
        {
            var response = await _http.PutAsJsonAsync($"usuarios/{model.Id}", model);

            if (!response.IsSuccessStatusCode)
                return AdapterResult<UsuarioListItemModel>.Fail("No se pudo actualizar el usuario.");

            var data = await response.Content.ReadFromJsonAsync<UsuarioListItemModel>();

            return AdapterResult<UsuarioListItemModel>
                .Ok(data!, "Usuario actualizado correctamente.");
        }

        public async Task<AdapterResult> ActivarUsuarioAsync(int id)
        {
            var response = await _http.PostAsync($"usuarios/{id}/activar", null);

            return response.IsSuccessStatusCode
                ? AdapterResult.Ok("Usuario activado correctamente.")
                : AdapterResult.Fail("No se pudo activar el usuario.");
        }

        public async Task<AdapterResult> DesactivarUsuarioAsync(int id)
        {
            var response = await _http.PostAsync($"usuarios/{id}/desactivar", null);

            return response.IsSuccessStatusCode
                ? AdapterResult.Ok("Usuario desactivado correctamente.")
                : AdapterResult.Fail("No se pudo desactivar el usuario.");
        }
    }
}
