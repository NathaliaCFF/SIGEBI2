using Microsoft.EntityFrameworkCore;
using SIGEBI.Domain.Entities;
using SIGEBI.Domain.Repository;
using SIGEBI.Persistence.Base;
using SIGEBI.Persistence.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SIGEBI.Persistence.Repositories
{
    // ============================================================================
    // REPOSITORIO: PrestamoRepository
    // MÓDULO: Préstamos y Devoluciones
    // DESCRIPCIÓN: Implementa las operaciones de acceso a datos para la entidad
    // Préstamo, incluyendo la consulta de préstamos activos, vencidos y asociados
    // a usuarios específicos.
    // CASOS DE USO RELACIONADOS:
    //   - CU-09: Registrar préstamo
    //   - CU-10: Registrar devolución
    //   - CU-11: Consultar préstamos activos por usuario
    //   - CU-12: Identificar préstamos vencidos
    // CAPA: Persistencia
    // ============================================================================
    public class PrestamoRepository : BaseRepository<Prestamo>, IPrestamoRepository
    {
        public PrestamoRepository(AppDbContext context) : base(context) { }

        // ============================================================================
        // CASO DE USO: CU-11 - Consultar préstamos activos por usuario
        // DESCRIPCIÓN: Devuelve los préstamos activos asociados a un usuario
        // específico (docente o estudiante), incluyendo los detalles de cada libro.
        // ============================================================================
        public async Task<IEnumerable<Prestamo>> ObtenerPorUsuarioAsync(int usuarioId)
        {
            return await _context.Prestamos
                .Include(p => p.Usuario)
                .Include(p => p.Detalles)
                    .ThenInclude(d => d.Libro)
                .Where(p => p.UsuarioId == usuarioId && p.Activo)
                .ToListAsync();
        }

        // ============================================================================
        // CASO DE USO: CU-12 - Identificar préstamos vencidos
        // DESCRIPCIÓN: Obtiene todos los préstamos cuyo período de vencimiento ha
        // expirado, incluyendo los datos del usuario y los libros asociados.
        // ============================================================================
        public async Task<IEnumerable<Prestamo>> ObtenerPrestamosVencidosAsync()
        {
            return await _context.Prestamos
                .Include(p => p.Usuario)
                .Include(p => p.Detalles)
                    .ThenInclude(d => d.Libro)
                .Where(p => p.Activo && p.FechaVencimiento < DateTime.Now)
                .ToListAsync();
        }
    }
}
