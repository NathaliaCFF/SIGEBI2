using Microsoft.EntityFrameworkCore;
using SIGEBI.Domain.Entities;
using SIGEBI.Domain.Repository;
using SIGEBI.Persistence.Context;

namespace SIGEBI.Persistence.Repositories
{
    // ============================================================================
    // REPOSITORIO: ReporteRepository
    // MÓDULO: Reportes
    // DESCRIPCIÓN: Implementa la lógica de acceso a datos necesaria para generar
    // reportes estadísticos del sistema, como la frecuencia de préstamos de libros.
    // CASOS DE USO RELACIONADOS:
    //   - CU-13: Generar reporte de libros más prestados
    // CAPA: Persistencia
    // ============================================================================
    public class ReporteRepository : IReporteRepository
    {
        private readonly AppDbContext _context;

        public ReporteRepository(AppDbContext context)
        {
            _context = context;
        }

        // ============================================================================
        // CASO DE USO: CU-13 - Generar reporte de libros más prestados
        // DESCRIPCIÓN: Calcula los libros con mayor cantidad de préstamos a partir de
        // los registros en la tabla DetallePrestamo. Devuelve una lista ordenada en
        // orden descendente según la cantidad de préstamos realizados.
        // ============================================================================
        public async Task<IEnumerable<Reporte>> LibrosMasPrestadosAsync()
        {
            return await _context.DetallePrestamos
                .GroupBy(dp => dp.Libro)
                .Select(g => new Reporte
                {
                    LibroId = g.Key.Id,
                    Titulo = g.Key.Titulo,
                    CantidadPrestamos = g.Count()
                })
                // CU-13: Ordenar por número de préstamos (mayor a menor)
                .OrderByDescending(r => r.CantidadPrestamos)
                .ToListAsync();
        }
    }
}

