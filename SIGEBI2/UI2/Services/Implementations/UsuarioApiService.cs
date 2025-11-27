using Shared;
using SIGEBI.Domain.Entities;
using UI2.Models.Common;
using UI2.Models.Usuarios;

namespace UI2.Services.Implementations
{
    public class UsuarioApiService : Services.Interfaces.IUsuarioApiService
    {
        private const string BaseEndpoint = "/api/usuario";
        private readonly ApiClient _apiClient;

        public UsuarioApiService(ApiClient apiClient)
        {
            _apiClient = apiClient;
        }

        public async Task<AdapterResult<IList<UsuarioListItemModel>>> ObtenerUsuariosAsync()
        {
            var response = await _apiClient.SendAsync<OperationResult<List<Usuario>>>(
                HttpMethod.Get,
                BaseEndpoint
            );

            if (!response.Success || response.Data == null)
            {
                return AdapterResult<IList<UsuarioListItemModel>>.Fail(
                    response.Message ?? "No se pudieron cargar los usuarios."
                );
            }

            var resultado = response.Data;
            if (!resultado.Success || resultado.Data == null)
            {
                return AdapterResult<IList<UsuarioListItemModel>>.Fail(
                    resultado.Message ?? "No se pudieron cargar los usuarios."
                );
            }

            var lista = resultado.Data
                .Select(Mapear)
                .ToList();

            return AdapterResult<IList<UsuarioListItemModel>>.Ok(
                lista,
                resultado.Message ?? "Usuarios cargados correctamente."
            );
        }

        public async Task<AdapterResult<UsuarioListItemModel>> CrearUsuarioAsync(UsuarioFormModel model)
        {
            var entidad = new Usuario
            {
                Nombre = model.Nombre,
                Email = model.Email,
                Contraseña = model.Password,
                Rol = model.Rol,
                Activo = model.Activo
            };

            var response = await _apiClient.SendAsync<OperationResult<Usuario>>(
                HttpMethod.Post,
                BaseEndpoint,
                entidad
            );

            return ProcesarRespuestaUsuario(
                response,
                "No se pudo registrar el usuario.",
                "Usuario registrado correctamente."
            );
        }

        public async Task<AdapterResult<UsuarioListItemModel>> ActualizarUsuarioAsync(UsuarioFormModel model)
        {
            var entidad = new Usuario
            {
                Id = model.Id,
                Nombre = model.Nombre,
                Email = model.Email,
                Rol = model.Rol,
                Activo = model.Activo
            };

            var response = await _apiClient.SendAsync<OperationResult<Usuario>>(
                HttpMethod.Put,
                BaseEndpoint,
                entidad
            );

            return ProcesarRespuestaUsuario(
                response,
                "No se pudo actualizar el usuario.",
                "Usuario actualizado correctamente."
            );
        }

        public async Task<AdapterResult> ActivarUsuarioAsync(int id)
        {
            var response = await _apiClient.SendAsync<OperationResult>(
                HttpMethod.Patch,
                $"{BaseEndpoint}/{id}/activar",
                body: null
            );

            return ProcesarOperacionSimple(
                response,
                "No se pudo activar el usuario.",
                "Usuario activado correctamente."
            );
        }

        public async Task<AdapterResult> DesactivarUsuarioAsync(int id)
        {
            var response = await _apiClient.SendAsync<OperationResult>(
                HttpMethod.Patch,
                $"{BaseEndpoint}/{id}/desactivar",
                body: null
            );

            return ProcesarOperacionSimple(
                response,
                "No se pudo desactivar el usuario.",
                "Usuario desactivado correctamente."
            );
        }

        private static AdapterResult<UsuarioListItemModel> ProcesarRespuestaUsuario(
            ApiResponse<OperationResult<Usuario>> response,
            string mensajeError,
            string mensajeExito
        )
        {
            if (!response.Success || response.Data == null)
            {
                return AdapterResult<UsuarioListItemModel>.Fail(
                    response.Message ?? mensajeError
                );
            }

            var resultado = response.Data;
            if (!resultado.Success || resultado.Data == null)
            {
                return AdapterResult<UsuarioListItemModel>.Fail(
                    resultado.Message ?? mensajeError
                );
            }

            return AdapterResult<UsuarioListItemModel>.Ok(
                Mapear(resultado.Data),
                resultado.Message ?? mensajeExito
            );
        }

        private static AdapterResult ProcesarOperacionSimple(
            ApiResponse<OperationResult> response,
            string mensajeError,
            string mensajeExito
        )
        {
            if (!response.Success || response.Data == null)
            {
                return AdapterResult.Fail(response.Message ?? mensajeError);
            }

            var resultado = response.Data;
            return resultado.Success
                ? AdapterResult.Ok(resultado.Message ?? mensajeExito)
                : AdapterResult.Fail(resultado.Message ?? mensajeError);
        }

        private static UsuarioListItemModel Mapear(Usuario usuario)
        {
            return new UsuarioListItemModel
            {
                Id = usuario.Id,
                Nombre = usuario.Nombre,
                Email = usuario.Email,
                Rol = usuario.Rol,
                Activo = usuario.Activo,
                FechaCreacion = usuario.FechaCreacion
            };
        }
    }
}
