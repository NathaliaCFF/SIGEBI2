using Application.Interfaces;
using SIGEBI.Domain.Entities;
using SIGEBI.Domain.Repository;
using SIGEBI.Shared.Base;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;

namespace Application.Services
{
    // ============================================================================
    // SERVICIO: ReporteService
    // MÓDULO: Reportes
    // DESCRIPCIÓN: Contiene la lógica de aplicación necesaria para generar los
    // reportes estadísticos del sistema SIGEBI. Coordina la obtención y validación
    // de los datos provenientes del repositorio.
    // CASOS DE USO RELACIONADOS:
    //   - CU-13: Generar reporte de libros más prestados
    // CAPA: Aplicación
    // ============================================================================
    public class ReporteService : IReporteService
    {
        private readonly IReporteRepository _reporteRepository;

        public ReporteService(IReporteRepository reporteRepository)
        {
            _reporteRepository = reporteRepository;
        }

        // ============================================================================
        // CASO DE USO: CU-13 - Generar reporte de libros más prestados
        // DESCRIPCIÓN: Obtiene y procesa el listado de libros ordenados según la
        // cantidad de préstamos registrados. Si no existen datos suficientes, retorna
        // un mensaje indicando que no se puede generar el reporte.
        // ============================================================================
        public async Task<ServiceResult<IEnumerable<Reporte>>> ObtenerLibrosMasPrestadosAsync()
        {
            var data = await _reporteRepository.LibrosMasPrestadosAsync();

            // CU-13: Validar existencia de registros para generar el reporte
            if (data == null || !data.Any())
                return ServiceResult<IEnumerable<Reporte>>.Fail("No hay préstamos registrados para generar el reporte.");

            // CU-13: Reporte generado correctamente
            return ServiceResult<IEnumerable<Reporte>>.Ok(data, "Reporte generado correctamente.");
        }
    }
}
