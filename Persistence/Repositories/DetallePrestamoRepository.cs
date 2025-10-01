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
    public class DetallePrestamoRepository : BaseRepository<DetallePrestamo>, IDetallePrestamoRepository
    {
        public DetallePrestamoRepository(AppDbContext context) : base(context) { }

        public async Task<IEnumerable<DetallePrestamo>> ObtenerPorPrestamoAsync(int prestamoId)
        {
            return await _context.DetallePrestamos
                .Include(dp => dp.Libro)
                .Where(dp => dp.PrestamoId == prestamoId)
                .ToListAsync();
        }

        public async Task RegistrarDevolucionAsync(int detalleId)
        {
            var detalle = await _context.DetallePrestamos.FindAsync(detalleId);
            if (detalle != null)
            {
                detalle.Devuelto = true;
                detalle.FechaDevolucion = DateTime.Now;
                await _context.SaveChangesAsync();
            }
        }
    }
}
