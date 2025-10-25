using SIGEBI.Shared.Base;
using System.Collections.Generic;

namespace SIGEBI.Domain.Entities
{
    public class Usuario : BaseEntity
    {
        public string Nombre { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Contraseña { get; set; } = string.Empty;
        public string Rol { get; set; } = "Usuario";

        public ICollection<Prestamo> Prestamos { get; set; } = new List<Prestamo>();
    }
}

