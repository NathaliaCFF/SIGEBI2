using Microsoft.EntityFrameworkCore;
using Persistence.Base;
using SIGEBI.Domain.Entities;
using SIGEBI.Domain.Repository;
using SIGEBI.Persistence.Context;
using SIGEBI.Shared.Base;
using System.Threading.Tasks;

namespace SIGEBI.Persistence.Repositories
{
    public class ConfigurationRepository : BaseRepository<Configuration>, IConfigurationRepository
    {
        public ConfigurationRepository(AppDbContext context) : base(context) { }

        public async Task<int> ObtenerDuracionPrestamoDiasAsync()
        {
            var config = await _context.Configuraciones.FirstOrDefaultAsync();
            return config?.DuracionPrestamoDias ?? 7;
        }

        public async Task<bool> ActualizarDuracionPrestamoDiasAsync(int dias)
        {
            var config = await _context.Configuraciones.FirstOrDefaultAsync();
            if (config == null) return false;

            config.DuracionPrestamoDias = dias;
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
