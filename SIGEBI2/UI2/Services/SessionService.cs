using Application.DTOs;
using SIGEBI.Domain.Entities;

namespace UI2.Services
{
    public class SessionService
    {
        public AuthResponseDTO? AuthInfo { get; private set; }
        public Usuario? UsuarioActual { get; private set; }
        public string Email { get; private set; } = string.Empty;

        public bool EstaAutenticado => AuthInfo != null && UsuarioActual != null;

        public void RegistrarSesion(string email, AuthResponseDTO auth)
        {
            Email = email;
            AuthInfo = auth;
            UsuarioActual = new Usuario
            {
                Email = email,
                Nombre = auth.NombreUsuario,
                Rol = auth.Rol,
                Activo = true
            };
        }

        public void CerrarSesion()
        {
            Email = string.Empty;
            AuthInfo = null;
            UsuarioActual = null;
        }
    }
}