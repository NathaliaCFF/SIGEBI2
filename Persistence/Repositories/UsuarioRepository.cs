using Microsoft.EntityFrameworkCore;
using SIGEBI.Domain.Entities;
using SIGEBI.Domain.Repository;
using SIGEBI.Persistence.Base;
using SIGEBI.Persistence.Context;

namespace SIGEBI.Persistence.Repositories
{
    // ============================================================================
    // REPOSITORIO: UsuarioRepository
    // MÓDULO: Gestión de Usuarios / Autenticación
    // DESCRIPCIÓN: Implementa las operaciones de acceso a datos relacionadas con
    // los usuarios del sistema, incluyendo autenticación, activación y desactivación.
    // CASOS DE USO RELACIONADOS:
    //   - CU-00: Autenticar usuario
    //   - CU-07: Activar usuario
    //   - CU-08: Desactivar usuario
    // CAPA: Persistencia
    // ============================================================================
    public class UsuarioRepository : BaseRepository<Usuario>, IUsuarioRepository
    {
        public UsuarioRepository(AppDbContext context) : base(context) { }

        // ============================================================================
        // CASO DE USO: CU-00 - Autenticar usuario
        // DESCRIPCIÓN: Busca un usuario activo por correo electrónico para validar
        // sus credenciales. La verificación de la contraseña se realiza en la capa
        // de aplicación (servicio de autenticación).
        // ============================================================================
        public async Task<Usuario?> AutenticarAsync(string email, string password)
        {
            var usuario = await _context.Usuarios
                .AsNoTracking()
                .FirstOrDefaultAsync(u => u.Email == email && u.Activo);

            // CU-00: El hash de la contraseña se valida en la capa de servicio
            return usuario;
        }

        // ============================================================================
        // MÉTODO AUXILIAR: ObtenerPorEmailAsync
        // DESCRIPCIÓN: Recupera un usuario por su dirección de correo electrónico.
        // Utilizado en varios casos de uso (CU-00, CU-05, CU-06) para validaciones previas.
        // ============================================================================
        public async Task<Usuario?> ObtenerPorEmailAsync(string email)
        {
            return await _context.Usuarios.FirstOrDefaultAsync(u => u.Email == email);
        }

        // ============================================================================
        // CASO DE USO: CU-07 - Activar usuario
        // DESCRIPCIÓN: Cambia el estado de un usuario inactivo a activo, permitiendo
        // que vuelva a realizar préstamos en el sistema.
        // ============================================================================
        public async Task<bool> ActivarUsuarioAsync(int id)
        {
            var usuario = await _context.Usuarios.FindAsync(id);
            if (usuario == null)
                return false;

            // CU-07: Verifica si el usuario ya se encuentra activo
            if (usuario.Activo)
                return true;

            usuario.Activo = true;
            usuario.FechaModificacion = DateTime.UtcNow;

            _context.Usuarios.Update(usuario);
            await _context.SaveChangesAsync();
            return true;
        }

        // ============================================================================
        // CASO DE USO: CU-08 - Desactivar usuario
        // DESCRIPCIÓN: Cambia el estado de un usuario activo a inactivo, impidiendo
        // que realice nuevos préstamos hasta su reactivación.
        // ============================================================================
        public async Task<bool> DesactivarUsuarioAsync(int id)
        {
            var usuario = await _context.Usuarios.FindAsync(id);
            if (usuario == null)
                return false;

            // CU-08: Verifica si el usuario ya se encuentra inactivo
            if (!usuario.Activo)
                return true;

            usuario.Activo = false;
            usuario.FechaModificacion = DateTime.UtcNow;

            _context.Usuarios.Update(usuario);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
