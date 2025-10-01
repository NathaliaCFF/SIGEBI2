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
    public class PrestamoRepository : BaseRepository<Prestamo>, IPrestamoRepository
    {
        public PrestamoRepository(AppDbContext context) : base(context) { }

        public async Task<IEnumerable<Prestamo>> ObtenerPorUsuarioAsync(int usuarioId)
        {
            return await _context.Prestamos
                .Include(p => p.Detalles)
                .Where(p => p.UsuarioId == usuarioId && p.Activo)
                .ToListAsync();
        }

        public async Task<IEnumerable<Prestamo>> ObtenerPrestamosVencidosAsync()
        {
            return await _context.Prestamos
                .Where(p => p.Activo && p.FechaVencimiento < DateTime.Now)
                .ToListAsync();
        }
    }
}
