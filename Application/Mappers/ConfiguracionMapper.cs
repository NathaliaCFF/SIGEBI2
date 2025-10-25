using SIGEBI.Application.DTOs;
using SIGEBI.Domain.Entities;

namespace SIGEBI.Application.Mappers
{
    public static class ConfiguracionMapper
    {
        public static ConfiguracionDTO ToDTO(this Configuration entity)
        {
            return new ConfiguracionDTO
            {
                Id = entity.Id,
                DuracionPrestamoDias = entity.DuracionPrestamoDias
            };
        }

        public static Configuration ToEntity(this ConfiguracionDTO dto)
        {
            return new Configuration
            {
                Id = dto.Id,
                DuracionPrestamoDias = dto.DuracionPrestamoDias
            };
        }
    }
}
