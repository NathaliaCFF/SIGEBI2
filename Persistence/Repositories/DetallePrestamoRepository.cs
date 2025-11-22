using Microsoft.EntityFrameworkCore;
using SIGEBI.Domain.Entities;
using SIGEBI.Domain.Repository;
using SIGEBI.Persistence.Base;
using SIGEBI.Persistence.Context;

namespace SIGEBI.Persistence.Repositories
{
    // ============================================================================
    // REPOSITORIO: DetallePrestamoRepository
    // MÓDULO: Préstamos y Devoluciones
    // DESCRIPCIÓN: Gestiona los registros de detalle asociados a cada préstamo,
    // incluyendo la consulta y actualización del estado de devolución de los libros.
    // CASOS DE USO RELACIONADOS:
    //   - CU-09: Registrar préstamo
    //   - CU-10: Registrar devolución
    // CAPA: Persistencia
    // ============================================================================
    public class DetallePrestamoRepository : BaseRepository<DetallePrestamo>, IDetallePrestamoRepository
    {
        public DetallePrestamoRepository(AppDbContext context) : base(context) { }

        // ============================================================================
        // CASO DE USO: CU-09 - Registrar préstamo
        // DESCRIPCIÓN: Permite obtener los detalles (libros asociados) de un préstamo
        // específico, utilizados para mostrar y validar los datos en la interfaz.
        // ============================================================================
        public async Task<IEnumerable<DetallePrestamo>> ObtenerPorPrestamoAsync(int prestamoId)
        {
            return await _context.DetallePrestamos
                .Include(dp => dp.Libro)
                .Where(dp => dp.PrestamoId == prestamoId)
                .ToListAsync();
        }

        // ============================================================================
        // CASO DE USO: CU-10 - Registrar devolución
        // DESCRIPCIÓN: Marca un libro como devuelto dentro de un préstamo,
        // actualizando la fecha de devolución y su estado.
        // ============================================================================
        public async Task RegistrarDevolucionAsync(int detalleId)
        {
            var detalle = await _context.DetallePrestamos.FindAsync(detalleId);
            if (detalle != null)
            {
                detalle.Devuelto = true;
                detalle.FechaDevolucion = DateTime.Now;

                // CU-10: Confirmar actualización de estado de devolución
                await _context.SaveChangesAsync();
            }
        }
    }
}
