using SIGEBI.Domain.Entities;
using Application.DTOs;

namespace SIGEBI.Application.Interfaces
{
    public interface IAuthService
    {
        AuthResponseDTO GenerarToken(Usuario usuario);
    }
}
