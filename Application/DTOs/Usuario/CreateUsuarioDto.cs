using System.ComponentModel.DataAnnotations;

namespace SIGEBI.Application.DTOs.Usuario
{
    public class CreateUsuarioDto
    {
        [Required(ErrorMessage = "El nombre es obligatorio.")]
        public string Nombre { get; set; } = string.Empty;

        [Required(ErrorMessage = "El correo electrónico es obligatorio.")]
        [EmailAddress(ErrorMessage = "Formato de correo no válido.")]
        public string Email { get; set; } = string.Empty;

        [Required(ErrorMessage = "La contraseña es obligatoria.")]
        public string Contraseña { get; set; } = string.Empty;

        public string Rol { get; set; } = "Usuario";
    }
}
