using SIGEBI.Application.DTOs;
using SIGEBI.Domain.Entities;

namespace SIGEBI.Application.Mappers
{
    public static class LibroMapper
    {
        public static LibroDTO ToDTO(this Libro entity)
        {
            return new LibroDTO
            {
                Id = entity.Id,
                Titulo = entity.Titulo,
                Autor = entity.Autor,
                ISBN = entity.ISBN,
                Editorial = entity.Editorial,
                AnioPublicacion = entity.AnioPublicacion,
                Categoria = entity.Categoria,
                Disponible = entity.Disponible,
                Activo = entity.Activo
            };
        }

        public static Libro ToEntity(this LibroDTO dto)
        {
            return new Libro
            {
                Id = dto.Id,
                Titulo = dto.Titulo,
                Autor = dto.Autor,
                ISBN = dto.ISBN,
                Editorial = dto.Editorial,
                AnioPublicacion = dto.AnioPublicacion,
                Categoria = dto.Categoria,
                Disponible = dto.Disponible,
                Activo = dto.Activo
            };
        }
    }
}

