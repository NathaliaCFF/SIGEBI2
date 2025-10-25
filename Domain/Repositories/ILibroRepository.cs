using SIGEBI.Domain.Entities;
using SIGEBI.Shared.Base;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SIGEBI.Domain.Repository
{
    public interface ILibroRepository : IBaseRepository<Libro>
    {
        Task<IEnumerable<Libro>> BuscarPorTituloOAutorAsync(string criterio);
        Task<bool> ExisteISBNAsync(string isbn);
        Task<bool> EstaDisponibleAsync(int libroId);
        Task<bool> CambiarDisponibilidadAsync(int id, bool disponible);
    }
}
