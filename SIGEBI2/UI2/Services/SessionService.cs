using Application.DTOs;

namespace UI2.Services
{
    public class SessionService
    {
        public AuthResponseDTO? AuthInfo { get; set; }
        public string Email { get; private set; } = string.Empty;

        public bool EstaAutenticado =>
            AuthInfo != null;

        public void RegistrarSesion(string email, AuthResponseDTO auth)
        {
            Email = email;
            AuthInfo = auth;
        }

        public void CerrarSesion()
        {
            Email = string.Empty;
            AuthInfo = null;
        }

        public string NombreUsuario =>
            AuthInfo?.NombreUsuario ?? string.Empty;

        public string Rol =>
            AuthInfo?.Rol ?? string.Empty;

        public string Token =>
            AuthInfo?.Token ?? string.Empty;
    }
}

