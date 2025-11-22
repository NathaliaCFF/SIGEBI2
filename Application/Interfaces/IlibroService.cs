using SIGEBI.Domain.Entities;
using SIGEBI.Shared.Base;

namespace SIGEBI.Application.Interfaces
{
    public interface ILibroService
    {
        Task<ServiceResult<Libro>> CrearAsync(Libro libro);

        Task<ServiceResult<Libro>> ActualizarAsync(int id, Libro libro);

        Task<ServiceResult<IEnumerable<Libro>>> BuscarAsync(string criterio);

        Task<ServiceResult<bool>> EstaDisponibleAsync(int id);

        Task<ServiceResult> CambiarDisponibilidadAsync(int id, bool disponible);

        Task<ServiceResult> DesactivarAsync(int id);

        Task<ServiceResult<Libro>> ObtenerPorIdAsync(int id);
        Task<ServiceResult<IEnumerable<Libro>>> ListarAsync();
    }
}
