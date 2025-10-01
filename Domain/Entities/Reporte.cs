namespace SIGEBI.Domain.Entities
{
    // Clase de resultado, no hereda de BaseEntity porque no se persiste en DB
    public class Reporte
    {
        public int LibroId { get; set; }
        public string? Titulo { get; set; }
        public int CantidadPrestamos { get; set; }
    }
}
