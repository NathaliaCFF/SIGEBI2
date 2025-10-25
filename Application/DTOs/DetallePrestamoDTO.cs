using SIGEBI.Application.DTOs;
using SIGEBI.Domain.Entities;
using System.Linq;

namespace SIGEBI.Application.DTOs
{
    public class DetallePrestamoDTO
    {
        public int Id { get; set; }
        public int PrestamoId { get; set; }
        public int LibroId { get; set; }
        public string TituloLibro { get; set; } = string.Empty;
        public DateTime? FechaDevolucion { get; set; }
        public bool Devuelto { get; set; }
    }
}
