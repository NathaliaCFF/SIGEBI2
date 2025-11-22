using SIGEBI.Application.DTOs;
using SIGEBI.Domain.Entities;

namespace SIGEBI.Application.Mappers
{
    public static class PrestamoMapper
    {
        public static PrestamoDTO ToDTO(this Prestamo entity)
        {
            return new PrestamoDTO
            {
                Id = entity.Id,
                UsuarioId = entity.UsuarioId,
                NombreUsuario = entity.Usuario?.Nombre ?? string.Empty,
                FechaPrestamo = entity.FechaPrestamo,
                FechaVencimiento = entity.FechaVencimiento,
                Activo = entity.Activo,


                Detalles = entity.Detalles != null
                    ? entity.Detalles.ToDtoList()
                    : new List<DetallePrestamoDTO>()
            };
        }
        public static Prestamo ToEntity(this PrestamoDTO dto)
        {
            return new Prestamo
            {
                Id = dto.Id,
                UsuarioId = dto.UsuarioId,
                FechaPrestamo = dto.FechaPrestamo,
                FechaVencimiento = dto.FechaVencimiento,
                Activo = dto.Activo,


                Detalles = dto.Detalles != null
                    ? dto.Detalles.Select(d => d.ToEntity()).ToList()
                    : new List<DetallePrestamo>()
            };
        }

        public static List<PrestamoDTO> ToDtoList(this IEnumerable<Prestamo> entities)
        {
            return entities.Select(e => e.ToDTO()).ToList();
        }
    }
}
