using System.ComponentModel.DataAnnotations;

namespace SIGEBI.Application.DTOs.Usuario
{
    public class UpdateUsuarioDto
    {
        [Required(ErrorMessage = "Debe especificar el ID del usuario.")]
        public int Id { get; set; }

        [Required(ErrorMessage = "El nombre es obligatorio.")]
        public string Nombre { get; set; } = string.Empty;

        [EmailAddress(ErrorMessage = "Formato de correo no válido.")]
        public string Email { get; set; } = string.Empty;

        public string Rol { get; set; } = "Usuario";
        public bool Activo { get; set; }
    }
}
