using SIGEBI.Application.DTOs;
using SIGEBI.Domain.Entities;

namespace SIGEBI.Application.Mappers
{
    public static class DetallePrestamoMapper
    {
        // Convierte de entidad a DTO
        public static DetallePrestamoDTO ToDTO(this DetallePrestamo entity)
        {
            return new DetallePrestamoDTO
            {
                Id = entity.Id,
                PrestamoId = entity.PrestamoId,
                LibroId = entity.LibroId,
                TituloLibro = entity.Libro?.Titulo ?? string.Empty,
                FechaDevolucion = entity.FechaDevolucion,
                Devuelto = entity.Devuelto
            };
        }

        // Convierte de DTO a entidad 
        public static DetallePrestamo ToEntity(this DetallePrestamoDTO dto)
        {
            return new DetallePrestamo
            {
                Id = dto.Id,
                PrestamoId = dto.PrestamoId,
                LibroId = dto.LibroId,
                FechaDevolucion = dto.FechaDevolucion,
                Devuelto = dto.Devuelto
            };
        }

        // Convierte una colección completa de entidades a DTOs
        public static List<DetallePrestamoDTO> ToDtoList(this IEnumerable<DetallePrestamo> entities)
        {
            return entities.Select(e => e.ToDTO()).ToList();
        }
    }
}
