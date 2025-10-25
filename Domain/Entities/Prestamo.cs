using SIGEBI.Shared.Base;
using System;
using System.Collections.Generic;

namespace SIGEBI.Domain.Entities
{
    public class Prestamo : BaseEntity
    {
        public int UsuarioId { get; set; }
        public DateTime FechaPrestamo { get; set; } = DateTime.Now;
        public DateTime FechaVencimiento { get; set; }

        public Usuario Usuario { get; set; } = null!;
        public ICollection<DetallePrestamo> Detalles { get; set; } = new List<DetallePrestamo>();
    }
}
