using SIGEBI.Application.Interfaces;
using SIGEBI.Domain.Entities;
using SIGEBI.Domain.Repository;
using SIGEBI.Shared.Base;
using System.Threading.Tasks;
using System.Linq;

namespace SIGEBI.Application.Services
{
    // ============================================================================
    // SERVICIO: ConfiguracionService
    // MÓDULO: Administración / Configuración del Sistema
    // DESCRIPCIÓN: Contiene la lógica de aplicación encargada de administrar los
    // parámetros generales del sistema, incluyendo la duración estándar de los
    // préstamos en días.
    // CASOS DE USO RELACIONADOS:
    //   - CU-14: Configurar duración estándar de préstamos
    // CAPA: Aplicación
    // ============================================================================
    public class ConfiguracionService : IConfiguracionService
    {
        private readonly IConfigurationRepository _repository;

        public ConfiguracionService(IConfigurationRepository repository)
        {
            _repository = repository;
        }

        // ============================================================================
        // CASO DE USO: CU-14 - Obtener configuración del sistema
        // DESCRIPCIÓN: Recupera los parámetros generales almacenados en la tabla de
        // configuración, como la duración estándar de los préstamos y otros ajustes
        // administrativos del sistema.
        // ============================================================================
        public async Task<ServiceResult<Configuration>> ObtenerConfiguracionAsync()
        {
            var all = await _repository.GetAllAsync();

            if (!all.Success || all.Data == null || !all.Data.Any())
                return ServiceResult<Configuration>.Fail("No se encontró una configuración registrada.");

            var config = all.Data.First();
            return ServiceResult<Configuration>.Ok(config, "Configuración obtenida correctamente.");
        }

        // ============================================================================
        // CASO DE USO: CU-14 - Actualizar duración estándar de préstamos
        // DESCRIPCIÓN: Permite modificar la cantidad de días establecida como
        // duración predeterminada de los préstamos, garantizando que el valor sea
        // positivo antes de guardar los cambios en la base de datos.
        // ============================================================================
        public async Task<ServiceResult> ActualizarDuracionPrestamoDiasAsync(int dias)
        {
            if (dias <= 0)
                return ServiceResult.Fail("La duración de préstamo debe ser mayor que 0 días.");

            // CU-14: Actualizar duración en la configuración general del sistema
            var actualizado = await _repository.ActualizarDuracionPrestamoDiasAsync(dias);
            return actualizado
                ? ServiceResult.Ok("Duración de préstamo actualizada correctamente.")
                : ServiceResult.Fail("No se pudo actualizar la configuración.");
        }
    }
}

