using Microsoft.EntityFrameworkCore;
using Persistence.Base;
using SIGEBI.Domain.Entities;
using SIGEBI.Domain.Repository;
using SIGEBI.Persistence.Context;
using SIGEBI.Shared.Base;
using System.Threading.Tasks;

namespace SIGEBI.Persistence.Repositories
{
    public class UsuarioRepository : BaseRepository<Usuario>, IUsuarioRepository
    {
        public UsuarioRepository(AppDbContext context) : base(context) { }

        public async Task<Usuario?> AutenticarAsync(string email, string password)
        {
            return await _context.Usuarios
                .FirstOrDefaultAsync(u => u.Email == email && u.Contraseña == password && u.Activo);
        }

        public async Task<Usuario?> ObtenerPorEmailAsync(string email)
        {
            return await _context.Usuarios.FirstOrDefaultAsync(u => u.Email == email);
        }

        public async Task<bool> ActivarUsuarioAsync(int id)
        {
            var usuario = await _context.Usuarios.FindAsync(id);
            if (usuario == null) return false;
            usuario.Activo = true;
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> DesactivarUsuarioAsync(int id)
        {
            var usuario = await _context.Usuarios.FindAsync(id);
            if (usuario == null) return false;
            usuario.Activo = false;
            await _context.SaveChangesAsync();
            return true;
        }
    }
}

