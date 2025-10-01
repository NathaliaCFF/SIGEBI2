using SIGEBI.Domain.Entities;
using SIGEBI.Shared.Base;
using System.Threading.Tasks;

namespace SIGEBI.Domain.Repository
{
    public interface IUsuarioRepository : IBaseRepository<Usuario>
    {
        Task<Usuario?> AutenticarAsync(string email, string password);
        Task<Usuario?> ObtenerPorEmailAsync(string email);
        Task<bool> ActivarUsuarioAsync(int id);
        Task<bool> DesactivarUsuarioAsync(int id);
    }
}
