using Microsoft.EntityFrameworkCore;
using Persistence.Base;
using SIGEBI.Domain.Entities;
using SIGEBI.Domain.Repository;
using SIGEBI.Persistence.Context;
using SIGEBI.Shared.Base;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SIGEBI.Persistence.Repositories
{
    public class LibroRepository : BaseRepository<Libro>, ILibroRepository
    {
        public LibroRepository(AppDbContext context) : base(context) { }

        public async Task<IEnumerable<Libro>> BuscarPorTituloOAutorAsync(string criterio)
        {
            return await _context.Libros
                .Where(l => l.Titulo.Contains(criterio) || l.Autor.Contains(criterio))
                .ToListAsync();
        }

        public async Task<bool> ExisteISBNAsync(string isbn)
        {
            return await _context.Libros.AnyAsync(l => l.ISBN == isbn);
        }

        public async Task<bool> EstaDisponibleAsync(int libroId)
        {
            var libro = await _context.Libros.FindAsync(libroId);
            return libro?.Disponible ?? false;
        }
    }
}
