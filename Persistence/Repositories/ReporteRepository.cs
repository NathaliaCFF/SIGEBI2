using Microsoft.EntityFrameworkCore;
using SIGEBI.Domain.Entities;
using SIGEBI.Domain.Repository;
using SIGEBI.Persistence.Context;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SIGEBI.Persistence.Repositories
{
    public class ReporteRepository : IReporteRepository
    {
        private readonly AppDbContext _context;

        public ReporteRepository(AppDbContext context)
        {
            _context = context;
        }

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
                .OrderByDescending(r => r.CantidadPrestamos)
                .ToListAsync();
        }
    }
}

