using SIGEBI.Shared.Base;
using System;

namespace SIGEBI.Domain.Entities
{
    public class DetallePrestamo : BaseEntity
    {
        public int PrestamoId { get; set; }
        public int LibroId { get; set; }
        public DateTime? FechaDevolucion { get; set; } 
        public bool Devuelto { get; set; }

        // Relaciones
        public Prestamo Prestamo { get; set; } = null!;
        public Libro Libro { get; set; } = null!;
    }

}
