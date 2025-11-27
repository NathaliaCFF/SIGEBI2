using UI2.Models.Common;
using UI2.Models.Libros;
using SIGEBI.Domain.Entities;

namespace UI2.Services.Interfaces
{
    public interface ILibroApiService
    {
        Task<AdapterResult<IList<LibroListItemModel>>> ListarLibrosAsync();
        Task<AdapterResult<bool>> EliminarLibroAsync(int id);
        Task<AdapterResult<IList<LibroListItemModel>>> BuscarLibrosAsync(string criterio);
        Task<AdapterResult<LibroListItemModel>> CrearLibroAsync(Libro libro);
        Task<AdapterResult<LibroListItemModel>> ActualizarLibroAsync(int id, Libro libro);
        Task<AdapterResult<bool>> ActivarLibroAsync(int id);
    }
}
