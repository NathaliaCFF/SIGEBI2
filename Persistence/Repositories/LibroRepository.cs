using Microsoft.EntityFrameworkCore;
using SIGEBI.Persistence.Base;
using SIGEBI.Domain.Entities;
using SIGEBI.Domain.Repository;
using SIGEBI.Persistence.Context;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System;

namespace SIGEBI.Persistence.Repositories
{
    // ============================================================================
    // REPOSITORIO: LibroRepository
    // MÓDULO: Gestión de Libros
    // DESCRIPCIÓN: Implementa las operaciones de acceso a datos para la entidad
    // Libro, permitiendo registrar, buscar y consultar disponibilidad.
    // CASOS DE USO RELACIONADOS:
    //   - CU-01: Registrar libro
    //   - CU-03: Buscar libro
    //   - CU-04: Consultar estado de libro
    // CAPA: Persistencia
    // ============================================================================
    public class LibroRepository : BaseRepository<Libro>, ILibroRepository
    {
        public LibroRepository(AppDbContext context) : base(context) { }

        // ============================================================================
        // CASO DE USO: CU-03 - Buscar libro
        // DESCRIPCIÓN: Permite a los usuarios (docente/estudiante) buscar libros
        // activos por título o autor utilizando coincidencias parciales.
        // ============================================================================
        public async Task<IEnumerable<Libro>> BuscarPorTituloOAutorAsync(string criterio)
        {
            criterio ??= string.Empty;

            // CU-03: Filtro de coincidencia parcial por título o autor
            return await _context.Libros
                .Where(l => l.Activo && (
                       EF.Functions.Like(l.Titulo, $"%{criterio}%") ||
                       EF.Functions.Like(l.Autor, $"%{criterio}%")))
                .ToListAsync();
        }

        // ============================================================================
        // CASO DE USO: CU-01 - Registrar libro
        // DESCRIPCIÓN: Valida si ya existe un libro activo con el mismo ISBN
        // antes de registrar un nuevo ejemplar en la base de datos.
        // ============================================================================
        public async Task<bool> ExisteISBNAsync(string isbn)
        {
            if (string.IsNullOrWhiteSpace(isbn))
                return false;

            // CU-01: Verificación de duplicidad de ISBN
            return await _context.Libros.AnyAsync(l => l.Activo && l.ISBN == isbn);
        }

        // ============================================================================
        // CASO DE USO: CU-04 - Consultar estado de libro
        // DESCRIPCIÓN: Retorna el estado de disponibilidad de un libro específico.
        // ============================================================================
        public async Task<bool> EstaDisponibleAsync(int libroId)
        {
            var libro = await _context.Libros
                .Where(l => l.Activo && l.Id == libroId)
                .Select(l => new { l.Disponible })
                .FirstOrDefaultAsync();

            // CU-04: Retornar disponibilidad actual (true = disponible)
            return libro?.Disponible ?? false;
        }

        // ============================================================================
        // CASO DE USO: CU-04 - Actualizar disponibilidad de libro
        // DESCRIPCIÓN: Permite cambiar el estado de disponibilidad de un libro,
        // utilizado por otros casos de uso como préstamos y devoluciones.
        // ============================================================================
        public async Task<bool> CambiarDisponibilidadAsync(int id, bool disponible)
        {
            var libro = await _context.Libros.FindAsync(id);
            if (libro is null || !libro.Activo)
                return false;

            // CU-04: Evitar actualización innecesaria si ya está en el estado deseado
            if (libro.Disponible == disponible)
                return true;

            libro.Disponible = disponible;
            libro.FechaModificacion = DateTime.UtcNow;

            _context.Libros.Update(libro);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
