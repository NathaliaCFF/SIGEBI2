namespace Application.DTOs
{
    public class AuthResponseDTO
    {
        // Token JWT generado
        public string Token { get; set; } = string.Empty;

        // Fecha de expiración del token
        public DateTime Expiracion { get; set; }

        // Datos básicos del usuario autenticado
        public string NombreUsuario { get; set; } = string.Empty;
        public string Rol { get; set; } = string.Empty;
    }
}
