using Microsoft.EntityFrameworkCore;
using SIGEBI.Domain.Entities;
using SIGEBI.Domain.Repository;
using SIGEBI.Persistence.Base;
using SIGEBI.Persistence.Context;

namespace SIGEBI.Persistence.Repositories
{
    // ============================================================================
    // REPOSITORIO: ConfigurationRepository
    // MÓDULO: Administración / Configuración del Sistema
    // DESCRIPCIÓN: Implementa las operaciones de acceso a datos relacionadas con
    // la configuración general del sistema SIGEBI, incluyendo la duración estándar
    // de los préstamos en días.
    // CASOS DE USO RELACIONADOS:
    //   - CU-14: Configurar duración estándar de préstamos
    // CAPA: Persistencia
    // ============================================================================
    public class ConfigurationRepository : BaseRepository<Configuration>, IConfigurationRepository
    {
        public ConfigurationRepository(AppDbContext context) : base(context) { }

        // ============================================================================
        // CASO DE USO: CU-14 - Obtener duración estándar de préstamos
        // DESCRIPCIÓN: Recupera desde la tabla de configuraciones el valor actual
        // definido como duración predeterminada de los préstamos (en días). Si no
        // existe un registro configurado, retorna el valor por defecto (7 días).
        // ============================================================================
        public async Task<int> ObtenerDuracionPrestamoDiasAsync()
        {
            var config = await _context.Configuraciones.FirstOrDefaultAsync();
            return config?.DuracionPrestamoDias ?? 7;
        }

        // ============================================================================
        // CASO DE USO: CU-14 - Actualizar duración estándar de préstamos
        // DESCRIPCIÓN: Permite modificar la cantidad de días establecida como
        // duración predeterminada de los préstamos. La operación requiere que
        // exista un registro previo de configuración en la base de datos.
        // ============================================================================
        public async Task<bool> ActualizarDuracionPrestamoDiasAsync(int dias)
        {
            var config = await _context.Configuraciones.FirstOrDefaultAsync();
            if (config == null)
                return false;

            // CU-14: Actualizar valor y guardar cambios
            config.DuracionPrestamoDias = dias;
            await _context.SaveChangesAsync();
            return true;
        }
    }
}

