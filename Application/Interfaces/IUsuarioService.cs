using Application.DTOs;
using Shared;
using SIGEBI.Domain.Entities;

namespace SIGEBI.Application.Interfaces
{
    public interface IUsuarioService
    {
        // LOGIN (devuelve AuthResponseDTO con token)
        Task<OperationResult<AuthResponseDTO>> AutenticarAsync(string email, string password);

        // CRUD con validación de rol
        Task<OperationResult<Usuario>> CrearAsync(Usuario usuario, Usuario usuarioActual);
        Task<OperationResult<Usuario>> ActualizarAsync(Usuario usuario, Usuario usuarioActual);
        Task<OperationResult<IEnumerable<Usuario>>> ObtenerTodosAsync(Usuario usuarioActual);
        Task<OperationResult> EliminarAsync(int id, Usuario usuarioActual);

        // Cambiar estado
        Task<OperationResult> ActivarAsync(int id, Usuario usuarioActual);
        Task<OperationResult> DesactivarAsync(int id, Usuario usuarioActual);
    }
}
