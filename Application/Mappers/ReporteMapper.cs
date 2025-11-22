using SIGEBI.Application.DTOs;
using SIGEBI.Domain.Entities;

namespace SIGEBI.Application.Mappers
{
    public static class ReporteMapper
    {
        public static ReporteDTO ToDTO(this Reporte entity)
        {
            return new ReporteDTO
            {
                LibroId = entity.LibroId,
                Titulo = entity.Titulo ?? string.Empty,
                CantidadPrestamos = entity.CantidadPrestamos
            };
        }

        public static List<ReporteDTO> ToDtoList(this IEnumerable<Reporte> entities)
        {
            return entities.Select(e => e.ToDTO()).ToList();
        }
    }
}
