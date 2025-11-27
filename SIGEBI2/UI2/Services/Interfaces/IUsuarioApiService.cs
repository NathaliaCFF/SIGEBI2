using UI2.Models.Common;
using UI2.Models.Usuarios;

namespace UI2.Services.Interfaces
{
    public interface IUsuarioApiService
    {
        Task<AdapterResult<IList<UsuarioListItemModel>>> ObtenerUsuariosAsync();
        Task<AdapterResult<UsuarioListItemModel>> CrearUsuarioAsync(UsuarioFormModel model);
        Task<AdapterResult<UsuarioListItemModel>> ActualizarUsuarioAsync(UsuarioFormModel model);
        Task<AdapterResult> ActivarUsuarioAsync(int id);
        Task<AdapterResult> DesactivarUsuarioAsync(int id);
    }
}

