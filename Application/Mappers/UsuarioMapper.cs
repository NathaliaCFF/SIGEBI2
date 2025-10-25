using Application.DTOs.Usuario;
using SIGEBI.Application.DTOs.Usuario;
using SIGEBI.Domain.Entities;

namespace SIGEBI.Application.Mappers
{
    public static class UsuarioMapper
    {
        public static UsuarioDto ToDto(Usuario entity)
        {
            return new UsuarioDto
            {
                Id = entity.Id,
                Nombre = entity.Nombre,
                Email = entity.Email,
                Rol = entity.Rol,
                Activo = entity.Activo,
                FechaCreacion = entity.FechaCreacion,
                FechaModificacion = entity.FechaModificacion
            };
        }
        public static IEnumerable<UsuarioDto> ToDtoList(IEnumerable<Usuario> entities)
        {
            return entities.Select(ToDto);
        }
        public static Usuario FromCreateDto(CreateUsuarioDto dto)
        {
            return new Usuario
            {
                Nombre = dto.Nombre,
                Email = dto.Email,
                Contraseña = dto.Contraseña,
                Rol = dto.Rol,
                Activo = true,
                FechaCreacion = DateTime.UtcNow
            };
        }
        public static Usuario FromUpdateDto(UpdateUsuarioDto dto)
        {
            return new Usuario
            {
                Id = dto.Id,
                Nombre = dto.Nombre,
                Email = dto.Email,
                Rol = dto.Rol,
                Activo = dto.Activo,
                FechaModificacion = DateTime.UtcNow
            };
        }
    }
}
