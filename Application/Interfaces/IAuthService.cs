using Application.DTOs;
using SIGEBI.Domain.Entities;

namespace SIGEBI.Application.Interfaces
{
    public interface IAuthService
    {
        AuthResponseDTO GenerarToken(Usuario usuario);
    }
}
